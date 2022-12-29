using AutomateBitlockerPlugin.Application.Labtech.Agent;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Properties;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Infrastructure.Persistence {
    public class Mapper {
        public static ParameterizedQuery MapToCreateQuery(object item) {
            var query = new ParameterizedQuery {
                Parameters = MapQueryParameters(item)
            };

            if (item.GetType() == typeof(BitlockerTPM))
                query.Query = String.Format(Resources.InsertBitlockerTPMRecord, PluginConst.BitlockerTPMTable);

            return query;
        }

        public static ParameterizedQuery MapToUpdateQuery(object item) {
            var query = new ParameterizedQuery {
                Parameters = MapQueryParameters(item)
            };

            if (item.GetType() == typeof(BitlockerTPM))
                query.Query = String.Format(Resources.UpdateBitlockerTPMRecord, PluginConst.BitlockerTPMTable);

            return query;
        }

        public static T MapRowToObject<T>(DataRow row, object obj) {
            var item = (T)obj;
            foreach(var property in item.GetType().GetProperties()) {
                property.SetValue(item, Convert.ChangeType(row[property.Name], property.PropertyType));
            }

            return item;
        }

        public static List<IQueryParameter> MapQueryParameters(object item) {
            var queryParameters = new List<IQueryParameter>();

            foreach (var property in item.GetType().GetProperties()) {
                var key = property.Name;
                var value = string.Empty;

                if (property.GetValue(item) != null) {
                    value = property.GetValue(item).ToString();
                    bool result = false;
                    if(bool.TryParse(value, out result)) {
                        value = "0";
                        if (result)
                            value = "1";
                    }
                }

                queryParameters.Add(new QueryParameter {
                    Name = $"@{key}",
                    Data = value
                });
            }

            return queryParameters;
        }
    }
}
