using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    internal class GroupDb
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public List<Node>? Nodes { get; set; }
    }
}
