using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.Models
{
    public class NodeData:ICloneable
    {
       public string? NodeName { get; set; }
        public int NodeId { get; set; }
        public int? PingRepeatTime { get; set; }
        public string? HostOrIPadress { get; set; }
        public int GroupID { get; set; } 

        private int? _PackageSize;
        public int? PackageSize
        {
            get { return _PackageSize; }
            set
            {
                if (value == null)
                {
                    _PackageSize = value;
                    return;
                }
                  
                if (value < 28)
                {
                     CreateBuffer(28);
                }
                else
                {
                    CreateBuffer((int)value);
                }

                _PackageSize = value;
              
            }
        }

        private void CreateBuffer(int bufferSize)
        {
            string data = new string('a', bufferSize);
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            _Buffer = buffer;
        }

        private byte[]? _Buffer;
        public byte[]? Buffer { get {return _Buffer; } }

        private int? _TTL;
        public int? TTL { get { return _TTL; } set { _TTL = value; UpdatePingOption(); } }

        private bool _DontFragment;
        public bool DontFragment { get { return _DontFragment; } set { _DontFragment = value; UpdatePingOption(); } }

        private void UpdatePingOption()
        {
            if (_PingOptions == null)
            {
                _PingOptions = new PingOptions();
            }

            if (TTL == 0||TTL<0||TTL==null)
            {
                _PingOptions.DontFragment = DontFragment;
                _PingOptions.Ttl = 1;

                return;
            }
        

            _PingOptions.DontFragment = DontFragment;
            _PingOptions.Ttl = (int)_TTL;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int? TimeOut { get; set; }
        public int? WarningTime { get; set; }

        public PingOptions _PingOptions;
        public PingOptions? PingOptions { get { return _PingOptions; } }
    }
}
