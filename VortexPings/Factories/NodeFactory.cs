using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VortexPings.Models;

namespace VortexPings.Factories
{
    public class NodeFactory
    {
        private NodeDataFactory _nodeDataFactory;

        public NodeFactory(NodeDataFactory nodeDataFactory)
        {
            _nodeDataFactory = nodeDataFactory;
        }

        public Node CreateNode(string nodeName, string hostOrIPAddress, int packageSize, int ttl, bool dontFragment, int timeout, int warningTime)
        {
            var nodeData = _nodeDataFactory.CreateNodeData(nodeName, hostOrIPAddress, packageSize, ttl, dontFragment, timeout, warningTime);

            var node = new Node
            {
               
                NodeData = nodeData,
                Order = 0 
            };

            return node;
        }

        public Node CreateNode(NodeData nodeData)
        {
            var node = new Node
            {

                NodeData = nodeData,
                Order = 0
            };

            return node;
        }

        public Node CreateNodeWithDefaultValue(string nodeName, string hostOrIPAddress)
        {
            var nodeData = _nodeDataFactory.CreateNodeDataDefaultValue(nodeName, hostOrIPAddress);

            var node = new Node
            {
                NodeData = nodeData,
                Order = 0 
            };

            return node;
        }
    }
}
