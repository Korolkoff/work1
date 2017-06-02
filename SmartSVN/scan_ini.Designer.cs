namespace SmartSVN_4
{
    partial class scaninif
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.period_tb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.vsfolder_tb = new System.Windows.Forms.TextBox();
            this.scanfolder_tb = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.accept_b = new System.Windows.Forms.Button();
            this.subsc_dgv = new System.Windows.Forms.DataGridView();
            this.scanfolder_fbd = new System.Windows.Forms.FolderBrowserDialog();
            this._check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this._name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._path = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subsc_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.subsc_dgv, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 263F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(217, 395);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.period_tb);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.vsfolder_tb);
            this.panel1.Controls.Add(this.scanfolder_tb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(211, 88);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Период (сек)";
            // 
            // period_tb
            // 
            this.period_tb.Location = new System.Drawing.Point(103, 59);
            this.period_tb.Name = "period_tb";
            this.period_tb.Size = new System.Drawing.Size(102, 22);
            this.period_tb.TabIndex = 4;
            this.period_tb.Text = "180";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Папка в VS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Каталог";
            // 
            // vsfolder_tb
            // 
            this.vsfolder_tb.Location = new System.Drawing.Point(103, 31);
            this.vsfolder_tb.Name = "vsfolder_tb";
            this.vsfolder_tb.Size = new System.Drawing.Size(102, 22);
            this.vsfolder_tb.TabIndex = 1;
            this.vsfolder_tb.Text = "PDM_EXPORT";
            // 
            // scanfolder_tb
            // 
            this.scanfolder_tb.Location = new System.Drawing.Point(62, 0);
            this.scanfolder_tb.Name = "scanfolder_tb";
            this.scanfolder_tb.Size = new System.Drawing.Size(143, 22);
            this.scanfolder_tb.TabIndex = 0;
            this.scanfolder_tb.Click += new System.EventHandler(this.scanfolder_tb_Click);
            this.scanfolder_tb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.scanfolder_tb_KeyPress);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.accept_b);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 360);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(211, 32);
            this.panel2.TabIndex = 1;
            // 
            // accept_b
            // 
            this.accept_b.Location = new System.Drawing.Point(71, 3);
            this.accept_b.Name = "accept_b";
            this.accept_b.Size = new System.Drawing.Size(75, 23);
            this.accept_b.TabIndex = 0;
            this.accept_b.Text = "Готово";
            this.accept_b.UseVisualStyleBackColor = true;
            this.accept_b.Click += new System.EventHandler(this.accept_b_Click);
            // 
            // subsc_dgv
            // 
            this.subsc_dgv.AllowUserToAddRows = false;
            this.subsc_dgv.AllowUserToResizeColumns = false;
            this.subsc_dgv.AllowUserToResizeRows = false;
            this.subsc_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.subsc_dgv.ColumnHeadersVisible = false;
            this.subsc_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._check,
            this._name,
            this._path});
            this.subsc_dgv.Location = new System.Drawing.Point(3, 97);
            this.subsc_dgv.MultiSelect = false;
            this.subsc_dgv.Name = "subsc_dgv";
            this.subsc_dgv.RowHeadersVisible = false;
            this.subsc_dgv.RowTemplate.Height = 24;
            this.subsc_dgv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.subsc_dgv.Size = new System.Drawing.Size(211, 257);
            this.subsc_dgv.TabIndex = 2;
            // 
            // scanfolder_fbd
            // 
            this.scanfolder_fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.scanfolder_fbd.SelectedPath = "./";
            this.scanfolder_fbd.ShowNewFolderButton = false;
            // 
            // _check
            // 
            this._check.HeaderText = "_check";
            this._check.Name = "_check";
            this._check.Width = 25;
            // 
            // _name
            // 
            this._name.HeaderText = "_name";
            this._name.Name = "_name";
            this._name.Width = 182;
            // 
            // _path
            // 
            this._path.HeaderText = "_path";
            this._path.Name = "_path";
            this._path.Visible = false;
            // 
            // scaninif
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 395);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(235, 440);
            this.Name = "scaninif";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавить подписку";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.subsc_dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox period_tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox vsfolder_tb;
        private System.Windows.Forms.TextBox scanfolder_tb;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button accept_b;
        private System.Windows.Forms.DataGridView subsc_dgv;
        private System.Windows.Forms.FolderBrowserDialog scanfolder_fbd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn _check;
        private System.Windows.Forms.DataGridViewTextBoxColumn _name;
        private System.Windows.Forms.DataGridViewTextBoxColumn _path;
    }
}