using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VortexPings.Models;

namespace VortexPings.Ping
{
    public class Pinger : IPinger
    {
        private ConcurrentBag<Node> _newNodes = new();

        private ConcurrentDictionary<Node, Task<Node>> _pingTasks = new();
        public bool IsPinging { get; private set; }

        public void StartPing(Node node)
        {

            if (node.IsInPingerQueue == true || _newNodes.Contains(node))
                return;

            _newNodes.Add(node);

            if (IsPinging == false)
            {
                IsPinging = true;
                Task.Run(() => PingCycles());
            }

        }

        private void CreateNewPingTasksFromNewNodes()
        {
            while (_newNodes.Count > 0)
            {
                var isNodePeek = _newNodes.TryTake(out Node node);
                if (isNodePeek)
                {
                    node.IsInPingerQueue = true;
                    _pingTasks.TryAdd(node, node.PingAsync());
                }

            }
        }

        private async Task PingCycles()
        {
            while (IsPinging == true)
            {
                try
                {
                    CreateNewPingTasksFromNewNodes();
                    RemoveCanceledTasks();

                    while (_pingTasks.Count > 0)
                    {

                        var completedTask = await Task.WhenAny(_pingTasks.Select(t => t.Value));

                        var nodeTaskToRestart = await completedTask;

                        _pingTasks.TryRemove(nodeTaskToRestart, out Task<Node> pingTask);

                        CreateNewPingTasksFromNewNodes();
                        if (completedTask.IsCanceled == false && nodeTaskToRestart.CancellationTokenSource.IsCancellationRequested == false)
                        {
                            _pingTasks.TryAdd(nodeTaskToRestart, nodeTaskToRestart.PingAsync());
                        }
                        else
                        {
                            nodeTaskToRestart.IsInPingerQueue = false;
                        }
                        RemoveCanceledTasks();
                    }

                }
                catch (OperationCanceledException ex)
                {

                }
                catch (Exception ex)
                {

                }

                if (_pingTasks.Count == 0)
                    IsPinging = false;
            }
        }

        private void RemoveCanceledTasks()
        {
            var canceledTask = _pingTasks.FirstOrDefault(t => t.Value.IsCanceled == true);
            if (canceledTask.Value != null)
            {
                _pingTasks.TryRemove(canceledTask);
                canceledTask.Key.IsInPingerQueue = false;
            }

            if (_pingTasks.Count == 0)
                IsPinging = false;

        }

        public void StopPing(Node node)
        {
            node.CancellationTokenSource.Cancel();

        }

    }
}
