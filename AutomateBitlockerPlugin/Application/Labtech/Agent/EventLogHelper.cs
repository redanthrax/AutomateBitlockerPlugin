using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent {
    public class EventLogHelper {
        public static void WriteLog(string message) {
            using (EventLog eventLog = new EventLog("Application")) {
                eventLog.Source = "BitlockerPlugin";
                eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
        }

        public static void WriteQuery(ParameterizedQuery query)
        {

        }
    }
}
