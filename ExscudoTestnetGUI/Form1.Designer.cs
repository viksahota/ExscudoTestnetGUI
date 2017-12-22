using System.ComponentModel;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace ExscudoTestnetGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.debugTB = new System.Windows.Forms.TextBox();
            this.commitBTN = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.infoLBL = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.walletTab = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.createAccountBTN = new System.Windows.Forms.Button();
            this.accountLV = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.rxAddressLBL = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txAmountTB = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txRecipientTB = new System.Windows.Forms.TextBox();
            this.txSendBTN = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.balanceLBL = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.depositLBL = new System.Windows.Forms.Label();
            this.withdrawBTN = new System.Windows.Forms.Button();
            this.balDepAmountTB = new System.Windows.Forms.TextBox();
            this.refillBTN = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tranactionsTab = new System.Windows.Forms.TabPage();
            this.uncommitBTN = new System.Windows.Forms.Button();
            this.transactionLV = new BrightIdeasSoftware.ObjectListView();
            this.timestampColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.typeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.amountColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.feeColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.senderColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.recipientColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.signatureColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.configTab = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.logScrollCB = new System.Windows.Forms.CheckBox();
            this.writeConfigBTN = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.payoutRawCB = new System.Windows.Forms.CheckBox();
            this.payoutThresholdNM = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.payoutTimeoutNM = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.payoutIntervalNM = new System.Windows.Forms.NumericUpDown();
            this.payoutFeeNM = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.payoutDeadlineNM = new System.Windows.Forms.NumericUpDown();
            this.payoutSeedTB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.payoutPeerTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.payoutEnabledCB = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rootIntervalNM = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.rootNameTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rootCoinTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rootThreadsNM = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.logTab = new System.Windows.Forms.TabPage();
            this.logTB = new System.Windows.Forms.TextBox();
            this.testTab = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.swapTestTxSize = new System.Windows.Forms.TextBox();
            this.swapTestStop_BTN = new System.Windows.Forms.Button();
            this.swapTestReset_BTN = new System.Windows.Forms.Button();
            this.swapTestStart_BTN = new System.Windows.Forms.Button();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label44 = new System.Windows.Forms.Label();
            this.swapStatsPendingB_lbl = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.swapStatsPendingA_lbl = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.swapStatsTXRate_lbl = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.swapStatsTestDuration_lbl = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.swapStatsTXFails_lbl = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.swapStatsTotalTX_lbl = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.swapStatsTxCountBA_lbl = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.swapStatsTxCountAB_lbl = new System.Windows.Forms.Label();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label42 = new System.Windows.Forms.Label();
            this.swaptestActBalA_lbl = new System.Windows.Forms.Label();
            this.swaptestActBalB_lbl = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.swaptestExpBalA_lbl = new System.Windows.Forms.Label();
            this.swaptestExpBalB_lbl = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.swaptestIncomingB_lbl = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.swaptestIncomingA_lbl = new System.Windows.Forms.Label();
            this.lbl5 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.swaptestSentB_lbl = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.swaptestSentA_lbl = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.swaptestFeesB_lbl = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.swaptestFeesA_lbl = new System.Windows.Forms.Label();
            this.swaptestStartBalB_lbl = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.swaptestStartBalA_lbl = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.swapAccountB_CB = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.swapAccountA_CB = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pubkeyTB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.accountTB = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importConfigjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportConfigjsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildWorkingFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetAllConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eonCommandLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openExscudoRegistationWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.walletTab.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountLV)).BeginInit();
            this.groupBox11.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tranactionsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.transactionLV)).BeginInit();
            this.configTab.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.payoutThresholdNM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutTimeoutNM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutIntervalNM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutFeeNM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutDeadlineNM)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootIntervalNM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootThreadsNM)).BeginInit();
            this.logTab.SuspendLayout();
            this.testTab.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox17.SuspendLayout();
            this.groupBox16.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // debugTB
            // 
            this.debugTB.Location = new System.Drawing.Point(21, 25);
            this.debugTB.Multiline = true;
            this.debugTB.Name = "debugTB";
            this.debugTB.ReadOnly = true;
            this.debugTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugTB.Size = new System.Drawing.Size(1450, 541);
            this.debugTB.TabIndex = 0;
            // 
            // commitBTN
            // 
            this.commitBTN.Location = new System.Drawing.Point(21, 29);
            this.commitBTN.Name = "commitBTN";
            this.commitBTN.Size = new System.Drawing.Size(216, 48);
            this.commitBTN.TabIndex = 5;
            this.commitBTN.Text = "Confirmed Transactions";
            this.commitBTN.UseVisualStyleBackColor = true;
            this.commitBTN.Click += new System.EventHandler(this.CommitBTN_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1391, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 39);
            this.button1.TabIndex = 7;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // infoLBL
            // 
            this.infoLBL.AutoSize = true;
            this.infoLBL.Location = new System.Drawing.Point(8, 36);
            this.infoLBL.Name = "infoLBL";
            this.infoLBL.Size = new System.Drawing.Size(85, 20);
            this.infoLBL.TabIndex = 8;
            this.infoLBL.Text = "EON INFO";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Controls.Add(this.infoLBL);
            this.groupBox1.Location = new System.Drawing.Point(49, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 185);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EON release info";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.walletTab);
            this.tabControl1.Controls.Add(this.tranactionsTab);
            this.tabControl1.Controls.Add(this.configTab);
            this.tabControl1.Controls.Add(this.logTab);
            this.tabControl1.Controls.Add(this.testTab);
            this.tabControl1.Location = new System.Drawing.Point(11, 92);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1525, 1092);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // walletTab
            // 
            this.walletTab.Controls.Add(this.groupBox12);
            this.walletTab.Controls.Add(this.groupBox11);
            this.walletTab.Controls.Add(this.groupBox10);
            this.walletTab.Controls.Add(this.groupBox2);
            this.walletTab.Location = new System.Drawing.Point(4, 29);
            this.walletTab.Name = "walletTab";
            this.walletTab.Padding = new System.Windows.Forms.Padding(3);
            this.walletTab.Size = new System.Drawing.Size(1517, 1059);
            this.walletTab.TabIndex = 1;
            this.walletTab.Text = "Wallet";
            this.walletTab.UseVisualStyleBackColor = true;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.createAccountBTN);
            this.groupBox12.Controls.Add(this.accountLV);
            this.groupBox12.Location = new System.Drawing.Point(18, 18);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(557, 420);
            this.groupBox12.TabIndex = 15;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Accounts";
            // 
            // createAccountBTN
            // 
            this.createAccountBTN.Location = new System.Drawing.Point(356, 376);
            this.createAccountBTN.Name = "createAccountBTN";
            this.createAccountBTN.Size = new System.Drawing.Size(195, 33);
            this.createAccountBTN.TabIndex = 25;
            this.createAccountBTN.Text = "Create new account";
            this.createAccountBTN.UseVisualStyleBackColor = true;
            this.createAccountBTN.Click += new System.EventHandler(this.CreateAccountBTN_Click);
            // 
            // accountLV
            // 
            this.accountLV.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.accountLV.AllColumns.Add(this.olvColumn1);
            this.accountLV.AllColumns.Add(this.olvColumn2);
            this.accountLV.AllColumns.Add(this.olvColumn3);
            this.accountLV.AllColumns.Add(this.olvColumn4);
            this.accountLV.AlternateRowBackColor = System.Drawing.Color.White;
            this.accountLV.CellEditUseWholeCell = false;
            this.accountLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4});
            this.accountLV.Cursor = System.Windows.Forms.Cursors.Default;
            this.accountLV.FullRowSelect = true;
            this.accountLV.GridLines = true;
            this.accountLV.HasCollapsibleGroups = false;
            this.accountLV.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.accountLV.HideSelection = false;
            this.accountLV.Location = new System.Drawing.Point(6, 29);
            this.accountLV.MultiSelect = false;
            this.accountLV.Name = "accountLV";
            this.accountLV.SelectAllOnControlA = false;
            this.accountLV.ShowGroups = false;
            this.accountLV.Size = new System.Drawing.Size(545, 341);
            this.accountLV.TabIndex = 0;
            this.accountLV.UseCompatibleStateImageBehavior = false;
            this.accountLV.UseHotItem = true;
            this.accountLV.View = System.Windows.Forms.View.Details;
            this.accountLV.SelectionChanged += new System.EventHandler(this.AccountLV_SelectionChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "NickName";
            this.olvColumn1.Text = "Name";
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "AccountID";
            this.olvColumn2.Text = "Account ID";
            this.olvColumn2.Width = 160;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Balance";
            this.olvColumn3.Text = "Balance";
            this.olvColumn3.Width = 80;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "Deposit";
            this.olvColumn4.Text = "Deposit";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.groupBox7);
            this.groupBox11.Controls.Add(this.groupBox6);
            this.groupBox11.Location = new System.Drawing.Point(581, 228);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(924, 210);
            this.groupBox11.TabIndex = 14;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Send and Receive";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.rxAddressLBL);
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Location = new System.Drawing.Point(459, 36);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(443, 153);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Receive";
            // 
            // rxAddressLBL
            // 
            this.rxAddressLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rxAddressLBL.Location = new System.Drawing.Point(22, 67);
            this.rxAddressLBL.Name = "rxAddressLBL";
            this.rxAddressLBL.Size = new System.Drawing.Size(399, 37);
            this.rxAddressLBL.TabIndex = 6;
            this.rxAddressLBL.Text = "-";
            this.rxAddressLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(159, 37);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(137, 20);
            this.label20.TabIndex = 25;
            this.label20.Text = "Receive Address :";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.txAmountTB);
            this.groupBox6.Controls.Add(this.label18);
            this.groupBox6.Controls.Add(this.txRecipientTB);
            this.groupBox6.Controls.Add(this.txSendBTN);
            this.groupBox6.Location = new System.Drawing.Point(16, 36);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(421, 153);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Send";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(33, 71);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 20);
            this.label19.TabIndex = 24;
            this.label19.Text = "Amount :";
            // 
            // txAmountTB
            // 
            this.txAmountTB.Location = new System.Drawing.Point(111, 68);
            this.txAmountTB.Name = "txAmountTB";
            this.txAmountTB.Size = new System.Drawing.Size(276, 26);
            this.txAmountTB.TabIndex = 23;
            this.txAmountTB.Text = "0";
            this.txAmountTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(22, 30);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(84, 20);
            this.label18.TabIndex = 22;
            this.label18.Text = "Recipient :";
            // 
            // txRecipientTB
            // 
            this.txRecipientTB.Location = new System.Drawing.Point(111, 27);
            this.txRecipientTB.Name = "txRecipientTB";
            this.txRecipientTB.Size = new System.Drawing.Size(276, 26);
            this.txRecipientTB.TabIndex = 6;
            this.txRecipientTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txSendBTN
            // 
            this.txSendBTN.Location = new System.Drawing.Point(280, 104);
            this.txSendBTN.Name = "txSendBTN";
            this.txSendBTN.Size = new System.Drawing.Size(107, 33);
            this.txSendBTN.TabIndex = 3;
            this.txSendBTN.Text = "Send";
            this.txSendBTN.UseVisualStyleBackColor = true;
            this.txSendBTN.Click += new System.EventHandler(this.TxSendBTN_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.groupBox5);
            this.groupBox10.Controls.Add(this.groupBox8);
            this.groupBox10.Controls.Add(this.withdrawBTN);
            this.groupBox10.Controls.Add(this.balDepAmountTB);
            this.groupBox10.Controls.Add(this.refillBTN);
            this.groupBox10.Location = new System.Drawing.Point(581, 18);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(924, 204);
            this.groupBox10.TabIndex = 13;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Wallet Balance and Deposit";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.balanceLBL);
            this.groupBox5.Location = new System.Drawing.Point(16, 34);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(325, 148);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Balance";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(98, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(132, 37);
            this.label15.TabIndex = 1;
            this.label15.Text = "Balance";
            // 
            // balanceLBL
            // 
            this.balanceLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.balanceLBL.Location = new System.Drawing.Point(18, 74);
            this.balanceLBL.Name = "balanceLBL";
            this.balanceLBL.Size = new System.Drawing.Size(290, 37);
            this.balanceLBL.TabIndex = 0;
            this.balanceLBL.Text = "-";
            this.balanceLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox8.Controls.Add(this.label16);
            this.groupBox8.Controls.Add(this.depositLBL);
            this.groupBox8.Location = new System.Drawing.Point(583, 34);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(325, 148);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Deposit (staked)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(92, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(125, 37);
            this.label16.TabIndex = 3;
            this.label16.Text = "Deposit";
            // 
            // depositLBL
            // 
            this.depositLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.depositLBL.Location = new System.Drawing.Point(18, 74);
            this.depositLBL.Name = "depositLBL";
            this.depositLBL.Size = new System.Drawing.Size(279, 37);
            this.depositLBL.TabIndex = 2;
            this.depositLBL.Text = "-";
            this.depositLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // withdrawBTN
            // 
            this.withdrawBTN.Location = new System.Drawing.Point(402, 129);
            this.withdrawBTN.Name = "withdrawBTN";
            this.withdrawBTN.Size = new System.Drawing.Size(124, 37);
            this.withdrawBTN.TabIndex = 4;
            this.withdrawBTN.Text = "< Withdraw <";
            this.withdrawBTN.UseVisualStyleBackColor = true;
            this.withdrawBTN.Click += new System.EventHandler(this.WithdrawBTN_Click);
            // 
            // balDepAmountTB
            // 
            this.balDepAmountTB.Location = new System.Drawing.Point(370, 91);
            this.balDepAmountTB.Name = "balDepAmountTB";
            this.balDepAmountTB.Size = new System.Drawing.Size(185, 26);
            this.balDepAmountTB.TabIndex = 3;
            this.balDepAmountTB.Text = "0";
            this.balDepAmountTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // refillBTN
            // 
            this.refillBTN.Location = new System.Drawing.Point(402, 45);
            this.refillBTN.Name = "refillBTN";
            this.refillBTN.Size = new System.Drawing.Size(124, 35);
            this.refillBTN.TabIndex = 5;
            this.refillBTN.Text = "> Refill >";
            this.refillBTN.UseVisualStyleBackColor = true;
            this.refillBTN.Click += new System.EventHandler(this.RefillBTN_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.debugTB);
            this.groupBox2.Location = new System.Drawing.Point(18, 444);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1493, 586);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Debug View";
            // 
            // tranactionsTab
            // 
            this.tranactionsTab.Controls.Add(this.uncommitBTN);
            this.tranactionsTab.Controls.Add(this.transactionLV);
            this.tranactionsTab.Controls.Add(this.commitBTN);
            this.tranactionsTab.Location = new System.Drawing.Point(4, 29);
            this.tranactionsTab.Name = "tranactionsTab";
            this.tranactionsTab.Size = new System.Drawing.Size(1517, 1059);
            this.tranactionsTab.TabIndex = 2;
            this.tranactionsTab.Text = "Transactions";
            this.tranactionsTab.UseVisualStyleBackColor = true;
            // 
            // uncommitBTN
            // 
            this.uncommitBTN.Location = new System.Drawing.Point(252, 29);
            this.uncommitBTN.Name = "uncommitBTN";
            this.uncommitBTN.Size = new System.Drawing.Size(216, 48);
            this.uncommitBTN.TabIndex = 7;
            this.uncommitBTN.Text = "Pending Transactions";
            this.uncommitBTN.UseVisualStyleBackColor = true;
            this.uncommitBTN.Click += new System.EventHandler(this.UncommitBTN_Click);
            // 
            // transactionLV
            // 
            this.transactionLV.AllColumns.Add(this.timestampColumn);
            this.transactionLV.AllColumns.Add(this.typeColumn);
            this.transactionLV.AllColumns.Add(this.amountColumn);
            this.transactionLV.AllColumns.Add(this.feeColumn);
            this.transactionLV.AllColumns.Add(this.senderColumn);
            this.transactionLV.AllColumns.Add(this.recipientColumn);
            this.transactionLV.AllColumns.Add(this.signatureColumn);
            this.transactionLV.CellEditUseWholeCell = false;
            this.transactionLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timestampColumn,
            this.typeColumn,
            this.amountColumn,
            this.feeColumn,
            this.senderColumn,
            this.recipientColumn,
            this.signatureColumn});
            this.transactionLV.Cursor = System.Windows.Forms.Cursors.Default;
            this.transactionLV.Location = new System.Drawing.Point(21, 83);
            this.transactionLV.Name = "transactionLV";
            this.transactionLV.ShowGroups = false;
            this.transactionLV.Size = new System.Drawing.Size(1482, 960);
            this.transactionLV.TabIndex = 6;
            this.transactionLV.UseCompatibleStateImageBehavior = false;
            this.transactionLV.View = System.Windows.Forms.View.Details;
            // 
            // timestampColumn
            // 
            this.timestampColumn.AspectName = "TimestampString";
            this.timestampColumn.AutoCompleteEditor = false;
            this.timestampColumn.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.timestampColumn.IsEditable = false;
            this.timestampColumn.IsTileViewColumn = true;
            this.timestampColumn.Text = "Timestamp";
            this.timestampColumn.Width = 140;
            // 
            // typeColumn
            // 
            this.typeColumn.AspectName = "TypeString";
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 90;
            // 
            // amountColumn
            // 
            this.amountColumn.AspectName = "Attachment.Amount";
            this.amountColumn.Text = "Amount";
            this.amountColumn.Width = 80;
            // 
            // feeColumn
            // 
            this.feeColumn.AspectName = "Fee";
            this.feeColumn.Text = "Fee";
            this.feeColumn.Width = 50;
            // 
            // senderColumn
            // 
            this.senderColumn.AspectName = "Sender";
            this.senderColumn.Text = "Sender";
            this.senderColumn.Width = 160;
            // 
            // recipientColumn
            // 
            this.recipientColumn.AspectName = "Attachment.Recipient";
            this.recipientColumn.Text = "Recipient";
            this.recipientColumn.Width = 160;
            // 
            // signatureColumn
            // 
            this.signatureColumn.AspectName = "Signature";
            this.signatureColumn.Text = "Signature";
            this.signatureColumn.Width = 280;
            // 
            // configTab
            // 
            this.configTab.Controls.Add(this.groupBox9);
            this.configTab.Controls.Add(this.writeConfigBTN);
            this.configTab.Controls.Add(this.groupBox4);
            this.configTab.Controls.Add(this.groupBox3);
            this.configTab.Controls.Add(this.groupBox1);
            this.configTab.Location = new System.Drawing.Point(4, 29);
            this.configTab.Name = "configTab";
            this.configTab.Padding = new System.Windows.Forms.Padding(3);
            this.configTab.Size = new System.Drawing.Size(1517, 1059);
            this.configTab.TabIndex = 0;
            this.configTab.Text = "Config";
            this.configTab.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.logScrollCB);
            this.groupBox9.Location = new System.Drawing.Point(891, 55);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(546, 185);
            this.groupBox9.TabIndex = 10;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Logging";
            // 
            // logScrollCB
            // 
            this.logScrollCB.AutoSize = true;
            this.logScrollCB.Checked = true;
            this.logScrollCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.logScrollCB.Location = new System.Drawing.Point(40, 36);
            this.logScrollCB.Name = "logScrollCB";
            this.logScrollCB.Size = new System.Drawing.Size(178, 24);
            this.logScrollCB.TabIndex = 0;
            this.logScrollCB.Text = "Auto-Scroll Log view";
            this.logScrollCB.UseVisualStyleBackColor = true;
            // 
            // writeConfigBTN
            // 
            this.writeConfigBTN.Location = new System.Drawing.Point(321, 973);
            this.writeConfigBTN.Name = "writeConfigBTN";
            this.writeConfigBTN.Size = new System.Drawing.Size(263, 59);
            this.writeConfigBTN.TabIndex = 2;
            this.writeConfigBTN.Text = "Write Changes to config.json";
            this.writeConfigBTN.UseVisualStyleBackColor = true;
            this.writeConfigBTN.Click += new System.EventHandler(this.WriteConfigBTN_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.payoutRawCB);
            this.groupBox4.Controls.Add(this.payoutThresholdNM);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.payoutTimeoutNM);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.payoutIntervalNM);
            this.groupBox4.Controls.Add(this.payoutFeeNM);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.payoutDeadlineNM);
            this.groupBox4.Controls.Add(this.payoutSeedTB);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.payoutPeerTB);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.payoutEnabledCB);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(49, 435);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(776, 503);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Payouts";
            // 
            // payoutRawCB
            // 
            this.payoutRawCB.AutoSize = true;
            this.payoutRawCB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.payoutRawCB.Enabled = false;
            this.payoutRawCB.Location = new System.Drawing.Point(61, 301);
            this.payoutRawCB.Name = "payoutRawCB";
            this.payoutRawCB.Size = new System.Drawing.Size(75, 24);
            this.payoutRawCB.TabIndex = 18;
            this.payoutRawCB.Text = "Raw: ";
            this.payoutRawCB.UseVisualStyleBackColor = true;
            // 
            // payoutThresholdNM
            // 
            this.payoutThresholdNM.Location = new System.Drawing.Point(114, 385);
            this.payoutThresholdNM.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.payoutThresholdNM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.payoutThresholdNM.Name = "payoutThresholdNM";
            this.payoutThresholdNM.Size = new System.Drawing.Size(253, 26);
            this.payoutThresholdNM.TabIndex = 16;
            this.payoutThresholdNM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 388);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 20);
            this.label10.TabIndex = 17;
            this.label10.Text = "Threshold :";
            // 
            // payoutTimeoutNM
            // 
            this.payoutTimeoutNM.Location = new System.Drawing.Point(113, 343);
            this.payoutTimeoutNM.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.payoutTimeoutNM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.payoutTimeoutNM.Name = "payoutTimeoutNM";
            this.payoutTimeoutNM.Size = new System.Drawing.Size(100, 26);
            this.payoutTimeoutNM.TabIndex = 14;
            this.payoutTimeoutNM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 345);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 20);
            this.label11.TabIndex = 15;
            this.label11.Text = "Timeout (s):";
            // 
            // payoutIntervalNM
            // 
            this.payoutIntervalNM.Location = new System.Drawing.Point(113, 86);
            this.payoutIntervalNM.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.payoutIntervalNM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.payoutIntervalNM.Name = "payoutIntervalNM";
            this.payoutIntervalNM.Size = new System.Drawing.Size(100, 26);
            this.payoutIntervalNM.TabIndex = 13;
            this.payoutIntervalNM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // payoutFeeNM
            // 
            this.payoutFeeNM.Location = new System.Drawing.Point(115, 259);
            this.payoutFeeNM.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.payoutFeeNM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.payoutFeeNM.Name = "payoutFeeNM";
            this.payoutFeeNM.Size = new System.Drawing.Size(100, 26);
            this.payoutFeeNM.TabIndex = 11;
            this.payoutFeeNM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(64, 262);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 20);
            this.label9.TabIndex = 12;
            this.label9.Text = "Fee :";
            // 
            // payoutDeadlineNM
            // 
            this.payoutDeadlineNM.Location = new System.Drawing.Point(114, 217);
            this.payoutDeadlineNM.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.payoutDeadlineNM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.payoutDeadlineNM.Name = "payoutDeadlineNM";
            this.payoutDeadlineNM.Size = new System.Drawing.Size(100, 26);
            this.payoutDeadlineNM.TabIndex = 7;
            this.payoutDeadlineNM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // payoutSeedTB
            // 
            this.payoutSeedTB.Location = new System.Drawing.Point(113, 174);
            this.payoutSeedTB.Name = "payoutSeedTB";
            this.payoutSeedTB.Size = new System.Drawing.Size(644, 26);
            this.payoutSeedTB.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 20);
            this.label8.TabIndex = 8;
            this.label8.Text = "Deadline :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(52, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "Seed :";
            // 
            // payoutPeerTB
            // 
            this.payoutPeerTB.Location = new System.Drawing.Point(113, 133);
            this.payoutPeerTB.Name = "payoutPeerTB";
            this.payoutPeerTB.Size = new System.Drawing.Size(644, 26);
            this.payoutPeerTB.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Peer :";
            // 
            // payoutEnabledCB
            // 
            this.payoutEnabledCB.AutoSize = true;
            this.payoutEnabledCB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.payoutEnabledCB.Location = new System.Drawing.Point(43, 47);
            this.payoutEnabledCB.Name = "payoutEnabledCB";
            this.payoutEnabledCB.Size = new System.Drawing.Size(94, 24);
            this.payoutEnabledCB.TabIndex = 3;
            this.payoutEnabledCB.Text = "Enabled";
            this.payoutEnabledCB.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Interval (m) :";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rootIntervalNM);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.rootNameTB);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.rootCoinTB);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.rootThreadsNM);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(49, 270);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(776, 135);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Root";
            // 
            // rootIntervalNM
            // 
            this.rootIntervalNM.Location = new System.Drawing.Point(571, 23);
            this.rootIntervalNM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rootIntervalNM.Name = "rootIntervalNM";
            this.rootIntervalNM.Size = new System.Drawing.Size(100, 26);
            this.rootIntervalNM.TabIndex = 5;
            this.rootIntervalNM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(358, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(209, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Upstream check interval (s) :";
            // 
            // rootNameTB
            // 
            this.rootNameTB.Location = new System.Drawing.Point(139, 66);
            this.rootNameTB.Name = "rootNameTB";
            this.rootNameTB.Size = new System.Drawing.Size(100, 26);
            this.rootNameTB.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name :";
            // 
            // rootCoinTB
            // 
            this.rootCoinTB.ImeMode = System.Windows.Forms.ImeMode.On;
            this.rootCoinTB.Location = new System.Drawing.Point(139, 25);
            this.rootCoinTB.Name = "rootCoinTB";
            this.rootCoinTB.Size = new System.Drawing.Size(100, 26);
            this.rootCoinTB.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(88, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Coin :";
            // 
            // rootThreadsNM
            // 
            this.rootThreadsNM.Location = new System.Drawing.Point(571, 66);
            this.rootThreadsNM.Name = "rootThreadsNM";
            this.rootThreadsNM.Size = new System.Drawing.Size(100, 26);
            this.rootThreadsNM.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(493, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Threads :";
            // 
            // logTab
            // 
            this.logTab.Controls.Add(this.button1);
            this.logTab.Controls.Add(this.logTB);
            this.logTab.Location = new System.Drawing.Point(4, 29);
            this.logTab.Name = "logTab";
            this.logTab.Size = new System.Drawing.Size(1517, 1059);
            this.logTab.TabIndex = 3;
            this.logTab.Text = "Log";
            this.logTab.UseVisualStyleBackColor = true;
            // 
            // logTB
            // 
            this.logTB.Location = new System.Drawing.Point(14, 71);
            this.logTB.Multiline = true;
            this.logTB.Name = "logTB";
            this.logTB.ReadOnly = true;
            this.logTB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTB.Size = new System.Drawing.Size(1486, 971);
            this.logTB.TabIndex = 8;
            // 
            // testTab
            // 
            this.testTab.Controls.Add(this.groupBox13);
            this.testTab.ForeColor = System.Drawing.Color.RoyalBlue;
            this.testTab.Location = new System.Drawing.Point(4, 29);
            this.testTab.Name = "testTab";
            this.testTab.Size = new System.Drawing.Size(1517, 1059);
            this.testTab.TabIndex = 4;
            this.testTab.Text = "Test Tools";
            this.testTab.UseVisualStyleBackColor = true;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.groupBox17);
            this.groupBox13.Controls.Add(this.groupBox16);
            this.groupBox13.Controls.Add(this.groupBox15);
            this.groupBox13.Controls.Add(this.groupBox14);
            this.groupBox13.Controls.Add(this.label24);
            this.groupBox13.Controls.Add(this.swaptestIncomingB_lbl);
            this.groupBox13.Controls.Add(this.label40);
            this.groupBox13.Controls.Add(this.swaptestIncomingA_lbl);
            this.groupBox13.Controls.Add(this.lbl5);
            this.groupBox13.Controls.Add(this.label26);
            this.groupBox13.Controls.Add(this.swaptestSentB_lbl);
            this.groupBox13.Controls.Add(this.label36);
            this.groupBox13.Controls.Add(this.swaptestSentA_lbl);
            this.groupBox13.Controls.Add(this.label30);
            this.groupBox13.Controls.Add(this.swaptestFeesB_lbl);
            this.groupBox13.Controls.Add(this.label32);
            this.groupBox13.Controls.Add(this.swaptestFeesA_lbl);
            this.groupBox13.Controls.Add(this.swaptestStartBalB_lbl);
            this.groupBox13.Controls.Add(this.label22);
            this.groupBox13.Controls.Add(this.swaptestStartBalA_lbl);
            this.groupBox13.Controls.Add(this.label21);
            this.groupBox13.Controls.Add(this.swapAccountB_CB);
            this.groupBox13.Controls.Add(this.label17);
            this.groupBox13.Controls.Add(this.swapAccountA_CB);
            this.groupBox13.Controls.Add(this.label14);
            this.groupBox13.ForeColor = System.Drawing.Color.Black;
            this.groupBox13.Location = new System.Drawing.Point(16, 23);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(1482, 1016);
            this.groupBox13.TabIndex = 0;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Swap-transaction generator";
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.linkLabel1);
            this.groupBox17.Controls.Add(this.label33);
            this.groupBox17.Location = new System.Drawing.Point(938, 25);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(548, 985);
            this.groupBox17.TabIndex = 33;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Help";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(91, 859);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(343, 20);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://testnet.eontechnology.org/peermap.html";
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(11, 22);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(527, 888);
            this.label33.TabIndex = 1;
            this.label33.Text = resources.GetString("label33.Text");
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.label29);
            this.groupBox16.Controls.Add(this.label25);
            this.groupBox16.Controls.Add(this.swapTestTxSize);
            this.groupBox16.Controls.Add(this.swapTestStop_BTN);
            this.groupBox16.Controls.Add(this.swapTestReset_BTN);
            this.groupBox16.Controls.Add(this.swapTestStart_BTN);
            this.groupBox16.Location = new System.Drawing.Point(10, 660);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(922, 128);
            this.groupBox16.TabIndex = 32;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = "Control";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(261, 38);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(43, 20);
            this.label29.TabIndex = 34;
            this.label29.Text = "EON";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(29, 37);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(124, 20);
            this.label25.TabIndex = 33;
            this.label25.Text = "Transaction size";
            // 
            // swapTestTxSize
            // 
            this.swapTestTxSize.Location = new System.Drawing.Point(159, 35);
            this.swapTestTxSize.Name = "swapTestTxSize";
            this.swapTestTxSize.Size = new System.Drawing.Size(96, 26);
            this.swapTestTxSize.TabIndex = 32;
            this.swapTestTxSize.Text = "0.01";
            this.swapTestTxSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.swapTestTxSize.TextChanged += new System.EventHandler(this.SwapTestTxSize_TextChanged);
            // 
            // swapTestStop_BTN
            // 
            this.swapTestStop_BTN.Enabled = false;
            this.swapTestStop_BTN.ForeColor = System.Drawing.Color.Black;
            this.swapTestStop_BTN.Location = new System.Drawing.Point(608, 29);
            this.swapTestStop_BTN.Name = "swapTestStop_BTN";
            this.swapTestStop_BTN.Size = new System.Drawing.Size(162, 37);
            this.swapTestStop_BTN.TabIndex = 6;
            this.swapTestStop_BTN.Text = "Stop";
            this.swapTestStop_BTN.UseVisualStyleBackColor = true;
            this.swapTestStop_BTN.Click += new System.EventHandler(this.SwapTestStop_BTN_Click);
            // 
            // swapTestReset_BTN
            // 
            this.swapTestReset_BTN.Enabled = false;
            this.swapTestReset_BTN.ForeColor = System.Drawing.Color.Black;
            this.swapTestReset_BTN.Location = new System.Drawing.Point(806, 29);
            this.swapTestReset_BTN.Name = "swapTestReset_BTN";
            this.swapTestReset_BTN.Size = new System.Drawing.Size(92, 37);
            this.swapTestReset_BTN.TabIndex = 31;
            this.swapTestReset_BTN.Text = "Reset";
            this.swapTestReset_BTN.UseVisualStyleBackColor = true;
            this.swapTestReset_BTN.Click += new System.EventHandler(this.SwapTestReset_BTN_Click);
            // 
            // swapTestStart_BTN
            // 
            this.swapTestStart_BTN.ForeColor = System.Drawing.Color.Black;
            this.swapTestStart_BTN.Location = new System.Drawing.Point(417, 30);
            this.swapTestStart_BTN.Name = "swapTestStart_BTN";
            this.swapTestStart_BTN.Size = new System.Drawing.Size(162, 37);
            this.swapTestStart_BTN.TabIndex = 5;
            this.swapTestStart_BTN.Text = "Start";
            this.swapTestStart_BTN.UseVisualStyleBackColor = true;
            this.swapTestStart_BTN.Click += new System.EventHandler(this.SwapTestStart_BTN_Click);
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label44);
            this.groupBox15.Controls.Add(this.swapStatsPendingB_lbl);
            this.groupBox15.Controls.Add(this.label47);
            this.groupBox15.Controls.Add(this.swapStatsPendingA_lbl);
            this.groupBox15.Controls.Add(this.label41);
            this.groupBox15.Controls.Add(this.swapStatsTXRate_lbl);
            this.groupBox15.Controls.Add(this.label38);
            this.groupBox15.Controls.Add(this.swapStatsTestDuration_lbl);
            this.groupBox15.Controls.Add(this.label35);
            this.groupBox15.Controls.Add(this.swapStatsTXFails_lbl);
            this.groupBox15.Controls.Add(this.label31);
            this.groupBox15.Controls.Add(this.swapStatsTotalTX_lbl);
            this.groupBox15.Controls.Add(this.label27);
            this.groupBox15.Controls.Add(this.swapStatsTxCountBA_lbl);
            this.groupBox15.Controls.Add(this.label23);
            this.groupBox15.Controls.Add(this.swapStatsTxCountAB_lbl);
            this.groupBox15.Location = new System.Drawing.Point(10, 462);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(922, 192);
            this.groupBox15.TabIndex = 30;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Stats";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Enabled = false;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.Color.Black;
            this.label44.Location = new System.Drawing.Point(79, 148);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(176, 25);
            this.label44.TabIndex = 39;
            this.label44.Text = "Pending Tx\'s to B :";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsPendingB_lbl
            // 
            this.swapStatsPendingB_lbl.Enabled = false;
            this.swapStatsPendingB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsPendingB_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsPendingB_lbl.Location = new System.Drawing.Point(270, 144);
            this.swapStatsPendingB_lbl.Name = "swapStatsPendingB_lbl";
            this.swapStatsPendingB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swapStatsPendingB_lbl.TabIndex = 38;
            this.swapStatsPendingB_lbl.Text = "-";
            this.swapStatsPendingB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Enabled = false;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.ForeColor = System.Drawing.Color.Black;
            this.label47.Location = new System.Drawing.Point(78, 113);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(177, 25);
            this.label47.TabIndex = 37;
            this.label47.Text = "Pending Tx\'s to A :";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsPendingA_lbl
            // 
            this.swapStatsPendingA_lbl.Enabled = false;
            this.swapStatsPendingA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsPendingA_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsPendingA_lbl.Location = new System.Drawing.Point(270, 109);
            this.swapStatsPendingA_lbl.Name = "swapStatsPendingA_lbl";
            this.swapStatsPendingA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swapStatsPendingA_lbl.TabIndex = 36;
            this.swapStatsPendingA_lbl.Text = "-";
            this.swapStatsPendingA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.Black;
            this.label41.Location = new System.Drawing.Point(495, 142);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(220, 25);
            this.label41.TabIndex = 35;
            this.label41.Text = "Transactions / Second :";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsTXRate_lbl
            // 
            this.swapStatsTXRate_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsTXRate_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsTXRate_lbl.Location = new System.Drawing.Point(731, 139);
            this.swapStatsTXRate_lbl.Name = "swapStatsTXRate_lbl";
            this.swapStatsTXRate_lbl.Size = new System.Drawing.Size(197, 32);
            this.swapStatsTXRate_lbl.TabIndex = 34;
            this.swapStatsTXRate_lbl.Text = "-";
            this.swapStatsTXRate_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.Color.Black;
            this.label38.Location = new System.Drawing.Point(575, 109);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(140, 25);
            this.label38.TabIndex = 33;
            this.label38.Text = "Test Duration :";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsTestDuration_lbl
            // 
            this.swapStatsTestDuration_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsTestDuration_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsTestDuration_lbl.Location = new System.Drawing.Point(731, 107);
            this.swapStatsTestDuration_lbl.Name = "swapStatsTestDuration_lbl";
            this.swapStatsTestDuration_lbl.Size = new System.Drawing.Size(197, 32);
            this.swapStatsTestDuration_lbl.TabIndex = 32;
            this.swapStatsTestDuration_lbl.Text = "-";
            this.swapStatsTestDuration_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.Color.Black;
            this.label35.Location = new System.Drawing.Point(501, 70);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(210, 25);
            this.label35.TabIndex = 31;
            this.label35.Text = "Transactions Failures :";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsTXFails_lbl
            // 
            this.swapStatsTXFails_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsTXFails_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsTXFails_lbl.Location = new System.Drawing.Point(731, 69);
            this.swapStatsTXFails_lbl.Name = "swapStatsTXFails_lbl";
            this.swapStatsTXFails_lbl.Size = new System.Drawing.Size(144, 32);
            this.swapStatsTXFails_lbl.TabIndex = 30;
            this.swapStatsTXFails_lbl.Text = "-";
            this.swapStatsTXFails_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(528, 35);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(185, 25);
            this.label31.TabIndex = 29;
            this.label31.Text = "Total Transactions :";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsTotalTX_lbl
            // 
            this.swapStatsTotalTX_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsTotalTX_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsTotalTX_lbl.Location = new System.Drawing.Point(731, 33);
            this.swapStatsTotalTX_lbl.Name = "swapStatsTotalTX_lbl";
            this.swapStatsTotalTX_lbl.Size = new System.Drawing.Size(144, 32);
            this.swapStatsTotalTX_lbl.TabIndex = 28;
            this.swapStatsTotalTX_lbl.Text = "-";
            this.swapStatsTotalTX_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(5, 74);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(243, 25);
            this.label27.TabIndex = 27;
            this.label27.Text = "Transactions count B > A :";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsTxCountBA_lbl
            // 
            this.swapStatsTxCountBA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsTxCountBA_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsTxCountBA_lbl.Location = new System.Drawing.Point(270, 72);
            this.swapStatsTxCountBA_lbl.Name = "swapStatsTxCountBA_lbl";
            this.swapStatsTxCountBA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swapStatsTxCountBA_lbl.TabIndex = 26;
            this.swapStatsTxCountBA_lbl.Text = "-";
            this.swapStatsTxCountBA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(5, 38);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(243, 25);
            this.label23.TabIndex = 25;
            this.label23.Text = "Transactions count A > B :";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swapStatsTxCountAB_lbl
            // 
            this.swapStatsTxCountAB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swapStatsTxCountAB_lbl.ForeColor = System.Drawing.Color.Black;
            this.swapStatsTxCountAB_lbl.Location = new System.Drawing.Point(270, 36);
            this.swapStatsTxCountAB_lbl.Name = "swapStatsTxCountAB_lbl";
            this.swapStatsTxCountAB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swapStatsTxCountAB_lbl.TabIndex = 24;
            this.swapStatsTxCountAB_lbl.Text = "-";
            this.swapStatsTxCountAB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label42);
            this.groupBox14.Controls.Add(this.swaptestActBalA_lbl);
            this.groupBox14.Controls.Add(this.swaptestActBalB_lbl);
            this.groupBox14.Controls.Add(this.label45);
            this.groupBox14.Controls.Add(this.label28);
            this.groupBox14.Controls.Add(this.swaptestExpBalA_lbl);
            this.groupBox14.Controls.Add(this.swaptestExpBalB_lbl);
            this.groupBox14.Controls.Add(this.label34);
            this.groupBox14.Location = new System.Drawing.Point(10, 333);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(922, 112);
            this.groupBox14.TabIndex = 29;
            this.groupBox14.TabStop = false;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.ForeColor = System.Drawing.Color.Black;
            this.label42.Location = new System.Drawing.Point(70, 60);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(181, 29);
            this.label42.TabIndex = 21;
            this.label42.Text = "Actual balance :";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestActBalA_lbl
            // 
            this.swaptestActBalA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestActBalA_lbl.ForeColor = System.Drawing.Color.Black;
            this.swaptestActBalA_lbl.Location = new System.Drawing.Point(256, 61);
            this.swaptestActBalA_lbl.Name = "swaptestActBalA_lbl";
            this.swaptestActBalA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestActBalA_lbl.TabIndex = 20;
            this.swaptestActBalA_lbl.Text = "-";
            this.swaptestActBalA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // swaptestActBalB_lbl
            // 
            this.swaptestActBalB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestActBalB_lbl.ForeColor = System.Drawing.Color.Black;
            this.swaptestActBalB_lbl.Location = new System.Drawing.Point(754, 61);
            this.swaptestActBalB_lbl.Name = "swaptestActBalB_lbl";
            this.swaptestActBalB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestActBalB_lbl.TabIndex = 22;
            this.swaptestActBalB_lbl.Text = "-";
            this.swaptestActBalB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.ForeColor = System.Drawing.Color.Black;
            this.label45.Location = new System.Drawing.Point(564, 60);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(181, 29);
            this.label45.TabIndex = 23;
            this.label45.Text = "Actual balance :";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label28.Location = new System.Drawing.Point(34, 21);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(217, 29);
            this.label28.TabIndex = 12;
            this.label28.Text = "Expected balance :";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestExpBalA_lbl
            // 
            this.swaptestExpBalA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestExpBalA_lbl.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.swaptestExpBalA_lbl.Location = new System.Drawing.Point(257, 22);
            this.swaptestExpBalA_lbl.Name = "swaptestExpBalA_lbl";
            this.swaptestExpBalA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestExpBalA_lbl.TabIndex = 11;
            this.swaptestExpBalA_lbl.Text = "-";
            this.swaptestExpBalA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // swaptestExpBalB_lbl
            // 
            this.swaptestExpBalB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestExpBalB_lbl.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.swaptestExpBalB_lbl.Location = new System.Drawing.Point(755, 22);
            this.swaptestExpBalB_lbl.Name = "swaptestExpBalB_lbl";
            this.swaptestExpBalB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestExpBalB_lbl.TabIndex = 13;
            this.swaptestExpBalB_lbl.Text = "-";
            this.swaptestExpBalB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.label34.Location = new System.Drawing.Point(528, 22);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(217, 29);
            this.label34.TabIndex = 19;
            this.label34.Text = "Expected balance :";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.ForestGreen;
            this.label24.Location = new System.Drawing.Point(613, 292);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(144, 29);
            this.label24.TabIndex = 28;
            this.label24.Text = "+ Returned :";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestIncomingB_lbl
            // 
            this.swaptestIncomingB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestIncomingB_lbl.ForeColor = System.Drawing.Color.ForestGreen;
            this.swaptestIncomingB_lbl.Location = new System.Drawing.Point(764, 292);
            this.swaptestIncomingB_lbl.Name = "swaptestIncomingB_lbl";
            this.swaptestIncomingB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestIncomingB_lbl.TabIndex = 27;
            this.swaptestIncomingB_lbl.Text = "-";
            this.swaptestIncomingB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.ForestGreen;
            this.label40.Location = new System.Drawing.Point(121, 292);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(144, 29);
            this.label40.TabIndex = 26;
            this.label40.Text = "+ Returned :";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestIncomingA_lbl
            // 
            this.swaptestIncomingA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestIncomingA_lbl.ForeColor = System.Drawing.Color.ForestGreen;
            this.swaptestIncomingA_lbl.Location = new System.Drawing.Point(266, 294);
            this.swaptestIncomingA_lbl.Name = "swaptestIncomingA_lbl";
            this.swaptestIncomingA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestIncomingA_lbl.TabIndex = 25;
            this.swaptestIncomingA_lbl.Text = "-";
            this.swaptestIncomingA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl5
            // 
            this.lbl5.AutoSize = true;
            this.lbl5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbl5.Location = new System.Drawing.Point(556, 184);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(200, 29);
            this.lbl5.TabIndex = 24;
            this.lbl5.Text = "Starting Balance :";
            this.lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(668, 221);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(88, 29);
            this.label26.TabIndex = 23;
            this.label26.Text = "- Sent :";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestSentB_lbl
            // 
            this.swaptestSentB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestSentB_lbl.ForeColor = System.Drawing.Color.Red;
            this.swaptestSentB_lbl.Location = new System.Drawing.Point(764, 221);
            this.swaptestSentB_lbl.Name = "swaptestSentB_lbl";
            this.swaptestSentB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestSentB_lbl.TabIndex = 22;
            this.swaptestSentB_lbl.Text = "-";
            this.swaptestSentB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Red;
            this.label36.Location = new System.Drawing.Point(174, 221);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(88, 29);
            this.label36.TabIndex = 21;
            this.label36.Text = "- Sent :";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestSentA_lbl
            // 
            this.swaptestSentA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestSentA_lbl.ForeColor = System.Drawing.Color.Red;
            this.swaptestSentA_lbl.Location = new System.Drawing.Point(266, 221);
            this.swaptestSentA_lbl.Name = "swaptestSentA_lbl";
            this.swaptestSentA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestSentA_lbl.TabIndex = 20;
            this.swaptestSentA_lbl.Text = "-";
            this.swaptestSentA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Red;
            this.label30.Location = new System.Drawing.Point(664, 253);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(94, 29);
            this.label30.TabIndex = 18;
            this.label30.Text = "- Fees :";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestFeesB_lbl
            // 
            this.swaptestFeesB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestFeesB_lbl.ForeColor = System.Drawing.Color.Red;
            this.swaptestFeesB_lbl.Location = new System.Drawing.Point(765, 253);
            this.swaptestFeesB_lbl.Name = "swaptestFeesB_lbl";
            this.swaptestFeesB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestFeesB_lbl.TabIndex = 17;
            this.swaptestFeesB_lbl.Text = "-";
            this.swaptestFeesB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Red;
            this.label32.Location = new System.Drawing.Point(170, 253);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(94, 29);
            this.label32.TabIndex = 16;
            this.label32.Text = "- Fees :";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestFeesA_lbl
            // 
            this.swaptestFeesA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestFeesA_lbl.ForeColor = System.Drawing.Color.Red;
            this.swaptestFeesA_lbl.Location = new System.Drawing.Point(266, 253);
            this.swaptestFeesA_lbl.Name = "swaptestFeesA_lbl";
            this.swaptestFeesA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestFeesA_lbl.TabIndex = 15;
            this.swaptestFeesA_lbl.Text = "-";
            this.swaptestFeesA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // swaptestStartBalB_lbl
            // 
            this.swaptestStartBalB_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestStartBalB_lbl.ForeColor = System.Drawing.Color.RoyalBlue;
            this.swaptestStartBalB_lbl.Location = new System.Drawing.Point(765, 184);
            this.swaptestStartBalB_lbl.Name = "swaptestStartBalB_lbl";
            this.swaptestStartBalB_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestStartBalB_lbl.TabIndex = 9;
            this.swaptestStartBalB_lbl.Text = "-";
            this.swaptestStartBalB_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label22.Location = new System.Drawing.Point(64, 184);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(200, 29);
            this.label22.TabIndex = 8;
            this.label22.Text = "Starting Balance :";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // swaptestStartBalA_lbl
            // 
            this.swaptestStartBalA_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.swaptestStartBalA_lbl.ForeColor = System.Drawing.Color.RoyalBlue;
            this.swaptestStartBalA_lbl.Location = new System.Drawing.Point(266, 184);
            this.swaptestStartBalA_lbl.Name = "swaptestStartBalA_lbl";
            this.swaptestStartBalA_lbl.Size = new System.Drawing.Size(144, 32);
            this.swaptestStartBalA_lbl.TabIndex = 7;
            this.swaptestStartBalA_lbl.Text = "-";
            this.swaptestStartBalA_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(677, 100);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(83, 20);
            this.label21.TabIndex = 4;
            this.label21.Text = "Account B";
            // 
            // swapAccountB_CB
            // 
            this.swapAccountB_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.swapAccountB_CB.ForeColor = System.Drawing.Color.Black;
            this.swapAccountB_CB.FormattingEnabled = true;
            this.swapAccountB_CB.Location = new System.Drawing.Point(526, 123);
            this.swapAccountB_CB.Name = "swapAccountB_CB";
            this.swapAccountB_CB.Size = new System.Drawing.Size(385, 28);
            this.swapAccountB_CB.TabIndex = 3;
            this.swapAccountB_CB.SelectedIndexChanged += new System.EventHandler(this.SwapAccountB_CB_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(176, 100);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 20);
            this.label17.TabIndex = 2;
            this.label17.Text = "Account A";
            // 
            // swapAccountA_CB
            // 
            this.swapAccountA_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.swapAccountA_CB.ForeColor = System.Drawing.Color.Black;
            this.swapAccountA_CB.FormattingEnabled = true;
            this.swapAccountA_CB.Location = new System.Drawing.Point(25, 123);
            this.swapAccountA_CB.Name = "swapAccountA_CB";
            this.swapAccountA_CB.Size = new System.Drawing.Size(385, 28);
            this.swapAccountA_CB.TabIndex = 1;
            this.swapAccountA_CB.SelectedIndexChanged += new System.EventHandler(this.SwapAccountA_CB_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(21, 41);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(581, 20);
            this.label14.TabIndex = 0;
            this.label14.Text = "Schedules continuous bidirectional  streams of payments between two accounts. ";
            // 
            // pubkeyTB
            // 
            this.pubkeyTB.Location = new System.Drawing.Point(906, 13);
            this.pubkeyTB.Name = "pubkeyTB";
            this.pubkeyTB.ReadOnly = true;
            this.pubkeyTB.Size = new System.Drawing.Size(630, 26);
            this.pubkeyTB.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(813, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 20);
            this.label12.TabIndex = 21;
            this.label12.Text = "Public Key :";
            // 
            // accountTB
            // 
            this.accountTB.Location = new System.Drawing.Point(95, 13);
            this.accountTB.Name = "accountTB";
            this.accountTB.ReadOnly = true;
            this.accountTB.Size = new System.Drawing.Size(283, 26);
            this.accountTB.TabIndex = 18;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(76, 20);
            this.label13.TabIndex = 19;
            this.label13.Text = "Account :";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.cMDToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1549, 33);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importConfigjsonToolStripMenuItem,
            this.exportConfigjsonToolStripMenuItem,
            this.rebuildWorkingFoldersToolStripMenuItem,
            this.resetAllConfigToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importConfigjsonToolStripMenuItem
            // 
            this.importConfigjsonToolStripMenuItem.Name = "importConfigjsonToolStripMenuItem";
            this.importConfigjsonToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
            this.importConfigjsonToolStripMenuItem.Text = "Import wallets.json";
            this.importConfigjsonToolStripMenuItem.Click += new System.EventHandler(this.ImportWalletsjsonToolStripMenuItem_Click);
            // 
            // exportConfigjsonToolStripMenuItem
            // 
            this.exportConfigjsonToolStripMenuItem.Name = "exportConfigjsonToolStripMenuItem";
            this.exportConfigjsonToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
            this.exportConfigjsonToolStripMenuItem.Text = "Export wallets.json";
            this.exportConfigjsonToolStripMenuItem.Click += new System.EventHandler(this.ExportWalletsJsonToolStripMenuItem_Click);
            // 
            // rebuildWorkingFoldersToolStripMenuItem
            // 
            this.rebuildWorkingFoldersToolStripMenuItem.Name = "rebuildWorkingFoldersToolStripMenuItem";
            this.rebuildWorkingFoldersToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
            this.rebuildWorkingFoldersToolStripMenuItem.Text = "Rebuild Working Folders";
            this.rebuildWorkingFoldersToolStripMenuItem.Click += new System.EventHandler(this.RebuildWorkingFoldersToolStripMenuItem_Click);
            // 
            // resetAllConfigToolStripMenuItem
            // 
            this.resetAllConfigToolStripMenuItem.Name = "resetAllConfigToolStripMenuItem";
            this.resetAllConfigToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
            this.resetAllConfigToolStripMenuItem.Text = "Reset All Config";
            this.resetAllConfigToolStripMenuItem.Click += new System.EventHandler(this.ResetAllConfigToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(291, 30);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.QuitToolStripMenuItem_Click);
            // 
            // cMDToolStripMenuItem
            // 
            this.cMDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eonCommandLineToolStripMenuItem,
            this.openWorkingFolderToolStripMenuItem,
            this.openExscudoRegistationWebsiteToolStripMenuItem});
            this.cMDToolStripMenuItem.Name = "cMDToolStripMenuItem";
            this.cMDToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.cMDToolStripMenuItem.Text = "Tools";
            // 
            // eonCommandLineToolStripMenuItem
            // 
            this.eonCommandLineToolStripMenuItem.Name = "eonCommandLineToolStripMenuItem";
            this.eonCommandLineToolStripMenuItem.Size = new System.Drawing.Size(405, 30);
            this.eonCommandLineToolStripMenuItem.Text = "eon command line [selected account]";
            this.eonCommandLineToolStripMenuItem.Click += new System.EventHandler(this.EonCommandLineToolStripMenuItem_Click);
            // 
            // openWorkingFolderToolStripMenuItem
            // 
            this.openWorkingFolderToolStripMenuItem.Name = "openWorkingFolderToolStripMenuItem";
            this.openWorkingFolderToolStripMenuItem.Size = new System.Drawing.Size(405, 30);
            this.openWorkingFolderToolStripMenuItem.Text = "open working folder [selected account]";
            this.openWorkingFolderToolStripMenuItem.Click += new System.EventHandler(this.OpenWorkingFolderToolStripMenuItem_Click);
            // 
            // openExscudoRegistationWebsiteToolStripMenuItem
            // 
            this.openExscudoRegistationWebsiteToolStripMenuItem.Name = "openExscudoRegistationWebsiteToolStripMenuItem";
            this.openExscudoRegistationWebsiteToolStripMenuItem.Size = new System.Drawing.Size(405, 30);
            this.openExscudoRegistationWebsiteToolStripMenuItem.Text = "Open Exscudo registation website";
            this.openExscudoRegistationWebsiteToolStripMenuItem.Click += new System.EventHandler(this.OpenExscudoRegistationWebsiteToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 29);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(147, 30);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.panel1.Controls.Add(this.pubkeyTB);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.accountTB);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1549, 50);
            this.panel1.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1549, 1196);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Exscudo Testnet GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.walletTab.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accountLV)).EndInit();
            this.groupBox11.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tranactionsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.transactionLV)).EndInit();
            this.configTab.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.payoutThresholdNM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutTimeoutNM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutIntervalNM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutFeeNM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.payoutDeadlineNM)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rootIntervalNM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rootThreadsNM)).EndInit();
            this.logTab.ResumeLayout(false);
            this.logTab.PerformLayout();
            this.testTab.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.groupBox16.ResumeLayout(false);
            this.groupBox16.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox debugTB;
        private Button commitBTN;
        private Button button1;
        private Label infoLBL;
        private GroupBox groupBox1;
        private TabControl tabControl1;
        private TabPage configTab;
        private TabPage walletTab;
        private GroupBox groupBox2;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private TextBox rootCoinTB;
        private Label label2;
        private NumericUpDown rootThreadsNM;
        private Label label1;
        private NumericUpDown payoutIntervalNM;
        private NumericUpDown payoutFeeNM;
        private Label label9;
        private NumericUpDown payoutDeadlineNM;
        private TextBox payoutSeedTB;
        private Label label8;
        private Label label7;
        private TextBox payoutPeerTB;
        private Label label5;
        private CheckBox payoutEnabledCB;
        private Label label6;
        private NumericUpDown rootIntervalNM;
        private Label label4;
        private TextBox rootNameTB;
        private Label label3;
        private NumericUpDown payoutThresholdNM;
        private Label label10;
        private NumericUpDown payoutTimeoutNM;
        private Label label11;
        private Button writeConfigBTN;
        private TextBox pubkeyTB;
        private Label label12;
        private TextBox accountTB;
        private Label label13;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem importConfigjsonToolStripMenuItem;
        private ToolStripMenuItem cMDToolStripMenuItem;
        private ToolStripMenuItem eonCommandLineToolStripMenuItem;
        private ToolStripMenuItem exportConfigjsonToolStripMenuItem;
        private ToolStripMenuItem openWorkingFolderToolStripMenuItem;
        private Panel panel1;
        private Label balanceLBL;
        private GroupBox groupBox5;
        private Button refillBTN;
        private Button withdrawBTN;
        private Label label16;
        private Label depositLBL;
        private Label label15;
        private GroupBox groupBox7;
        private Label rxAddressLBL;
        private Label label20;
        private GroupBox groupBox6;
        private Label label19;
        private TextBox txAmountTB;
        private Label label18;
        private TextBox txRecipientTB;
        private Button txSendBTN;
        private TextBox balDepAmountTB;
        private GroupBox groupBox8;
        private TabPage tranactionsTab;
        private TabPage logTab;
        private TextBox logTB;
        private ObjectListView transactionLV;
        private OLVColumn timestampColumn;
        private GroupBox groupBox9;
        private CheckBox logScrollCB;
        private ToolStripMenuItem openExscudoRegistationWebsiteToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem quitToolStripMenuItem;
        private Button uncommitBTN;
        private GroupBox groupBox10;
        private GroupBox groupBox11;
        private GroupBox groupBox12;
        private ObjectListView accountLV;
        private OLVColumn olvColumn2;
        private OLVColumn olvColumn3;
        private OLVColumn olvColumn4;
        private OLVColumn olvColumn1;
        private CheckBox payoutRawCB;
        private OLVColumn typeColumn;
        private OLVColumn amountColumn;
        private OLVColumn feeColumn;
        private OLVColumn recipientColumn;
        private OLVColumn signatureColumn;
        private ToolStripMenuItem rebuildWorkingFoldersToolStripMenuItem;
        private ToolStripMenuItem resetAllConfigToolStripMenuItem;
        private Button createAccountBTN;
        private OLVColumn senderColumn;
        private TabPage testTab;
        private GroupBox groupBox13;
        private ComboBox swapAccountA_CB;
        private Label label14;
        private Button swapTestStop_BTN;
        private Button swapTestStart_BTN;
        private Label label21;
        private ComboBox swapAccountB_CB;
        private Label label17;
        private GroupBox groupBox14;
        private Label label42;
        private Label swaptestActBalA_lbl;
        private Label swaptestActBalB_lbl;
        private Label label45;
        private Label label28;
        private Label swaptestExpBalA_lbl;
        private Label swaptestExpBalB_lbl;
        private Label label34;
        private Label label24;
        private Label swaptestIncomingB_lbl;
        private Label label40;
        private Label swaptestIncomingA_lbl;
        private Label lbl5;
        private Label label26;
        private Label swaptestSentB_lbl;
        private Label label36;
        private Label swaptestSentA_lbl;
        private Label label30;
        private Label swaptestFeesB_lbl;
        private Label label32;
        private Label swaptestFeesA_lbl;
        private Label swaptestStartBalB_lbl;
        private Label label22;
        private Label swaptestStartBalA_lbl;
        private Button swapTestReset_BTN;
        private GroupBox groupBox15;
        private Label label44;
        private Label swapStatsPendingB_lbl;
        private Label label47;
        private Label swapStatsPendingA_lbl;
        private Label label41;
        private Label swapStatsTXRate_lbl;
        private Label label38;
        private Label swapStatsTestDuration_lbl;
        private Label label35;
        private Label swapStatsTXFails_lbl;
        private Label label31;
        private Label swapStatsTotalTX_lbl;
        private Label label27;
        private Label swapStatsTxCountBA_lbl;
        private Label label23;
        private Label swapStatsTxCountAB_lbl;
        private GroupBox groupBox16;
        private Label label29;
        private Label label25;
        private TextBox swapTestTxSize;
        private GroupBox groupBox17;
        private Label label33;
        private LinkLabel linkLabel1;
    }
}

