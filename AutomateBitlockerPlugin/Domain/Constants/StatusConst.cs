using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Constants {
    public class StatusConst {
        public const int WaitingToSend = 0;
        public const int Sent = 1;
        public const int Executing = 2;
        public const int Success = 3;
        public const int Error = 4;
    }
}
