using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomateBitlockerPlugin.Application.Labtech.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent.Tests {
    [TestClass()]
    public class PowershellCommandTests {
        [TestMethod()]
        public void IsDomainControllerTest() {
            Assert.IsFalse(PowershellCommand.IsDomainController());
        }

        [TestMethod()]
        public void ADBitlockerBackupTest() {
            PowershellCommand.BitlockerBackup();
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void EnableBitlockerTest() {
            PowershellCommand.EnableBitlocker();
            Assert.IsTrue(true);
        }
    }
}