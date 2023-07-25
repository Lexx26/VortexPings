using System;
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
        private List<Task> _PingTasks = new List<Task>();
        public bool IsPinging { get; private set; }

        public int MinPingTime { get; set; } = 1000;

        public async Task StartPing(Node node)
        {
            if (_Nodes.Contains(node))
                return;

            _Nodes.Add(node);

            if (IsPinging == false)
            {
                PingCycles();
            }

        }

        public async Task PingCycles()
        {
            IsPinging = true;

            while (_Nodes.Count > 0)
            {
                for (int i = 0; i < _Nodes.Count; i++)
                {
                    Node? node = _Nodes[i];
                    if (node.CancellationTokenSource == null || !node.CancellationTokenSource.IsCancellationRequested)
                    {
                        _PingTasks.Add(node.PingAsync(MinPingTime));
                    }

                    if (node.CancellationTokenSource != null && node.CancellationTokenSource.IsCancellationRequested)
                    {
                        _Nodes.Remove(node);
                    }

                }

                while (_PingTasks.Count > 0)
                {
                    var compledTask = await Task.WhenAny(_PingTasks);
                    _PingTasks.Remove(compledTask);

                }
            }

            IsPinging = false;
        }

        public void StopPing(Node node)
        {
            _Nodes.Remove(node);
            node.CancellationTokenSource.Cancel();

        }

    }
}
