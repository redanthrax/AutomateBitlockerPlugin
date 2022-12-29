using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using Microsoft.BitLocker.Structures;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent {
    public class PowershellCommand {
        public static BitlockerTPM GetBitlockerTPMData() {
            //EventLogHelper.WriteLog("Gathering Bitlocker and TPM data via Powershell.");
            var ps = PowerShell.Create().AddCommand("Get-TPM");
            var btpm = new BitlockerTPM();
            try {
                Collection<PSObject> result = ps.Invoke();
                foreach (var item in result) {
                    btpm.TPMPresent = (bool)item.Properties["TpmPresent"].Value;
                    btpm.TPMReady = (bool)item.Properties["TpmReady"].Value;
                    if (btpm.TPMPresent) {
                        btpm.TpmManagedAuthLevel = item.Properties["ManagedAuthLevel"].Value.ToString();
                        btpm.TpmManagedAuthLevel = item.Properties["AutoProvisioning"].Value.ToString();
                    }
                }
            }
            catch (Exception ex) {
                EventLogHelper.WriteLog(String.Format("Error gathering TPM data: {0}", ex.Message));
            }

            ps = PowerShell.Create().AddCommand("Get-BitlockerVolume");

            try {
                Collection<PSObject> result = ps.Invoke();
                foreach (var bitlockerObject in result) {
                    if (bitlockerObject.Properties["VolumeType"].Value.ToString().Contains("OperatingSystem")) {
                        var systemDrive = bitlockerObject;
                        btpm.VolumeType = systemDrive.Properties["VolumeType"].Value.ToString();
                        btpm.MountPoint = systemDrive.Properties["MountPoint"].Value.ToString();
                        btpm.CapacityGB = systemDrive.Properties["CapacityGB"].Value.ToString();
                        btpm.VolumeStatus = systemDrive.Properties["VolumeStatus"].Value.ToString();
                        btpm.EncryptionPercentage = systemDrive.Properties["EncryptionPercentage"].Value.ToString();
                        btpm.ProtectionStatus = systemDrive.Properties["ProtectionStatus"].Value.ToString();
                        //Get KeyProtector Data
                        var keyProtector = systemDrive.Properties["KeyProtector"];
                        var protectorList = (ReadOnlyCollection<BitLockerVolumeKeyProtector>)keyProtector.Value;
                        string keyProtectorNames = string.Empty;
                        foreach (var property in protectorList) {
                            keyProtectorNames = String.Format("{0} {1}", keyProtectorNames, property.KeyProtectorType);
                            if (property.KeyProtectorType.ToString() == "RecoveryPassword")
                                btpm.RecoveryKey = property.RecoveryPassword;
                        }

                        btpm.RecoveryKey = btpm.RecoveryKey == null ? String.Empty : btpm.RecoveryKey;
                        btpm.KeyProtector = keyProtectorNames;
                    }
                    else {
                        EventLogHelper.WriteLog("Could not find Operating System drive.");
                    }
                }
            }
            catch (Exception ex) {
                EventLogHelper.WriteLog(String.Format("Error gathering Bitlocker data: {0}", ex.Message));
            }

            try {
                btpm.IsDomainController = IsDomainController();
            }
            catch (Exception ex) {
                EventLogHelper.WriteLog(String.Format("Error detecting Domain Controller: {0}", ex.Message));
            }

            return btpm;
        }

        public static void EnableBitlocker() {
            //EventLogHelper.WriteLog("Starting C: drive encryption via Powershell.");
            var btpm = GetBitlockerTPMData();

            if (btpm.TPMPresent && btpm.TPMReady) {
                if (btpm.TPMReady) {
                    if (!btpm.KeyProtector.Contains("Tpm")) {
                        var ps = PowerShell.Create().AddCommand("Enable-Bitlocker");
                        ps.AddParameter("MountPoint", btpm.MountPoint);
                        ps.AddParameter("SkipHardwareTest");
                        ps.AddParameter("TpmProtector");
                        Collection<PSObject> result = ps.Invoke();

                        if (!btpm.KeyProtector.Contains("RecoveryPassword")) {
                            ps = PowerShell.Create().AddCommand("Add-BitLockerKeyProtector");
                            ps.AddParameter("MountPoint", btpm.MountPoint);
                            ps.AddParameter("RecoveryPasswordProtector");
                            result = ps.Invoke();
                        }
                    }
                    if (btpm.ProtectionStatus == BitlockerConst.Off) {
                        var ps = PowerShell.Create().AddCommand("Resume-Bitlocker");
                        ps.AddParameter("MountPoint", btpm.MountPoint);
                        Collection<PSObject> result = ps.Invoke();
                    }
                }
                else {
                    //TODO: TPM Not ready. Take steps to fix this on the client
                    EventLogHelper.WriteLog("TPM not in a ready state, unable to encrypt.");
                }
            }
            else {
                //TODO: Setup bitlocker for machines without TPM - password etc.
                EventLogHelper.WriteLog("No TPM available to use.");
            }
        }

        public static void DisableBitlocker() {
            var bitlocker = GetBitlockerTPMData();
            //EventLogHelper.WriteLog("Disabling encryption via Powershell.");
            var ps = PowerShell.Create().AddCommand("Disable-BitLocker");
            ps.AddParameter("MountPoint", bitlocker.MountPoint);
            Collection<PSObject> result = ps.Invoke();
            //var resultData = result.First();
        }

        /// <summary>
        /// Use powershell to determine if current computer is a domain controller.
        /// </summary>
        /// <returns>true if domain controller</returns>
        public static bool IsDomainController() {
            var ps = PowerShell.Create().AddCommand("Get-CimInstance");
            ps.AddParameter("ClassName", "Win32_OperatingSystem");
            var result = ps.Invoke();
            foreach (var item in result) {
                if (Convert.ToInt32(item.Properties["ProductType"].Value) == 2)
                    return true;
            }

            return false;
        }

        public static void CreateGPO(string name) {
            var ps = PowerShell.Create().AddCommand("New-GPO");
            ps.AddParameter("Name", name);
            ps.Invoke();
        }

        public static void BackupGPO(string name, string path) {
            var ps = PowerShell.Create().AddCommand("New-Item");
            ps.AddParameter("ItemType", "Directory");
            ps.AddParameter("Path", path);
            ps.AddParameter("Force");
            ps.Invoke();
            ps = PowerShell.Create().AddCommand("Backup-GPO");
            ps.AddParameter("Name", name);
            ps.AddParameter("Path", path);
            ps.Invoke();
        }

        public static void RemoveGPO(string name) {
            var ps = PowerShell.Create().AddCommand("Remove-GPO");
            ps.AddParameter("Name", name);
            ps.Invoke();
        }

        public static void RestoreGPO(string path) {
            //get guid from directory foldername
            var ps = PowerShell.Create().AddCommand("Get-ChildItem");
            ps.AddParameter("Path", path);
            var result = ps.Invoke();
            var foldername = string.Empty;
            foreach (var dir in result) {
                foldername = dir.Properties["Name"].ToString();
            }

            if (!string.IsNullOrEmpty(foldername)) {
                ps = PowerShell.Create().AddCommand("Restore-GPO");
                ps.AddParameter("BackupId", foldername);
                ps.AddParameter("Path", path);
            }
        }

        public static void RemoveDir(string path) {
            var ps = PowerShell.Create().AddCommand("Remove-Item");
            ps.AddParameter("Path", path);
            ps.AddParameter("Recurse");
            ps.AddParameter("Force");
            ps.Invoke();
        }

        public static void LinkGPO(string name) {
            var ps = PowerShell.Create().AddCommand("New-GPLink");
            ps.AddParameter("Name", name);
            var root = GetADDomain();
            ps.AddParameter("Target", root);
            ps.AddParameter("Enforced", "Yes");
            ps.Invoke();
        }

        private static string GetADDomain() {
            var ps = PowerShell.Create().AddCommand("Get-ADDomain");
            var result = ps.Invoke();
            foreach (var item in result) {
                return item.Properties["DistinguishedName"].ToString();
            }

            return string.Empty;
        }

        public static List<string> FindDomainAdminUsers() {
            var ps = PowerShell.Create().AddCommand("get-adgroupmember");
            ps.AddParameter("Identity", "Domain Admins");
            var psResult = ps.Invoke();
            var adminlist = new List<string>();
            foreach (var item in psResult) {
                //Check if account disabled
                ps = PowerShell.Create().AddCommand("get-aduser");
                ps.AddParameter("Identity", item.Properties["SamAccountName"].Value.ToString());
                var userResult = ps.Invoke();
                foreach (var userItem in userResult) {
                    if ((bool)userItem.Properties["Enabled"].Value) {
                        adminlist.Add(item.Properties["SamAccountName"].Value.ToString().ToLower());
                    }
                }
            }

            return adminlist;
        }

        public static void AddGroupPolicyWindowsFeature() {
            var ps = PowerShell.Create().AddCommand("Install-WindowsFeature");
            ps.AddParameter("Name", "GPMC");
            ps.Invoke();
        }

        public static void BitlockerBackup() {
            var ps = PowerShell.Create().AddCommand("Get-BitlockerVolume");
            Collection<PSObject> result = ps.Invoke();
            foreach (var bitlockerObject in result) {
                var operatingSystem = (BitLockerVolume)bitlockerObject.BaseObject;
                if (operatingSystem.VolumeType == BitLockerVolumeType.OperatingSystem) {
                    foreach(var protector in operatingSystem.KeyProtector) {
                        if (protector.KeyProtectorType == BitLockerVolumeKeyProtectorType.RecoveryPassword) {
                            ps = PowerShell.Create().AddCommand("Backup-BitLockerKeyProtector");
                            ps.AddParameter("MountPoint", operatingSystem.MountPoint);
                            ps.AddParameter("KeyProtectorId", protector.KeyProtectorId);
                            ps.Invoke();
                        }
                    }
                }
            }
        }
    }
}
