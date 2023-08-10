using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.Models
{
    internal class PingReplyResult
    {

        public PingReplyResult(string iPStatus, IPAddress iPaddress, long rountripTime)
        {
            Status = iPStatus;
            Address = iPaddress;
            RoundtripTime = rountripTime;
        }

        public PingReplyResult(PingReply pingReply)
        {
            Status = pingReply.Status.ToString();
            Address = pingReply.Address;
            RoundtripTime = pingReply.RoundtripTime;
        }
        public string Status { get; }

        public IPAddress Address { get; }

        public long RoundtripTime { get; }

      
    }
}
