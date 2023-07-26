using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.Models
{
    public class NodeGroup
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        private List<Node>? _nodes = new List<Node>();
        public IEnumerable<Node>? Nodes { get { return _nodes; } }

        public void AddNode(Node node)
        {
            var isContain = _nodes.Contains(node);

            if(isContain==false)
            {
                _nodes.Add(node);
            }
        }

        public void DeleteNode(Node node)
        {
            var isContain = _nodes.Contains(node);

            if (isContain == true)
            {
                _nodes.Remove(node);
            }
        }

        public void DeleteGroup()
        {
            foreach (var node in _nodes)
            {
               

            }

        }
    }
}
