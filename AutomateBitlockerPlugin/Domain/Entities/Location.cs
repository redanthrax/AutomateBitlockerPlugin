using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Entities {
    public class Location {
        public int LocationID { get; set; }
        public bool Encrypt { get; set; }
        public string DomainControllers { get; set; }
        public bool HasBitlockerGPO { get; set; }
    }
}
