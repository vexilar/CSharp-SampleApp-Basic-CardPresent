namespace SampleCode
{
    partial class TransactionManagementServices
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
            System.Windows.Forms.LinkLabel lnkQueryTransactionDetails;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransactionManagementServices));
            System.Windows.Forms.LinkLabel lnkQueryTransactions;
            this.label44 = new System.Windows.Forms.Label();
            this.grpQueryTransactionsParameters = new System.Windows.Forms.GroupBox();
            this.TxtTransactionDetailFormat = new System.Windows.Forms.TextBox();
            this.CboTransactionDetailFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CboQTP_ServiceIds = new System.Windows.Forms.ComboBox();
            this.CboQTP_MerchantProfileIds = new System.Windows.Forms.ComboBox();
            this.TxtClearTransactionIds = new System.Windows.Forms.Button();
            this.txtQTP_OrderNumber = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkIncludeRelated = new System.Windows.Forms.CheckBox();
            this.lnkQueryTransactionFamilies = new System.Windows.Forms.LinkLabel();
            this.cboQTP_CardTypes = new System.Windows.Forms.ComboBox();
            this.cboQTP_CaptureStates = new System.Windows.Forms.ComboBox();
            this.cboQTP_TransactionStates = new System.Windows.Forms.ComboBox();
            this.cboQTP_QueryType = new System.Windows.Forms.ComboBox();
            this.cboQTP_IsAcknowledged = new System.Windows.Forms.ComboBox();
            this.txtQTP_ApprovalCodes = new System.Windows.Forms.TextBox();
            this.txtQTP_TransactionIds = new System.Windows.Forms.TextBox();
            this.cmdQueryTransactionsDetail = new System.Windows.Forms.Button();
            this.txtQTP_ServiceKeys = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.cmdQueryTransactionsSummary = new System.Windows.Forms.Button();
            this.txtQTP_TransactionClassTypePairs = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.cmdQueryTransactionFamilies = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.txtQTP_Amounts = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtQTP_BatchIds = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.chkUseTransactionIdSelected = new System.Windows.Forms.CheckBox();
            this.grpQueryBatchParameters = new System.Windows.Forms.GroupBox();
            this.CboQBP_MercProfileIds = new System.Windows.Forms.ComboBox();
            this.TxtClearBatchTransactionIds = new System.Windows.Forms.Button();
            this.lnkQueryBatch = new System.Windows.Forms.LinkLabel();
            this.txtQBP_TransactionIds = new System.Windows.Forms.TextBox();
            this.txtQBP_BatchIds = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtQBP_ServiceKeys = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.cmdQueryBatch = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpEndTimeTMS = new System.Windows.Forms.DateTimePicker();
            this.dtpStartTimeTMS = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.chklstTMSResults = new System.Windows.Forms.CheckedListBox();
            this.LnkNext = new System.Windows.Forms.LinkLabel();
            this.LnkPrevious = new System.Windows.Forms.LinkLabel();
            this.lblPageNumber = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RtxtTMSResults = new System.Windows.Forms.RichTextBox();
            this.LblCurrentUTCTime = new System.Windows.Forms.Label();
            this.TmrUTCTIme = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            lnkQueryTransactionDetails = new System.Windows.Forms.LinkLabel();
            lnkQueryTransactions = new System.Windows.Forms.LinkLabel();
            this.grpQueryTransactionsParameters.SuspendLayout();
            this.grpQueryBatchParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkQueryTransactionDetails
            // 
            lnkQueryTransactionDetails.AutoSize = true;
            lnkQueryTransactionDetails.Cursor = System.Windows.Forms.Cursors.Hand;
            lnkQueryTransactionDetails.Image = ((System.Drawing.Image)(resources.GetObject("lnkQueryTransactionDetails.Image")));
            lnkQueryTransactionDetails.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            lnkQueryTransactionDetails.Location = new System.Drawing.Point(431, 114);
            lnkQueryTransactionDetails.Margin = new System.Windows.Forms.Padding(0);
            lnkQueryTransactionDetails.MinimumSize = new System.Drawing.Size(20, 20);
            lnkQueryTransactionDetails.Name = "lnkQueryTransactionDetails";
            lnkQueryTransactionDetails.Size = new System.Drawing.Size(20, 20);
            lnkQueryTransactionDetails.TabIndex = 130;
            lnkQueryTransactionDetails.Tag = "";
            this.toolTip1.SetToolTip(lnkQueryTransactionDetails, "Click Here to view more information");
            lnkQueryTransactionDetails.UseCompatibleTextRendering = true;
            lnkQueryTransactionDetails.Click += new System.EventHandler(this.lnkQueryTransactionDetails_Click);
            // 
            // lnkQueryTransactions
            // 
            lnkQueryTransactions.AutoSize = true;
            lnkQueryTransactions.Cursor = System.Windows.Forms.Cursors.Hand;
            lnkQueryTransactions.Image = ((System.Drawing.Image)(resources.GetObject("lnkQueryTransactions.Image")));
            lnkQueryTransactions.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            lnkQueryTransactions.Location = new System.Drawing.Point(431, 62);
            lnkQueryTransactions.Margin = new System.Windows.Forms.Padding(0);
            lnkQueryTransactions.MinimumSize = new System.Drawing.Size(20, 20);
            lnkQueryTransactions.Name = "lnkQueryTransactions";
            lnkQueryTransactions.Size = new System.Drawing.Size(20, 20);
            lnkQueryTransactions.TabIndex = 128;
            lnkQueryTransactions.Tag = "";
            this.toolTip1.SetToolTip(lnkQueryTransactions, "Click Here to view more information");
            lnkQueryTransactions.UseCompatibleTextRendering = true;
            lnkQueryTransactions.Click += new System.EventHandler(this.lnkQueryTransactions_Click);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.Color.DarkOrange;
            this.label44.Location = new System.Drawing.Point(12, 9);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(178, 24);
            this.label44.TabIndex = 31;
            this.label44.Text = "TMS Web Service";
            // 
            // grpQueryTransactionsParameters
            // 
            this.grpQueryTransactionsParameters.Controls.Add(this.TxtTransactionDetailFormat);
            this.grpQueryTransactionsParameters.Controls.Add(this.CboTransactionDetailFormat);
            this.grpQueryTransactionsParameters.Controls.Add(this.label5);
            this.grpQueryTransactionsParameters.Controls.Add(this.CboQTP_ServiceIds);
            this.grpQueryTransactionsParameters.Controls.Add(this.CboQTP_MerchantProfileIds);
            this.grpQueryTransactionsParameters.Controls.Add(this.TxtClearTransactionIds);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_OrderNumber);
            this.grpQueryTransactionsParameters.Controls.Add(this.label4);
            this.grpQueryTransactionsParameters.Controls.Add(this.chkIncludeRelated);
            this.grpQueryTransactionsParameters.Controls.Add(lnkQueryTransactionDetails);
            this.grpQueryTransactionsParameters.Controls.Add(this.lnkQueryTransactionFamilies);
            this.grpQueryTransactionsParameters.Controls.Add(lnkQueryTransactions);
            this.grpQueryTransactionsParameters.Controls.Add(this.cboQTP_CardTypes);
            this.grpQueryTransactionsParameters.Controls.Add(this.cboQTP_CaptureStates);
            this.grpQueryTransactionsParameters.Controls.Add(this.cboQTP_TransactionStates);
            this.grpQueryTransactionsParameters.Controls.Add(this.cboQTP_QueryType);
            this.grpQueryTransactionsParameters.Controls.Add(this.cboQTP_IsAcknowledged);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_ApprovalCodes);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_TransactionIds);
            this.grpQueryTransactionsParameters.Controls.Add(this.cmdQueryTransactionsDetail);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_ServiceKeys);
            this.grpQueryTransactionsParameters.Controls.Add(this.label43);
            this.grpQueryTransactionsParameters.Controls.Add(this.cmdQueryTransactionsSummary);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_TransactionClassTypePairs);
            this.grpQueryTransactionsParameters.Controls.Add(this.label42);
            this.grpQueryTransactionsParameters.Controls.Add(this.label41);
            this.grpQueryTransactionsParameters.Controls.Add(this.cmdQueryTransactionFamilies);
            this.grpQueryTransactionsParameters.Controls.Add(this.label40);
            this.grpQueryTransactionsParameters.Controls.Add(this.label39);
            this.grpQueryTransactionsParameters.Controls.Add(this.label38);
            this.grpQueryTransactionsParameters.Controls.Add(this.label37);
            this.grpQueryTransactionsParameters.Controls.Add(this.label31);
            this.grpQueryTransactionsParameters.Controls.Add(this.label36);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_Amounts);
            this.grpQueryTransactionsParameters.Controls.Add(this.label35);
            this.grpQueryTransactionsParameters.Controls.Add(this.label32);
            this.grpQueryTransactionsParameters.Controls.Add(this.label33);
            this.grpQueryTransactionsParameters.Controls.Add(this.txtQTP_BatchIds);
            this.grpQueryTransactionsParameters.Controls.Add(this.label34);
            this.grpQueryTransactionsParameters.Controls.Add(this.label30);
            this.grpQueryTransactionsParameters.Location = new System.Drawing.Point(8, 277);
            this.grpQueryTransactionsParameters.Name = "grpQueryTransactionsParameters";
            this.grpQueryTransactionsParameters.Size = new System.Drawing.Size(453, 523);
            this.grpQueryTransactionsParameters.TabIndex = 28;
            this.grpQueryTransactionsParameters.TabStop = false;
            this.grpQueryTransactionsParameters.Text = "Query Transactions Parameters";
            // 
            // TxtTransactionDetailFormat
            // 
            this.TxtTransactionDetailFormat.Enabled = false;
            this.TxtTransactionDetailFormat.Location = new System.Drawing.Point(26, 447);
            this.TxtTransactionDetailFormat.MaxLength = 0;
            this.TxtTransactionDetailFormat.Multiline = true;
            this.TxtTransactionDetailFormat.Name = "TxtTransactionDetailFormat";
            this.TxtTransactionDetailFormat.Size = new System.Drawing.Size(402, 70);
            this.TxtTransactionDetailFormat.TabIndex = 139;
            this.TxtTransactionDetailFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtTransactionDetailFormat_KeyDown);
            // 
            // CboTransactionDetailFormat
            // 
            this.CboTransactionDetailFormat.FormattingEnabled = true;
            this.CboTransactionDetailFormat.Location = new System.Drawing.Point(214, 420);
            this.CboTransactionDetailFormat.Name = "CboTransactionDetailFormat";
            this.CboTransactionDetailFormat.Size = new System.Drawing.Size(214, 21);
            this.CboTransactionDetailFormat.TabIndex = 137;
            this.CboTransactionDetailFormat.SelectedIndexChanged += new System.EventHandler(this.CboTransactionDetailFormat_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 427);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 13);
            this.label5.TabIndex = 138;
            this.label5.Text = "Transaction Detail Format (Txn Detail)";
            // 
            // CboQTP_ServiceIds
            // 
            this.CboQTP_ServiceIds.FormattingEnabled = true;
            this.CboQTP_ServiceIds.Location = new System.Drawing.Point(129, 293);
            this.CboQTP_ServiceIds.Name = "CboQTP_ServiceIds";
            this.CboQTP_ServiceIds.Size = new System.Drawing.Size(299, 21);
            this.CboQTP_ServiceIds.TabIndex = 136;
            // 
            // CboQTP_MerchantProfileIds
            // 
            this.CboQTP_MerchantProfileIds.FormattingEnabled = true;
            this.CboQTP_MerchantProfileIds.Location = new System.Drawing.Point(129, 241);
            this.CboQTP_MerchantProfileIds.Name = "CboQTP_MerchantProfileIds";
            this.CboQTP_MerchantProfileIds.Size = new System.Drawing.Size(299, 21);
            this.CboQTP_MerchantProfileIds.TabIndex = 135;
            // 
            // TxtClearTransactionIds
            // 
            this.TxtClearTransactionIds.Location = new System.Drawing.Point(387, 369);
            this.TxtClearTransactionIds.Name = "TxtClearTransactionIds";
            this.TxtClearTransactionIds.Size = new System.Drawing.Size(41, 23);
            this.TxtClearTransactionIds.TabIndex = 134;
            this.TxtClearTransactionIds.Text = "Clear";
            this.TxtClearTransactionIds.UseVisualStyleBackColor = true;
            this.TxtClearTransactionIds.Click += new System.EventHandler(this.TxtClearTransactionIds_Click);
            // 
            // txtQTP_OrderNumber
            // 
            this.txtQTP_OrderNumber.Location = new System.Drawing.Point(129, 139);
            this.txtQTP_OrderNumber.Name = "txtQTP_OrderNumber";
            this.txtQTP_OrderNumber.Size = new System.Drawing.Size(299, 20);
            this.txtQTP_OrderNumber.TabIndex = 133;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 132;
            this.label4.Text = "Order Number";
            // 
            // chkIncludeRelated
            // 
            this.chkIncludeRelated.AutoSize = true;
            this.chkIncludeRelated.Location = new System.Drawing.Point(286, 42);
            this.chkIncludeRelated.Name = "chkIncludeRelated";
            this.chkIncludeRelated.Size = new System.Drawing.Size(101, 17);
            this.chkIncludeRelated.TabIndex = 131;
            this.chkIncludeRelated.Text = "Include Related";
            this.chkIncludeRelated.UseVisualStyleBackColor = true;
            // 
            // lnkQueryTransactionFamilies
            // 
            this.lnkQueryTransactionFamilies.AutoSize = true;
            this.lnkQueryTransactionFamilies.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkQueryTransactionFamilies.Image = ((System.Drawing.Image)(resources.GetObject("lnkQueryTransactionFamilies.Image")));
            this.lnkQueryTransactionFamilies.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lnkQueryTransactionFamilies.Location = new System.Drawing.Point(431, 88);
            this.lnkQueryTransactionFamilies.Margin = new System.Windows.Forms.Padding(0);
            this.lnkQueryTransactionFamilies.MinimumSize = new System.Drawing.Size(20, 20);
            this.lnkQueryTransactionFamilies.Name = "lnkQueryTransactionFamilies";
            this.lnkQueryTransactionFamilies.Size = new System.Drawing.Size(20, 20);
            this.lnkQueryTransactionFamilies.TabIndex = 129;
            this.lnkQueryTransactionFamilies.Tag = "";
            this.toolTip1.SetToolTip(this.lnkQueryTransactionFamilies, "Click Here to view more information");
            this.lnkQueryTransactionFamilies.UseCompatibleTextRendering = true;
            this.lnkQueryTransactionFamilies.Click += new System.EventHandler(this.lnkQueryTransactionFamilies_Click);
            // 
            // cboQTP_CardTypes
            // 
            this.cboQTP_CardTypes.FormattingEnabled = true;
            this.cboQTP_CardTypes.Location = new System.Drawing.Point(129, 189);
            this.cboQTP_CardTypes.Name = "cboQTP_CardTypes";
            this.cboQTP_CardTypes.Size = new System.Drawing.Size(299, 21);
            this.cboQTP_CardTypes.TabIndex = 11;
            // 
            // cboQTP_CaptureStates
            // 
            this.cboQTP_CaptureStates.FormattingEnabled = true;
            this.cboQTP_CaptureStates.Location = new System.Drawing.Point(129, 163);
            this.cboQTP_CaptureStates.Name = "cboQTP_CaptureStates";
            this.cboQTP_CaptureStates.Size = new System.Drawing.Size(299, 21);
            this.cboQTP_CaptureStates.TabIndex = 10;
            // 
            // cboQTP_TransactionStates
            // 
            this.cboQTP_TransactionStates.FormattingEnabled = true;
            this.cboQTP_TransactionStates.Location = new System.Drawing.Point(129, 393);
            this.cboQTP_TransactionStates.Name = "cboQTP_TransactionStates";
            this.cboQTP_TransactionStates.Size = new System.Drawing.Size(299, 21);
            this.cboQTP_TransactionStates.TabIndex = 19;
            // 
            // cboQTP_QueryType
            // 
            this.cboQTP_QueryType.FormattingEnabled = true;
            this.cboQTP_QueryType.Location = new System.Drawing.Point(129, 267);
            this.cboQTP_QueryType.Name = "cboQTP_QueryType";
            this.cboQTP_QueryType.Size = new System.Drawing.Size(299, 21);
            this.cboQTP_QueryType.TabIndex = 14;
            // 
            // cboQTP_IsAcknowledged
            // 
            this.cboQTP_IsAcknowledged.FormattingEnabled = true;
            this.cboQTP_IsAcknowledged.Location = new System.Drawing.Point(129, 215);
            this.cboQTP_IsAcknowledged.Name = "cboQTP_IsAcknowledged";
            this.cboQTP_IsAcknowledged.Size = new System.Drawing.Size(299, 21);
            this.cboQTP_IsAcknowledged.TabIndex = 12;
            // 
            // txtQTP_ApprovalCodes
            // 
            this.txtQTP_ApprovalCodes.Location = new System.Drawing.Point(129, 88);
            this.txtQTP_ApprovalCodes.Name = "txtQTP_ApprovalCodes";
            this.txtQTP_ApprovalCodes.Size = new System.Drawing.Size(136, 20);
            this.txtQTP_ApprovalCodes.TabIndex = 8;
            // 
            // txtQTP_TransactionIds
            // 
            this.txtQTP_TransactionIds.Location = new System.Drawing.Point(129, 370);
            this.txtQTP_TransactionIds.Name = "txtQTP_TransactionIds";
            this.txtQTP_TransactionIds.Size = new System.Drawing.Size(252, 20);
            this.txtQTP_TransactionIds.TabIndex = 18;
            // 
            // cmdQueryTransactionsDetail
            // 
            this.cmdQueryTransactionsDetail.Location = new System.Drawing.Point(271, 113);
            this.cmdQueryTransactionsDetail.Name = "cmdQueryTransactionsDetail";
            this.cmdQueryTransactionsDetail.Size = new System.Drawing.Size(157, 23);
            this.cmdQueryTransactionsDetail.TabIndex = 12;
            this.cmdQueryTransactionsDetail.Text = "Query Transactions Detail";
            this.cmdQueryTransactionsDetail.UseVisualStyleBackColor = true;
            this.cmdQueryTransactionsDetail.Click += new System.EventHandler(this.cmdQueryTransactionsDetail_Click);
            // 
            // txtQTP_ServiceKeys
            // 
            this.txtQTP_ServiceKeys.Location = new System.Drawing.Point(129, 319);
            this.txtQTP_ServiceKeys.Name = "txtQTP_ServiceKeys";
            this.txtQTP_ServiceKeys.Size = new System.Drawing.Size(299, 20);
            this.txtQTP_ServiceKeys.TabIndex = 16;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(23, 400);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(99, 13);
            this.label43.TabIndex = 33;
            this.label43.Text = "TransactionState(s)";
            // 
            // cmdQueryTransactionsSummary
            // 
            this.cmdQueryTransactionsSummary.Location = new System.Drawing.Point(271, 61);
            this.cmdQueryTransactionsSummary.Name = "cmdQueryTransactionsSummary";
            this.cmdQueryTransactionsSummary.Size = new System.Drawing.Size(157, 23);
            this.cmdQueryTransactionsSummary.TabIndex = 11;
            this.cmdQueryTransactionsSummary.Text = "Query Transactions Summary";
            this.cmdQueryTransactionsSummary.UseVisualStyleBackColor = true;
            this.cmdQueryTransactionsSummary.Click += new System.EventHandler(this.cmdQueryTransactionsSummary_Click);
            // 
            // txtQTP_TransactionClassTypePairs
            // 
            this.txtQTP_TransactionClassTypePairs.Enabled = false;
            this.txtQTP_TransactionClassTypePairs.Location = new System.Drawing.Point(170, 344);
            this.txtQTP_TransactionClassTypePairs.Name = "txtQTP_TransactionClassTypePairs";
            this.txtQTP_TransactionClassTypePairs.Size = new System.Drawing.Size(258, 20);
            this.txtQTP_TransactionClassTypePairs.TabIndex = 17;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Enabled = false;
            this.label42.Location = new System.Drawing.Point(23, 348);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(141, 13);
            this.label42.TabIndex = 31;
            this.label42.Text = "TransactionClassTypePair(s)";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(23, 296);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(63, 13);
            this.label41.TabIndex = 29;
            this.label41.Text = "ServiceId(s)";
            // 
            // cmdQueryTransactionFamilies
            // 
            this.cmdQueryTransactionFamilies.Location = new System.Drawing.Point(271, 87);
            this.cmdQueryTransactionFamilies.Name = "cmdQueryTransactionFamilies";
            this.cmdQueryTransactionFamilies.Size = new System.Drawing.Size(157, 23);
            this.cmdQueryTransactionFamilies.TabIndex = 10;
            this.cmdQueryTransactionFamilies.Text = "Query Transaction Families";
            this.cmdQueryTransactionFamilies.UseVisualStyleBackColor = true;
            this.cmdQueryTransactionFamilies.Click += new System.EventHandler(this.cmdQueryTransactionFamilies_Click);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(23, 270);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(59, 13);
            this.label40.TabIndex = 27;
            this.label40.Text = "QueryType";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(23, 218);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(86, 13);
            this.label39.TabIndex = 25;
            this.label39.Text = "IsAcknowledged";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(23, 192);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(64, 13);
            this.label38.TabIndex = 23;
            this.label38.Text = "CardType(s)";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(23, 166);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(80, 13);
            this.label37.TabIndex = 21;
            this.label37.Text = "CaptureState(s)";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(23, 374);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(83, 13);
            this.label31.TabIndex = 15;
            this.label31.Text = "TransactionId(s)";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(23, 91);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(85, 13);
            this.label36.TabIndex = 19;
            this.label36.Text = "ApprovalCode(s)";
            // 
            // txtQTP_Amounts
            // 
            this.txtQTP_Amounts.Location = new System.Drawing.Point(129, 62);
            this.txtQTP_Amounts.Name = "txtQTP_Amounts";
            this.txtQTP_Amounts.Size = new System.Drawing.Size(136, 20);
            this.txtQTP_Amounts.TabIndex = 7;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(23, 65);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(54, 13);
            this.label35.TabIndex = 17;
            this.label35.Text = "Amount(s)";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(23, 322);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(72, 13);
            this.label32.TabIndex = 13;
            this.label32.Text = "ServiceKey(s)";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(23, 244);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(80, 13);
            this.label33.TabIndex = 11;
            this.label33.Text = "MercProfileId(s)";
            // 
            // txtQTP_BatchIds
            // 
            this.txtQTP_BatchIds.Location = new System.Drawing.Point(129, 114);
            this.txtQTP_BatchIds.Name = "txtQTP_BatchIds";
            this.txtQTP_BatchIds.Size = new System.Drawing.Size(136, 20);
            this.txtQTP_BatchIds.TabIndex = 9;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(23, 117);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(55, 13);
            this.label34.TabIndex = 9;
            this.label34.Text = "BatchId(s)";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.DarkOrange;
            this.label30.Location = new System.Drawing.Point(23, 16);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(349, 15);
            this.label30.TabIndex = 3;
            this.label30.Text = "Please use a comma to seperate more than one entry";
            // 
            // chkUseTransactionIdSelected
            // 
            this.chkUseTransactionIdSelected.AutoSize = true;
            this.chkUseTransactionIdSelected.Location = new System.Drawing.Point(616, 57);
            this.chkUseTransactionIdSelected.Name = "chkUseTransactionIdSelected";
            this.chkUseTransactionIdSelected.Size = new System.Drawing.Size(154, 17);
            this.chkUseTransactionIdSelected.TabIndex = 36;
            this.chkUseTransactionIdSelected.Text = "Use Selected Transactions";
            this.chkUseTransactionIdSelected.UseVisualStyleBackColor = true;
            // 
            // grpQueryBatchParameters
            // 
            this.grpQueryBatchParameters.Controls.Add(this.CboQBP_MercProfileIds);
            this.grpQueryBatchParameters.Controls.Add(this.TxtClearBatchTransactionIds);
            this.grpQueryBatchParameters.Controls.Add(this.lnkQueryBatch);
            this.grpQueryBatchParameters.Controls.Add(this.txtQBP_TransactionIds);
            this.grpQueryBatchParameters.Controls.Add(this.txtQBP_BatchIds);
            this.grpQueryBatchParameters.Controls.Add(this.label29);
            this.grpQueryBatchParameters.Controls.Add(this.txtQBP_ServiceKeys);
            this.grpQueryBatchParameters.Controls.Add(this.label28);
            this.grpQueryBatchParameters.Controls.Add(this.label27);
            this.grpQueryBatchParameters.Controls.Add(this.label26);
            this.grpQueryBatchParameters.Controls.Add(this.label20);
            this.grpQueryBatchParameters.Controls.Add(this.cmdQueryBatch);
            this.grpQueryBatchParameters.Location = new System.Drawing.Point(8, 120);
            this.grpQueryBatchParameters.Name = "grpQueryBatchParameters";
            this.grpQueryBatchParameters.Size = new System.Drawing.Size(453, 141);
            this.grpQueryBatchParameters.TabIndex = 27;
            this.grpQueryBatchParameters.TabStop = false;
            this.grpQueryBatchParameters.Text = "Query Batch Parameters";
            // 
            // CboQBP_MercProfileIds
            // 
            this.CboQBP_MercProfileIds.FormattingEnabled = true;
            this.CboQBP_MercProfileIds.Location = new System.Drawing.Point(130, 61);
            this.CboQBP_MercProfileIds.Name = "CboQBP_MercProfileIds";
            this.CboQBP_MercProfileIds.Size = new System.Drawing.Size(135, 21);
            this.CboQBP_MercProfileIds.TabIndex = 136;
            // 
            // TxtClearBatchTransactionIds
            // 
            this.TxtClearBatchTransactionIds.Location = new System.Drawing.Point(387, 107);
            this.TxtClearBatchTransactionIds.Name = "TxtClearBatchTransactionIds";
            this.TxtClearBatchTransactionIds.Size = new System.Drawing.Size(41, 23);
            this.TxtClearBatchTransactionIds.TabIndex = 135;
            this.TxtClearBatchTransactionIds.Text = "Clear";
            this.TxtClearBatchTransactionIds.UseVisualStyleBackColor = true;
            this.TxtClearBatchTransactionIds.Click += new System.EventHandler(this.TxtClearBatchTransactionIds_Click);
            // 
            // lnkQueryBatch
            // 
            this.lnkQueryBatch.AutoSize = true;
            this.lnkQueryBatch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkQueryBatch.Image = ((System.Drawing.Image)(resources.GetObject("lnkQueryBatch.Image")));
            this.lnkQueryBatch.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lnkQueryBatch.Location = new System.Drawing.Point(430, 35);
            this.lnkQueryBatch.Margin = new System.Windows.Forms.Padding(0);
            this.lnkQueryBatch.MinimumSize = new System.Drawing.Size(20, 20);
            this.lnkQueryBatch.Name = "lnkQueryBatch";
            this.lnkQueryBatch.Size = new System.Drawing.Size(20, 20);
            this.lnkQueryBatch.TabIndex = 127;
            this.lnkQueryBatch.Tag = "";
            this.toolTip1.SetToolTip(this.lnkQueryBatch, "Click Here to view more information");
            this.lnkQueryBatch.UseCompatibleTextRendering = true;
            this.lnkQueryBatch.Click += new System.EventHandler(this.lnkQueryBatch_Click);
            // 
            // txtQBP_TransactionIds
            // 
            this.txtQBP_TransactionIds.Location = new System.Drawing.Point(129, 109);
            this.txtQBP_TransactionIds.Name = "txtQBP_TransactionIds";
            this.txtQBP_TransactionIds.Size = new System.Drawing.Size(252, 20);
            this.txtQBP_TransactionIds.TabIndex = 6;
            // 
            // txtQBP_BatchIds
            // 
            this.txtQBP_BatchIds.Location = new System.Drawing.Point(129, 37);
            this.txtQBP_BatchIds.Name = "txtQBP_BatchIds";
            this.txtQBP_BatchIds.Size = new System.Drawing.Size(138, 20);
            this.txtQBP_BatchIds.TabIndex = 3;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(23, 116);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(83, 13);
            this.label29.TabIndex = 7;
            this.label29.Text = "TransactionId(s)";
            // 
            // txtQBP_ServiceKeys
            // 
            this.txtQBP_ServiceKeys.Location = new System.Drawing.Point(129, 86);
            this.txtQBP_ServiceKeys.Name = "txtQBP_ServiceKeys";
            this.txtQBP_ServiceKeys.Size = new System.Drawing.Size(139, 20);
            this.txtQBP_ServiceKeys.TabIndex = 5;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(23, 90);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(72, 13);
            this.label28.TabIndex = 5;
            this.label28.Text = "ServiceKey(s)";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(23, 64);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(80, 13);
            this.label27.TabIndex = 3;
            this.label27.Text = "MercProfileId(s)";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.DarkOrange;
            this.label26.Location = new System.Drawing.Point(23, 16);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(349, 15);
            this.label26.TabIndex = 2;
            this.label26.Text = "Please use a comma to seperate more than one entry";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(23, 38);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(55, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "BatchId(s)";
            // 
            // cmdQueryBatch
            // 
            this.cmdQueryBatch.Location = new System.Drawing.Point(271, 35);
            this.cmdQueryBatch.Name = "cmdQueryBatch";
            this.cmdQueryBatch.Size = new System.Drawing.Size(157, 23);
            this.cmdQueryBatch.TabIndex = 5;
            this.cmdQueryBatch.Text = "QueryBatch";
            this.cmdQueryBatch.UseVisualStyleBackColor = true;
            this.cmdQueryBatch.Click += new System.EventHandler(this.cmdQueryBatch_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.DarkOrange;
            this.label18.Location = new System.Drawing.Point(31, 43);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(303, 15);
            this.label18.TabIndex = 25;
            this.label18.Text = "Note : Times are converted to UTC in the Code";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(31, 93);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(26, 13);
            this.label25.TabIndex = 24;
            this.label25.Text = "End";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Start";
            // 
            // dtpEndTimeTMS
            // 
            this.dtpEndTimeTMS.CustomFormat = "";
            this.dtpEndTimeTMS.Location = new System.Drawing.Point(66, 90);
            this.dtpEndTimeTMS.Name = "dtpEndTimeTMS";
            this.dtpEndTimeTMS.Size = new System.Drawing.Size(229, 20);
            this.dtpEndTimeTMS.TabIndex = 22;
            // 
            // dtpStartTimeTMS
            // 
            this.dtpStartTimeTMS.CustomFormat = "";
            this.dtpStartTimeTMS.Location = new System.Drawing.Point(66, 64);
            this.dtpStartTimeTMS.Name = "dtpStartTimeTMS";
            this.dtpStartTimeTMS.Size = new System.Drawing.Size(229, 20);
            this.dtpStartTimeTMS.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(474, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Select a row to view details";
            // 
            // chklstTMSResults
            // 
            this.chklstTMSResults.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.chklstTMSResults.FormattingEnabled = true;
            this.chklstTMSResults.HorizontalScrollbar = true;
            this.chklstTMSResults.Location = new System.Drawing.Point(477, 79);
            this.chklstTMSResults.Name = "chklstTMSResults";
            this.chklstTMSResults.Size = new System.Drawing.Size(487, 184);
            this.chklstTMSResults.TabIndex = 35;
            this.chklstTMSResults.SelectedIndexChanged += new System.EventHandler(this.chklstTMSResults_SelectedIndexChanged);
            // 
            // LnkNext
            // 
            this.LnkNext.AutoSize = true;
            this.LnkNext.Location = new System.Drawing.Point(926, 61);
            this.LnkNext.Name = "LnkNext";
            this.LnkNext.Size = new System.Drawing.Size(38, 13);
            this.LnkNext.TabIndex = 36;
            this.LnkNext.TabStop = true;
            this.LnkNext.Text = "Next >";
            this.LnkNext.Visible = false;
            this.LnkNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkNext_LinkClicked);
            // 
            // LnkPrevious
            // 
            this.LnkPrevious.AutoSize = true;
            this.LnkPrevious.Location = new System.Drawing.Point(809, 61);
            this.LnkPrevious.Name = "LnkPrevious";
            this.LnkPrevious.Size = new System.Drawing.Size(57, 13);
            this.LnkPrevious.TabIndex = 37;
            this.LnkPrevious.TabStop = true;
            this.LnkPrevious.Text = "< Previous";
            this.LnkPrevious.Visible = false;
            this.LnkPrevious.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkPrevious_LinkClicked);
            // 
            // lblPageNumber
            // 
            this.lblPageNumber.AutoSize = true;
            this.lblPageNumber.Location = new System.Drawing.Point(872, 61);
            this.lblPageNumber.Name = "lblPageNumber";
            this.lblPageNumber.Size = new System.Drawing.Size(41, 13);
            this.lblPageNumber.TabIndex = 38;
            this.lblPageNumber.Text = "Page : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(474, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "Details";
            // 
            // RtxtTMSResults
            // 
            this.RtxtTMSResults.Location = new System.Drawing.Point(477, 282);
            this.RtxtTMSResults.Name = "RtxtTMSResults";
            this.RtxtTMSResults.Size = new System.Drawing.Size(487, 528);
            this.RtxtTMSResults.TabIndex = 40;
            this.RtxtTMSResults.Text = "";
            // 
            // LblCurrentUTCTime
            // 
            this.LblCurrentUTCTime.AutoSize = true;
            this.LblCurrentUTCTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblCurrentUTCTime.ForeColor = System.Drawing.Color.DarkOrange;
            this.LblCurrentUTCTime.Location = new System.Drawing.Point(474, 16);
            this.LblCurrentUTCTime.Name = "LblCurrentUTCTime";
            this.LblCurrentUTCTime.Size = new System.Drawing.Size(133, 15);
            this.LblCurrentUTCTime.TabIndex = 41;
            this.LblCurrentUTCTime.Text = "Current UTC Time : ";
            // 
            // TmrUTCTIme
            // 
            this.TmrUTCTIme.Enabled = true;
            this.TmrUTCTIme.Interval = 1000;
            this.TmrUTCTIme.Tick += new System.EventHandler(this.TmrUTCTIme_Tick);
            // 
            // TransactionManagementServices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 812);
            this.Controls.Add(this.LblCurrentUTCTime);
            this.Controls.Add(this.RtxtTMSResults);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPageNumber);
            this.Controls.Add(this.LnkPrevious);
            this.Controls.Add(this.chkUseTransactionIdSelected);
            this.Controls.Add(this.LnkNext);
            this.Controls.Add(this.chklstTMSResults);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.grpQueryTransactionsParameters);
            this.Controls.Add(this.grpQueryBatchParameters);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpEndTimeTMS);
            this.Controls.Add(this.dtpStartTimeTMS);
            this.ForeColor = System.Drawing.Color.DarkMagenta;
            this.Name = "TransactionManagementServices";
            this.Text = "TransactionManagementServices - CWS 2.0.19";
            this.Load += new System.EventHandler(this.TransactionManagementServices_Load);
            this.grpQueryTransactionsParameters.ResumeLayout(false);
            this.grpQueryTransactionsParameters.PerformLayout();
            this.grpQueryBatchParameters.ResumeLayout(false);
            this.grpQueryBatchParameters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.GroupBox grpQueryTransactionsParameters;
        private System.Windows.Forms.LinkLabel lnkQueryTransactionFamilies;
        private System.Windows.Forms.ComboBox cboQTP_CardTypes;
        private System.Windows.Forms.ComboBox cboQTP_CaptureStates;
        private System.Windows.Forms.ComboBox cboQTP_TransactionStates;
        private System.Windows.Forms.ComboBox cboQTP_QueryType;
        private System.Windows.Forms.ComboBox cboQTP_IsAcknowledged;
        private System.Windows.Forms.TextBox txtQTP_ApprovalCodes;
        private System.Windows.Forms.TextBox txtQTP_TransactionIds;
        private System.Windows.Forms.Button cmdQueryTransactionsDetail;
        private System.Windows.Forms.TextBox txtQTP_ServiceKeys;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button cmdQueryTransactionsSummary;
        private System.Windows.Forms.TextBox txtQTP_TransactionClassTypePairs;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button cmdQueryTransactionFamilies;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtQTP_Amounts;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtQTP_BatchIds;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.GroupBox grpQueryBatchParameters;
        private System.Windows.Forms.LinkLabel lnkQueryBatch;
        private System.Windows.Forms.TextBox txtQBP_TransactionIds;
        private System.Windows.Forms.TextBox txtQBP_BatchIds;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtQBP_ServiceKeys;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button cmdQueryBatch;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpEndTimeTMS;
        private System.Windows.Forms.DateTimePicker dtpStartTimeTMS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox chklstTMSResults;
        private System.Windows.Forms.CheckBox chkUseTransactionIdSelected;
        private System.Windows.Forms.CheckBox chkIncludeRelated;
        private System.Windows.Forms.LinkLabel LnkNext;
        private System.Windows.Forms.LinkLabel LnkPrevious;
        private System.Windows.Forms.Label lblPageNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQTP_OrderNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button TxtClearTransactionIds;
        private System.Windows.Forms.Button TxtClearBatchTransactionIds;
        private System.Windows.Forms.ComboBox CboQTP_ServiceIds;
        private System.Windows.Forms.ComboBox CboQTP_MerchantProfileIds;
        private System.Windows.Forms.ComboBox CboQBP_MercProfileIds;
        private System.Windows.Forms.RichTextBox RtxtTMSResults;
        private System.Windows.Forms.ComboBox CboTransactionDetailFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtTransactionDetailFormat;
        private System.Windows.Forms.Label LblCurrentUTCTime;
        private System.Windows.Forms.Timer TmrUTCTIme;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}