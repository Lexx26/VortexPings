using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UIWPF.Controls.Custom;
using UIWPF.ViewModels;
using VortexPings.Models;

namespace UIWPF.Controls.Models
{
    public class PingGroupControlData
    {
        public NodeGroupViewModel NodeGroupViewModel { get; private set; }
        public PingGroupControlData(NodeGroupViewModel nodeGroupViewModel, PingGroupPanel parentPingGroupPanel, UIElement controlToStorePingNodeButtons)
        {
            NodeGroupViewModel = nodeGroupViewModel;
            ParentPingGroupPanel = parentPingGroupPanel;
            ControlToStorePingNodeButtons = controlToStorePingNodeButtons;
        }

        public PingGroupPanel ParentPingGroupPanel { get; private set; }

        public PingGroupButton PingGroupButtonControl { get; set; }

        public UIElement ControlToStorePingNodeButtons { get; private set; }

        public List<PingNodeButton> PingNodeButtons { get; set; }
    }
}
