using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Common {
    public class AuditableEntity {
        public int ComputerID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
