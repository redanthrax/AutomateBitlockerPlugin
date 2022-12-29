using AutomateBitlockerPlugin.Domain.Constants;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech {
    public class Plugin : IPlugin {
        public string Name { get { return PluginConst.Name; } }

        public string Author { get { return PluginConst.Author; } }

        public int Version { get { return PluginConst.Version; } }

        public string About { get { return PluginConst.About; } }

        public string hMD5 { get; set; }

        public string Filename { get { return PluginConst.Filename; } set { } }

        public string PLUGIN_GUID { get { return PluginConst.Guid; } }

        public bool Install(IControlCenter host) {
            return true;
        }

        public bool IsCompatible(IControlCenter host) {
            return true;
        }

        public bool IsLicensed(IControlCenter host) {
            return true;
        }

        public bool IsLicensed() {
            return true;
        }

        public bool Remove(IControlCenter host) {
            return true;
        }
    }
}
