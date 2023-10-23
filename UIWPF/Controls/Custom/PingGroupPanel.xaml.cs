using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using UIWPF.Commands;
using UIWPF.Controls.Models;
using UIWPF.ViewModels;
using VortexPings.Models;

namespace UIWPF.Controls.Custom
{
    /// <summary>
    /// Interaction logic for PingGroupPanel.xaml
    /// </summary>
    public partial class PingGroupPanel : UserControl
    {
        public static readonly DependencyProperty DataItemsProperty =
           DependencyProperty.Register("DataItems", typeof(ObservableCollection<NodeGroupViewModel>), typeof(PingGroupPanel), new PropertyMetadata(null));

        public static readonly DependencyProperty ClickedGroupNodeProperty =
           DependencyProperty.Register("ClickedGroupNode", typeof(NodeGroupViewModel), typeof(PingGroupPanel), new PropertyMetadata(null));

        public static readonly DependencyProperty ClickedNodeProperty =
            DependencyProperty.Register("ClickedNode", typeof(NodeViewModel), typeof(PingGroupPanel), new PropertyMetadata(null));

        public NodeViewModel ClickedNode
        {
            get { return (NodeViewModel)GetValue(ClickedNodeProperty); }
            set { SetValue(ClickedNodeProperty, value); }
        }
        public NodeGroupViewModel ClickedGroupNode
        {
            get { return (NodeGroupViewModel)GetValue(ClickedGroupNodeProperty); }
            set { SetValue(ClickedGroupNodeProperty, value); }
        }

        public ObservableCollection<NodeGroupViewModel> DataItems
        {
            get { return (ObservableCollection<NodeGroupViewModel>)GetValue(DataItemsProperty); }
            set { SetValue(DataItemsProperty, value); }
        }

        private List<PingGroupController> _GroupsControls = new List<PingGroupController>();

        private ObservableCollection<NodeGroupViewModel> _previousDataItems;

        #region Commands
        public static readonly DependencyProperty NodeCommandPropert =
            DependencyProperty.Register("NodeCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand NodeCommand
        {
            get { return (ICommand)GetValue(NodeCommandPropert); }
            set { SetValue(NodeCommandPropert, value); }
        }

        public static readonly DependencyProperty AddGroupCommandProperty =
        DependencyProperty.Register("AddGroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand AddGroupCommand
        {
            get { return (ICommand)GetValue(AddGroupCommandProperty); }
            set { SetValue(AddGroupCommandProperty, value); }
        }

        private void ExecuteAddGroupCommand(object parameter)
        {
           
        }

       public static readonly DependencyProperty ButtonLeftCommandProperty =
       DependencyProperty.Register("ButtonLeftCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand ButtonLeftCommand
        {
            get { return (ICommand)GetValue(ButtonLeftCommandProperty); }
            set { SetValue(ButtonLeftCommandProperty, value); }
        }

      public static readonly DependencyProperty ButtonRightCommandProperty =
      DependencyProperty.Register("ButtonRightCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand ButtonRightCommand
        {
            get { return (ICommand)GetValue(ButtonRightCommandProperty); }
            set { SetValue(ButtonRightCommandProperty, value); }
        }



        public static readonly DependencyProperty GroupCommandProperty =
        DependencyProperty.Register("GroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand GroupCommand
        {
            get { return (ICommand)GetValue(GroupCommandProperty); }
            set { SetValue(GroupCommandProperty, value); }
        }

        private void ExecuteGroupCommand(object parameter)
        {

        }
        #endregion

        public PingGroupPanel()
        {
            InitializeComponent();
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(DataItemsProperty, typeof(PingGroupPanel));
            dpd?.AddValueChanged(this, OnDataItemsChanged);
        }

        private void OnDataItemsChanged(object sender, EventArgs e)
        {
            if (_previousDataItems != null)
            {
                _previousDataItems.CollectionChanged -= DataItems_CollectionChanged;
            }
            
            if (DataItems != null)
            {
                DataItems.CollectionChanged += DataItems_CollectionChanged;
            }

            _previousDataItems = DataItems;

            ChangingDataItems();
        }

        private void DataItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (var removedItem in e.OldItems)
                    {
                        DeleteNodeGroup(removedItem as NodeGroupViewModel);
                    }
                }
            }

            if(e.Action==NotifyCollectionChangedAction.Add)
            {
                if(e.NewItems !=null)
                {
                    foreach(var addedItem in e.NewItems)
                    {
                        AddGroupNode(addedItem as NodeGroupViewModel);
                    }
                }
            }

            if(e.Action == NotifyCollectionChangedAction.Reset)
            {
                while(_GroupsControls.Count>0)
                {
                    var pingGroupController = _GroupsControls.FirstOrDefault();
                    RemovePingGroupController(pingGroupController);
                }
            }
        }

        private void DeleteNodeGroup(NodeGroupViewModel nodeGroup)
        {
            if (nodeGroup == null)
                return;

            var pingGroupController = _GroupsControls.FirstOrDefault(t => t.NodeGroupViewModel == nodeGroup);
            if (pingGroupController != null) ;
            RemovePingGroupController(pingGroupController);
        }

        private void AddGroupNode(NodeGroupViewModel addedItem)
        {
            if (addedItem == null)
                return;

            var pingGroup = new PingGroupController(addedItem, this);
            _GroupsControls.Add(pingGroup);
        }

        /// <summary>
        /// When main collection changed
        /// </summary>
        public void ChangingDataItems()
        {
            if(_GroupsControls.Count>0)
            {
                RemoveAllControls();
            }

            if (DataItems == null)
                return;

            foreach (var dataItem in DataItems)
            {
                AddGroupNode(dataItem);
            }
        }

        /// <summary>
        /// Remove all child controls
        /// </summary>
        private void RemoveAllControls()
        {
            while (_GroupsControls.Count>0)
            {
                RemovePingGroupController(_GroupsControls.First()); 
            }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            if (DataItems == null)
                return;

            AddGroupCommand.Execute(null);

        }

        public void RemovePingGroupController(PingGroupController groupControls)
        {
            groupControls.DestroyControls();
            _GroupsControls.Remove(groupControls);
          
        }

    }
}
