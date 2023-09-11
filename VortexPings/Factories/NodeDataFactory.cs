using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VortexPings.Models;

namespace VortexPings.Factories
{
    public class NodeDataFactory
    {
        public NodeData CreateNodeData(string nodeName, string hostOrIPAddress, int packageSize, int ttl, bool dontFragment, int timeout, int warningTime)
        {
            var nodeData = new NodeData
            {
                NodeName = nodeName,
                HostOrIPadress = hostOrIPAddress,
                PackageSize = packageSize,
                TTL = ttl,
                DontFragment = dontFragment,
                TimeOut = timeout,
                WarningTime = warningTime,
                NodeId = 0
            };

            return nodeData;
        }

        public NodeData CreateNodeDataDefaultValue(string nodeName, string hostOrIPAddress)
        {
            var nodeData = new NodeData
            {
                NodeName = nodeName,
                PingRepeatTime = 1000,
                HostOrIPadress = hostOrIPAddress,
                PackageSize = 32,
                TTL = 54,
                DontFragment = false,
                TimeOut = 2000,
                WarningTime = 200,
                NodeId = 0
            };

            return nodeData;
        }
    }
}
