using AutomateBitlockerPlugin.Application.Common;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent {
    public class SvcCommand : ISvcCommand {
        public string Name { get { return PluginConst.Name; } }

        public int CommandNumber { get { return PluginConst.EncryptCommandNumber; } }

        private IServiceHost4 _serviceHost;

        /// <summary>
        /// When the command number is received from the server execute this method.
        /// </summary>
        /// <param name="parameters">string parameters received from the server</param>
        /// <param name="errorLevel"></param>
        /// <returns>string</returns>
        public string ExecuteCommand(string parameters, ref int errorLevel) {

            switch(parameters)
            {
                case BitlockerConst.Parameters.Encrypt:
                    return BitlockerEncrypt(ref errorLevel);
                case BitlockerConst.Parameters.Decrypt:
                    return BitlockerDecrypt(ref errorLevel);
                case BitlockerConst.Parameters.GetData:
                    return BitlockerGetData(ref errorLevel);
                case BitlockerConst.Parameters.CheckBitlockerGPO:
                    return BitlockerCheckBitlockerGPO(ref errorLevel);
                case BitlockerConst.Parameters.DeployBitlockerGPO:
                    return DeployBitlockerGPO(ref errorLevel);
            }

            return "Missing parameters.";
        }

        public void Initialize(IServiceHost2 host) {
            _serviceHost = (IServiceHost4)host;
        }

        public object MessageFromTray(int consoleNumber, object parameters) {
            return null;
        }

        public void Decommision() {
            
        }

        private string BitlockerEncrypt(ref int errorLevel)
        {
            //check to see if drive unencrypted
            var btpm = PowershellCommand.GetBitlockerTPMData();
            if(btpm.ProtectionStatus == BitlockerConst.Off &&
                btpm.VolumeStatus != BitlockerConst.EncryptionInProgress) {

                PowershellCommand.EnableBitlocker();
                errorLevel = 0;
                return "Started Bitlocker encryption.";
            }
            else {
                return "Bitlocker volume not fully decrypted.";
            }
        }

        private string BitlockerDecrypt(ref int errorLevel)
        {
            //check to see if the drive is encrypted.
            var btpm = PowershellCommand.GetBitlockerTPMData();
            if(btpm.VolumeStatus == BitlockerConst.FullyEncrypted) {
                PowershellCommand.DisableBitlocker();
                errorLevel = 0;
                return "Started removing Bitlocker encryption.";
            }
            else {
                return "Bitlocker volume not fully encrypted.";
            }
        }

        private string BitlockerGetData(ref int errorLevel)
        {
            try {
                var status = new NameValueCollection();
                var btpm = PowershellCommand.GetBitlockerTPMData();
                status.Add(
                    ObjConvert.ToCollection(new List<object> {
                    btpm
                    })
                );

                //EventLogHelper.WriteLog("Sending TPM and Bitlocker data to Automate host.");

                _serviceHost.IGatherProcessSendData(status, PluginConst.GatherConfigNumber);

                errorLevel = 0;

                return "Gathered data.";
            }
            catch(Exception ex) {
                return ex.Message;
            }
        }

        private string BitlockerCheckBitlockerGPO(ref int errorLevel) {
            if(PowershellCommand.IsDomainController()) {
                //scan gpos for bitlocker settings
                var gpoHelper = new GPOHelper();
                if(gpoHelper.HasBitlockerGPO()) {
                    return GPOConst.HASGPO;
                }
                else {
                    return GPOConst.NOGPO;
                }
                
            }
            else {
                return GPOConst.NODC;
            }
        }

        private string DeployBitlockerGPO(ref int errorLevel) {
            if(PowershellCommand.IsDomainController()) {
                var gpoHelper = new GPOHelper();
                if(!gpoHelper.CheckForGPTools()) {
                    PowershellCommand.AddGroupPolicyWindowsFeature();
                }

                //import starter gpo
                var bitlockerStarterGpo = gpoHelper.ImportBitlockerStarterGPO();
                //create gpo from starter gpo
                var gpo = gpoHelper.CreateBitlockerGPO(bitlockerStarterGpo);
                //link gpo and enforce
                gpoHelper.LinkBitlockerGPO(gpo);
                return GPOConst.GPODEPLOY;
            }
            else {
                return GPOConst.NODC;
            }
        }
    }
}
