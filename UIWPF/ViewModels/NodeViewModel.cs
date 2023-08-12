using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    public class NodeViewModel : BindableBase, IDisposable
    {
        private NodeDataViewModel _nodeDataViewModel;
        public NodeDataViewModel? NodeDataViewModel
        {
            get { return _nodeDataViewModel; }
            set { SetProperty(ref _nodeDataViewModel, value); NodeModel.NodeData = _nodeDataViewModel.NodeDataModel; }
        }

        private PingResultDataViewModel _pingResultData;
        public PingResultDataViewModel? PingResultData
        {
            get { return _pingResultData; }
            private set { SetProperty(ref _pingResultData, value); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        public bool IsInPingerQueue
        {
            get { return NodeModel.IsInPingerQueue; }
        }

        public Node NodeModel { get; private set; }
        public NodeViewModel(Node node)
        {
            NodeModel = node;
            Order = node.Order;
            NodeDataViewModel = new NodeDataViewModel(node.NodeData);
            PingResultData = new PingResultDataViewModel(node.PingResultData);
            NodeModel.PingResultDataUpdated += _Node_PingResultDataUpdated;
            NodeModel.IsInPingerQueueChanged += NodeModel_IsInPingerQueueChanged;
        }

        private void NodeModel_IsInPingerQueueChanged()
        {
            RaisePropertyChanged(nameof(IsInPingerQueue));
        }

        private readonly object pingResultLock = new object();
        private void _Node_PingResultDataUpdated()
        {

            RaisePropertyChanged(nameof(PingResultData));

        }

        public void Dispose()
        {
            if (NodeModel != null)
            {
                NodeModel.PingResultDataUpdated -= _Node_PingResultDataUpdated;
                NodeModel.IsInPingerQueueChanged -= NodeModel_IsInPingerQueueChanged;
                NodeModel.Dispose();
            }

        }
    }
}
