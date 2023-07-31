using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public PingGroupController(NodeGroupViewModel nodeGroupViewModel, PingGroupPanel parentPingGroupPanel)
        {
            NodeGroupViewModel = nodeGroupViewModel;
            ParentPingGroupPanel = parentPingGroupPanel;
            AddGroupButton();
            AddWarpPanel();
            AddNodes();
        }

        private void AddGroupButton()
        {
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(150);
            ParentPingGroupPanel.GroupGrid.RowDefinitions.Add(newRow);

            var newButton = new PingGroupButton();

            newButton.ParentPingGroupPanel = ParentPingGroupPanel;
            newButton.NodeGroupItem = NodeGroupViewModel;
            Grid.SetColumn(newButton, 0);
            Grid.SetRow(newButton, ParentPingGroupPanel.GroupGrid.RowDefinitions.Count - 1);
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
            var nodeButton = new PingNodeButton(node);
            NodesWrapPanel.Children.Add(nodeButton);

            PingNodeButtons.Add(nodeButton);
        }

        private void RemoveAllPingNodes()
        {
            while (PingNodeButtons.Count>0)
            {
                var pingNodeButton = PingNodeButtons.FirstOrDefault();
                RemoveNode(pingNodeButton);
                PingNodeButtons.Remove(pingNodeButton);
            }
        }

        public void RemoveNode(PingNodeButton pingNodeButton)
        {
            NodesWrapPanel.Children.Remove(pingNodeButton);
            pingNodeButton.Destroy();
        }

        public void Destroy()
        {
            int rowIndex = Grid.GetRow(PingGroupButtonControl);
            if (rowIndex != -1)
            {
                ParentPingGroupPanel.GroupGrid.RowDefinitions.RemoveAt(rowIndex);

                ParentPingGroupPanel.GroupGrid.Children.Remove(PingGroupButtonControl);

                RebuildRowIndex(rowIndex);
            }

            PingGroupButtonControl.Destroy();
            RemoveAllPingNodes();
            ParentPingGroupPanel.GroupGrid.Children.Remove(NodesWrapPanel);
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
