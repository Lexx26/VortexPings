using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows;
using System.Windows.Documents;
using VortexPings.Factories;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    class MainWindowViewModel:BindableBase
    {
        private NodeFactory _nodeFactory;
        public MainWindowViewModel(NodeFactory nodeFactory)
        {
            _nodeFactory = nodeFactory;
            NodeGroups = new ObservableCollection<NodeGroupViewModel>();

            var node = _nodeFactory.CreateNodeWithDefaultValue("test", "localhost");

            var nodeGroup = new NodeGroup() { Id = 0, Name = "TestGroup", Order = 0};
            nodeGroup.Nodes.Add(node);

            var node1 = _nodeFactory.CreateNodeWithDefaultValue("test", "localhost");

            nodeGroup.Nodes.Add(node1);

             var groupViewModel = new NodeGroupViewModel(nodeGroup);

            NodeGroups.Add(groupViewModel);
        }


        private ObservableCollection<NodeGroupViewModel> _NodeGroups;
        public ObservableCollection<NodeGroupViewModel> NodeGroups { get { return _NodeGroups; } set { _NodeGroups = value; RaisePropertyChanged(nameof(NodeGroups)); } }

        
        public NodeGroupViewModel ClikedNodeGroup { get; set; }

        #region Commands
        private DelegateCommand _AddGroupCommand;
        public DelegateCommand AddGroupCommand =>
            _AddGroupCommand ?? (_AddGroupCommand = new DelegateCommand(AddGroupCommandExecute, CanExecuteAddGroupCommand));

        private int _Counter = 0;
        void AddGroupCommandExecute()
        {
            var groupNodeItem = new NodeGroup();
            groupNodeItem.Name = " New Test №"+_Counter++;
            groupNodeItem.Id = _Counter;

            var groupViewModel = new NodeGroupViewModel(groupNodeItem);
            NodeGroups.Add(groupViewModel);


        }

        bool CanExecuteAddGroupCommand()
        {
            return true;
        }


        private DelegateCommand _DeleteGroupCommand;
        public DelegateCommand DeleteGroupCommand =>
            _DeleteGroupCommand ?? (_DeleteGroupCommand = new DelegateCommand(DeleteGroupCommandExecute, CanExecuteDeleteGroupCommand));

        void DeleteGroupCommandExecute()
        {
            NodeGroups.Remove(ClikedNodeGroup);
        }

        bool CanExecuteDeleteGroupCommand()
        {
            return true;
        }


        private DelegateCommand _EditGroupCommand;
        public DelegateCommand EditGroupCommand =>
            _EditGroupCommand ?? (_EditGroupCommand = new DelegateCommand(EditGroupCommandExecute, CanExecuteEditGroupCommand));

        void EditGroupCommandExecute()
        {
            
            var node = _nodeFactory.CreateNodeWithDefaultValue("SuperNew", "localhost");
            var nodeViewModel = new NodeViewModel(node);
            if (ClikedNodeGroup.Nodes == null)
                ClikedNodeGroup.Nodes = new ObservableCollection<NodeViewModel>();
            ClikedNodeGroup.Nodes.Add(nodeViewModel);

        }

        bool CanExecuteEditGroupCommand()
        {
            return true;
        }


        private DelegateCommand _GroupCommand;
        public DelegateCommand GroupCommand =>
            _GroupCommand ?? (_GroupCommand = new DelegateCommand(GroupCommandExecute, CanExecuteGroupCommand));

        void GroupCommandExecute()
        {
            MessageBox.Show("You clicked "+ ClikedNodeGroup.Name);
            ClikedNodeGroup.Nodes = null;
        }

        bool CanExecuteGroupCommand()
        {
            return true;
        }
        #endregion
    }
}
