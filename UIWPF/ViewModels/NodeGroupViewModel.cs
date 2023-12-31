﻿using Prism.Mvvm;
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

        public int NodesCount
        {
            get
            {
                if (Nodes != null)
                {
                    return Nodes.Count();
                }
                else
                {
                    return 0;
                }
            }
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
            set { UnSubscribeNodesCollectionChange(); SetProperty(ref _nodes, value);SubScribeNodesCollectionChanged(); }
        }

        private void SubScribeNodesCollectionChanged()
        {
            if (Nodes != null)
            {
                Nodes.CollectionChanged += Nodes_CollectionChanged;
            }
        }

        private void UnSubscribeNodesCollectionChange()
        {
            if(Nodes!=null)
            {
                Nodes.CollectionChanged -= Nodes_CollectionChanged;
            }
        }

        private void Nodes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(NodesCount));
        }

        private NodeGroup _nodeGroup;
        public NodeGroupViewModel(NodeGroup nodeGroup)
        {
            _nodeGroup = nodeGroup;
            Name = _nodeGroup.Name;
            Id = _nodeGroup.Id;
            Order = _nodeGroup.Order;

            if(_nodeGroup.Nodes!=null)
            {
                Nodes = new ObservableCollection<NodeViewModel>();
                foreach (var node in _nodeGroup.Nodes)
                {
                    var nodeViewModel = new NodeViewModel(node);
                    Nodes.Add(nodeViewModel);
                }
            }

        }

        public async Task DeleteNode(NodeViewModel nodeViewModel)
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
            foreach (var node in Nodes)
            {
                node.Dispose();
            }
        }
    }
}
