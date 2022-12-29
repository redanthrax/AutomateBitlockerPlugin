using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Entities {
    public class Computer : BitlockerTPM {
        public string ComputerName { get; set; }
        public bool HasRecoveryKey { get; set; }
        public string IsOnline { get; set; }
    }
}
