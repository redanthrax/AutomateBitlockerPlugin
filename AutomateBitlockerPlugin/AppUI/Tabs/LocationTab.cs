using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabTech.Interfaces;
using AutomateBitlockerPlugin.Application.Labtech.Control;
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Domain.Constants;
using System.Threading;
using System.Windows.Threading;

namespace AutomateBitlockerPlugin.AppUI.Tabs {
    public partial class LocationTab : UserControl {
        private int LocationID;
        private Location location;
        ControlHelper _helper;
        string _deployText;

        public LocationTab(IControlCenter host, int locationId) {
            InitializeComponent();
            if (host == null) {
                return;
            }

            LocationID = locationId;
            _helper = new ControlHelper(host);

            locationGrid1.NotifyBoxChecked += CheckToShowEncryptButton;
        }

        private async void LocationTab_Load(object sender, EventArgs e) {
            var computers = await _helper.GetLocationComputerList(LocationID);
            var modified = new List<Computer>();
            var dcs = string.Empty;
            foreach(var computer in computers) {
                computer.IsOnline = "Refresh...";
                modified.Add(computer);
                if (computer.IsDomainController)
                    dcs += String.Format("{0}, ", computer.ComputerName);
            }

            //setup domain controller text
            if(dcs.Length > 0)
                domainControllerText.Text = dcs.Remove(dcs.Length - 2, 2);

            location = _helper.GetLocation(LocationID);

            //check if location has gpo
            if(location.HasBitlockerGPO) {
                gpoStatusText.Text = "Deployed";
            }

            if(gpoStatusText.Text == "Undeployed" && !string.IsNullOrEmpty(dcs))
                deployGPOButton.Visible = true;

            keepEncryptedCheckbox.Checked = location.Encrypt;

            await locationGrid1.LoadData(modified);
        }

        private async void refreshButton_MouseUp(object sender, MouseEventArgs e) {
            var messageBox = MessageBox.Show("Are you sure you want to refresh all computers? This can take a while.", "Refresh", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if(messageBox == DialogResult.OK) {
                var progress = new Progress<int>();
                progress.ProgressChanged += Progress_ProgressChanged;
                refreshStatus.Visible = true;
                refreshButton.Enabled = false;
                refreshStatus.Value = 0;
                var computers = new List<Computer>();
                foreach(var computer in locationGrid1.items) {
                    computers.Add(computer);
                }

                int donecount = 0;
                await locationGrid1.ClearData();
                await Task.Run(() =>
                {
                    Parallel.ForEach<Computer>(computers, (computer) =>
                    {
                        var onlineStatus = "Offline";
                        if (_helper.ComputerIsOnline(computer.ComputerID)) {
                            try {
                                _helper.SendCommand(computer.ComputerID, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.GetData, true);
                                onlineStatus = "Online";
                            }
                            catch { }
                        }

                        computer = _helper.GetLocationComputer(computer.ComputerID);
                        computer.IsOnline = onlineStatus;
                        locationGrid1.AddComputer(computer).ConfigureAwait(false);
                        donecount++;
                        ((IProgress<int>)progress).Report((donecount * 100) / computers.Count);

                    });
                });

                refreshStatus.Visible = false;
                refreshButton.Enabled = true;
            }
        }

        private void Progress_ProgressChanged(object sender, int e) {
            refreshStatus.Value = e;
        }

        private async void encryptButton_MouseUp(object sender, MouseEventArgs e) {
            var message = MessageBox.Show(String.Format("Are you sure you want to encrypt {0} machine(s)?", locationGrid1.encryptionList.Count), "Encrypt", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if(message == DialogResult.OK) {
                encryptButton.Visible = false;
                refreshButton.Enabled = false;
                int donecount = 0;
                refreshStatus.Value = 0;
                refreshStatus.Visible = true;
                var progress = new Progress<int>();
                progress.ProgressChanged += Progress_ProgressChanged;
                //run in parallel for async hotness
                await Task.Run(() =>
                {
                    Parallel.ForEach<Computer>(locationGrid1.encryptionList, (computer) => {
                        //remove computer from grid
                        locationGrid1.RemoveComputer(computer);
                        //check to make sure the computer is online, if not update computer on list as offline
                        computer.IsOnline = "Offline";
                        if (_helper.ComputerIsOnline(computer.ComputerID)) {
                            //Send encryption command
                            try {
                                _helper.SendCommand(computer.ComputerID, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.Encrypt, true, 60);
                                _helper.SendCommand(computer.ComputerID, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.GetData, true, 60);
                                var locComputer = _helper.GetLocationComputer(computer.ComputerID);
                                computer = locComputer;
                                computer.IsOnline = "Online";
                            }
                            catch { }
                        }

                        donecount++;
                        ((IProgress<int>)progress).Report((donecount * 100) / locationGrid1.encryptionList.Count);
                        locationGrid1.AddComputer(computer).ConfigureAwait(false);
                    });
                });

                locationGrid1.encryptionList = new List<Computer>();
                refreshButton.Enabled = true;
                refreshStatus.Visible = false;
            }
        }

        protected void CheckToShowEncryptButton(object sender, EventArgs e) {
            if(locationGrid1.encryptionList.Count > 0) {
                encryptButton.Visible = true;
            }
            else {
                encryptButton.Visible = false;
            }
        }

        private async void deployGPOButton_MouseUp(object sender, MouseEventArgs e) {
            deployGPOButton.Visible = false;
            var cts = new CancellationTokenSource();
            DeployTextRefresh(cts.Token);
            if(await _helper.DeployGPOToLocation(LocationID)) {
                //GPO was deployed.
                _deployText = "Deployed";
                cts.Cancel();
            }
            else {
                //there was a problem
                deployGPOButton.Visible = true;
                _deployText = "Error deploying";
                cts.Cancel();
            }
        }

        private void DeployTextRefresh(CancellationToken ct) {
            Task.Run(() =>
            {
                while (true) {
                    if (ct.IsCancellationRequested) {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            gpoStatusText.Text = _deployText;
                        });
                        break;
                    }
                    for (var i = 1; i < 4; i++) {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            gpoStatusText.Text = String.Format("Deploying{0}", String.Join("", Enumerable.Repeat(".", i).ToArray()));
                        });

                        Thread.Sleep(500);
                    }
                }
            }).ConfigureAwait(false);
        }

        private void saveEncryptSettingButton_MouseUp(object sender, MouseEventArgs e) {
            if (!keepEncryptedCheckbox.Checked) {
                MessageBox.Show("Unchecking this box does not automatically decrypt all machines.", "Check Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            location.Encrypt = keepEncryptedCheckbox.Checked;
            _helper.UpdateLocation(location);
            saveEncryptSettingButton.Visible = false;
        }

        private void keepEncryptedCheckbox_MouseUp(object sender, MouseEventArgs e) {
            if(location.Encrypt != keepEncryptedCheckbox.Checked) {
                saveEncryptSettingButton.Visible = true;
            }
            else {
                saveEncryptSettingButton.Visible = false;
            }
        }
    }
}
