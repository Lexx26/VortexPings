using LiveCharts;
using LiveCharts.Configurations;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UIWPF.Models;
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
            SetupChart();
        }

        private void Node_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;

            switch (propertyName)
            {
                case "IsInPingerQueue":
                    RaisePropertyChanged(nameof(PingStateCaption));
                    AddChatrtEmptyValue();
                    break;
                case "NodeDataViewModel":
                    SetupChart();
                    break;
                case "PingResultData":
                    AddChartValue();
                    ChangeGaugesValue();
                    break;
            }
        }

        private void ChangeGaugesValue()
        {
            if(Node!=null&&Node.PingResultData!=null&&Node.PingResultData.LastRoundTripTime!=null)
            GaugesValue = (Double)Node.PingResultData.LastRoundTripTime;
        }

        private double _gaugesValue;
        public double GaugesValue
        {
            get { return _gaugesValue; }
            set { SetProperty(ref _gaugesValue, value); }
        }

        #region Chart
        public ChartValues<PingMeasureModel?> ChartValues { get; set; } = new ChartValues<PingMeasureModel>();
        public Func<double, string> DateTimeFormatter { get; set; }
        public double XAxisStep { get; set; }
       

        private double _xAxisMax;
        public double XAxisMax
        {
            get { return _xAxisMax; }
            set { SetProperty(ref _xAxisMax, value); }
        }

        private double _xAxisMin;
        public double XAxisMin
        {
            get { return _xAxisMin; }
            set { SetProperty(ref _xAxisMin, value); }
        }

        private void SetupChart()
        {
            var mapper = Mappers.Xy<PingMeasureModel>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.LastRoundTripTime);
            Charting.For<PingMeasureModel>(mapper);

            DateTimeFormatter = value => new DateTime((long)value).ToString("HH:mm:ss");

            uint calcXAxisStep;

            if(Node.NodeDataViewModel.PingRepeatTime>1000)
            {
                calcXAxisStep = Convert.ToUInt32(Node.NodeDataViewModel.PingRepeatTime / 100) * 4;
            }
            else
            {
                calcXAxisStep = 4;
            }
            XAxisStep = TimeSpan.FromSeconds(calcXAxisStep).Ticks;
            SetXAxisLimits();
        }

        private void SetXAxisLimits()
        {
            var dateTime = DateTime.Now;
            uint calculateAxisMinValue;
            if(Node.NodeDataViewModel.PingRepeatTime > 500)
            {

                calculateAxisMinValue = Convert.ToUInt32((Node.NodeDataViewModel.PingRepeatTime / 100) * 4);
            }
            else
            {
                calculateAxisMinValue = 10;
            }

            XAxisMax = dateTime.Ticks + TimeSpan.FromSeconds(0).Ticks; // lets force the axis to be 0 second ahead
            XAxisMin = dateTime.Ticks - TimeSpan.FromSeconds(calculateAxisMinValue).Ticks; // and calcvalue seconds behind
        }

        private void AddChartValue()
        {
            if (Node is null || Node.PingResultData is null || Node.PingResultData.LastRoundTripTime is null || Node.PingResultData.DateTime is null)
                return;

            var pingMeasureModel = new PingMeasureModel() { DateTime = (DateTime)Node.PingResultData.DateTime, LastRoundTripTime = (long)Node.PingResultData.LastRoundTripTime };
            ChartValues.Add(pingMeasureModel);
            SetXAxisLimits();
            if (ChartValues.Count > 200)
                ChartValues.RemoveAt(0);
            
        }
        //Add LastRoundTripTime NaN to brake chart line when new ping starts
        private void AddChatrtEmptyValue()
        {
            if (Node.PingResultData.DateTime == null)
                return;
            var pingMeasureModel = new PingMeasureModel() { DateTime = (DateTime)Node.PingResultData.DateTime, LastRoundTripTime = double.NaN};
            ChartValues.Add(pingMeasureModel);
        }

        #endregion

        #region Commands
        private DelegateCommand _pingNodeCommand;
        public DelegateCommand PingNodeCommand =>
            _pingNodeCommand ?? (_pingNodeCommand = new DelegateCommand(ExecutePingNodeCommand, CanExecutePingNodeCommand));

        void ExecutePingNodeCommand()
        {
            if(Node.IsInPingerQueue==true)
            {
                _pinger.StopPing(Node.NodeModel);
                GaugesValue = 0;
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
