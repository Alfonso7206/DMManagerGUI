namespace WindowsFormsApp4
{
    partial class Form1
    {
  //      private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button BtnApriLink;
        private System.Windows.Forms.TextBox txtUrl;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.BtnApriLink = new System.Windows.Forms.Button();
            this.DMListBox = new System.Windows.Forms.ListBox();
            this.DMCopyButton = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.BtnDownloadTutti = new System.Windows.Forms.Button();
            this.BtnDownloadSelezionati = new System.Windows.Forms.Button();
            this.BtnKill = new System.Windows.Forms.Button();
            this.btnSelezionaCartella = new System.Windows.Forms.Button();
            this.btnApriCartella = new System.Windows.Forms.Button();
            this.lblDownloadPath = new System.Windows.Forms.Label();
            this.chkPlaylist = new System.Windows.Forms.CheckBox();
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnListClear = new System.Windows.Forms.Button();
            this.lblResetArgs = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbFormato = new System.Windows.Forms.ComboBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.MSAggiornaYtdlp = new System.Windows.Forms.ToolStripMenuItem();
            this.aggiornaYtdlp = new System.Windows.Forms.ToolStripMenuItem();
            this.infoYtdlp = new System.Windows.Forms.ToolStripMenuItem();
            this.mostraFormati = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnScaricaTool = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtUrl.Font = new System.Drawing.Font("Bahnschrift SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrl.Location = new System.Drawing.Point(12, 36);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(500, 87);
            this.txtUrl.TabIndex = 0;
            this.txtUrl.Text = "https://www.youtube.com/watch?v=OC1nLOxfa8o";
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            // 
            // BtnApriLink
            // 
            this.BtnApriLink.Enabled = false;
            this.BtnApriLink.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnApriLink.Location = new System.Drawing.Point(518, 129);
            this.BtnApriLink.Name = "BtnApriLink";
            this.BtnApriLink.Size = new System.Drawing.Size(149, 25);
            this.BtnApriLink.TabIndex = 4;
            this.BtnApriLink.Text = "Video Info";
            this.BtnApriLink.Click += new System.EventHandler(this.BtnApriLink_Click);
            // 
            // DMListBox
            // 
            this.DMListBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.DMListBox.Font = new System.Drawing.Font("Bahnschrift SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DMListBox.FormattingEnabled = true;
            this.DMListBox.ItemHeight = 16;
            this.DMListBox.Location = new System.Drawing.Point(12, 248);
            this.DMListBox.Name = "DMListBox";
            this.DMListBox.Size = new System.Drawing.Size(655, 228);
            this.DMListBox.TabIndex = 2;
            this.DMListBox.SelectedIndexChanged += new System.EventHandler(this.DMListBox_SelectedIndexChanged);
            // 
            // DMCopyButton
            // 
            this.DMCopyButton.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DMCopyButton.Location = new System.Drawing.Point(518, 36);
            this.DMCopyButton.Name = "DMCopyButton";
            this.DMCopyButton.Size = new System.Drawing.Size(149, 25);
            this.DMCopyButton.TabIndex = 3;
            this.DMCopyButton.Text = "Copy to list";
            this.DMCopyButton.UseVisualStyleBackColor = true;
            this.DMCopyButton.Click += new System.EventHandler(this.DMCopyButton_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Location = new System.Drawing.Point(6, 15);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 15);
            this.lblProgress.TabIndex = 5;
            // 
            // BtnDownloadTutti
            // 
            this.BtnDownloadTutti.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDownloadTutti.Location = new System.Drawing.Point(518, 98);
            this.BtnDownloadTutti.Name = "BtnDownloadTutti";
            this.BtnDownloadTutti.Size = new System.Drawing.Size(149, 25);
            this.BtnDownloadTutti.TabIndex = 6;
            this.BtnDownloadTutti.Text = "Download All";
            this.BtnDownloadTutti.UseVisualStyleBackColor = true;
            this.BtnDownloadTutti.Click += new System.EventHandler(this.BtnDownloadTutti_Click);
            // 
            // BtnDownloadSelezionati
            // 
            this.BtnDownloadSelezionati.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDownloadSelezionati.Location = new System.Drawing.Point(518, 67);
            this.BtnDownloadSelezionati.Name = "BtnDownloadSelezionati";
            this.BtnDownloadSelezionati.Size = new System.Drawing.Size(149, 25);
            this.BtnDownloadSelezionati.TabIndex = 7;
            this.BtnDownloadSelezionati.Text = "Download Select";
            this.BtnDownloadSelezionati.UseVisualStyleBackColor = true;
            this.BtnDownloadSelezionati.Click += new System.EventHandler(this.BtnDownloadSelezionati_Click);
            // 
            // BtnKill
            // 
            this.BtnKill.Location = new System.Drawing.Point(519, 494);
            this.BtnKill.Name = "BtnKill";
            this.BtnKill.Size = new System.Drawing.Size(149, 25);
            this.BtnKill.TabIndex = 8;
            this.BtnKill.Text = "Stop";
            this.BtnKill.UseVisualStyleBackColor = true;
            this.BtnKill.Click += new System.EventHandler(this.BtnKill_Click);
            // 
            // btnSelezionaCartella
            // 
            this.btnSelezionaCartella.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelezionaCartella.Location = new System.Drawing.Point(12, 129);
            this.btnSelezionaCartella.Name = "btnSelezionaCartella";
            this.btnSelezionaCartella.Size = new System.Drawing.Size(149, 25);
            this.btnSelezionaCartella.TabIndex = 9;
            this.btnSelezionaCartella.Text = "Select folder";
            this.btnSelezionaCartella.Click += new System.EventHandler(this.btnSelezionaCartella_Click);
            // 
            // btnApriCartella
            // 
            this.btnApriCartella.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApriCartella.Location = new System.Drawing.Point(167, 129);
            this.btnApriCartella.Name = "btnApriCartella";
            this.btnApriCartella.Size = new System.Drawing.Size(149, 25);
            this.btnApriCartella.TabIndex = 10;
            this.btnApriCartella.Text = "Open Folder";
            this.btnApriCartella.Click += new System.EventHandler(this.btnApriCartella_Click);
            // 
            // lblDownloadPath
            // 
            this.lblDownloadPath.AutoSize = true;
            this.lblDownloadPath.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloadPath.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblDownloadPath.Location = new System.Drawing.Point(9, 162);
            this.lblDownloadPath.Name = "lblDownloadPath";
            this.lblDownloadPath.Size = new System.Drawing.Size(97, 13);
            this.lblDownloadPath.TabIndex = 11;
            this.lblDownloadPath.Text = "lblDownloadPath";
            this.lblDownloadPath.Click += new System.EventHandler(this.lblDownloadPath_Click);
            // 
            // chkPlaylist
            // 
            this.chkPlaylist.AutoSize = true;
            this.chkPlaylist.Location = new System.Drawing.Point(607, 184);
            this.chkPlaylist.Name = "chkPlaylist";
            this.chkPlaylist.Size = new System.Drawing.Size(61, 17);
            this.chkPlaylist.TabIndex = 12;
            this.chkPlaylist.Text = "Playlist";
            this.chkPlaylist.UseVisualStyleBackColor = true;
            this.chkPlaylist.CheckedChanged += new System.EventHandler(this.chkPlaylist_CheckedChanged);
            // 
            // txtArgs
            // 
            this.txtArgs.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtArgs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArgs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtArgs.Location = new System.Drawing.Point(12, 179);
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.Size = new System.Drawing.Size(417, 25);
            this.txtArgs.TabIndex = 13;
            this.txtArgs.TextChanged += new System.EventHandler(this.txtArgs_TextChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Argumets";
            // 
            // btnListClear
            // 
            this.btnListClear.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnListClear.Location = new System.Drawing.Point(322, 129);
            this.btnListClear.Name = "btnListClear";
            this.btnListClear.Size = new System.Drawing.Size(149, 25);
            this.btnListClear.TabIndex = 14;
            this.btnListClear.Text = "Clear list";
            this.btnListClear.Click += new System.EventHandler(this.btnListClear_Click);
            // 
            // lblResetArgs
            // 
            this.lblResetArgs.AutoSize = true;
            this.lblResetArgs.Location = new System.Drawing.Point(574, 185);
            this.lblResetArgs.Name = "lblResetArgs";
            this.lblResetArgs.Size = new System.Drawing.Size(19, 13);
            this.lblResetArgs.TabIndex = 15;
            this.lblResetArgs.Text = "🔄";
            this.lblResetArgs.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblProgress);
            this.groupBox1.Location = new System.Drawing.Point(12, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(655, 39);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(3, 496);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // cmbFormato
            // 
            this.cmbFormato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormato.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cmbFormato.FormattingEnabled = true;
            this.cmbFormato.Location = new System.Drawing.Point(435, 179);
            this.cmbFormato.Name = "cmbFormato";
            this.cmbFormato.Size = new System.Drawing.Size(133, 25);
            this.cmbFormato.TabIndex = 18;
            this.cmbFormato.SelectedIndexChanged += new System.EventHandler(this.cmbFormato_SelectedIndexChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MSAggiornaYtdlp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(687, 24);
            this.menuStrip.TabIndex = 19;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip_ItemClicked);
            // 
            // MSAggiornaYtdlp
            // 
            this.MSAggiornaYtdlp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aggiornaYtdlp,
            this.infoYtdlp,
            this.mostraFormati,
            this.BtnScaricaTool});
            this.MSAggiornaYtdlp.Name = "MSAggiornaYtdlp";
            this.MSAggiornaYtdlp.Size = new System.Drawing.Size(89, 20);
            this.MSAggiornaYtdlp.Text = "Opzioni Extra";
            // 
            // aggiornaYtdlp
            // 
            this.aggiornaYtdlp.Name = "aggiornaYtdlp";
            this.aggiornaYtdlp.Size = new System.Drawing.Size(180, 22);
            this.aggiornaYtdlp.Text = "Aggiorna yt-dlp";
            this.aggiornaYtdlp.Click += new System.EventHandler(this.aggiornaYtdlpToolStripMenuItem1_Click);
            // 
            // infoYtdlp
            // 
            this.infoYtdlp.Name = "infoYtdlp";
            this.infoYtdlp.Size = new System.Drawing.Size(180, 22);
            this.infoYtdlp.Text = "Info yt-dlp";
            this.infoYtdlp.Click += new System.EventHandler(this.infoYtdlp_Click);
            // 
            // mostraFormati
            // 
            this.mostraFormati.Name = "mostraFormati";
            this.mostraFormati.Size = new System.Drawing.Size(180, 22);
            this.mostraFormati.Text = "Mostra Formati";
            this.mostraFormati.Click += new System.EventHandler(this.mostraFormati_Click);
            // 
            // BtnScaricaTool
            // 
            this.BtnScaricaTool.Name = "BtnScaricaTool";
            this.BtnScaricaTool.Size = new System.Drawing.Size(180, 22);
            this.BtnScaricaTool.Text = "Scarica Binary";
            this.BtnScaricaTool.Click += new System.EventHandler(this.BtnScaricaTool_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 530);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(687, 10);
            this.progressBar1.TabIndex = 20;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(687, 540);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cmbFormato);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblResetArgs);
            this.Controls.Add(this.btnListClear);
            this.Controls.Add(this.txtArgs);
            this.Controls.Add(this.chkPlaylist);
            this.Controls.Add(this.lblDownloadPath);
            this.Controls.Add(this.btnApriCartella);
            this.Controls.Add(this.btnSelezionaCartella);
            this.Controls.Add(this.BtnKill);
            this.Controls.Add(this.BtnDownloadSelezionati);
            this.Controls.Add(this.BtnDownloadTutti);
            this.Controls.Add(this.DMCopyButton);
            this.Controls.Add(this.DMListBox);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.BtnApriLink);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download Manager v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListBox DMListBox;
        private System.Windows.Forms.Button DMCopyButton;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button BtnDownloadTutti;
        private System.Windows.Forms.Button BtnDownloadSelezionati;
        private System.Windows.Forms.Button BtnKill;
        private System.Windows.Forms.Button btnSelezionaCartella;
        private System.Windows.Forms.Button btnApriCartella;
        private System.Windows.Forms.Label lblDownloadPath;
        private System.Windows.Forms.CheckBox chkPlaylist;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btnListClear;
        private System.Windows.Forms.Label lblResetArgs;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox cmbFormato;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem MSAggiornaYtdlp;
        private System.Windows.Forms.ToolStripMenuItem aggiornaYtdlp;
        private System.Windows.Forms.ToolStripMenuItem infoYtdlp;
        private System.Windows.Forms.ToolStripMenuItem mostraFormati;
        private System.Windows.Forms.ToolStripMenuItem BtnScaricaTool;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
