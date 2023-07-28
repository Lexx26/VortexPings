using VortexPings.Models;
using VortexPings.Ping;

namespace UIConsole
{
    internal class Program
    {
        private static Node _node;
        static void Main(string[] args)
        {
            var node = new Node();
            _node = node;
            var nodeData = new NodeData() { DontFragment = true, HostOrIPadress = "192.168.2.1", PackageSize = 56, TimeOut = 3000 };
            node.NodeData = nodeData;

            node.PingResultDataUpdated += Node_PingResultDataUpdated;

            var pinger = new Pinger();
            pinger.StartPing(_node);

            Console.ReadLine();

            pinger.StopPing(node);

            Console.WriteLine(node.CancellationTokenSource.IsCancellationRequested);

            Console.ReadLine();

            node.NodeData.HostOrIPadress = "localHost";

            pinger.StartPing(_node);

            Console.ReadLine();
        }

        private static void Node_PingResultDataUpdated()
        {
            Console.WriteLine(_node.PingResultData.ResponseAdress + " " + _node.PingResultData.LastRoundTripTime + " " + _node.PingResultData.PingResult + " " + _node.PingResultData.PingStatus);
        }
    }
}