 using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Infrastructure.Persistence;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutomateBitlockerPlugin.Application.Labtech.Control {
    public class ControlHelper {

        ApplicationDbContext _context;
        IControlCenter _controlCenter;

        public ControlHelper(IControlCenter host) {
            _context = new ApplicationDbContext((IDatabaseAccess)host);
            _controlCenter = host;
        }

        public BitlockerTPM GetBitlockerTPM(int computerId) {
            _context.ComputerID = computerId;
            _context.Refresh();
            return _context._bitlockerTPM;
        }

        public async Task<BitlockerTPM> GetBitlockerTPMAsync(int computerId) {
            var btpm = new BitlockerTPM();
            await Task.Run(() =>
            {
                _context.ComputerID = computerId;
                _context.Refresh();
                btpm = _context._bitlockerTPM;
            });

            return btpm;
        }

        public async Task<List<Computer>> GetLocationComputerList(int locationId) {
            var computers = new List<Computer>();
            await Task.Run(() =>
            {
                computers = _context.GetLocationComputerList(locationId);
            });

            return computers;
        }

        public Computer GetLocationComputer(int computerId) {
            var computer = new Computer();
            computer = _context.GetLocationComputer(computerId);
            return computer;
        }

        public bool LocationHasGPO(int locationId) {
            return _context.LocationHasGPO(locationId);
        }

        public Location GetLocation(int locationId) {
            return _context.GetLocation(locationId);
        }

        public void UpdateLocation(Location location) {
            _context.SaveLocation(location);
        }

        public async Task<bool> DeployGPOToLocation(int locationId) {
            var returnVal = false;
            await Task.Run(() =>
            {
                var computers = _context.GetLocationComputerList(locationId);
                var dc = computers.Where(c => c.IsDomainController).First();
                if (dc != null) {
                    var result = SendCommandWithReturn(dc.ComputerID, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.DeployBitlockerGPO);
                    if(result == GPOConst.GPODEPLOY) {
                        _context.AddGpoToLocation(locationId);
                        returnVal = true;
                    }
                    else {
                        returnVal = false;
                    }
                }
            });

            return returnVal;
        }

        public bool ComputerIsOnline(int computerId) {
            return _context.CheckComputerOnline(computerId);
        }

        /// <summary>
        /// Send the computer the command and wait for it to complete unless there's an error then exit.
        /// </summary>
        /// <param name="computerId">ComputerID</param>
        /// <param name="commandId">CommandID coordinated between the interfaces.</param>
        /// <param name="parameters">String of parameters sent.</param>
        /// <param name="wait">Wait for the command to complete on the agent before ending.</param>
        /// <param name="timeout">How long to wait for the command to run in seconds.</param>
        public void SendCommand(int computerId, int commandId, string parameters = "", bool wait = true, int timeout = 60) {
            var seconds = 0;
            if (!_context.IsFasttalkRunning(computerId)) {
                var fastTalkId = _controlCenter.SendCommand(computerId, CommandConst.FastTalk, null);
                var fTStatus = _context.GetCommandStatus(fastTalkId);
                while (fTStatus != StatusConst.Success) {
                    if (seconds < timeout) {
                        Thread.Sleep(1000);
                        seconds++;
                        fTStatus = _context.GetCommandStatus(fastTalkId);
                        if (fTStatus == StatusConst.Error)
                            break;
                    }
                    else {
                        throw new Exception("TIMEOUT");
                    }
                }
            }

            var runId = _controlCenter.SendCommand(computerId, commandId, parameters);
            if (wait) {
                var status = _context.GetCommandStatus(runId);
                while (status != StatusConst.Success) {
                    if (seconds < timeout) {
                        Thread.Sleep(1000);
                        seconds++;
                        status = _context.GetCommandStatus(runId);
                        if (status == StatusConst.Error)
                            break;
                    }
                    else {
                        throw new Exception("TIMEOUT");
                    }
                }
                //get the data from the run
            }
        }

        public string SendCommandWithReturn(int computerId, int commandId, string parameters = "", int timeout = 60) {
            var seconds = 0;
            if (!_context.IsFasttalkRunning(computerId)) {
                var fastTalkId = _controlCenter.SendCommand(computerId, CommandConst.FastTalk, null);
                var fTStatus = _context.GetCommandStatus(fastTalkId);
                while (fTStatus != StatusConst.Success) {
                    if (seconds < timeout) {
                        Thread.Sleep(1000);
                        seconds++;
                        fTStatus = _context.GetCommandStatus(fastTalkId);
                        if (fTStatus == StatusConst.Error)
                            break;
                    }
                    else {
                        throw new Exception("TIMEOUT");
                    }
                }
            }

            var runId = _controlCenter.SendCommand(computerId, commandId, parameters);
            var status = _context.GetCommandStatus(runId);
            while (status != StatusConst.Success) {
                if (seconds < timeout) {
                    Thread.Sleep(1000);
                    seconds++;
                    status = _context.GetCommandStatus(runId);
                    if (status == StatusConst.Error)
                        break;
                }
                else {
                    throw new Exception("TIMEOUT");
                }
            }

            var result = _context.GetCommandResult(runId);
            return result;
        }

        /// <summary>
        /// Send the computer the command and wait for it to complete async.
        /// </summary>
        /// <param name="computerId">ComputerID</param>
        /// <param name="commandId">CommandID coordinated between the interfaces.</param>
        /// <param name="parameters">String of parameters sent.</param>
        /// <param name="wait">Wait for the command to complete on the agent before ending.</param>
        /// <param name="timeout">Timeout the command in seconds.</param>
        public async Task SendCommandAsync(int computerId, int commandId, string parameters = "", bool wait = true, int timeout = 60) {
            await Task.Run(() =>
            {
                try {
                    var seconds = 0;
                    if (!_context.IsFasttalkRunning(computerId)) {
                        var fastTalkId = _controlCenter.SendCommand(computerId, CommandConst.FastTalk, null);
                        var fTStatus = _context.GetCommandStatus(fastTalkId);
                        while (fTStatus != StatusConst.Success) {
                            if (seconds < timeout) {
                                Thread.Sleep(1000);
                                seconds++;
                                fTStatus = _context.GetCommandStatus(fastTalkId);
                                if (fTStatus == StatusConst.Error)
                                    break;
                            }
                            else {
                                throw new Exception("TIMEOUT");
                            }
                        }
                    }

                    var runId = _controlCenter.SendCommand(computerId, commandId, parameters);
                    if (wait) {
                        var status = _context.GetCommandStatus(runId);
                        while (status != StatusConst.Success) {
                            if (seconds < timeout) {
                                Thread.Sleep(1000);
                                seconds++;
                                status = _context.GetCommandStatus(runId);
                                if (status == StatusConst.Error)
                                    break;
                            }
                            else {
                                throw new Exception("TIMEOUT");
                            }
                        }
                        //get the data from the run
                    }
                }
                catch (Exception ex) {
                    if (ex.Message == "TIMEOUT")
                        throw new Exception(ex.Message);
                }
            });
        }

        public bool CommandRunning(int computerId) {
            return _context.CommandRunning(computerId);
        }

        public List<History> GetComputerHistory(int computerId) {
            var historyList = _context.GetComputerHistory(computerId);
            return historyList;
        }
    }
}
