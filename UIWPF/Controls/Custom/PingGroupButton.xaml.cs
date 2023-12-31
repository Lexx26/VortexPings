﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIWPF.ViewModels;
using VortexPings.Models;

namespace UIWPF.Controls.Custom
{
    /// <summary>
    /// Interaction logic for PingGroupButton.xaml
    /// </summary>
    public partial class PingGroupButton : UserControl
    {
        public static readonly DependencyProperty GroupItemProperty =
          DependencyProperty.Register("NodeGroupItem", typeof(NodeGroupViewModel), typeof(PingGroupPanel), new PropertyMetadata(null));

        public NodeGroupViewModel NodeGroupItem
        {
            get { return (NodeGroupViewModel)GetValue(GroupItemProperty); }
            set {Unbind() ; SetValue(GroupItemProperty, value); BindData(); }
        }

        private void BindData()
        {
            Binding binding = new Binding("NodeGroupItem.Name");
            binding.Source = this;
            binding.Mode = BindingMode.OneWay;
            this.GroupName.SetBinding(TextBlock.TextProperty, binding);

            Binding binding2 = new Binding("NodeGroupItem.NodesCount");
            binding2.Source = this;
            binding2.Mode = BindingMode.OneWay;
            this.NodeCount.SetBinding(TextBlock.TextProperty, binding2);
        }

        private void Unbind()
        {
            if (NodeGroupItem == null)
                return;

            BindingOperations.ClearAllBindings(this);
        }

        public PingGroupPanel ParentPingGroupPanel { get; set; }

        
        public PingGroupButton()
        {
            InitializeComponent();
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedGroupNode = NodeGroupItem;
            if(ParentPingGroupPanel.ButtonLeftCommand!=null)
            ParentPingGroupPanel.ButtonLeftCommand.Execute(null);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedGroupNode = NodeGroupItem;
            if(ParentPingGroupPanel.ButtonRightCommand!=null)
            ParentPingGroupPanel.ButtonRightCommand.Execute(null);
            
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedGroupNode = NodeGroupItem;
            if (ParentPingGroupPanel.GroupCommand != null)
                ParentPingGroupPanel.GroupCommand.Execute(null);

        }

        public void Dispose()
        {
            Unbind();
        }
    }
}
