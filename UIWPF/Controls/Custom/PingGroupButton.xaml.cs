using System;
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
          DependencyProperty.Register("GroupItem", typeof(NodeGroup), typeof(PingGroupPanel), new PropertyMetadata(null));

        public NodeGroupViewModel NodeGroupItem
        {
            get { return (NodeGroup)GetValue(GroupItemProperty); }
            set { SetValue(GroupItemProperty, value); SetGroupName(); }
        }

        public PingGroupPanel ParentPingGroupPanel { get; set; }

        private void SetGroupName()
        {
            this.GroupName.Text = NodeGroupItem.Name;
        }


        public PingGroupButton()
        {
            InitializeComponent();
        }

        private void DeleteGroupButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedCroupeNode = NodeGroupItem;
            if(ParentPingGroupPanel.DeleteGroupCommand!=null)
            ParentPingGroupPanel.DeleteGroupCommand.Execute(null);
        }

        private void EditGroupButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedCroupeNode = NodeGroupItem;
            if(ParentPingGroupPanel.EditGroupCommand!=null)
            ParentPingGroupPanel.EditGroupCommand.Execute(null);
            SetGroupName();
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedCroupeNode = NodeGroupItem;
            if (ParentPingGroupPanel.GroupCommand != null)
                ParentPingGroupPanel.GroupCommand.Execute(null);

        }
    }
}
