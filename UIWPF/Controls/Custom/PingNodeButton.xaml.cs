using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIWPF.ViewModels;
using VortexPings.Models;

namespace UIWPF.Controls.Custom
{
    /// <summary>
    /// Interaction logic for PingNodeButton.xaml
    /// </summary>
    public partial class PingNodeButton : UserControl
    {
        private NodeViewModel _nodeViewModel;

        public NodeViewModel NodeViewModel { get { return _nodeViewModel; } private set { _nodeViewModel = value; } }

        public PingGroupPanel ParentPingGroupPanel { get; private set; }

        private Style _ButtonStylePing;
        private Style _ButtonStyleNotPing;
        private Style _ButtonStylePingRed;
        private Style _ButtonStylePingYellow;
        public PingNodeButton(NodeViewModel nodeViewModel, PingGroupPanel _parentPingGroupPanel)
        {
            NodeViewModel = nodeViewModel;
            ParentPingGroupPanel = _parentPingGroupPanel;
            _ButtonStylePing = (Style)FindResource("NodePingNormal");
            _ButtonStyleNotPing = (Style)FindResource("NodeNotPing");
            _ButtonStylePingRed = (Style)FindResource("NodePingRed");
            _ButtonStylePingYellow = (Style)FindResource("NodePingYellow");
            InitializeComponent();
            BindData();

        }

        private void BindData()
        {
            var nodeNameBinding = new Binding("NodeViewModel.NodeDataViewModel.NodeName");
            nodeNameBinding.Source = this;
            nodeNameBinding.Mode = BindingMode.OneWay;

            this.NodeName.SetBinding(TextBlock.TextProperty, nodeNameBinding);

            var pingMsBinding = new Binding("NodeViewModel.PingResultData.LastRoundTripTime");
            pingMsBinding.Source = this;
            pingMsBinding.Mode = BindingMode.OneWay;

            this.PingResultMs.SetBinding(TextBlock.TextProperty, pingMsBinding);

            var pingResultBinding = new Binding("NodeViewModel.PingResultData.PingResult");
            pingResultBinding.Source = this;
            pingResultBinding.Mode = BindingMode.OneWay;

            this.PingResult.SetBinding(TextBlock.TextProperty, pingResultBinding);

            NodeViewModel.PropertyChanged += NodeViewModel_PropertyChanged;
        }

        private void NodeViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName=="IsInPingerQueue")
            {
                if(NodeViewModel.IsInPingerQueue==true)
                {
                    Dispatcher.Invoke(()=>
                    PingNodeButtonMain.Style = _ButtonStylePing);

                }
                else
                {
                    Dispatcher.Invoke(() =>
                   PingNodeButtonMain.Style = _ButtonStyleNotPing);
                }

                return;
            }

            if (e.PropertyName == "PingResultData")
            {
                if (NodeViewModel.PingResultData.PingStatus == PingStatus.Red)
                {
                    if (PingNodeButtonMain.Style != _ButtonStylePingRed)
                    {
                        Dispatcher.Invoke(() =>
                    PingNodeButtonMain.Style = _ButtonStylePingRed);
                    }
                    return;
                }

                if (NodeViewModel.PingResultData.PingStatus == PingStatus.Yellow)
                {

                    if (PingNodeButtonMain.Style != _ButtonStylePingYellow)
                    {
                        Dispatcher.Invoke(() =>
                    PingNodeButtonMain.Style = _ButtonStylePingYellow);
                    }

                    return;
                }

                if (NodeViewModel.PingResultData.PingStatus == PingStatus.Green)
                {
                    if (PingNodeButtonMain.Style != _ButtonStylePing)
                    {
                        Dispatcher.Invoke(() =>
                    PingNodeButtonMain.Style = _ButtonStylePing);
                    }
                    return; 
                }
            }
        }

        private void Unbind()
        {
            BindingOperations.ClearAllBindings(this);
            NodeViewModel.PropertyChanged -= NodeViewModel_PropertyChanged;
        }

       public void DestroyControl()
        {
            Unbind();

            _nodeViewModel = null;
            NodeViewModel = null;
            ParentPingGroupPanel = null;
            _ButtonStylePing = null;
            _ButtonStyleNotPing = null;
            _ButtonStylePingRed = null;
            _ButtonStylePingYellow = null;
        }

        private void PingNodeButtonMain_Click(object sender, RoutedEventArgs e)
        {
            ParentPingGroupPanel.ClickedNode = _nodeViewModel;
            if(ParentPingGroupPanel.NodeCommand!=null)
            ParentPingGroupPanel.NodeCommand.Execute(null);
        }
    }
}
