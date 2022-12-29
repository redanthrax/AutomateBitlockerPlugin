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
using AutomateBitlockerPlugin.Domain.Entities;
using AutomateBitlockerPlugin.Application.Labtech.Control;
using System.Windows.Threading;
using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.AppUI.Tabs.Controls;

namespace AutomateBitlockerPlugin.AppUI.Tabs {
    public partial class ComputerTab : UserControl {
        ControlHelper _helper;
        BitlockerTPM btpm;
        int computerId;
        ComputerTab thisTab;

        public ComputerTab(IControlCenter host, int targetComputerId) {
            InitializeComponent();
            if (host == null) {
                return;
            }

            _helper = new ControlHelper(host);
            computerId = targetComputerId;
            thisTab = this;
        }

        private async void refreshButton_MouseUp(object sender, MouseEventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            refreshButton.Enabled = false;
            cryptButton.Visible = false;
            try {
                await _helper.SendCommandAsync(computerId, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.GetData, true, 60);
            }
            catch (Exception ex) {
                if (ex.Message == "TIMEOUT")
                    MessageBox.Show("Sending refresh command timed out.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            btpm = await _helper.GetBitlockerTPMAsync(computerId);
            RefreshCryptButton();
            Fill();
            this.Cursor = Cursors.Arrow;
            refreshButton.Enabled = true;
        }

        private async void cryptButton_MouseUp(object sender, MouseEventArgs e) {
            if (cryptButton.Text == "Encrypt") {
                var messageResult = MessageBox.Show("Are you sure you want to encrypt this computer?", "Encryption Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (messageResult == DialogResult.OK) {
                    cryptButton.Visible = false;
                    refreshButton.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    try {
                        await _helper.SendCommandAsync(computerId, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.Encrypt, true, 60);
                        await _helper.SendCommandAsync(computerId, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.GetData, true, 60);
                        btpm = await _helper.GetBitlockerTPMAsync(computerId);
                        Fill();
                    }
                    catch (Exception ex) {
                        if (ex.Message == "TIMEOUT") {
                            MessageBox.Show("Sending encrypt command timed out.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    this.Cursor = Cursors.Arrow;
                    refreshButton.Enabled = true;
                    RefreshCryptButton();
                }
            }
            else {
                var messageResult = MessageBox.Show("Are you sure you want to decrypt this computer?", "Encryption Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if(messageResult == DialogResult.OK) {
                    cryptButton.Visible = false;
                    refreshButton.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    try {
                        await _helper.SendCommandAsync(computerId, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.Decrypt, false, 60);
                        await _helper.SendCommandAsync(computerId, PluginConst.EncryptCommandNumber, BitlockerConst.Parameters.GetData, true, 60);
                        btpm = await _helper.GetBitlockerTPMAsync(computerId);
                        Fill();
                    }
                    catch(Exception ex) {
                        if (ex.Message == "TIMEOUT") {
                            MessageBox.Show("Sending decrypt command timed out.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    this.Cursor = Cursors.Arrow;
                    refreshButton.Enabled = true;
                    RefreshCryptButton();
                }
            }
        }

        private async void ComputerTab_Load(object sender, EventArgs e) {
            btpm = await _helper.GetBitlockerTPMAsync(computerId);
            RefreshCryptButton();
            Fill();
        }

        private void RefreshCryptButton() {
            if (btpm.ProtectionStatus == BitlockerConst.On &&
                btpm.VolumeStatus != BitlockerConst.EncryptionInProgress &&
                btpm.VolumeStatus != BitlockerConst.DecryptionInProgress) {
                cryptButton.Text = "Decrypt";
                cryptButton.Visible = true;

            }

            if (btpm.ProtectionStatus == BitlockerConst.Off &&
                btpm.VolumeStatus != BitlockerConst.EncryptionInProgress &&
                btpm.VolumeStatus != BitlockerConst.DecryptionInProgress &&
                btpm.TPMPresent && btpm.TPMReady) {
                cryptButton.Text = "Encrypt";
                cryptButton.Visible = true;
            }
        }

        private void Fill() {
            TPMPresentText.Text = btpm.TPMPresent.ToString();
            if (btpm.TPMPresent) {
                TPMReadyText.Text = btpm.TPMReady.ToString();
                TPMManagedAuthLevelText.Text = btpm.TpmManagedAuthLevel;
                TPMAutoProvisioningText.Text = btpm.TpmAutoProvisioning;
                VolumeTypeText.Text = btpm.VolumeType;
                MountPointText.Text = btpm.MountPoint;
                CapacityText.Text = btpm.CapacityGB;
                VolumeStatusText.Text = btpm.VolumeStatus;
                EncryptionPercentText.Text = btpm.EncryptionPercentage;
                KeyProtectorText.Text = btpm.KeyProtector;
                ProtectionStatusText.Text = btpm.ProtectionStatus;
                RecoveryKeyTextbox.Text = btpm.RecoveryKey;
            }
        }

        private async void showHistoryButton_MouseUp(object sender, MouseEventArgs e) {
            //get history data
            //var historyData = _helper.GetComputerHistory(computerId);
            //var historyGrid = new HistoryGrid();
            //historyGrid.LoadHistory(historyData);
            //var elementHost = new System.Windows.Forms.Integration.ElementHost();
            //elementHost.AutoSize = true;
            //elementHost.Child = historyGrid;
            //var form = new Form();
            //form.AutoSize = true;
            //form.Text = String.Format("Bitlocker History on {0}", computerId);
            //form.Controls.Add(elementHost);
            //form.Show();
            var historyData = _helper.GetComputerHistory(computerId);
            var historyForm = new HistoryForm();
            historyForm.LoadData(historyData);
            historyForm.Show();
        }
    }
}
