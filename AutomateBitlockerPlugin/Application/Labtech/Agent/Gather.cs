using AutomateBitlockerPlugin.Application.Common;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent {
    /// <summary>
    /// This is the process that runs on the client machine and passes
    /// the data up to IGatherProcess on the server so the DB can be updated.
    /// </summary>
    public class Gather : IGather {

        private IServiceHost4 _serviceHost;

        public string Name { get { return PluginConst.Name; } }

        /// <summary>
        /// This is the configuration number for the gather function on the client. It must match with the GatherProcess on the server.
        /// </summary>
        public int ConfigurationNumber { get { return PluginConst.GatherConfigNumber; } }

        /// <summary>
        /// This is what inventory section to schedule the GatherData option with.
        /// </summary>
        public int InventorySection { get { return InventoryConst.System; } }

        public void Initialize(IServiceHost2 host) {
            //load the bitlocker assembly on the client so we can use BitLockerVolumeKeyProtector type
            var assembly = Assembly.LoadFrom("C:\\Windows\\SysWOW64\\WindowsPowerShell\\v1.0\\Modules\\BitLocker\\Microsoft.BitLocker.Structures.dll");
            //execute the gather process on first load
            _serviceHost = (IServiceHost4)host;
            var status = new NameValueCollection();
            _serviceHost.IGatherProcessSendData(GatherData(status), this.ConfigurationNumber);
        }

        /// <summary>
        /// This process runs on the agent and does the main work, triggered from the inventory section number.
        /// </summary>
        /// <param name="status">Collection data that's added to and passed back to the server.</param>
        /// <returns>NameValueCollection status</returns>
        public NameValueCollection GatherData(NameValueCollection status) {
            //EventLogHelper.WriteLog("Gathering TPM and Bitlocker data.");
            var btpm = PowershellCommand.GetBitlockerTPMData();
            status.Add(
                ObjConvert.ToCollection(new List<object> {
                    btpm
                })
            );
            
            try {
                PowershellCommand.BitlockerBackup();
                //EventLogHelper.WriteLog("Backed up Bitlocker Recovery Password to AD if available.");
            }
            catch(Exception ex) {
                EventLogHelper.WriteLog($"Error backing up to AD {ex.Message}");
            }

            //EventLogHelper.WriteLog("Sending TPM and Bitlocker data to Automate host.");
            return status;
        }

        /// <summary>
        /// ProcessReturn is used for the post-processing of the string that's returned 
        /// from the IGatherProcess.ProcessGatheredData function.
        /// </summary>
        /// <param name="data">If the Data is numeric and greater than 0, the data has 
        /// been inserted into the database.</param>
        public void ProcessReturn(string data) {
            ///nothing to do here
        }

        public void Decommision() {
            _serviceHost = null;
        }
    }
}
