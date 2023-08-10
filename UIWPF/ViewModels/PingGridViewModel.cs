﻿using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using VortexPings.Factories;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    public class PingGridViewModel:BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;
        private NodeFactory _nodeFactory;
        public PingGridViewModel(NodeFactory nodeFactory, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _nodeFactory = nodeFactory;
            NodeGroups = new ObservableCollection<NodeGroupViewModel>();

            var node = _nodeFactory.CreateNodeWithDefaultValue("test", "localhost");

            var nodeGroup = new NodeGroup() { Id = 0, Name = "TestGroup", Order = 0 };
            nodeGroup.Nodes.Add(node);

            var node1 = _nodeFactory.CreateNodeWithDefaultValue("test", "localhost");

            nodeGroup.Nodes.Add(node1);

            var groupViewModel = new NodeGroupViewModel(nodeGroup);

            NodeGroups.Add(groupViewModel);
           
        }



        private ObservableCollection<NodeGroupViewModel> _NodeGroups;
        public ObservableCollection<NodeGroupViewModel> NodeGroups { get { return _NodeGroups; } set { _NodeGroups = value; RaisePropertyChanged(nameof(NodeGroups)); } }


        public NodeGroupViewModel ClikedNodeGroup { get; set; }

        public NodeViewModel ClikedNode { get; set; }

        #region Commands


        private DelegateCommand _AddGroupCommand;
        public DelegateCommand AddGroupCommand =>
            _AddGroupCommand ?? (_AddGroupCommand = new DelegateCommand(AddGroupCommandExecute, CanExecuteAddGroupCommand));

        private int _Counter = 0;
        void AddGroupCommandExecute()
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("GroupName", "");
            var list = NodeGroups.Select(t=>t.Name).ToList();
            dialogParameters.Add("GroupNames", list);

            _dialogService.ShowDialog("NodeGroupEditView", dialogParameters, r => 
            { 
              if(r.Result==ButtonResult.OK)
                {
                   if(r.Parameters!=null)
                    {
                        var groupName = r.Parameters.GetValue<string>("GroupName");
                        var groupNodeItem = new NodeGroup();
                        groupNodeItem.Name = groupName;
                        groupNodeItem.Id = _Counter;

                        var groupViewModel = new NodeGroupViewModel(groupNodeItem);
                        NodeGroups.Add(groupViewModel);
                    }
                }
            },"FixedDialogWindow");
           


        }

        bool CanExecuteAddGroupCommand()
        {
            return true;
        }

        private DelegateCommand _GroupCommand;
        public DelegateCommand GroupCommand =>
            _GroupCommand ?? (_GroupCommand = new DelegateCommand(GroupCommandExecute, CanExecuteGroupCommand));

        void GroupCommandExecute()
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("NodeGroup", ClikedNodeGroup);
            dialogParameters.Add("NodeGroups", NodeGroups);

            _dialogService.ShowDialog("NodeGroupDetailView", dialogParameters, null
            , "FixedDialogWindow");
        }

        bool CanExecuteGroupCommand()
        {
            return true;
        }


        private DelegateCommand _NodeCommand;
        public DelegateCommand NodeCommand =>
            _NodeCommand ?? (_NodeCommand = new DelegateCommand(ExecuteNodeCommand));

        void ExecuteNodeCommand()
        {
            var dialogParameters = new DialogParameters();
            dialogParameters.Add("Node", ClikedNode);

            var nodeGroup = NodeGroups.Where(t => t.Nodes.Contains(ClikedNode)).FirstOrDefault();
            dialogParameters.Add("NodeGroup", nodeGroup);
            dialogParameters.Add("NodeGroups", NodeGroups);

            _dialogService.ShowDialog("NodeDetailView", dialogParameters, null
            , "FixedDialogWindow");
        }
    }
    #endregion
}
