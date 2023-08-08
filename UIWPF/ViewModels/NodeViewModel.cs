using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    public class NodeViewModel:BindableBase,IDisposable
    {
        private NodeDataViewModel _nodeDataViewModel;
        public NodeDataViewModel? NodeDataViewModel
        { 
            get { return _nodeDataViewModel;} 
            private set { SetProperty(ref _nodeDataViewModel, value); } }

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
            set { SetProperty(ref _order, value);}
        }

        public Node _node;
        public NodeViewModel(Node node)
        {
            _node = node;
            Order = node.Order;
            NodeDataViewModel = new NodeDataViewModel(node.NodeData);
            PingResultData = new PingResultDataViewModel(node.PingResultData);
            _node.PingResultDataUpdated += _Node_PingResultDataUpdated;
        }

        private void _Node_PingResultDataUpdated()
        {
            RaisePropertyChanged(nameof(PingResultData));
        }

        public void Dispose()
        {
            if (_node != null)
            {
                _node.PingResultDataUpdated -= _Node_PingResultDataUpdated;
                _node.Dispose();
            }
              
        }
    }
}
