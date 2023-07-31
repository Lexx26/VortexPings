using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UIWPF.Controls.Custom;
using UIWPF.ViewModels;
using VortexPings.Models;

namespace UIWPF.Controls.Models
{
    public class PingGroupController
    {
        public NodeGroupViewModel NodeGroupViewModel { get; private set; }


        private ObservableCollection<NodeViewModel> _previosNodes;
        public PingGroupController(NodeGroupViewModel nodeGroupViewModel, PingGroupPanel parentPingGroupPanel)
        {
            NodeGroupViewModel = nodeGroupViewModel;
            ParentPingGroupPanel = parentPingGroupPanel;
            AddGroupButton();
            AddWarpPanel();
            AddNodes();

            _previosNodes = NodeGroupViewModel.Nodes;
            NodeGroupViewModel.PropertyChanged += NodeGroupViewModel_PropertyChanged;
            SubscribeNodesChange();
        }

        private void NodeGroupViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="Nodes")
            {
                UnsubscribeNodesChange();
                RemoveAllPingNodes();
                if(NodeGroupViewModel.Nodes!=null)
                {
                    AddNodes();
                    SubscribeNodesChange();
                    _previosNodes = NodeGroupViewModel.Nodes;
                }
            }
        }

        private void UnsubscribeNodesChange()
        {
            if(_previosNodes!=null)
            {
                _previosNodes.CollectionChanged -= Nodes_CollectionChanged;
            }
        }

        private void SubscribeNodesChange()
        {
            if(NodeGroupViewModel.Nodes!=null)
            NodeGroupViewModel.Nodes.CollectionChanged += Nodes_CollectionChanged;
        }

        private void Nodes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NodeViewModel newNode in e.NewItems)
                {
                    AddNode(newNode);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NodeViewModel removedNode in e.OldItems)
                {
                    var pingNodeButton = PingNodeButtons.FirstOrDefault(t => t.NodeViewModel == removedNode);
                    if(pingNodeButton!=null)
                    RemoveNode(pingNodeButton);
                }
            }
            else if(e.Action==NotifyCollectionChangedAction.Reset)
            {
                RemoveAllPingNodes();
            }
        }

        private void AddGroupButton()
        {
            var newRow = new RowDefinition();
            newRow.MinHeight = 100;
            ParentPingGroupPanel.GroupGrid.RowDefinitions.Add(newRow);

            var newButton = new PingGroupButton();

            newButton.ParentPingGroupPanel = ParentPingGroupPanel;
            newButton.NodeGroupItem = NodeGroupViewModel;
            Grid.SetColumn(newButton, 0);
            Grid.SetRow(newButton, ParentPingGroupPanel.GroupGrid.RowDefinitions.Count - 1);
            var dockPanel = new DockPanel();

            ParentPingGroupPanel.GroupGrid.Children.Add(newButton);

            PingGroupButtonControl = newButton;
        }

        private void AddWarpPanel()
        {
            var wrapPanel = new WrapPanel();
            wrapPanel.HorizontalAlignment = HorizontalAlignment.Left;
            wrapPanel.VerticalAlignment = VerticalAlignment.Stretch;

            Grid.SetColumn(wrapPanel, 1);
            Grid.SetRow(wrapPanel, ParentPingGroupPanel.GroupGrid.RowDefinitions.Count - 1);
            ParentPingGroupPanel.GroupGrid.Children.Add(wrapPanel);
            NodesWrapPanel = wrapPanel;
        }

        private void AddNodes()
        {
            if(NodeGroupViewModel!=null&&NodeGroupViewModel.Nodes!=null)
            {
                foreach (var nodeViewModel in NodeGroupViewModel.Nodes)
                {
                    if (nodeViewModel != null)
                        AddNode(nodeViewModel);
                }
            }

        }

        private void AddNode(NodeViewModel node)
        {
            var nodeButton = new PingNodeButton(node, ParentPingGroupPanel);
            NodesWrapPanel.Children.Add(nodeButton);

            PingNodeButtons.Add(nodeButton);
        }

        private void RemoveAllPingNodes()
        {
            while (PingNodeButtons.Count>0)
            {
                var pingNodeButton = PingNodeButtons.FirstOrDefault();
                RemoveNode(pingNodeButton);
               
            }
        }

        public void RemoveNode(PingNodeButton pingNodeButton)
        {
            NodesWrapPanel.Children.Remove(pingNodeButton);
            pingNodeButton.DestroyControl();
            PingNodeButtons.Remove(pingNodeButton);
        }

        public void DestroyControls()
        {
            int rowIndex = Grid.GetRow(PingGroupButtonControl);
            if (rowIndex != -1)
            {
                ParentPingGroupPanel.GroupGrid.RowDefinitions.RemoveAt(rowIndex);

                ParentPingGroupPanel.GroupGrid.Children.Remove(PingGroupButtonControl);

                RebuildRowIndex(rowIndex);
            }

            PingGroupButtonControl.Dispose();
            RemoveAllPingNodes();
            ParentPingGroupPanel.GroupGrid.Children.Remove(NodesWrapPanel);
            UnsubscribeNodesChange();
            NodeGroupViewModel.PropertyChanged -= NodeGroupViewModel_PropertyChanged;
        }

        private void RebuildRowIndex(int rowIndex)
        {
            for (int i = rowIndex; i < ParentPingGroupPanel.GroupGrid.RowDefinitions.Count; i++)
            {
                foreach (UIElement child in ParentPingGroupPanel.GroupGrid.Children)
                {
                    if (Grid.GetRow(child) == i + 1)
                    {
                        Grid.SetRow(child, i);
                    }
                }
            }
        }

        public PingGroupPanel ParentPingGroupPanel { get; private set; }

        public PingGroupButton PingGroupButtonControl { get; private set; }

        public WrapPanel NodesWrapPanel { get; private set; }

        public List<PingNodeButton> PingNodeButtons { get; private set; } = new List<PingNodeButton>();





    }
}
