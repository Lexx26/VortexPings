using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    class NodeGroupDetailViewModel : FixedDialogBaseViewModel
    {
        private readonly IDialogService _dialogService;

        public NodeGroupDetailViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        private ObservableCollection<NodeGroupViewModel> _NodeGroups;

        private NodeGroupViewModel _nodeGroup;

        public NodeGroupViewModel NodeGroup
        {
            get { return _nodeGroup; }
            set { SetProperty(ref _nodeGroup, value); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            NodeGroup = parameters.GetValue<NodeGroupViewModel>("NodeGroup");
            _NodeGroups = parameters.GetValue<ObservableCollection<NodeGroupViewModel>>("NodeGroups");
        }

        #region Commands
        private DelegateCommand _renameGroupCommand;
        public DelegateCommand RenameGroupCommand =>
            _renameGroupCommand ?? (_renameGroupCommand = new DelegateCommand(ExecuteCommandRenameNodeGroup, CanExecuteCommandRenameNodeGroup));

        void ExecuteCommandRenameNodeGroup()
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("GroupName", NodeGroup.Name);
            dialogParameters.Add("GroupNames", _NodeGroups.Select(t => t.Name).ToList());

            _dialogService.ShowDialog("NodeGroupEditView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    if (r.Parameters != null)
                    {
                        NodeGroup.Name = r.Parameters.GetValue<string>("GroupName");

                    }
                }
            }, "FixedDialogWindow");
        }

        bool CanExecuteCommandRenameNodeGroup()
        {
            return true;
        }

        private DelegateCommand _deleteGroupNodeCommand;
        public DelegateCommand DeleteGroupNodeCommand =>
            _deleteGroupNodeCommand ?? (_deleteGroupNodeCommand = new DelegateCommand(ExecuteDeleteGroupNodeCommand, CanExecuteDeleteGroupNodeCommand));

        void ExecuteDeleteGroupNodeCommand()
        {
            _NodeGroups.Remove(NodeGroup);
            NodeGroup.Dispose();
            base.CloseDialogCommand.Execute("true");
        }

        public override void Dispose()
        {
            base.Dispose();
            _NodeGroups = null;
            NodeGroup = null;
        }

        bool CanExecuteDeleteGroupNodeCommand()
        {
            return true;
        }

        #endregion
    }
}
