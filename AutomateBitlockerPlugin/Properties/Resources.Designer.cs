﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutomateBitlockerPlugin.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AutomateBitlockerPlugin.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] ADBitlockerBackup {
            get {
                object obj = ResourceManager.GetObject("ADBitlockerBackup", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE `labtech`.`plugin_bitlockertpmplugin_data` ADD COLUMN `IsDomainController` TINYINT(1) NOT NULL AFTER `Updated`;.
        /// </summary>
        internal static string AddIsDomainController {
            get {
                return ResourceManager.GetString("AddIsDomainController", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to select count(*) from computers where computerid = @ComputerID and lastcontact &gt; (now() - interval 15 minute).
        /// </summary>
        internal static string CheckComputerOnline {
            get {
                return ResourceManager.GetString("CheckComputerOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE `labtech`.`plugin_bitlockertpmplugin_data` (
        ///  `ComputerID` INT NOT NULL,
        ///  `TpmPresent` TINYINT(1) NOT NULL,
        ///  `TpmReady` TINYINT(1) NULL,
        ///  `TpmManagedAuthLevel` VARCHAR(255) NULL,
        ///  `TpmAutoProvisioning` TINYINT(1) NULL,
        ///  `VolumeType` VARCHAR(255) NULL,
        ///  `MountPoint` VARCHAR(255) NULL,
        ///  `CapacityGB` VARCHAR(255) NULL,
        ///  `VolumeStatus` VARCHAR(255) NULL,
        ///  `EncryptionPercentage` VARCHAR(255) NULL,
        ///  `KeyProtector` VARCHAR(255) NULL,
        ///  `ProtectionStatus` VARCHAR(255) NULL, [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreatePluginBitlockerTPMTable {
            get {
                return ResourceManager.GetString("CreatePluginBitlockerTPMTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE `{0}` (
        ///`id` int(11) NOT NULL AUTO_INCREMENT,
        ///`created` datetime NOT NULL DEFAULT NOW(),
        ///`action` varchar(300) NOT NULL,
        ///`computerid` int(11) NOT NULL,
        ///`locationid` int(11) NOT NULL,
        ///PRIMARY KEY (`id`),
        ///UNIQUE KEY `id_UNIQUE` (`id`));.
        /// </summary>
        internal static string CreatePluginHistoryTable {
            get {
                return ResourceManager.GetString("CreatePluginHistoryTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE `{0}` (
        ///  `LocationID` INT NOT NULL,
        ///  `Encrypt` TINYINT(1) NOT NULL,
        ///  `DomainControllers` VARCHAR(255) NULL,
        ///  `HasBitlockerGPO` TINYINT(1) NULL,
        ///  PRIMARY KEY (`LocationID`));.
        /// </summary>
        internal static string CreatePluginLocationTable {
            get {
                return ResourceManager.GetString("CreatePluginLocationTable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `{0}` (
        ///`LocationID`,
        ///`Encrypt`,
        ///`DomainControllers`,
        ///`HasBitlockerGPO`)
        ///SELECT `LocationID`,
        ///0 AS `Encrypt`,
        ///&apos;&apos; AS `DomainControllers`,
        ///0 AS `HasBitlockerGPO`
        ///FROM locations;.
        /// </summary>
        internal static string GenerateLocationData {
            get {
                return ResourceManager.GetString("GenerateLocationData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `{0}` (
        ///`ComputerID`,
        ///`TpmPresent`, 
        ///`TpmReady`,
        ///`TpmManagedAuthLevel`,
        ///`TpmAutoProvisioning`,
        ///`VolumeType`,
        ///`MountPoint`, 
        ///`CapacityGB`, 
        ///`VolumeStatus`, 
        ///`EncryptionPercentage`, 
        ///`KeyProtector`,
        ///`ProtectionStatus`,
        ///`RecoveryKey`,
        ///`Created`,
        ///`IsDomainController`
        ///) VALUES (
        ///@ComputerID,
        ///@TpmPresent, 
        ///@TpmReady, 
        ///@TpmManagedAuthLevel,
        ///@TpmAutoProvisioning,
        ///@VolumeType,
        ///@MountPoint, 
        ///@CapacityGB, 
        ///@VolumeStatus, 
        ///@EncryptionPercentage, 
        ///@KeyProtector,
        ///@ProtectionStatus, [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InsertBitlockerTPMRecord {
            get {
                return ResourceManager.GetString("InsertBitlockerTPMRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `{0}` (
        ///`action`,
        ///`computerid`,
        ///`locationid`
        ///) VALUES (
        ///@Action,
        ///@ComputerID, 
        ///@LocationID
        ///).
        /// </summary>
        internal static string InsertHistoryRecord {
            get {
                return ResourceManager.GetString("InsertHistoryRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `{0}` (
        ///`LocationID`,
        ///`Encrypt`,
        ///`DomainControllers`,
        ///`HasBitlockerGPO`
        ///) VALUES (
        ///@LocationID,
        ///@Encrypt, 
        ///@DomainControllers, 
        ///@HasBitlockerGPO
        ///).
        /// </summary>
        internal static string InsertLocationRecord {
            get {
                return ResourceManager.GetString("InsertLocationRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM `{0}`
        ///WHERE ComputerID = @ComputerID.
        /// </summary>
        internal static string SelectBitlockerTPMRecord {
            get {
                return ResourceManager.GetString("SelectBitlockerTPMRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT created,action FROM {0}
        ///WHERE computerid = @ComputerID
        ///ORDER BY created DESC;.
        /// </summary>
        internal static string SelectHistoryRecords {
            get {
                return ResourceManager.GetString("SelectHistoryRecords", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT action FROM {0}
        ///WHERE computerid = @ComputerID
        ///ORDER BY id DESC
        ///LIMIT 1.
        /// </summary>
        internal static string SelectLastHistoryRecord {
            get {
                return ResourceManager.GetString("SelectLastHistoryRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT c.ComputerID, 
        ///c.Name, 
        ///b.ProtectionStatus, 
        ///b.KeyProtector, 
        ///b.VolumeStatus, 
        ///b.TPMPresent, 
        ///b.TPMReady, 
        ///IF(b.RecoveryKey IS NULL or b.RecoveryKey = &apos;&apos;, &quot;False&quot;, &quot;True&quot;) AS HasRecoveryKey,
        ///b.IsDomainController
        ///FROM computers c
        ///LEFT JOIN {0} b ON c.ComputerID = b.ComputerID
        ///WHERE c.ComputerID = @ComputerID;.
        /// </summary>
        internal static string SelectLocationComputer {
            get {
                return ResourceManager.GetString("SelectLocationComputer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT c.ComputerID, 
        ///c.Name, 
        ///b.ProtectionStatus, 
        ///b.KeyProtector, 
        ///b.VolumeStatus, 
        ///b.TPMPresent, 
        ///b.TPMReady, 
        ///IF(b.RecoveryKey IS NULL or b.RecoveryKey = &apos;&apos;, &quot;False&quot;, &quot;True&quot;) AS HasRecoveryKey,
        ///b.IsDomainController
        ///FROM computers c
        ///LEFT JOIN {0} b ON c.ComputerID = b.ComputerID
        ///WHERE LocationID = @LocationID;
        ///.
        /// </summary>
        internal static string SelectLocationComputerList {
            get {
                return ResourceManager.GetString("SelectLocationComputerList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM `{0}`
        ///WHERE locationid = @LocationID.
        /// </summary>
        internal static string SelectLocationHistory {
            get {
                return ResourceManager.GetString("SelectLocationHistory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM `{0}`
        ///WHERE LocationID = @LocationID.
        /// </summary>
        internal static string SelectLocationRecord {
            get {
                return ResourceManager.GetString("SelectLocationRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `{0}`
        ///SET
        ///`TpmPresent` = @TPMPresent,
        ///`TpmReady` = @TPMReady,
        ///`TpmManagedAuthLevel` = @TpmManagedAuthLevel,
        ///`TpmAutoProvisioning` = @TpmAutoProvisioning,
        ///`VolumeType` = @VolumeType,
        ///`MountPoint` = @MountPoint,
        ///`CapacityGB` = @CapacityGB,
        ///`VolumeStatus` = @VolumeStatus,
        ///`EncryptionPercentage` = @EncryptionPercentage,
        ///`KeyProtector` = @KeyProtector,
        ///`ProtectionStatus` = @ProtectionStatus,
        ///`RecoveryKey` = @RecoveryKey,
        ///`Updated` = NOW(),
        ///`IsDomainController` = @IsDomainController
        ///WHERE ( [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UpdateBitlockerTPMRecord {
            get {
                return ResourceManager.GetString("UpdateBitlockerTPMRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `{0}`
        ///SET
        ///`Encrypt` = @Encrypt,
        ///`DomainControllers` = @DomainControllers,
        ///`HasBitlockerGPO` = @HasBitlockerGPO
        ///WHERE (`LocationID` = @LocationID).
        /// </summary>
        internal static string UpdateLocationRecord {
            get {
                return ResourceManager.GetString("UpdateLocationRecord", resourceCulture);
            }
        }
    }
}
