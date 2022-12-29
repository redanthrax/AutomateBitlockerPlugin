using AutomateBitlockerPlugin.Application.Common;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Properties;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Infrastructure.Persistence.Migrations {
    public class SetupTables {

        private static readonly string CreatePluginBitlockerTPMTable = Resources.CreatePluginBitlockerTPMTable;

        public static void Run(IDatabaseAccess databaseAccess) {
            //Do Initial Table Setup
            ParameterizedQuery query = new ParameterizedQuery() {
                Query = String.Format("Show Tables LIKE '{0}'", PluginConst.BitlockerTPMTable)
            };

            string result = databaseAccess.ParameterizedScalarQuery(query);
            Logger.Log(databaseAccess, $"Result from scalar: {result}");
            if(!(result == PluginConst.BitlockerTPMTable)) {
                var createPluginBitlockerQuery = new ParameterizedQuery() {
                    Query = CreatePluginBitlockerTPMTable
                };

                databaseAccess.ParameterizedNonQuery(createPluginBitlockerQuery);
            }

            query = new ParameterizedQuery()
            {
                Query = String.Format("Show Tables LIKE '{0}'", PluginConst.BitlockerLocationTable)
            };

            result = databaseAccess.ParameterizedScalarQuery(query);
            if(!(result == PluginConst.BitlockerLocationTable)) {
                var createLocationTableQuery = new ParameterizedQuery()
                {
                    Query = String.Format(Resources.CreatePluginLocationTable, PluginConst.BitlockerLocationTable)
                };

                databaseAccess.ParameterizedNonQuery(createLocationTableQuery);

                //Generate location data on first run
                var generateQuery = new ParameterizedQuery()
                {
                    Query = String.Format(Resources.GenerateLocationData, PluginConst.BitlockerLocationTable)
                };

                databaseAccess.ParameterizedNonQuery(generateQuery);
            }

            //Add IsDomainController Column to data table
            query = new ParameterizedQuery()
            {
                Query = String.Format("SHOW COLUMNS FROM {0} LIKE 'IsDomainController'", PluginConst.BitlockerTPMTable)
            };

            var dcResult = databaseAccess.ParameterizedQuery(query);
            if(dcResult.Tables[0].Rows.Count == 0) {
                var dcQuery = new ParameterizedQuery()
                {
                    Query = Resources.AddIsDomainController
                };

                databaseAccess.ParameterizedNonQuery(dcQuery);
            }

            //add bitlocker history table
            query = new ParameterizedQuery()
            {
                Query = String.Format("Show Tables LIKE '{0}'", PluginConst.BitlockerHistoryTable)
            };

            result = databaseAccess.ParameterizedScalarQuery(query);
            if (!(result == PluginConst.BitlockerHistoryTable)) {
                var createHistoryTableQuery = new ParameterizedQuery()
                {
                    Query = String.Format(Resources.CreatePluginHistoryTable, PluginConst.BitlockerHistoryTable)
                };

                databaseAccess.ParameterizedNonQuery(createHistoryTableQuery);
            }
        }
    }
}
