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
        public PingResultData PingResultDataModel { get; private set; }

        public PingResultDataViewModel(PingResultData pingResultData)
        {
            if (pingResultData != null)
                PingResultDataModel = pingResultData;
            else
                PingResultDataModel = new PingResultData();
        }

        public string? ResponseAdress
        {
            get { return PingResultDataModel.ResponseAdress; }
        }

        public long? LastRoundTripTime
        {
            get { return PingResultDataModel.LastRoundTripTime; }
        }

        public DateTime? DateTime
        {
            get { return PingResultDataModel.DateTime; }
        }

        public PingStatus PingStatus
        {
            get { return PingResultDataModel.PingStatus; }
        }

        public string? PingResult
        {
            get { return PingResultDataModel.PingResult; }
        }
    }
}
