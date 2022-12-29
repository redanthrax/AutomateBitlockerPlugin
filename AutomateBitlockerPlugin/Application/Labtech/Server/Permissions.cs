using AutomateBitlockerPlugin.Domain.Constants;
using LabTech.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech.Server {
    public class Permissions : IPermissions {
        public string Name { get { return PluginConst.Name; } }

        private IControlCenter controlCenterHost;

        // IsSuperAdmin is a Boolean value that indicates the user's Super Admin status.
        // UserClasses is a permissions string that can be used in conjunction with the IControlCenter2 interface's UserSystemAccess function to determine the users current access levels.
        // the different permissions available are SELECT,INSERT,UPDATE & DELETE
        public Hashtable GetPermissionSet(int userId, bool isSuperAdmin, string userClasses) {
            try {
                Hashtable permissionsTable = new Hashtable();
                permissionsTable.Add((object)PluginConst.BitlockerTPMTable, (object) "ALL");
                permissionsTable.Add((object)PluginConst.BitlockerHistoryTable, (object)"ALL");
                permissionsTable.Add((object)PluginConst.BitlockerLocationTable, (object)"ALL");
                permissionsTable.Add((object)"userclasspluginpermissions", (object)"SELECT");
                permissionsTable.Add((object)"cacheactions", (object)"INSERT");
                permissionsTable.Add((object)"h_users", (object)"INSERT");
                permissionsTable.Add((object)"auditactions", (object)"SELECT");
                return permissionsTable;
            }
            catch (Exception ex) {
                controlCenterHost.LogMessage(string.Format("Plugin {0} commited an error in {1} {2}", Name, ex.TargetSite.Name, ex.Message));
                return null;
            }
        }

        public void Initialize(IControlCenter host) {
            controlCenterHost = host;
        }

        public void Decommision() {
            controlCenterHost = null;
        }
    }
}
