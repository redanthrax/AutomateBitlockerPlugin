using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Infrastructure.Persistence
{
    public class Global
    {
        private static volatile Global _instance;
        private static readonly object SyncRoot = new object();
        public IDatabaseAccess Host { get; set; }
        public IControlCenter2 CC2 { get; set; }
        public ApplicationDbContext AppDbContext { get; set; }
        public static Global Instance
        {
            get
            {
                lock (Global.SyncRoot)
                {
                    if (Global._instance != null)
                    {
                        if (Global._instance.AppDbContext == null && Global._instance.Host != null)
                        {
                            Global._instance.AppDbContext = new ApplicationDbContext(Global._instance.Host);
                        }
                    }
                    else
                        Global._instance = new Global();
                    return Global._instance;
                }
            }
        }

    }
}
