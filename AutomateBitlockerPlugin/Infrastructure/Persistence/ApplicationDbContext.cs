using AutomateBitlockerPlugin.Application.Common;
using AutomateBitlockerPlugin.Application.Labtech.Agent;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Properties;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Management.Automation.Language;
using System.Text;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Infrastructure.Persistence {
    public class ApplicationDbContext {
        private IDatabaseAccess _databaseAccess;
        private object _host;
        public bool BadHost;

        public BitlockerTPM _bitlockerTPM { get; set; }
        private int _computerID;

        /// <summary>
        /// Custom getter and setter to setup object computerid
        /// </summary>
        public int ComputerID { 
            get {
                return _computerID;
            }
            set {
                _computerID = value;
                if(this._bitlockerTPM != null) {
                    this._bitlockerTPM.ComputerID = value;
                }
            }
        }

        public ApplicationDbContext(object host) {
            _databaseAccess = (IDatabaseAccess)host;
            _bitlockerTPM = new BitlockerTPM();
            if(host.GetType() == typeof(IASPHost))
            {
                _host = (IASPHost)host;
            }
        }

        public void Refresh() {
            //TODO: Figure out what to do for computers without records
            var bitlockerTPMQuery = new ParameterizedQuery {
                Query = String.Format(Resources.SelectBitlockerTPMRecord, PluginConst.BitlockerTPMTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@ComputerID", Convert.ToString(ComputerID))
                }
            };

            var dataSet = _databaseAccess.ParameterizedQuery(bitlockerTPMQuery);

            if(dataSet != null) {
                foreach (DataRow row in dataSet.Tables[0].Rows) {
                    _bitlockerTPM = Mapper.MapRowToObject<BitlockerTPM>(row, _bitlockerTPM); 
                }
            }
        }

        public void SaveChanges() {
            ParameterizedQuery bitlockerTPMQuery;

            if (Exists(ComputerID)) {
                bitlockerTPMQuery = Mapper.MapToUpdateQuery(_bitlockerTPM);
            }
            else {
                bitlockerTPMQuery = Mapper.MapToCreateQuery(_bitlockerTPM);
            }

            try {
                AddHistory(ComputerID, GetHistoryString(_bitlockerTPM));
            }
            catch(Exception e) {
                EventLogHelper.WriteLog($"Error adding history: {e.Message}");
            }

            try {
                _databaseAccess.ParameterizedQuery(bitlockerTPMQuery);
            }
            catch(Exception e) {
                EventLogHelper.WriteLog($"Error in bitlocker query: {e.Message}");
            }

            //check if domain controller and add to location
            if (_bitlockerTPM.IsDomainController) {

            }
        }

        public bool Exists(int computerId) {
            var query = new ParameterizedQuery {
                Query = String.Format("SELECT `ComputerID` FROM `{0}` WHERE `ComputerID` = @computerId", PluginConst.BitlockerTPMTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@computerId", Convert.ToString(computerId))
                }
            };

            var computer = _databaseAccess.ParameterizedQuery(query);
            if(computer.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public int GetCommandStatus(int CommandID) {
            var query = new ParameterizedQuery {
                Query = "Select Status from commands where CmdID=@CommandID",
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@CommandID", Convert.ToString(CommandID))
                }
            };

            var status = _databaseAccess.ParameterizedScalarQuery(query);

            var statusNumber = StatusConst.Error;

            if(int.TryParse(status, out statusNumber)) {
                return statusNumber;
            }

            return statusNumber;
        }

        public string GetCommandResult(int CommandID) {
            var query = new ParameterizedQuery
            {
                Query = "Select * from commands where CmdID=@CommandID",
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@CommandID", Convert.ToString(CommandID))
                }
            };

            var result = _databaseAccess.ParameterizedQuery(query);
            if(result.Tables[0].Rows.Count > 0) {
                return result.Tables[0].Rows[0]["OUTPUT"].ToString();
            }

            return string.Empty;
        }

        public bool CommandRunning(int ComputerID) {
            var query = new ParameterizedQuery {
                Query = "Select COUNT(`Status`) FROM commands WHERE ComputerID = @ComputerID AND `Status` <> 4 AND `Status` <> 3",
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@ComputerID", Convert.ToString(ComputerID))
                }
            };

            var countResult = _databaseAccess.ParameterizedScalarQuery(query);
            int count = 0;
            if(int.TryParse(countResult, out count)) {
                if(count > 0) {
                    return true;
                }
            }

            return false;
        }

        public void CheckAndCreateLocation(int locationId) {
            //check if location exists first and create if not
            var locationQuery = new ParameterizedQuery {
                Query = String.Format(Resources.SelectLocationRecord, PluginConst.BitlockerLocationTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@LocationID", Convert.ToString(locationId))
                }
            };
            var location = _databaseAccess.ParameterizedQuery(locationQuery);
            if (location.Tables[0].Rows.Count == 0) {
                //insert a record for the location that doesn't exist
                var newLocationParams = new {
                    LocationID = locationId,
                    Encrypt = 0,
                    DomainControllers = string.Empty,
                    HasBitlockerGPO = 0
                };
                var newLocationQuery = new ParameterizedQuery {
                    Query = String.Format(Resources.InsertLocationRecord, PluginConst.BitlockerLocationTable),
                    Parameters = Mapper.MapQueryParameters(newLocationParams)
                };

                _databaseAccess.ParameterizedNonQuery(newLocationQuery);
            }
        }

        public List<Computer> GetLocationComputerList(int LocationID) {
            CheckAndCreateLocation(LocationID);
            var list = new List<Computer>();
            var query = new ParameterizedQuery {
                Query = String.Format(Resources.SelectLocationComputerList, PluginConst.BitlockerTPMTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@LocationID", Convert.ToString(LocationID))
                }
            };

            var computers = _databaseAccess.ParameterizedQuery(query);
            if(computers != null) {
                foreach(DataRow row in computers.Tables[0].Rows) {
                    list.Add(new Computer {
                        ComputerID = (int)row["ComputerID"],
                        ComputerName = (string)row["Name"],
                        ProtectionStatus = String.IsNullOrEmpty(row["ProtectionStatus"].ToString()) ? null : row["ProtectionStatus"].ToString(),
                        KeyProtector = String.IsNullOrEmpty(row["KeyProtector"].ToString()) ? null : row["KeyProtector"].ToString(),
                        VolumeStatus = String.IsNullOrEmpty(row["VolumeStatus"].ToString()) ? null : row["VolumeStatus"].ToString(),
                        TPMPresent = String.IsNullOrEmpty(row["TPMPresent"].ToString()) ? false : (bool)row["TPMPresent"],
                        TPMReady = String.IsNullOrEmpty(row["TPMReady"].ToString()) ? false : (bool)row["TPMReady"],
                        HasRecoveryKey = String.IsNullOrEmpty(row["HasRecoveryKey"].ToString()) ? false : bool.Parse(row["HasRecoveryKey"].ToString()),
                        IsDomainController = String.IsNullOrEmpty(row["IsDomainController"].ToString()) ? false : bool.Parse(row["IsDomainController"].ToString())
                    });
                }
            }

            return list;
        }

        public Computer GetLocationComputer(int ComputerID) {
            var query = new ParameterizedQuery
            {
                Query = String.Format(Resources.SelectLocationComputer, PluginConst.BitlockerTPMTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@ComputerID", Convert.ToString(ComputerID))
                }
            };

            var dataItem = _databaseAccess.ParameterizedQuery(query);

            DataRow row = dataItem.Tables[0].Rows[0];

            var computer = new Computer
            {
                ComputerID = (int)row["ComputerID"],
                ComputerName = (string)row["Name"],
                ProtectionStatus = String.IsNullOrEmpty(row["ProtectionStatus"].ToString()) ? null : row["ProtectionStatus"].ToString(),
                KeyProtector = String.IsNullOrEmpty(row["KeyProtector"].ToString()) ? null : row["KeyProtector"].ToString(),
                VolumeStatus = String.IsNullOrEmpty(row["VolumeStatus"].ToString()) ? null : row["VolumeStatus"].ToString(),
                TPMPresent = String.IsNullOrEmpty(row["TPMPresent"].ToString()) ? false : (bool)row["TPMPresent"],
                TPMReady = String.IsNullOrEmpty(row["TPMReady"].ToString()) ? false : (bool)row["TPMReady"],
                HasRecoveryKey = String.IsNullOrEmpty(row["HasRecoveryKey"].ToString()) ? false : bool.Parse(row["HasRecoveryKey"].ToString())
            };

            return computer;
        }

        public bool CheckComputerOnline(int ComputerID) {
            var query = new ParameterizedQuery
            {
                Query = Resources.CheckComputerOnline,
                Parameters = new List<IQueryParameter>()
                {
                    new QueryParameter("@ComputerID", Convert.ToString(ComputerID))
                }
            };

            var data = _databaseAccess.ParameterizedScalarQuery(query);

            int count = 0;
            if(int.TryParse(data, out count)) {
                if (count > 0)
                    return true;
            }

            return false;
        }

        public bool IsFasttalkRunning(int ComputerID) {
            var query = new ParameterizedQuery
            {
                Query = "Select COUNT(`flags`) FROM computers WHERE ComputerID = @ComputerID AND (1 & `flags`) = 1",
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@ComputerID", Convert.ToString(ComputerID))
                }
            };

            var countResult = _databaseAccess.ParameterizedScalarQuery(query);
            int count = 0;
            if (int.TryParse(countResult, out count)) {
                if (count > 0) {
                    return true;
                }
            }

            return false;
        }

        public int GetComputerLocation(BitlockerTPM computer) {
            var computerQuery = new ParameterizedQuery
            {
                Query = "SELECT LocationID FROM computers WHERE ComputerID = @ComputerID",
                Parameters = new List<IQueryParameter>()
                {
                    new QueryParameter("@ComputerID", Convert.ToString(computer.ComputerID))
                }
            };

            var result = _databaseAccess.ParameterizedScalarQuery(computerQuery);
            var id = 0;
            if(int.TryParse(result, out id)) {
                return id;
            }

            return 0;
        }

        public void AddGpoToLocation(int locationId) {
            var updateQuery = new ParameterizedQuery
            {
                Query = $"UPDATE {PluginConst.BitlockerLocationTable} SET `HasBitlockerGPO` = '1' WHERE LocationID = @LocationID",
                Parameters = new List<IQueryParameter>()
                {
                    new QueryParameter("@LocationID", Convert.ToString(locationId))
                }
            };

            _databaseAccess.ParameterizedNonQuery(updateQuery);
        }

        public bool LocationHasGPO(int locationId) {
            var checkQuery = new ParameterizedQuery
            {
                Query = $"SELECT HasBitlockerGPO FROM {PluginConst.BitlockerLocationTable} WHERE LocationID = @LocationID",
                Parameters = new List<IQueryParameter>()
                {
                    new QueryParameter("@LocationID", Convert.ToString(locationId))
                }
            };

            var result = _databaseAccess.ParameterizedScalarQuery(checkQuery);
            if(bool.TryParse(result, out bool hasGpo)) {
                return hasGpo;
            }

            return false;
        }

        public Location GetLocation(int locationId) {
            CheckAndCreateLocation(locationId);
            var locationQuery = new ParameterizedQuery
            {
                Query = String.Format(Resources.SelectLocationRecord, PluginConst.BitlockerLocationTable),
                Parameters = new List<IQueryParameter>()
                {
                    new QueryParameter("@LocationID", locationId.ToString())
                }
            };

            var dataItem = _databaseAccess.ParameterizedQuery(locationQuery);

            DataRow row = dataItem.Tables[0].Rows[0];

            var location = new Location
            {
                LocationID = (int)row["LocationID"],
                Encrypt = (bool)row["Encrypt"],
                DomainControllers = row["DomainControllers"].ToString(),
                HasBitlockerGPO = (bool)row["HasBitlockerGPO"]
            };

            return location;
        }

        public Location GetComputerLocation(int computerId) {
            var locationIdQuery = new ParameterizedQuery
            {
                Query = "SELECT LocationID FROM Computers WHERE ComputerID = @computerId",
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@computerId", computerId.ToString())
                }
            };

            var locationId = _databaseAccess.ParameterizedScalarQuery(locationIdQuery);
            if (int.TryParse(locationId, out int id)) {
                return GetLocation(id);
            }

            return null;
        }

        public void SaveLocation(Location location) {
            var locationQuery = new ParameterizedQuery
            {
                Query = String.Format(Resources.UpdateLocationRecord, PluginConst.BitlockerLocationTable),
                Parameters = Mapper.MapQueryParameters(location)
            };

            _databaseAccess.ParameterizedNonQuery(locationQuery);
        }

        public void AddHistory(int computerid, string action) {
            var lastRecordQuery = new ParameterizedQuery
            {
                Query = String.Format(Resources.SelectLastHistoryRecord, PluginConst.BitlockerHistoryTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@ComputerID", computerid.ToString())
                }
            };

            string lastRecord;
            try {
                lastRecord = _databaseAccess.ParameterizedScalarQuery(lastRecordQuery);
            }
            catch(Exception ex) {
                lastRecord = ex.Message;
            }
            
            if(lastRecord != action) {
                var location = GetComputerLocation(computerid);
                var parameters = new
                {
                    Action = action,
                    ComputerID = computerid,
                    LocationId = location.LocationID
                };
                var insertQuery = new ParameterizedQuery
                {
                    Query = String.Format(Resources.InsertHistoryRecord, PluginConst.BitlockerHistoryTable),
                    Parameters = Mapper.MapQueryParameters(parameters)
                };
                _databaseAccess.ParameterizedNonQuery(insertQuery);
            }
        }

        private string GetHistoryString(BitlockerTPM data) {
            return String.Format("TPM Present: {0}, TPM Ready: {1}, Volume Type: {2}, Mount Point: {3}, Volume Status: {4}, " +
                "Key Protector: {5}, Protection Status: {6}, Recovery Key: {7}", data.TPMPresent, data.TPMReady, data.VolumeType,
                data.MountPoint, data.VolumeStatus, data.KeyProtector, data.ProtectionStatus, data.RecoveryKey);
                
        }

        public List<History> GetComputerHistory(int computerid) {
            var list = new List<History>();

            var query = new ParameterizedQuery
            {
                Query = String.Format(Resources.SelectHistoryRecords, PluginConst.BitlockerHistoryTable),
                Parameters = new List<IQueryParameter>() {
                    new QueryParameter("@ComputerID", Convert.ToString(computerid))
                }
            };

            var history = _databaseAccess.ParameterizedQuery(query);

            if (history != null) {
                foreach (DataRow row in history.Tables[0].Rows) {
                    list.Add(new History
                    {
                        Created = DateTime.Parse(row["created"].ToString()),
                        Action = row["action"].ToString()
                    });
                }
            }

            return list;
        }

        public void Dispose() {
            _databaseAccess = null;
        }
    }
}
