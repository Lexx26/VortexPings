using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VortexPings.Models;

namespace VortexPings.Ping
{
    public class Pinger
    {
        private List<Node> _Nodes = new List<Node>();
        private List<Task<Node>> _PingTasks = new List<Task<Node>>();

        public bool IsPinging { get; private set; }

        public int MinPingTime { get; set; } = 1000;

        private object _PingTasksLock = new object();

        public void StartPing(Node node)
        {

            if (_Nodes.Contains(node))
                return;

            _Nodes.Add(node);
            node.IsInPingerQueue = true;
            lock (_PingTasksLock)
            {
                _PingTasks.Add(node.PingAsync(MinPingTime));
            }
            if (IsPinging == false)
            {
                IsPinging = true;
                Task.Run(() => PingCycles());
            }

        }

        private async Task PingCycles()
        {
            try
            {

                while (_PingTasks.Count > 0)
                {
                    
                    var completedTask = await Task.WhenAny(_PingTasks);

                    var nodeTaskToRestart = completedTask.Result;

                    _PingTasks.Remove(completedTask);

                    if (completedTask.IsCanceled == false && nodeTaskToRestart.CancellationTokenSource.IsCancellationRequested == false)
                    {
                     
                        _PingTasks.Add(nodeTaskToRestart.PingAsync(MinPingTime));
                    }
                    else
                    {
                        _Nodes.Remove(nodeTaskToRestart);
                        nodeTaskToRestart.IsInPingerQueue = false;
                    }

                } 

            }
            catch (Exception ex)
            {
                for (int i = 0; i < _Nodes.Count; i++)
                {
                    Node? node = _Nodes[i];
                    node.IsInPingerQueue = false;
                    
                }
                _PingTasks.Clear();
                _Nodes.Clear();
                
                IsPinging = false;
            }

            IsPinging = false;
        }

        public void StopPing(Node node)
        { 
            node.CancellationTokenSource.Cancel();

        }

    }
}
