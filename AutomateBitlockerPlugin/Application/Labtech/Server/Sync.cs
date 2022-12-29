using AutomateBitlockerPlugin.Application.Common;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Infrastructure.Persistence.Migrations;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech {
    public class Sync : ISync {

        public string Name { get { return PluginConst.Name; }}

        /// <summary>
        /// This runs every time the db agent starts.
        /// </summary>
        /// <param name="host">IControlCenter Instance passed in from Automate, use this to access the DB</param>
        public void Initialize(IControlCenter host) {
            try {
                Logger.Log(host, "Making sure tables are setup.");
                SetupTables.Run((IDatabaseAccess)host);
                Logger.Log(host, "Database setup.");
            }
            catch(Exception ex) {
                Logger.Log(host, "Error during setup " + ex.Message);
            }
        }

        /// <summary>
        /// This process runs every 24 hours on the server.
        /// </summary>
        public void Syncronize() {
            //check for locations for encrypt settings, encrypt tpm machines that aren't encrypted

        }

        public void Decommision() {
        }
    }
}