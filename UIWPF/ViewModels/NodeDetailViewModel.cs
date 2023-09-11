using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VortexPings.Factories;
using VortexPings.Models;
using VortexPings.Ping;

namespace UIWPF.ViewModels
{
    class NodeDetailViewModel:FixedDialogBaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IPinger _pinger;

        public NodeDetailViewModel(IDialogService dialogService, IPinger pinger)
        {
            _dialogService = dialogService;
            _pinger = pinger;
        }

        private NodeViewModel _node;
        public NodeViewModel Node
        {
            get { return _node; }
            set { SetProperty(ref _node, value); }
        }

        private NodeGroupViewModel _nodeGroup;
        public NodeGroupViewModel NodeGroup
        {
            get { return _nodeGroup; }
            set { SetProperty(ref _nodeGroup, value); }
        }

        public string PingStateCaption
        {
            get {
                if(Node!=null&&Node.IsInPingerQueue==true)
                {
                    return "Stop ping";
                }
                
                return "Start ping"; 
            }
          
        }

        private ObservableCollection<NodeGroupViewModel> _NodeGroups;

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            NodeGroup = parameters.GetValue<NodeGroupViewModel>("NodeGroup");
            Node = parameters.GetValue<NodeViewModel>("Node");
            _NodeGroups = parameters.GetValue<ObservableCollection<NodeGroupViewModel>>("NodeGroups");
            base.OnDialogOpened(parameters);
            Node.PropertyChanged += Node_PropertyChanged;
            RaisePropertyChanged(nameof(PingStateCaption));
        }

        private void Node_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;

            switch (propertyName)
            {
                case "IsInPingerQueue":
                    RaisePropertyChanged(nameof(PingStateCaption));
                    break;
            }
        }

        #region Commands
        private DelegateCommand _pingNodeCommand;
        public DelegateCommand PingNodeCommand =>
            _pingNodeCommand ?? (_pingNodeCommand = new DelegateCommand(ExecutePingNodeCommand, CanExecutePingNodeCommand));

        void ExecutePingNodeCommand()
        {
            if(Node.IsInPingerQueue==true)
            {
                _pinger.StopPing(Node.NodeModel);
            }
            else
            {
                _pinger.StartPing(Node.NodeModel);
            }
           
        }

        bool CanExecutePingNodeCommand()
        {
            return true;
        }


        private DelegateCommand _EditNodeCommand;
        public DelegateCommand EditNodeCommand =>
            _EditNodeCommand ?? (_EditNodeCommand = new DelegateCommand(ExecuteEditNodeCommand, CanExecuteEditNodeCommand));

        void ExecuteEditNodeCommand()
        {

            var dialogParameters = new DialogParameters();
            var nodeData = Node.NodeDataViewModel.NodeDataModel.Clone() as NodeData;
            var nodeDataViewModel = new NodeDataViewModel(nodeData);
            dialogParameters.Add("NodeDataViewModel", nodeDataViewModel);
            var nodeNames = _NodeGroups.SelectMany(t => t.Nodes.Select(n => n.NodeDataViewModel.NodeName).Where(i=>i!=Node.NodeDataViewModel.NodeName)).ToList();
            
            dialogParameters.Add("NodeNames", nodeNames);

            _dialogService.ShowDialog("NodeEditView", dialogParameters, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    if (r.Parameters != null)
                    {
                        nodeDataViewModel = r.Parameters.GetValue<NodeDataViewModel>("NodeDataViewModel");
                        Node.NodeDataViewModel = nodeDataViewModel;
                    }
                }
            }, "FixedDialogWindow");
        }

        bool CanExecuteEditNodeCommand()
        {
            return true;
        }

        private DelegateCommand _DeleteNodeCommand;
        public DelegateCommand DeleteNodeCommand =>
            _DeleteNodeCommand ?? (_DeleteNodeCommand = new DelegateCommand(ExecuteDeleteNodeCommand, CanExecuteDeleteNodeCommand));

         void ExecuteDeleteNodeCommand()
        {
           
            NodeGroup.DeleteNode(Node);
            CloseDialogCommand.Execute("true");
        }

        bool CanExecuteDeleteNodeCommand()
        {
            return true;
        }

        #endregion

        public override void OnDialogClosed()
        {
            base.OnDialogClosed();
            Node.PropertyChanged -= Node_PropertyChanged;

        }
    }
}
