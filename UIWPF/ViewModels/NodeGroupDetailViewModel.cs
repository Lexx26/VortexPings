using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
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
            StackedSeriesSetValue();
            PieChartSetValues();
            InitTimer();
        }

        public override void OnDialogClosed()
        {
            base.OnDialogClosed();
            _timer.Stop();
            _timer.Tick -= TimerTick;
        }

        private DispatcherTimer _timer = new DispatcherTimer();
        private void InitTimer()
        {
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += TimerTick;
            _timer.Start();
                
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            _timer.Stop();
            StackedSeriesUpdate();
            PieChartSeriesUpdate();
            _timer.Start();
        }

       

        #region Charts
        public SeriesCollection StackedSeries { get; set; } = new SeriesCollection();

        private void StackedSeriesSetValue()
        {
            var nodesInPingerQueueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true).Count();
            var nodesIdleCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == false).Count();

            if(nodesInPingerQueueCount==0&&nodesIdleCount==0)
            {
                return;
            }

            var stackedColumnSeriesNodesPinging = new StackedColumnSeries
            {
                Values = new ChartValues<int> { nodesInPingerQueueCount },
                StackMode = StackMode.Values,
                Title = "Nodes pinging",
                DataLabels = true,
                Name = "NodesPingin"
            };

            var stackedColumnSeries2NodesIdle = new StackedColumnSeries
            {
                Values = new ChartValues<int> {nodesIdleCount },
                StackMode = StackMode.Values,
                Title = "Nodes idle",
                DataLabels = true,
                Name ="NodesIdle"
            };

            StackedSeries.Add(stackedColumnSeriesNodesPinging);
            StackedSeries.Add(stackedColumnSeries2NodesIdle);
        }

        private void StackedSeriesUpdate()
        {
            var nodesPinging = (int)StackedSeries[0].Values[0];
            var nodesIdle = (int)StackedSeries[1].Values[0];

            var nodesInPingerQueueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true).Count();
            var nodesIdleCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == false).Count();

            if(nodesPinging!= nodesInPingerQueueCount||nodesIdle!=nodesIdleCount)
            {
                StackedSeries[0].Values[0] = nodesInPingerQueueCount;
                StackedSeries[1].Values[0] = nodesIdleCount;
            }
        }

        public SeriesCollection PieChartSeries { get; set; } = new SeriesCollection();

        private Color _successColorPieChartFill = (Color)Application.Current.Resources["ForegroundColor"];
        private Color _warningColorPieChartFill = (Color)Application.Current.Resources["YellowColor.Foreground"];
        private Color _alertColorPieChartFill = (Color)Application.Current.Resources["RedColor"];

        private ObservableValue? _pieChartNoDataValue = null;
        private ObservableValue? _pieChartSuccessValue = null;
        private ObservableValue? _pieChartWarningValue = null;
        private ObservableValue? _pieChartAlertValue = null;

        private void PieChartSeriesUpdate()
        {
            var successValueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true && t.PingResultData.PingStatus == PingStatus.Green).Count();
            var warningValueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true && t.PingResultData.PingStatus == PingStatus.Yellow).Count();
            var alertValueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true && t.PingResultData.PingStatus == PingStatus.Red).Count();

            if(successValueCount==0&& warningValueCount==0&&alertValueCount==0&& _pieChartNoDataValue==null)
            {
                PieChartSeries.Clear();
                CreatePieSeriesNoData(1);
               return;
            }

            if(successValueCount != 0 || warningValueCount != 0 || alertValueCount != 0)
            {
                if(_pieChartNoDataValue != null)
                {
                    PieChartSeries.Clear();
                    _pieChartNoDataValue = null;
                }
               
            }
            else
            {
               
            }

            if (successValueCount > 0)
            {
                if(_pieChartSuccessValue==null)
                {
                    CreatePieSeriesSuccess(successValueCount);
                }

                _pieChartSuccessValue.Value = successValueCount;
            }
            else
            {
               
            }

            if(warningValueCount>0)
            {
                if(_pieChartWarningValue==null)
                {
                    CreatePieSeriesWarning(warningValueCount);
                }

                _pieChartWarningValue.Value = warningValueCount;
            }
            else
            {
                
            }
            

            if(alertValueCount>0)
            {
                if(_pieChartAlertValue==null)
                {
                  CreratePieSeriesAlert(alertValueCount); 
                }

                _pieChartAlertValue.Value = alertValueCount;
            }
            else
            {
                
            }
        }
        private void PieChartSetValues()
        {
            var successValueCount = NodeGroup.Nodes.Where(t =>t.IsInPingerQueue==true&&t.PingResultData.PingStatus == PingStatus.Green).Count();
            var warningValueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true && t.PingResultData.PingStatus == PingStatus.Yellow).Count();
            var alertValueCount = NodeGroup.Nodes.Where(t => t.IsInPingerQueue == true && t.PingResultData.PingStatus == PingStatus.Red).Count();

            if(successValueCount==0&& warningValueCount==0&& alertValueCount==0)
            {
                PieChartSeries.Clear();
                CreatePieSeriesNoData(1);
            }
            else
            {
                PieChartSeries.Clear();
                if(successValueCount!=0)
                {
                  CreatePieSeriesSuccess(successValueCount);
                }

                if (warningValueCount != 0)
                {
                  CreatePieSeriesWarning(warningValueCount);
                }

                if (alertValueCount != 0)
                {
                   CreratePieSeriesAlert(alertValueCount);
                }
            }  
        }

        private void CreatePieSeriesNoData(int value)
        {
            _pieChartNoDataValue = new ObservableValue(value);

            var pieSerie = new PieSeries()
            {
                Title = "No data",
                Values = new ChartValues<ObservableValue> {_pieChartNoDataValue},
                DataLabels = false,
                Fill = new SolidColorBrush(_successColorPieChartFill),
                Stroke = new SolidColorBrush(_successColorPieChartFill),
                Name = "NoDataSeries"
            };

            PieChartSeries.Add(pieSerie);
        }

        private void CreratePieSeriesAlert(int value)
        {
            _pieChartAlertValue = new ObservableValue(value);

            var pieSerie = new PieSeries()
            {
                Title = "Alert status nodes",
                Values = new ChartValues<ObservableValue> {_pieChartAlertValue},
                DataLabels = true,
                Fill = new SolidColorBrush(_alertColorPieChartFill),
                Stroke = new SolidColorBrush(_alertColorPieChartFill),
                PushOut = 5,
                Name = "AlertSeries"
            };

            PieChartSeries.Add(pieSerie);
        }

        private void CreatePieSeriesWarning(int value)
        {
            _pieChartWarningValue = new ObservableValue(value);
            var pieSerie = new PieSeries()
            {
                Title = "Warning status nodes",
                Values = new ChartValues<ObservableValue> {_pieChartWarningValue},
                DataLabels = true,
                Fill = new SolidColorBrush(_warningColorPieChartFill),
                Stroke = new SolidColorBrush(_warningColorPieChartFill),
                PushOut = 10,
                Name = "WarningSeries"
            };
            PieChartSeries.Add(pieSerie);
        }

        private void CreatePieSeriesSuccess(int value)
        {
            _pieChartSuccessValue = new ObservableValue(value);

            var pieSerie = new PieSeries()
            {
                Title = "Success status nodes",
                Values = new ChartValues<ObservableValue> {_pieChartSuccessValue},
                DataLabels = true,
                Fill = new SolidColorBrush(_successColorPieChartFill),
                Stroke = new SolidColorBrush(_successColorPieChartFill),
                Name = "SuccessSeries"
            };
            PieChartSeries.Add(pieSerie);
        }
        #endregion

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
            var nodeNames = _NodeGroups.SelectMany(t => t.Nodes.Select(n => n.NodeDataViewModel.NodeName)).ToList();
            dialogParameters.Add("NodeNames", nodeNames);
            
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
