using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Constants {
    public class PluginConst {
        public const string Name = "Automate Bitlocker Plugin";
        public const string Author = "Gary Gagnon";
        public const int Version = 1;
        public const string About = "An Automate plugin to manage Bitlocker and TPM info.";
        public const string Filename = "AutomateBitlockerPlugin.dll";
        public const int ConfigurationNumber = 66696;
        public const string BitlockerTPMTable = "plugin_bitlockertpmplugin_data";
        public const string BitlockerLocationTable = "plugin_bitlockertpmplugin_locations";
        public const string BitlockerHistoryTable = "plugin_bitlockertpmplugin_history";
        public const int GatherConfigNumber = ConfigurationNumber + 1;
        public const int EncryptCommandNumber = GatherConfigNumber + 1;
        public const string Guid = "b8a650c2-a593-45d4-bff5-5c82b1103b54";
    }
}
