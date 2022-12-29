using AutomateBitlockerPlugin.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Entities {
    public class BitlockerTPM : AuditableEntity {
        //TPM Data
        public bool TPMPresent { get; set; }
        public bool TPMReady { get; set; }
        public string TpmManagedAuthLevel { get; set; }
        public string TpmAutoProvisioning { get; set; }

        //Bitlocker Data
        public string VolumeType { get; set; }
        public string MountPoint { get; set; }
        public string CapacityGB { get; set; }
        public string VolumeStatus { get; set; }
        public string EncryptionPercentage { get; set; }
        public string KeyProtector { get; set; }
        public string ProtectionStatus { get; set; }
        public string RecoveryKey { get; set; }

        //Domain Controller
        public bool IsDomainController { get; set; }
    }
}
