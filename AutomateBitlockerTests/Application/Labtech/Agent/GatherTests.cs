using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomateBitlockerPlugin.Application.Labtech.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace AutomateBitlockerPlugin.Application.Labtech.Agent.Tests {
    [TestClass()]
    public class GatherTests {
        [TestMethod()]
        public void GatherDataTest() {
            var inCol = new NameValueCollection();
            var gather = new Gather();
            var returnCol = gather.GatherData(inCol);
            Assert.IsNotNull(returnCol);
        }
    }
}