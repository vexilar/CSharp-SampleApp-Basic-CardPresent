namespace SampleCode
{
    partial class ManageApplicationData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageApplicationData));
            this.cmdClose = new System.Windows.Forms.Button();
            this.txtPTLSSocketId = new System.Windows.Forms.TextBox();
            this.txtApplicationProfileId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtApplicationName = new System.Windows.Forms.TextBox();
            this.txtDeveloperId = new System.Windows.Forms.TextBox();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.txtSoftwareVersion = new System.Windows.Forms.TextBox();
            this.txtSoftwareVersionDate = new System.Windows.Forms.TextBox();
            this.txtVendorId = new System.Windows.Forms.TextBox();
            this.cboApplicationAttended = new System.Windows.Forms.ComboBox();
            this.cboApplicationLocation = new System.Windows.Forms.ComboBox();
            this.cboHardwareType = new System.Windows.Forms.ComboBox();
            this.cboPINCapability = new System.Windows.Forms.ComboBox();
            this.cboReadCapability = new System.Windows.Forms.ComboBox();
            this.cmdPopulateTestValues = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CboEncryptionType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtDeviceSerialNumber = new System.Windows.Forms.TextBox();
            this.GrpGetApplicationData = new System.Windows.Forms.GroupBox();
            this.cboApplicationDataAction = new System.Windows.Forms.ComboBox();
            this.lnkManageApplicationData = new System.Windows.Forms.LinkLabel();
            this.label15 = new System.Windows.Forms.Label();
            this.CmdPerformWebRequest = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.GrpGetApplicationData.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(377, 533);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // txtPTLSSocketId
            // 
            this.txtPTLSSocketId.Location = new System.Drawing.Point(184, 244);
            this.txtPTLSSocketId.Multiline = true;
            this.txtPTLSSocketId.Name = "txtPTLSSocketId";
            this.txtPTLSSocketId.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPTLSSocketId.Size = new System.Drawing.Size(256, 51);
            this.txtPTLSSocketId.TabIndex = 39;
            // 
            // txtApplicationProfileId
            // 
            this.txtApplicationProfileId.BackColor = System.Drawing.SystemColors.Window;
            this.txtApplicationProfileId.Location = new System.Drawing.Point(115, 46);
            this.txtApplicationProfileId.Name = "txtApplicationProfileId";
            this.txtApplicationProfileId.Size = new System.Drawing.Size(88, 20);
            this.txtApplicationProfileId.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "ApplicationAttended (Required)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "ApplicationLocation (Required)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "ApplicationName (Required)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "DeveloperId (Conditional)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "PINCapability (Required)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "HardwareType (Required)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "PTLSSocketId (Required)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 304);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "ReadCapability (Required)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 330);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "SerialNumber (Required)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 356);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(136, 13);
            this.label11.TabIndex = 18;
            this.label11.Text = "SoftwareVersion (Required)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 382);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(159, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "SoftwareVersionDate (Required)";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 408);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "VendorId (Conditional)";
            // 
            // txtApplicationName
            // 
            this.txtApplicationName.Location = new System.Drawing.Point(184, 91);
            this.txtApplicationName.Name = "txtApplicationName";
            this.txtApplicationName.Size = new System.Drawing.Size(256, 20);
            this.txtApplicationName.TabIndex = 35;
            // 
            // txtDeveloperId
            // 
            this.txtDeveloperId.Location = new System.Drawing.Point(184, 117);
            this.txtDeveloperId.Name = "txtDeveloperId";
            this.txtDeveloperId.Size = new System.Drawing.Size(256, 20);
            this.txtDeveloperId.TabIndex = 36;
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Location = new System.Drawing.Point(184, 327);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(256, 20);
            this.txtSerialNumber.TabIndex = 41;
            // 
            // txtSoftwareVersion
            // 
            this.txtSoftwareVersion.Location = new System.Drawing.Point(184, 353);
            this.txtSoftwareVersion.Name = "txtSoftwareVersion";
            this.txtSoftwareVersion.Size = new System.Drawing.Size(256, 20);
            this.txtSoftwareVersion.TabIndex = 42;
            // 
            // txtSoftwareVersionDate
            // 
            this.txtSoftwareVersionDate.Location = new System.Drawing.Point(184, 379);
            this.txtSoftwareVersionDate.Name = "txtSoftwareVersionDate";
            this.txtSoftwareVersionDate.Size = new System.Drawing.Size(256, 20);
            this.txtSoftwareVersionDate.TabIndex = 43;
            // 
            // txtVendorId
            // 
            this.txtVendorId.Location = new System.Drawing.Point(184, 405);
            this.txtVendorId.Name = "txtVendorId";
            this.txtVendorId.Size = new System.Drawing.Size(256, 20);
            this.txtVendorId.TabIndex = 44;
            // 
            // cboApplicationAttended
            // 
            this.cboApplicationAttended.FormattingEnabled = true;
            this.cboApplicationAttended.Location = new System.Drawing.Point(184, 39);
            this.cboApplicationAttended.Name = "cboApplicationAttended";
            this.cboApplicationAttended.Size = new System.Drawing.Size(256, 21);
            this.cboApplicationAttended.TabIndex = 33;
            // 
            // cboApplicationLocation
            // 
            this.cboApplicationLocation.FormattingEnabled = true;
            this.cboApplicationLocation.Location = new System.Drawing.Point(184, 65);
            this.cboApplicationLocation.Name = "cboApplicationLocation";
            this.cboApplicationLocation.Size = new System.Drawing.Size(256, 21);
            this.cboApplicationLocation.TabIndex = 34;
            this.cboApplicationLocation.SelectedIndexChanged += new System.EventHandler(this.cboApplicationLocation_SelectedIndexChanged);
            // 
            // cboHardwareType
            // 
            this.cboHardwareType.FormattingEnabled = true;
            this.cboHardwareType.Location = new System.Drawing.Point(184, 192);
            this.cboHardwareType.Name = "cboHardwareType";
            this.cboHardwareType.Size = new System.Drawing.Size(256, 21);
            this.cboHardwareType.TabIndex = 37;
            this.cboHardwareType.SelectedIndexChanged += new System.EventHandler(this.cboHardwareType_SelectedIndexChanged);
            // 
            // cboPINCapability
            // 
            this.cboPINCapability.FormattingEnabled = true;
            this.cboPINCapability.Location = new System.Drawing.Point(184, 218);
            this.cboPINCapability.Name = "cboPINCapability";
            this.cboPINCapability.Size = new System.Drawing.Size(256, 21);
            this.cboPINCapability.TabIndex = 38;
            this.cboPINCapability.SelectedIndexChanged += new System.EventHandler(this.cboPINCapability_SelectedIndexChanged);
            // 
            // cboReadCapability
            // 
            this.cboReadCapability.FormattingEnabled = true;
            this.cboReadCapability.Location = new System.Drawing.Point(184, 301);
            this.cboReadCapability.Name = "cboReadCapability";
            this.cboReadCapability.Size = new System.Drawing.Size(256, 21);
            this.cboReadCapability.TabIndex = 40;
            this.cboReadCapability.SelectedIndexChanged += new System.EventHandler(this.cboReadCapability_SelectedIndexChanged);
            // 
            // cmdPopulateTestValues
            // 
            this.cmdPopulateTestValues.Location = new System.Drawing.Point(293, 10);
            this.cmdPopulateTestValues.Name = "cmdPopulateTestValues";
            this.cmdPopulateTestValues.Size = new System.Drawing.Size(147, 23);
            this.cmdPopulateTestValues.TabIndex = 45;
            this.cmdPopulateTestValues.Text = "Populate With Test Values";
            this.cmdPopulateTestValues.UseVisualStyleBackColor = true;
            this.cmdPopulateTestValues.Click += new System.EventHandler(this.cmdPopulateTestValues_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CboEncryptionType);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TxtDeviceSerialNumber);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmdPopulateTestValues);
            this.groupBox1.Controls.Add(this.cboReadCapability);
            this.groupBox1.Controls.Add(this.cboPINCapability);
            this.groupBox1.Controls.Add(this.txtPTLSSocketId);
            this.groupBox1.Controls.Add(this.cboHardwareType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboApplicationLocation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboApplicationAttended);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtVendorId);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtSoftwareVersionDate);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSoftwareVersion);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtSerialNumber);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtDeveloperId);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtApplicationName);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(12, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(446, 432);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application Data";
            // 
            // CboEncryptionType
            // 
            this.CboEncryptionType.FormattingEnabled = true;
            this.CboEncryptionType.Location = new System.Drawing.Point(184, 167);
            this.CboEncryptionType.Name = "CboEncryptionType";
            this.CboEncryptionType.Size = new System.Drawing.Size(256, 21);
            this.CboEncryptionType.TabIndex = 49;
            this.CboEncryptionType.SelectedIndexChanged += new System.EventHandler(this.CboEncryptionType_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 170);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(142, 13);
            this.label14.TabIndex = 48;
            this.label14.Text = "EncryptionType (Conditional)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "DeviceSerialNumber (Conditional)";
            // 
            // TxtDeviceSerialNumber
            // 
            this.TxtDeviceSerialNumber.AcceptsReturn = true;
            this.TxtDeviceSerialNumber.Location = new System.Drawing.Point(184, 141);
            this.TxtDeviceSerialNumber.Name = "TxtDeviceSerialNumber";
            this.TxtDeviceSerialNumber.Size = new System.Drawing.Size(256, 20);
            this.TxtDeviceSerialNumber.TabIndex = 47;
            // 
            // GrpGetApplicationData
            // 
            this.GrpGetApplicationData.Controls.Add(this.cboApplicationDataAction);
            this.GrpGetApplicationData.Controls.Add(this.lnkManageApplicationData);
            this.GrpGetApplicationData.Controls.Add(this.label15);
            this.GrpGetApplicationData.Controls.Add(this.CmdPerformWebRequest);
            this.GrpGetApplicationData.Controls.Add(this.txtApplicationProfileId);
            this.GrpGetApplicationData.Location = new System.Drawing.Point(12, 12);
            this.GrpGetApplicationData.Name = "GrpGetApplicationData";
            this.GrpGetApplicationData.Size = new System.Drawing.Size(446, 77);
            this.GrpGetApplicationData.TabIndex = 47;
            this.GrpGetApplicationData.TabStop = false;
            this.GrpGetApplicationData.Text = "Managing Application Configuration Data";
            // 
            // cboApplicationDataAction
            // 
            this.cboApplicationDataAction.FormattingEnabled = true;
            this.cboApplicationDataAction.Location = new System.Drawing.Point(6, 19);
            this.cboApplicationDataAction.Name = "cboApplicationDataAction";
            this.cboApplicationDataAction.Size = new System.Drawing.Size(197, 21);
            this.cboApplicationDataAction.TabIndex = 147;
            this.toolTip1.SetToolTip(this.cboApplicationDataAction, "Select which ApplicationData API call to use");
            this.cboApplicationDataAction.SelectedIndexChanged += new System.EventHandler(this.cboApplicationDataAction_SelectedIndexChanged);
            // 
            // lnkManageApplicationData
            // 
            this.lnkManageApplicationData.AutoSize = true;
            this.lnkManageApplicationData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkManageApplicationData.Image = ((System.Drawing.Image)(resources.GetObject("lnkManageApplicationData.Image")));
            this.lnkManageApplicationData.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lnkManageApplicationData.Location = new System.Drawing.Point(420, 41);
            this.lnkManageApplicationData.Margin = new System.Windows.Forms.Padding(0);
            this.lnkManageApplicationData.MinimumSize = new System.Drawing.Size(20, 20);
            this.lnkManageApplicationData.Name = "lnkManageApplicationData";
            this.lnkManageApplicationData.Size = new System.Drawing.Size(20, 20);
            this.lnkManageApplicationData.TabIndex = 124;
            this.lnkManageApplicationData.Tag = "Learn more about \"Sign On\"";
            this.lnkManageApplicationData.Click += new System.EventHandler(this.lnkManageApplicationData_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 49);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(103, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Application Profile Id";
            // 
            // CmdPerformWebRequest
            // 
            this.CmdPerformWebRequest.Location = new System.Drawing.Point(293, 15);
            this.CmdPerformWebRequest.Name = "CmdPerformWebRequest";
            this.CmdPerformWebRequest.Size = new System.Drawing.Size(147, 23);
            this.CmdPerformWebRequest.TabIndex = 0;
            this.CmdPerformWebRequest.Text = "Perform Web Request";
            this.CmdPerformWebRequest.UseVisualStyleBackColor = true;
            this.CmdPerformWebRequest.Click += new System.EventHandler(this.CmdPerformWebRequest_Click);
            // 
            // ManageApplicationData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 563);
            this.Controls.Add(this.GrpGetApplicationData);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdClose);
            this.ForeColor = System.Drawing.Color.DarkMagenta;
            this.Name = "ManageApplicationData";
            this.Text = "Manage Application Configuration Data - CWS 2.0.19";
            this.Load += new System.EventHandler(this.ManageApplicationData_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GrpGetApplicationData.ResumeLayout(false);
            this.GrpGetApplicationData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.TextBox txtPTLSSocketId;
        private System.Windows.Forms.TextBox txtApplicationProfileId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtApplicationName;
        private System.Windows.Forms.TextBox txtDeveloperId;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private System.Windows.Forms.TextBox txtSoftwareVersion;
        private System.Windows.Forms.TextBox txtSoftwareVersionDate;
        private System.Windows.Forms.TextBox txtVendorId;
        private System.Windows.Forms.ComboBox cboApplicationAttended;
        private System.Windows.Forms.ComboBox cboApplicationLocation;
        private System.Windows.Forms.ComboBox cboHardwareType;
        private System.Windows.Forms.ComboBox cboPINCapability;
        private System.Windows.Forms.ComboBox cboReadCapability;
        private System.Windows.Forms.Button cmdPopulateTestValues;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtDeviceSerialNumber;
        private System.Windows.Forms.ComboBox CboEncryptionType;
        private System.Windows.Forms.GroupBox GrpGetApplicationData;
        private System.Windows.Forms.Button CmdPerformWebRequest;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.LinkLabel lnkManageApplicationData;
        private System.Windows.Forms.ComboBox cboApplicationDataAction;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}