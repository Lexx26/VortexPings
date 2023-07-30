using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    public class NodeGroupViewModel : BindableBase, IDisposable
    {
        private int _id;            
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        private ObservableCollection<NodeViewModel> _nodes;
        public ObservableCollection<NodeViewModel> Nodes
        {
            get { return _nodes; }
            set { SetProperty(ref _nodes, value); }
        }

        public NodeGroup NodeGroup { get;private set; }
        public NodeGroupViewModel(NodeGroup nodeGroup)
        {
            NodeGroup = nodeGroup;
            Name = NodeGroup.Name;
            Id = NodeGroup.Id;
            Order = NodeGroup.Order;

            if(NodeGroup.Nodes!=null)
            {
                Nodes = new ObservableCollection<NodeViewModel>();
                foreach (var node in NodeGroup.Nodes)
                {
                    var nodeViewModel = new NodeViewModel(node);
                    Nodes.Add(nodeViewModel);
                }
            }

        }

        public void DeleteNode(NodeViewModel nodeViewModel)
        {
            var isContain = Nodes.Contains(nodeViewModel);

            if(isContain==true)
            {
                nodeViewModel.Dispose();
                Nodes.Remove(nodeViewModel);
            }
        }

        public void Dispose()
        {
            NodeGroup.Dispose();

            foreach (var node in Nodes)
            {
                node.Dispose();
            }
        }
    }
}
