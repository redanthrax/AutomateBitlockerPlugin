using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Entities {
    public class History {
        public DateTime Created { get; set; }
        public string Action { get; set; }
    }
}
