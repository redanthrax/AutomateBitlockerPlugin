namespace AutomateBitlockerPlugin.AppUI.Tabs {
    partial class LocationTab {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.refreshButton = new System.Windows.Forms.Button();
            this.refreshStatus = new System.Windows.Forms.ProgressBar();
            this.encryptButton = new System.Windows.Forms.Button();
            this.gpoStatusLabel = new System.Windows.Forms.Label();
            this.dcLabel = new System.Windows.Forms.Label();
            this.domainControllerText = new System.Windows.Forms.Label();
            this.gpoStatusText = new System.Windows.Forms.Label();
            this.deployGPOButton = new System.Windows.Forms.Button();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.locationGrid1 = new AutomateBitlockerPlugin.AppUI.Tabs.Controls.LocationGrid();
            this.keepEncryptedCheckbox = new System.Windows.Forms.CheckBox();
            this.saveEncryptSettingButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(5, 80);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(83, 30);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.refreshButton_MouseUp);
            // 
            // refreshStatus
            // 
            this.refreshStatus.Location = new System.Drawing.Point(104, 85);
            this.refreshStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.refreshStatus.Name = "refreshStatus";
            this.refreshStatus.Size = new System.Drawing.Size(144, 18);
            this.refreshStatus.TabIndex = 2;
            this.refreshStatus.Visible = false;
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(134, 80);
            this.encryptButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(83, 30);
            this.encryptButton.TabIndex = 3;
            this.encryptButton.Text = "Encrypt";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Visible = false;
            this.encryptButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.encryptButton_MouseUp);
            // 
            // gpoStatusLabel
            // 
            this.gpoStatusLabel.AutoSize = true;
            this.gpoStatusLabel.Location = new System.Drawing.Point(2, 19);
            this.gpoStatusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gpoStatusLabel.Name = "gpoStatusLabel";
            this.gpoStatusLabel.Size = new System.Drawing.Size(66, 13);
            this.gpoStatusLabel.TabIndex = 4;
            this.gpoStatusLabel.Text = "GPO Status:";
            // 
            // dcLabel
            // 
            this.dcLabel.AutoSize = true;
            this.dcLabel.Location = new System.Drawing.Point(2, 6);
            this.dcLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.dcLabel.Name = "dcLabel";
            this.dcLabel.Size = new System.Drawing.Size(98, 13);
            this.dcLabel.TabIndex = 5;
            this.dcLabel.Text = "Domain Controllers:";
            // 
            // domainControllerText
            // 
            this.domainControllerText.AutoSize = true;
            this.domainControllerText.Location = new System.Drawing.Point(100, 6);
            this.domainControllerText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.domainControllerText.Name = "domainControllerText";
            this.domainControllerText.Size = new System.Drawing.Size(80, 13);
            this.domainControllerText.TabIndex = 6;
            this.domainControllerText.Text = "None Detected";
            // 
            // gpoStatusText
            // 
            this.gpoStatusText.AutoSize = true;
            this.gpoStatusText.Location = new System.Drawing.Point(67, 19);
            this.gpoStatusText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.gpoStatusText.Name = "gpoStatusText";
            this.gpoStatusText.Size = new System.Drawing.Size(64, 13);
            this.gpoStatusText.TabIndex = 7;
            this.gpoStatusText.Text = "Undeployed";
            // 
            // deployGPOButton
            // 
            this.deployGPOButton.Location = new System.Drawing.Point(263, 80);
            this.deployGPOButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.deployGPOButton.Name = "deployGPOButton";
            this.deployGPOButton.Size = new System.Drawing.Size(86, 30);
            this.deployGPOButton.TabIndex = 8;
            this.deployGPOButton.Text = "Deploy GPO";
            this.deployGPOButton.UseVisualStyleBackColor = true;
            this.deployGPOButton.Visible = false;
            this.deployGPOButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.deployGPOButton_MouseUp);
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(2, 114);
            this.elementHost1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(939, 593);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.locationGrid1;
            // 
            // keepEncryptedCheckbox
            // 
            this.keepEncryptedCheckbox.AutoSize = true;
            this.keepEncryptedCheckbox.Location = new System.Drawing.Point(5, 47);
            this.keepEncryptedCheckbox.Name = "keepEncryptedCheckbox";
            this.keepEncryptedCheckbox.Size = new System.Drawing.Size(177, 17);
            this.keepEncryptedCheckbox.TabIndex = 9;
            this.keepEncryptedCheckbox.Text = "Keep TPM Machines Encrypted";
            this.keepEncryptedCheckbox.UseVisualStyleBackColor = true;
            this.keepEncryptedCheckbox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.keepEncryptedCheckbox_MouseUp);
            // 
            // saveEncryptSettingButton
            // 
            this.saveEncryptSettingButton.Location = new System.Drawing.Point(188, 43);
            this.saveEncryptSettingButton.Name = "saveEncryptSettingButton";
            this.saveEncryptSettingButton.Size = new System.Drawing.Size(75, 23);
            this.saveEncryptSettingButton.TabIndex = 10;
            this.saveEncryptSettingButton.Text = "Save";
            this.saveEncryptSettingButton.UseVisualStyleBackColor = true;
            this.saveEncryptSettingButton.Visible = false;
            this.saveEncryptSettingButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.saveEncryptSettingButton_MouseUp);
            // 
            // LocationTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.saveEncryptSettingButton);
            this.Controls.Add(this.keepEncryptedCheckbox);
            this.Controls.Add(this.deployGPOButton);
            this.Controls.Add(this.gpoStatusText);
            this.Controls.Add(this.domainControllerText);
            this.Controls.Add(this.dcLabel);
            this.Controls.Add(this.gpoStatusLabel);
            this.Controls.Add(this.encryptButton);
            this.Controls.Add(this.refreshStatus);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.elementHost1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "LocationTab";
            this.Size = new System.Drawing.Size(945, 714);
            this.Load += new System.EventHandler(this.LocationTab_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private Controls.LocationGrid locationGrid1;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ProgressBar refreshStatus;
        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.Label gpoStatusLabel;
        private System.Windows.Forms.Label dcLabel;
        private System.Windows.Forms.Label domainControllerText;
        private System.Windows.Forms.Label gpoStatusText;
        private System.Windows.Forms.Button deployGPOButton;
        private System.Windows.Forms.CheckBox keepEncryptedCheckbox;
        private System.Windows.Forms.Button saveEncryptSettingButton;
    }
}
