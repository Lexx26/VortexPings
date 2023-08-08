namespace VortexPings.Models
{
    public class NodeGroup:IDisposable
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public int Order { get; set; }

        private List<Node>? _nodes = new List<Node>();
        public List<Node>? Nodes { get { return _nodes; } }

        public void DeleteNode(Node node)
        {
            var isContain = _nodes.Contains(node);
            
            if (isContain == true)
            {
                 node.Dispose();
                _nodes.Remove(node);
            }
        }

        public void Dispose()
        {
            foreach (var node in _nodes)
            {
                node.Dispose();
            }

            _nodes = null;
        }
    }
}
