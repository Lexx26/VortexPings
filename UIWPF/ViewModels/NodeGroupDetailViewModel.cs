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
using VortexPings.Factories;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    class NodeGroupDetailViewModel : FixedDialogBaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly NodeFactory _nodeFactory;
        private readonly NodeDataFactory _nodeDataFactory;

        public NodeGroupDetailViewModel(IDialogService dialogService, NodeFactory nodeFactory, NodeDataFactory nodeDataFactory)
        {
            _dialogService = dialogService;
            _nodeFactory = nodeFactory;
            _nodeDataFactory = nodeDataFactory;
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

        private DelegateCommand _addNodeCommand;
        public DelegateCommand AddNodeCommand =>
            _addNodeCommand ?? (_addNodeCommand = new DelegateCommand(ExecuteAddNodeCommand, CanExecuteAddNodeCommand));

        void ExecuteAddNodeCommand()
        {
            var nodeData = _nodeDataFactory.CreateNodeDataDefaultValue("", "");

            var nodeDataViewModel = new NodeDataViewModel(nodeData);
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("NodeDataViewModel", nodeDataViewModel);
            
            _dialogService.ShowDialog("NodeEditView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    if (r.Parameters != null)
                    {
                        nodeDataViewModel = r.Parameters.GetValue<NodeDataViewModel>("NodeDataViewModel");
                        var node = _nodeFactory.CreateNode(nodeDataViewModel.NodeDataModel);
                        var nodeViewModel = new NodeViewModel(node);
                        NodeGroup.Nodes.Add(nodeViewModel);
                    }
                }
            }, "FixedDialogWindow");

        }

        bool CanExecuteAddNodeCommand()
        {
            return true;
        }

        #endregion
    }
}
