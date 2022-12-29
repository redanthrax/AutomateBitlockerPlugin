using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Domain.Constants {
    public class BitlockerConst {
        public static string FullyDecrypted = "FullyDecrypted";
        public static string FullyEncrypted = "FullyEncrypted";
        public static string EncryptionInProgress = "EncryptionInProgress";
        public static string DecryptionInProgress = "DecryptionInProgress";
        public static string Off = "Off";
        public static string On = "On";
        public static string GPOName = "AD Bitlocker Backup";
        public static string GPOBackupPath = "C:\\Windows\\Temp\\GPOSetup";

        public class Parameters {
            public const string Encrypt = "Encrypt";
            public const string Decrypt = "Decrypt";
            public const string GetData = "GetData";
            public const string CheckBitlockerGPO = "GetBitlockerGPO";
            public const string DeployBitlockerGPO = "DeployBitlockerGPO";
        }
    }
}
