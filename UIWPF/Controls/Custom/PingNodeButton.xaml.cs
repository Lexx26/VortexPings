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

        public PingNodeButton(NodeViewModel nodeViewModel)
        {
            NodeViewModel = nodeViewModel;
            InitializeComponent();
        }

        private void BindData()
        {

        }

        private void Unbind()
        {

        }

       public void DestroyControl()
        {

        }
    }
}
