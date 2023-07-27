using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    class MainWindowViewModel:BindableBase
    {
        public MainWindowViewModel()
        {
            NodeGroups = new ObservableCollection<NodeGroup>();

            var groupNodeItem = new NodeGroup();
            groupNodeItem.Name = "Test";
            groupNodeItem.Id = 1;

            NodeGroups.Add(groupNodeItem);
        }


        private ObservableCollection<NodeGroup> _NodeGroups;
        public ObservableCollection<NodeGroup> NodeGroups { get { return _NodeGroups; } set { _NodeGroups = value; RaisePropertyChanged(nameof(NodeGroups)); } }

        
        public NodeGroup ClikedNodeGroup { get; set; }

        #region Commands
        private DelegateCommand _AddGroupCommand;
        public DelegateCommand AddGroupCommand =>
            _AddGroupCommand ?? (_AddGroupCommand = new DelegateCommand(AddGroupCommandExecute, CanExecuteAddGroupCommand));

        private int Counter = 0;
        void AddGroupCommandExecute()
        {
            var groupNodeItem = new NodeGroup();
            groupNodeItem.Name = "Test"+Counter++;
            groupNodeItem.Id = 2;

            NodeGroups.Add(groupNodeItem);


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
            MessageBox.Show($"EditCommand for Groupe {ClikedNodeGroup.Name}");

            ClikedNodeGroup.Name = "dfffsdf";
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
            MessageBox.Show($"EditCommand for Groupe {ClikedNodeGroup.Name}");

            ClikedNodeGroup.Name = "dfffsdf";
        }

        bool CanExecuteGroupCommand()
        {
            return true;
        }
        #endregion
    }
}
