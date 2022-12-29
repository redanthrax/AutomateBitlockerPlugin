using AutomateBitlockerPlugin.Application.Common;
using AutomateBitlockerPlugin.Application.Labtech.Agent;
using AutomateBitlockerPlugin.Application.Labtech.Control;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Infrastructure.Persistence;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace AutomateBitlockerPlugin.Application.Labtech.Server {
    /// <summary>
    /// This is the process that runs on the server, do db stuff here with the
    /// collected namevaluecollection passed from the client machine.
    /// </summary>
    public class GatherProcess : IGatherProcess {
        public string Name { get { return PluginConst.Name; } }

        public int ConfigurationNumber { get { return PluginConst.GatherConfigNumber; } }

        private ApplicationDbContext _dbContext;
        private IASPHost _host;

        public void Initialize(IASPHost host) {
            try
            {
                _dbContext = new ApplicationDbContext(host);
                if (host is IDatabaseAccess)
                {
                    _dbContext.BadHost = false;
                    EventLogHelper.WriteLog("Obtained ApplicationDBContext.");
                }
                else
                {
                    _dbContext.BadHost = true;
                    throw new Exception("Host was not DatabaseAccess");
                }

                _host = host;
            }
            catch(Exception ex)
            {
                EventLogHelper.WriteLog($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// This process runs on the server and gets the data from Gather on the client.
        /// The configuration number must match the Gather interface configuration number in order to run.
        /// </summary>
        /// <param name="computerId">The computer ID calling this method</param>
        /// <param name="data">A name/value collection of data received from ther computer.</param>
        /// <param name="internal">Not used by this method.</param>
        /// <returns>A string of data sent back to the Gather ProcessReturn method</returns>
        public string ProcessGatheredData(int computerId, NameValueCollection data, ref object @internalUseOnly) {
            if (!_dbContext.BadHost)
            {
                try
                {
                    //add data to database
                    var objects = ObjConvert.FromCollection(data);
                    foreach (var item in objects)
                    {
                        if (item.GetType() == _dbContext._bitlockerTPM.GetType())
                        {
                            _dbContext._bitlockerTPM = (BitlockerTPM)item;
                            _dbContext._bitlockerTPM.ComputerID = computerId;
                        }
                    }

                    _dbContext.ComputerID = computerId;
                    _dbContext.SaveChanges();

                    var location = _dbContext.GetComputerLocation(computerId);
                    if (location.Encrypt)
                    {
                        if (_dbContext._bitlockerTPM.TPMPresent && _dbContext._bitlockerTPM.TPMReady && _dbContext._bitlockerTPM.VolumeStatus != BitlockerConst.FullyEncrypted)
                        {
                            var helper = new ControlHelper(_host);
                            helper.SendCommand(computerId, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.Encrypt, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogHelper.WriteLog($"Error: {ex.Message}");
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// This is not used/depricated
        /// </summary>
        /// <param name="computerId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ProcessGatheredData(int computerId, NameValueCollection data) {
            return null;
        }

        public void Decommision() {
            
        }
    }
}
