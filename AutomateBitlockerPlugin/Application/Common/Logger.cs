using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Common {
    public class Logger {
        public static void Log(object host, string message) {
            if (host is IControlCenter) {
                ((IControlCenter)host).LogMessage(message);
            }
            else if (host is IServiceHost4) {
                ((IServiceHost4)host).LogEvent(message, true);
            }
            else {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }
    }
}
