using System;
using System.IO;
using System.Linq;
using System.Threading;
using AutomateBitlockerPlugin.Properties;
using Microsoft.GroupPolicy;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent {
    public class GPOHelper {

        private string _upn;
        private string _domain;
        private const string GPONAME = "AD Bitlocker Backup";

        public GPOHelper() {
            var admins = PowershellCommand.FindDomainAdminUsers();
            if(admins.Count > 0) {
                _domain = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain().Name;
                _upn = String.Format("{0}@{1}", admins.First(), _domain);
            }
        }

        public bool HasBitlockerGPO() {
            using (new SuperImpersonate(_upn)) {
                try {
                    GPDomain domain = new GPDomain(_domain, Environment.MachineName);
                    var gpo = domain.GetGpo(GPONAME);
                    if (gpo != null)
                        return true;
                }
                catch { }

                return false;
            }
        }

        public Gpo CreateBitlockerGPO(StarterGpo bitlockerStarterGPO) {
            var gpo = CreateGPO(GPONAME, bitlockerStarterGPO);
            return gpo;
        }

        public StarterGpo ImportBitlockerStarterGPO() {
            //write bitlocker starter gpo to file
            var fileLocation = @"C:\Windows\Temp\ADBitlockerBackup.cab";
            File.WriteAllBytes(fileLocation, Resources.ADBitlockerBackup);
            //run import
            ImportStarterGPO(fileLocation);
            var starterGpo = GetStarterGPO(GPONAME);
            return starterGpo;
        }

        public void LinkBitlockerGPO(Gpo gpo) {
            using (new SuperImpersonate(_upn)) {
                GPDomain domain = new GPDomain(_domain, Environment.MachineName);
                var split = _domain.Split('.');
                var path = $"dc={split[0]},dc={split[1]}";
                var som = domain.GetSom(path);
                som.LinkGpo(1, gpo);
            }
        }

        private StarterGpo GetStarterGPO(string name) {
            using (new SuperImpersonate(_upn)) {
                GPDomain domain = new GPDomain(_domain, Environment.MachineName);
                return domain.GetStarterGpo(name);
            }
        }

        /// <summary>
        /// Creates GPO based on the starter GPO
        /// </summary>
        /// <param name="name">The name of the GPO</param>
        /// <param name="starterGpo">The starter GPO object</param>
        private Gpo CreateGPO(string name, StarterGpo starterGpo) {
            using (new SuperImpersonate(_upn)) {
                GPDomain domain = new GPDomain(_domain, Environment.MachineName);
                Gpo gpo = domain.CreateGpo(name, starterGpo);
                return gpo;
            }
        }

        /// <summary>
        /// Creates blank GPO with specified name.
        /// </summary>
        /// <param name="name">Name of the GPO</param>
        private Gpo CreateGPO(string name) {
            using (new SuperImpersonate(_upn)) {
                GPDomain domain = new GPDomain(_domain, Environment.MachineName);
                Gpo gpo = domain.CreateGpo(name);
                return gpo;
            }
        }

        /// <summary>
        /// Imports the starter GPO cab file from the specified location
        /// </summary>
        /// <param name="location">File location on the local system drive .cab file</param>
        /// <returns>True of created, false if not</returns>
        private void ImportStarterGPO(string location) {
            GPDomain domain = new GPDomain(_domain, Environment.MachineName);
            try {
                domain.ImportStarterGpo(location, true);
            }
            catch {
                //this is thrown no matter what, still imports the file
            }
        }

        public bool CheckForGPTools() {
            try {
                GPDomain domain = new GPDomain(_domain, Environment.MachineName);
                return true;
            }
            catch(Exception ex) {
                EventLogHelper.WriteLog($"Error with domain tools: {ex.Message}");
                return false;
            }
        }
    }
}
