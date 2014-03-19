namespace SampleCode
{
    partial class Magensa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Magensa));
            this.TxtTrackInformation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtMagnePrintData = new System.Windows.Forms.TextBox();
            this.TxtDukptKeySerialNumber = new System.Windows.Forms.TextBox();
            this.TxtTrack2EncryptedData = new System.Windows.Forms.TextBox();
            this.TxtMagnePrintStatus = new System.Windows.Forms.TextBox();
            this.CmdGenerateFields = new System.Windows.Forms.Button();
            this.CmdProcessMagensaTxn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.TxtAmount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.CboTriggerTests = new System.Windows.Forms.ComboBox();
            this.CmdPopulateWithTestValues = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TxtDeviceSerialNumber = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.TxtScoreThreshold = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TxtMaskedPAN = new System.Windows.Forms.TextBox();
            this.CboMagensaCardType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtTrack2Masked = new System.Windows.Forms.TextBox();
            this.CmdClose = new System.Windows.Forms.Button();
            this.lnkManageAccountById = new System.Windows.Forms.LinkLabel();
            this.ChkPipeDelimited = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtTrackInformation
            // 
            this.TxtTrackInformation.AcceptsReturn = true;
            this.TxtTrackInformation.Location = new System.Drawing.Point(12, 33);
            this.TxtTrackInformation.MaxLength = 0;
            this.TxtTrackInformation.Multiline = true;
            this.TxtTrackInformation.Name = "TxtTrackInformation";
            this.TxtTrackInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TxtTrackInformation.Size = new System.Drawing.Size(597, 141);
            this.TxtTrackInformation.TabIndex = 0;
            this.toolTip1.SetToolTip(this.TxtTrackInformation, "Depending on your MagTek output please check or uncheck the above checkbox.  Once" +
        " you have pasted your MagTek read please click the Generate Fields From Track bu" +
        "tton.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Track Data from USB MSR Demo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MagnePrint data";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "DUKPT Key Serial Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Track 2 encrypted data";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "MagnePrint Status";
            // 
            // TxtMagnePrintData
            // 
            this.TxtMagnePrintData.Location = new System.Drawing.Point(152, 41);
            this.TxtMagnePrintData.Name = "TxtMagnePrintData";
            this.TxtMagnePrintData.Size = new System.Drawing.Size(439, 20);
            this.TxtMagnePrintData.TabIndex = 6;
            // 
            // TxtDukptKeySerialNumber
            // 
            this.TxtDukptKeySerialNumber.Location = new System.Drawing.Point(152, 67);
            this.TxtDukptKeySerialNumber.Name = "TxtDukptKeySerialNumber";
            this.TxtDukptKeySerialNumber.Size = new System.Drawing.Size(439, 20);
            this.TxtDukptKeySerialNumber.TabIndex = 7;
            // 
            // TxtTrack2EncryptedData
            // 
            this.TxtTrack2EncryptedData.Location = new System.Drawing.Point(152, 93);
            this.TxtTrack2EncryptedData.Name = "TxtTrack2EncryptedData";
            this.TxtTrack2EncryptedData.Size = new System.Drawing.Size(439, 20);
            this.TxtTrack2EncryptedData.TabIndex = 8;
            // 
            // TxtMagnePrintStatus
            // 
            this.TxtMagnePrintStatus.Location = new System.Drawing.Point(152, 119);
            this.TxtMagnePrintStatus.Name = "TxtMagnePrintStatus";
            this.TxtMagnePrintStatus.Size = new System.Drawing.Size(439, 20);
            this.TxtMagnePrintStatus.TabIndex = 9;
            // 
            // CmdGenerateFields
            // 
            this.CmdGenerateFields.Location = new System.Drawing.Point(431, 12);
            this.CmdGenerateFields.Name = "CmdGenerateFields";
            this.CmdGenerateFields.Size = new System.Drawing.Size(160, 23);
            this.CmdGenerateFields.TabIndex = 10;
            this.CmdGenerateFields.Text = "Generate Fields From Track Data";
            this.CmdGenerateFields.UseVisualStyleBackColor = true;
            this.CmdGenerateFields.Click += new System.EventHandler(this.CmdGenerateFields_Click);
            // 
            // CmdProcessMagensaTxn
            // 
            this.CmdProcessMagensaTxn.Location = new System.Drawing.Point(164, 569);
            this.CmdProcessMagensaTxn.Name = "CmdProcessMagensaTxn";
            this.CmdProcessMagensaTxn.Size = new System.Drawing.Size(160, 23);
            this.CmdProcessMagensaTxn.TabIndex = 11;
            this.CmdProcessMagensaTxn.Text = "Process Magensa Transaction";
            this.CmdProcessMagensaTxn.UseVisualStyleBackColor = true;
            this.CmdProcessMagensaTxn.Click += new System.EventHandler(this.CmdUseValues_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 543);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Amount";
            // 
            // TxtAmount
            // 
            this.TxtAmount.Location = new System.Drawing.Point(164, 540);
            this.TxtAmount.Name = "TxtAmount";
            this.TxtAmount.Size = new System.Drawing.Size(60, 20);
            this.TxtAmount.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 516);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Trigger tests";
            // 
            // CboTriggerTests
            // 
            this.CboTriggerTests.FormattingEnabled = true;
            this.CboTriggerTests.Location = new System.Drawing.Point(164, 513);
            this.CboTriggerTests.Name = "CboTriggerTests";
            this.CboTriggerTests.Size = new System.Drawing.Size(439, 21);
            this.CboTriggerTests.TabIndex = 15;
            this.CboTriggerTests.SelectedIndexChanged += new System.EventHandler(this.CboTriggerTests_SelectedIndexChanged);
            // 
            // CmdPopulateWithTestValues
            // 
            this.CmdPopulateWithTestValues.Location = new System.Drawing.Point(443, 4);
            this.CmdPopulateWithTestValues.Name = "CmdPopulateWithTestValues";
            this.CmdPopulateWithTestValues.Size = new System.Drawing.Size(160, 23);
            this.CmdPopulateWithTestValues.TabIndex = 16;
            this.CmdPopulateWithTestValues.Text = "Populate with test values";
            this.CmdPopulateWithTestValues.UseVisualStyleBackColor = true;
            this.CmdPopulateWithTestValues.Click += new System.EventHandler(this.CmdPopulateWithTestValues_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.TxtDeviceSerialNumber);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.TxtScoreThreshold);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.TxtMaskedPAN);
            this.groupBox1.Controls.Add(this.CboMagensaCardType);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.TxtTrack2Masked);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.CmdGenerateFields);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TxtMagnePrintData);
            this.groupBox1.Controls.Add(this.TxtDukptKeySerialNumber);
            this.groupBox1.Controls.Add(this.TxtTrack2EncryptedData);
            this.groupBox1.Controls.Add(this.TxtMagnePrintStatus);
            this.groupBox1.Location = new System.Drawing.Point(12, 191);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 316);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Obtain values from swipe above";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 228);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(110, 13);
            this.label12.TabIndex = 149;
            this.label12.Text = "Device Serial Number";
            // 
            // TxtDeviceSerialNumber
            // 
            this.TxtDeviceSerialNumber.Location = new System.Drawing.Point(152, 225);
            this.TxtDeviceSerialNumber.Name = "TxtDeviceSerialNumber";
            this.TxtDeviceSerialNumber.Size = new System.Drawing.Size(155, 20);
            this.TxtDeviceSerialNumber.TabIndex = 150;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.Image = ((System.Drawing.Image)(resources.GetObject("linkLabel1.Image")));
            this.linkLabel1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.linkLabel1.Location = new System.Drawing.Point(215, 251);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabel1.MinimumSize = new System.Drawing.Size(20, 20);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(20, 20);
            this.linkLabel1.TabIndex = 148;
            this.linkLabel1.Tag = "";
            this.toolTip1.SetToolTip(this.linkLabel1, "Click Here to view more information");
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 254);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 13);
            this.label11.TabIndex = 146;
            this.label11.Text = "Score Threshold (0-1)";
            // 
            // TxtScoreThreshold
            // 
            this.TxtScoreThreshold.Location = new System.Drawing.Point(152, 251);
            this.TxtScoreThreshold.Name = "TxtScoreThreshold";
            this.TxtScoreThreshold.Size = new System.Drawing.Size(60, 20);
            this.TxtScoreThreshold.TabIndex = 147;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 174);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 142;
            this.label9.Text = "CardType";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 201);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 144;
            this.label10.Text = "Masked PAN";
            // 
            // TxtMaskedPAN
            // 
            this.TxtMaskedPAN.Location = new System.Drawing.Point(152, 198);
            this.TxtMaskedPAN.Name = "TxtMaskedPAN";
            this.TxtMaskedPAN.Size = new System.Drawing.Size(155, 20);
            this.TxtMaskedPAN.TabIndex = 145;
            // 
            // CboMagensaCardType
            // 
            this.CboMagensaCardType.FormattingEnabled = true;
            this.CboMagensaCardType.Location = new System.Drawing.Point(152, 171);
            this.CboMagensaCardType.Name = "CboMagensaCardType";
            this.CboMagensaCardType.Size = new System.Drawing.Size(155, 21);
            this.CboMagensaCardType.TabIndex = 143;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 140;
            this.label8.Text = "Track 2 Masked";
            // 
            // TxtTrack2Masked
            // 
            this.TxtTrack2Masked.Location = new System.Drawing.Point(152, 145);
            this.TxtTrack2Masked.Name = "TxtTrack2Masked";
            this.TxtTrack2Masked.Size = new System.Drawing.Size(261, 20);
            this.TxtTrack2Masked.TabIndex = 141;
            // 
            // CmdClose
            // 
            this.CmdClose.Location = new System.Drawing.Point(534, 578);
            this.CmdClose.Name = "CmdClose";
            this.CmdClose.Size = new System.Drawing.Size(75, 23);
            this.CmdClose.TabIndex = 18;
            this.CmdClose.Text = "Close";
            this.CmdClose.UseVisualStyleBackColor = true;
            this.CmdClose.Click += new System.EventHandler(this.CmdClose_Click);
            // 
            // lnkManageAccountById
            // 
            this.lnkManageAccountById.AutoSize = true;
            this.lnkManageAccountById.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkManageAccountById.Image = ((System.Drawing.Image)(resources.GetObject("lnkManageAccountById.Image")));
            this.lnkManageAccountById.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lnkManageAccountById.Location = new System.Drawing.Point(182, 10);
            this.lnkManageAccountById.Margin = new System.Windows.Forms.Padding(0);
            this.lnkManageAccountById.MinimumSize = new System.Drawing.Size(20, 20);
            this.lnkManageAccountById.Name = "lnkManageAccountById";
            this.lnkManageAccountById.Size = new System.Drawing.Size(20, 20);
            this.lnkManageAccountById.TabIndex = 133;
            this.lnkManageAccountById.Tag = "";
            this.toolTip1.SetToolTip(this.lnkManageAccountById, "Click Here to view more information");
            this.lnkManageAccountById.Click += new System.EventHandler(this.lnkManageAccountById_Click);
            // 
            // ChkPipeDelimited
            // 
            this.ChkPipeDelimited.AutoSize = true;
            this.ChkPipeDelimited.Checked = true;
            this.ChkPipeDelimited.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkPipeDelimited.Location = new System.Drawing.Point(230, 15);
            this.ChkPipeDelimited.Name = "ChkPipeDelimited";
            this.ChkPipeDelimited.Size = new System.Drawing.Size(175, 17);
            this.ChkPipeDelimited.TabIndex = 134;
            this.ChkPipeDelimited.Text = "MagTek Read is Pipe Delimited";
            this.ChkPipeDelimited.UseVisualStyleBackColor = true;
            // 
            // Magensa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 612);
            this.Controls.Add(this.ChkPipeDelimited);
            this.Controls.Add(this.CboTriggerTests);
            this.Controls.Add(this.lnkManageAccountById);
            this.Controls.Add(this.CmdClose);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CmdPopulateWithTestValues);
            this.Controls.Add(this.TxtAmount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CmdProcessMagensaTxn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtTrackInformation);
            this.ForeColor = System.Drawing.Color.DarkMagenta;
            this.Name = "Magensa";
            this.Text = "Magensa";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtTrackInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtMagnePrintData;
        private System.Windows.Forms.TextBox TxtDukptKeySerialNumber;
        private System.Windows.Forms.TextBox TxtTrack2EncryptedData;
        private System.Windows.Forms.TextBox TxtMagnePrintStatus;
        private System.Windows.Forms.Button CmdGenerateFields;
        private System.Windows.Forms.Button CmdProcessMagensaTxn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TxtAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CboTriggerTests;
        private System.Windows.Forms.Button CmdPopulateWithTestValues;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CmdClose;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox TxtMaskedPAN;
        private System.Windows.Forms.ComboBox CboMagensaCardType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtTrack2Masked;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TxtScoreThreshold;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel lnkManageAccountById;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TxtDeviceSerialNumber;
        private System.Windows.Forms.CheckBox ChkPipeDelimited;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}