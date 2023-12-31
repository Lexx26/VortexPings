﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.Models
{
    public class PingResultData
    {
        public string? ResponseAdress { get; set; }

        public long? LastRoundTripTime { get; set; }
        public PingStatus PingStatus { get; set; }
        public string? PingResult { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
