namespace SmartSVN_4
{
    partial class mainf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainf));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadIniFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSubscribeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanSubscribesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveSmartSVNiniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSubscribeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Start_b = new System.Windows.Forms.Button();
            this.Stop_b = new System.Windows.Forms.Button();
            this.run_now_b = new System.Windows.Forms.Button();
            this.log_errors_cb = new System.Windows.Forms.CheckBox();
            this.ClearLog_b = new System.Windows.Forms.Button();
            this.Close_b = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comments_cb = new System.Windows.Forms.CheckBox();
            this.SmartSVN_ini_link = new System.Windows.Forms.LinkLabel();
            this.logs_rtb = new System.Windows.Forms.RichTextBox();
            this.Files_dgv = new System.Windows.Forms.DataGridView();
            this._check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._name = new System.Windows.Forms.DataGridViewLinkColumn();
            this._path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._check_period = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text_dgv = new System.Windows.Forms.DataGridView();
            this.dataGridViewLinkColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OpenINI_ofd = new System.Windows.Forms.OpenFileDialog();
            this.SaveINI_df = new System.Windows.Forms.SaveFileDialog();
            this.Subsc_ofd = new System.Windows.Forms.OpenFileDialog();
            this.tray_ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.tray_ni_cm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.запускToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выполнитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Files_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Text_dgv)).BeginInit();
            this.tray_ni_cm.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(795, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadIniFilesToolStripMenuItem,
            this.addSubscribeToolStripMenuItem,
            this.scanSubscribesToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveSmartSVNiniToolStripMenuItem,
            this.saveSubscribeToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // reloadIniFilesToolStripMenuItem
            // 
            this.reloadIniFilesToolStripMenuItem.Name = "reloadIniFilesToolStripMenuItem";
            this.reloadIniFilesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.reloadIniFilesToolStripMenuItem.Text = "Reload ini files";
            this.reloadIniFilesToolStripMenuItem.Click += new System.EventHandler(this.refresh_ini_b_Click);
            // 
            // addSubscribeToolStripMenuItem
            // 
            this.addSubscribeToolStripMenuItem.Name = "addSubscribeToolStripMenuItem";
            this.addSubscribeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.addSubscribeToolStripMenuItem.Text = "Add Subscribe";
            this.addSubscribeToolStripMenuItem.Click += new System.EventHandler(this.loadiniToolStripMenuItem_Click);
            // 
            // scanSubscribesToolStripMenuItem
            // 
            this.scanSubscribesToolStripMenuItem.Name = "scanSubscribesToolStripMenuItem";
            this.scanSubscribesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.scanSubscribesToolStripMenuItem.Text = "Scan subscribes";
            this.scanSubscribesToolStripMenuItem.Click += new System.EventHandler(this.scanSubscribesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // saveSmartSVNiniToolStripMenuItem
            // 
            this.saveSmartSVNiniToolStripMenuItem.Name = "saveSmartSVNiniToolStripMenuItem";
            this.saveSmartSVNiniToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.saveSmartSVNiniToolStripMenuItem.Text = "Save SmartSVN.ini";
            this.saveSmartSVNiniToolStripMenuItem.Click += new System.EventHandler(this.saveSmartSVNiniToolStripMenuItem_Click);
            // 
            // saveSubscribeToolStripMenuItem
            // 
            this.saveSubscribeToolStripMenuItem.Name = "saveSubscribeToolStripMenuItem";
            this.saveSubscribeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.saveSubscribeToolStripMenuItem.Text = "Save Subscribe";
            this.saveSubscribeToolStripMenuItem.Click += new System.EventHandler(this.saveSubscribeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(167, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.99509F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.004902F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(795, 667);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.log_errors_cb);
            this.panel1.Controls.Add(this.ClearLog_b);
            this.panel1.Controls.Add(this.Close_b);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 628);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 37);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.Start_b);
            this.panel3.Controls.Add(this.Stop_b);
            this.panel3.Controls.Add(this.run_now_b);
            this.panel3.Location = new System.Drawing.Point(2, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(246, 33);
            this.panel3.TabIndex = 10;
            // 
            // Start_b
            // 
            this.Start_b.BackColor = System.Drawing.Color.Lime;
            this.Start_b.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Start_b.Location = new System.Drawing.Point(2, 1);
            this.Start_b.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Start_b.Name = "Start_b";
            this.Start_b.Size = new System.Drawing.Size(68, 27);
            this.Start_b.TabIndex = 0;
            this.Start_b.Text = "Запуск";
            this.Start_b.UseVisualStyleBackColor = false;
            this.Start_b.Click += new System.EventHandler(this.Start_b_Click);
            // 
            // Stop_b
            // 
            this.Stop_b.Enabled = false;
            this.Stop_b.Location = new System.Drawing.Point(74, 1);
            this.Stop_b.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Stop_b.Name = "Stop_b";
            this.Stop_b.Size = new System.Drawing.Size(80, 28);
            this.Stop_b.TabIndex = 4;
            this.Stop_b.Text = "Остановить";
            this.Stop_b.UseVisualStyleBackColor = false;
            this.Stop_b.Click += new System.EventHandler(this.Stop_b_Click);
            // 
            // run_now_b
            // 
            this.run_now_b.BackColor = System.Drawing.Color.OrangeRed;
            this.run_now_b.Location = new System.Drawing.Point(158, 1);
            this.run_now_b.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.run_now_b.Name = "run_now_b";
            this.run_now_b.Size = new System.Drawing.Size(80, 28);
            this.run_now_b.TabIndex = 7;
            this.run_now_b.Text = "Выполнить";
            this.run_now_b.UseVisualStyleBackColor = false;
            this.run_now_b.Click += new System.EventHandler(this.run_now_b_Click);
            // 
            // log_errors_cb
            // 
            this.log_errors_cb.AutoSize = true;
            this.log_errors_cb.Checked = true;
            this.log_errors_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.log_errors_cb.Location = new System.Drawing.Point(640, 9);
            this.log_errors_cb.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.log_errors_cb.Name = "log_errors_cb";
            this.log_errors_cb.Size = new System.Drawing.Size(64, 17);
            this.log_errors_cb.TabIndex = 9;
            this.log_errors_cb.Text = "ошибки";
            this.log_errors_cb.UseVisualStyleBackColor = true;
            // 
            // ClearLog_b
            // 
            this.ClearLog_b.BackColor = System.Drawing.Color.Orange;
            this.ClearLog_b.Location = new System.Drawing.Point(545, 2);
            this.ClearLog_b.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ClearLog_b.Name = "ClearLog_b";
            this.ClearLog_b.Size = new System.Drawing.Size(90, 28);
            this.ClearLog_b.TabIndex = 8;
            this.ClearLog_b.Text = "Очистить Лог";
            this.ClearLog_b.UseVisualStyleBackColor = false;
            this.ClearLog_b.Click += new System.EventHandler(this.ClearLog_b_Click);
            // 
            // Close_b
            // 
            this.Close_b.Location = new System.Drawing.Point(726, 3);
            this.Close_b.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Close_b.Name = "Close_b";
            this.Close_b.Size = new System.Drawing.Size(60, 27);
            this.Close_b.TabIndex = 2;
            this.Close_b.Text = "Закрыть";
            this.Close_b.UseVisualStyleBackColor = true;
            this.Close_b.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comments_cb);
            this.panel2.Controls.Add(this.SmartSVN_ini_link);
            this.panel2.Controls.Add(this.logs_rtb);
            this.panel2.Controls.Add(this.Files_dgv);
            this.panel2.Controls.Add(this.Text_dgv);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(791, 622);
            this.panel2.TabIndex = 2;
            // 
            // comments_cb
            // 
            this.comments_cb.AutoSize = true;
            this.comments_cb.Location = new System.Drawing.Point(655, 518);
            this.comments_cb.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comments_cb.Name = "comments_cb";
            this.comments_cb.Size = new System.Drawing.Size(136, 17);
            this.comments_cb.TabIndex = 12;
            this.comments_cb.Text = "Скрыть комментарии";
            this.comments_cb.UseVisualStyleBackColor = true;
            this.comments_cb.CheckedChanged += new System.EventHandler(this.comments_cb_CheckedChanged);
            // 
            // SmartSVN_ini_link
            // 
            this.SmartSVN_ini_link.AutoSize = true;
            this.SmartSVN_ini_link.Location = new System.Drawing.Point(46, 2);
            this.SmartSVN_ini_link.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SmartSVN_ini_link.Name = "SmartSVN_ini_link";
            this.SmartSVN_ini_link.Size = new System.Drawing.Size(69, 13);
            this.SmartSVN_ini_link.TabIndex = 7;
            this.SmartSVN_ini_link.TabStop = true;
            this.SmartSVN_ini_link.Text = "SmartSVN.ini";
            this.SmartSVN_ini_link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SmartSVN_ini_link_LinkClicked);
            // 
            // logs_rtb
            // 
            this.logs_rtb.BackColor = System.Drawing.Color.Gainsboro;
            this.logs_rtb.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logs_rtb.Location = new System.Drawing.Point(0, 540);
            this.logs_rtb.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.logs_rtb.Name = "logs_rtb";
            this.logs_rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.logs_rtb.Size = new System.Drawing.Size(786, 76);
            this.logs_rtb.TabIndex = 6;
            this.logs_rtb.Text = "";
            // 
            // Files_dgv
            // 
            this.Files_dgv.AllowUserToAddRows = false;
            this.Files_dgv.AllowUserToOrderColumns = true;
            this.Files_dgv.AllowUserToResizeColumns = false;
            this.Files_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Files_dgv.ColumnHeadersVisible = false;
            this.Files_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._check,
            this._name,
            this._path,
            this._check_period});
            this.Files_dgv.EnableHeadersVisualStyles = false;
            this.Files_dgv.Location = new System.Drawing.Point(0, 19);
            this.Files_dgv.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Files_dgv.MultiSelect = false;
            this.Files_dgv.Name = "Files_dgv";
            this.Files_dgv.RowHeadersVisible = false;
            this.Files_dgv.RowTemplate.Height = 24;
            this.Files_dgv.Size = new System.Drawing.Size(154, 517);
            this.Files_dgv.TabIndex = 5;
            this.Files_dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Files_dgv_CellClick);
            this.Files_dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Files_dgv_CellContentClick);
            this.Files_dgv.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Files_dgv_CellContentDoubleClick);
            // 
            // _check
            // 
            this._check.FalseValue = "false";
            this._check.HeaderText = "_check";
            this._check.IndeterminateValue = "";
            this._check.Name = "_check";
            this._check.TrueValue = "true";
            this._check.Width = 25;
            // 
            // _name
            // 
            this._name.HeaderText = "_name";
            this._name.Name = "_name";
            this._name.Width = 170;
            // 
            // _path
            // 
            this._path.HeaderText = "_path";
            this._path.Name = "_path";
            this._path.Visible = false;
            // 
            // _check_period
            // 
            this._check_period.HeaderText = "_check_period";
            this._check_period.Name = "_check_period";
            this._check_period.Visible = false;
            // 
            // Text_dgv
            // 
            this.Text_dgv.AllowUserToOrderColumns = true;
            this.Text_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Text_dgv.ColumnHeadersVisible = false;
            this.Text_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewLinkColumn1});
            this.Text_dgv.EnableHeadersVisualStyles = false;
            this.Text_dgv.Location = new System.Drawing.Point(158, 2);
            this.Text_dgv.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Text_dgv.MultiSelect = false;
            this.Text_dgv.Name = "Text_dgv";
            this.Text_dgv.RowHeadersVisible = false;
            this.Text_dgv.RowTemplate.Height = 24;
            this.Text_dgv.Size = new System.Drawing.Size(628, 512);
            this.Text_dgv.TabIndex = 3;
            // 
            // dataGridViewLinkColumn1
            // 
            this.dataGridViewLinkColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewLinkColumn1.HeaderText = "Files";
            this.dataGridViewLinkColumn1.MinimumWidth = 814;
            this.dataGridViewLinkColumn1.Name = "dataGridViewLinkColumn1";
            this.dataGridViewLinkColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLinkColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewLinkColumn1.Width = 814;
            // 
            // OpenINI_ofd
            // 
            this.OpenINI_ofd.Filter = "INI Файлы |*.ini";
            this.OpenINI_ofd.InitialDirectory = ".\\";
            this.OpenINI_ofd.Title = "Открыть файл настроек INI";
            // 
            // Subsc_ofd
            // 
            this.Subsc_ofd.Filter = "ini files (*.ini)|*.ini";
            this.Subsc_ofd.RestoreDirectory = true;
            // 
            // tray_ni
            // 
            this.tray_ni.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tray_ni.BalloonTipText = "SmartSVN";
            this.tray_ni.BalloonTipTitle = "Info";
            this.tray_ni.ContextMenuStrip = this.tray_ni_cm;
            this.tray_ni.Icon = ((System.Drawing.Icon)(resources.GetObject("tray_ni.Icon")));
            this.tray_ni.Text = "SmartSVN";
            this.tray_ni.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tray_ni_MouseClick);
            this.tray_ni.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tray_ni_MouseDoubleClick);
            // 
            // tray_ni_cm
            // 
            this.tray_ni_cm.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tray_ni_cm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запускToolStripMenuItem,
            this.остановитьToolStripMenuItem,
            this.выполнитьToolStripMenuItem});
            this.tray_ni_cm.Name = "tray_ni_cm";
            this.tray_ni_cm.Size = new System.Drawing.Size(143, 82);
            // 
            // запускToolStripMenuItem
            // 
            this.запускToolStripMenuItem.BackColor = System.Drawing.Color.Lime;
            this.запускToolStripMenuItem.Image = global::SmartSVN_4.Properties.Resources.play;
            this.запускToolStripMenuItem.Name = "запускToolStripMenuItem";
            this.запускToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
            this.запускToolStripMenuItem.Text = "Запуск";
            this.запускToolStripMenuItem.Click += new System.EventHandler(this.Start_b_Click);
            // 
            // остановитьToolStripMenuItem
            // 
            this.остановитьToolStripMenuItem.Enabled = false;
            this.остановитьToolStripMenuItem.Name = "остановитьToolStripMenuItem";
            this.остановитьToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
            this.остановитьToolStripMenuItem.Text = "Остановить";
            this.остановитьToolStripMenuItem.Click += new System.EventHandler(this.Stop_b_Click);
            // 
            // выполнитьToolStripMenuItem
            // 
            this.выполнитьToolStripMenuItem.BackColor = System.Drawing.Color.OrangeRed;
            this.выполнитьToolStripMenuItem.Image = global::SmartSVN_4.Properties.Resources.run;
            this.выполнитьToolStripMenuItem.Name = "выполнитьToolStripMenuItem";
            this.выполнитьToolStripMenuItem.Size = new System.Drawing.Size(142, 26);
            this.выполнитьToolStripMenuItem.Text = "Выполнить";
            this.выполнитьToolStripMenuItem.Click += new System.EventHandler(this.run_now_b_Click);
            // 
            // mainf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 691);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(811, 730);
            this.MinimumSize = new System.Drawing.Size(811, 730);
            this.Name = "mainf";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Синхронизация SVN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainf_FormClosing);
            this.Resize += new System.EventHandler(this.mainf_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Files_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Text_dgv)).EndInit();
            this.tray_ni_cm.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Close_b;
        private System.Windows.Forms.Button Start_b;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenINI_ofd;
        private System.Windows.Forms.SaveFileDialog SaveINI_df;
        private System.Windows.Forms.Button Stop_b;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView Text_dgv;
        private System.Windows.Forms.DataGridView Files_dgv;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.RichTextBox logs_rtb;
        private System.Windows.Forms.LinkLabel SmartSVN_ini_link;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _check;
        private System.Windows.Forms.DataGridViewLinkColumn _name;
        private System.Windows.Forms.DataGridViewTextBoxColumn _path;
        private System.Windows.Forms.DataGridViewTextBoxColumn _check_period;
        private System.Windows.Forms.Button run_now_b;
        private System.Windows.Forms.Button ClearLog_b;
        private System.Windows.Forms.CheckBox log_errors_cb;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.OpenFileDialog Subsc_ofd;
        private System.Windows.Forms.ToolStripMenuItem saveSmartSVNiniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSubscribeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadIniFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem scanSubscribesToolStripMenuItem;
        private System.Windows.Forms.CheckBox comments_cb;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewLinkColumn1;
        private System.Windows.Forms.ToolStripMenuItem addSubscribeToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon tray_ni;
        private System.Windows.Forms.ContextMenuStrip tray_ni_cm;
        private System.Windows.Forms.ToolStripMenuItem запускToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem остановитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выполнитьToolStripMenuItem;
    }
}

