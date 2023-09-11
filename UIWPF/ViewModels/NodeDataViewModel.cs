using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VortexPings.Models;
using VortexPings.ValidationRules;
using ValidationResult = VortexPings.ValidationRules.ValidationResult;

namespace UIWPF.ViewModels
{
    public class NodeDataViewModel:ViewModelValidatingBase
    {
        public NodeData NodeDataModel { get; private set; }

        public NodeDataViewModel(NodeData nodeData)
        {
            NodeDataModel = nodeData;
        }

        public string? NodeName
        {
            get { return NodeDataModel.NodeName; }
            set
            {
                if (NodeDataModel.NodeName != value)
                {
                    NodeDataModel.NodeName = value;
                    RaisePropertyChanged(nameof(NodeName));
                }
            }
        }

        public int NodeId
        {
            get { return NodeDataModel.NodeId; }
            set
            {
                if (NodeDataModel.NodeId != value)
                {
                    NodeDataModel.NodeId = value;
                    RaisePropertyChanged(nameof(NodeId));
                }
            }
        }

        public int? PingRepeatTime 
        {
            get { return NodeDataModel.PingRepeatTime; }
            set
            {
                if(NodeDataModel.PingRepeatTime != value)
                {
                    NodeDataModel.PingRepeatTime = value;
                    RaisePropertyChanged(nameof(PingRepeatTime));
                }
            }
        
        }

        public string? HostOrIPaddress
        {
            get { return NodeDataModel.HostOrIPadress; }
            set
            {
                if (NodeDataModel.HostOrIPadress != value)
                {
                    NodeDataModel.HostOrIPadress = value;
                    RaisePropertyChanged(nameof(HostOrIPaddress));
                }
            }
        }

        public int GroupID
        {
            get { return NodeDataModel.GroupID; }
            set
            {
                if (NodeDataModel.GroupID != value)
                {
                    NodeDataModel.GroupID = value;
                    RaisePropertyChanged(nameof(GroupID));
                }
            }
        }

        public int? PackageSize
        {
            get { return NodeDataModel.PackageSize; }
            set
            {
                if (NodeDataModel.PackageSize != value)
                {
                        NodeDataModel.PackageSize = value;
                        RaisePropertyChanged(nameof(PackageSize));
                        RaisePropertyChanged(nameof(Buffer));
                }
            }
        }

        public byte[]? Buffer
        {
            get { return NodeDataModel.Buffer; }
        }

        public int? TTL
        {
            get { return NodeDataModel.TTL; }
            set
            {
                if (NodeDataModel.TTL != value)
                {
                    NodeDataModel.TTL = value;
                    RaisePropertyChanged(nameof(TTL));
                    RaisePropertyChanged(nameof(PingOptions));
                }
            }
        }

        public bool DontFragment
        {
            get { return NodeDataModel.DontFragment; }
            set
            {
                if (NodeDataModel.DontFragment != value)
                {
                    NodeDataModel.DontFragment = value;
                    RaisePropertyChanged(nameof(DontFragment));
                    RaisePropertyChanged(nameof(PingOptions));
                }
            }
        }

        public int? TimeOut
        {
            get { return NodeDataModel.TimeOut; }
            set
            {
                if (NodeDataModel.TimeOut != value)
                {
                    NodeDataModel.TimeOut = value;
                    RaisePropertyChanged(nameof(TimeOut));
                }
            }
        }

        public int? WarningTime
        {
            get { return NodeDataModel.WarningTime; }
            set
            {
                if (NodeDataModel.WarningTime != value)
                {
                    NodeDataModel.WarningTime = value;
                    RaisePropertyChanged(nameof(WarningTime));
                }
            }
        }

        public PingOptions? PingOptions
        {
            get { return NodeDataModel.PingOptions; }
        }

       
    }
}
