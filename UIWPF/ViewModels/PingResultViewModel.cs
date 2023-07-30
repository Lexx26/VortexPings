using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexPings.Models;

namespace UIWPF.ViewModels
{
    public class PingResultDataViewModel:BindableBase
    {
        public PingResultData PingResultDataModel;

        public PingResultDataViewModel(PingResultData pingResultData)
        {
            PingResultDataModel = pingResultData;
        }

        public string? ResponseAdress
        {
            get { return PingResultDataModel.ResponseAdress; }
            set
            {
                if (PingResultDataModel.ResponseAdress != value)
                {
                    PingResultDataModel.ResponseAdress = value;
                    RaisePropertyChanged(nameof(ResponseAdress));
                }
            }
        }

        public long? LastRoundTripTime
        {
            get { return PingResultDataModel.LastRoundTripTime; }
            set
            {
                if (PingResultDataModel.LastRoundTripTime != value)
                {
                    PingResultDataModel.LastRoundTripTime = value;
                    RaisePropertyChanged(nameof(LastRoundTripTime));
                }
            }
        }

        public PingStatus PingStatus
        {
            get { return PingResultDataModel.PingStatus; }
            set
            {
                if (PingResultDataModel.PingStatus != value)
                {
                    PingResultDataModel.PingStatus = value;
                    RaisePropertyChanged(nameof(PingStatus));
                }
            }
        }

        public string? PingResult
        {
            get { return PingResultDataModel.PingResult; }
            set
            {
                if (PingResultDataModel.PingResult != value)
                {
                    PingResultDataModel.PingResult = value;
                    RaisePropertyChanged(nameof(PingResult));
                }
            }
        }
    }
}
