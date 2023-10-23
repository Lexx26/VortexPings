using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    internal class Node
    {
        public int Id { get; set; }
        public string NodeName { get; set; }
        public int PingRepeatTime { get; set; }
        public string HostOrIPadress { get; set; }
        public int GroupID { get; set; }

        public int PackageSize { get; set; }
        
        public int TTL { get; set; }

        public bool DontFragment { get; set; }

        public int? TimeOut { get; set; }
        public int? WarningTime { get; set; }

    }
}
