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
        private PingResultData _pingResultDataModel;

        public PingResultDataViewModel(PingResultData pingResultData)
        {
            _pingResultDataModel = pingResultData;
        }

        public string? ResponseAdress
        {
            get { return _pingResultDataModel.ResponseAdress; }
            set
            {
                if (_pingResultDataModel.ResponseAdress != value)
                {
                    _pingResultDataModel.ResponseAdress = value;
                    RaisePropertyChanged(nameof(ResponseAdress));
                }
            }
        }

        public long? LastRoundTripTime
        {
            get { return _pingResultDataModel.LastRoundTripTime; }
            set
            {
                if (_pingResultDataModel.LastRoundTripTime != value)
                {
                    _pingResultDataModel.LastRoundTripTime = value;
                    RaisePropertyChanged(nameof(LastRoundTripTime));
                }
            }
        }

        public PingStatus PingStatus
        {
            get { return _pingResultDataModel.PingStatus; }
            set
            {
                if (_pingResultDataModel.PingStatus != value)
                {
                    _pingResultDataModel.PingStatus = value;
                    RaisePropertyChanged(nameof(PingStatus));
                }
            }
        }

        public string? PingResult
        {
            get { return _pingResultDataModel.PingResult; }
            set
            {
                if (_pingResultDataModel.PingResult != value)
                {
                    _pingResultDataModel.PingResult = value;
                    RaisePropertyChanged(nameof(PingResult));
                }
            }
        }
    }
}
