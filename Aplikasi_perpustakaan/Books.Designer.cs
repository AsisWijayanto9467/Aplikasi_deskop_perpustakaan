namespace Aplikasi_perpustakaan
{
    partial class Books
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

            this.Load += new System.EventHandler(this.Books_Load);
        }



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Books));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            comboKategori = new Guna.UI2.WinForms.Guna2ComboBox();
            btnTambah = new Guna.UI2.WinForms.Guna2Button();
            panel2 = new Panel();
            lblTersedia = new Label();
            label4 = new Label();
            lblTotalBuku = new Label();
            dataGridBuku = new Guna.UI2.WinForms.Guna2DataGridView();
            btnSearch = new Guna.UI2.WinForms.Guna2Button();
            label3 = new Label();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            panel1 = new Panel();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            guna2Panel2.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridBuku).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel2
            // 
            guna2Panel2.BackColor = Color.FromArgb(240, 242, 245);
            guna2Panel2.BorderColor = Color.Silver;
            guna2Panel2.BorderRadius = 10;
            guna2Panel2.BorderThickness = 1;
            guna2Panel2.Controls.Add(comboKategori);
            guna2Panel2.Controls.Add(btnTambah);
            guna2Panel2.Controls.Add(panel2);
            guna2Panel2.Controls.Add(dataGridBuku);
            guna2Panel2.Controls.Add(btnSearch);
            guna2Panel2.Controls.Add(label3);
            guna2Panel2.Controls.Add(txtSearch);
            guna2Panel2.Controls.Add(panel1);
            guna2Panel2.CustomizableEdges = customizableEdges11;
            guna2Panel2.Location = new Point(12, 12);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges12;
            guna2Panel2.Size = new Size(1111, 708);
            guna2Panel2.TabIndex = 5;
            // 
            // comboKategori
            // 
            comboKategori.BackColor = Color.Transparent;
            comboKategori.BorderRadius = 12;
            comboKategori.CustomizableEdges = customizableEdges1;
            comboKategori.DrawMode = DrawMode.OwnerDrawFixed;
            comboKategori.DropDownStyle = ComboBoxStyle.DropDownList;
            comboKategori.FillColor = Color.Black;
            comboKategori.FocusedColor = Color.FromArgb(94, 148, 255);
            comboKategori.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            comboKategori.Font = new Font("Inter", 10.1999989F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboKategori.ForeColor = Color.White;
            comboKategori.ItemHeight = 30;
            comboKategori.Location = new Point(936, 94);
            comboKategori.Name = "comboKategori";
            comboKategori.ShadowDecoration.CustomizableEdges = customizableEdges2;
            comboKategori.Size = new Size(161, 36);
            comboKategori.TabIndex = 25;
            comboKategori.SelectedIndexChanged += comboKategori_SelectedIndexChanged;
            // 
            // btnTambah
            // 
            btnTambah.BorderRadius = 15;
            btnTambah.CustomizableEdges = customizableEdges3;
            btnTambah.DisabledState.BorderColor = Color.DarkGray;
            btnTambah.DisabledState.CustomBorderColor = Color.DarkGray;
            btnTambah.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnTambah.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnTambah.FillColor = Color.Black;
            btnTambah.Font = new Font("Inter", 10.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTambah.ForeColor = Color.White;
            btnTambah.Image = (Image)resources.GetObject("btnTambah.Image");
            btnTambah.Location = new Point(17, 584);
            btnTambah.Name = "btnTambah";
            btnTambah.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnTambah.Size = new Size(151, 48);
            btnTambah.TabIndex = 23;
            btnTambah.Text = "Tambah";
            btnTambah.Click += btnTambah_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(33, 37, 41);
            panel2.Controls.Add(lblTersedia);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(lblTotalBuku);
            panel2.Location = new Point(0, 638);
            panel2.Name = "panel2";
            panel2.Size = new Size(1111, 70);
            panel2.TabIndex = 22;
            // 
            // lblTersedia
            // 
            lblTersedia.AutoSize = true;
            lblTersedia.BackColor = Color.Transparent;
            lblTersedia.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTersedia.ForeColor = Color.White;
            lblTersedia.Location = new Point(216, 25);
            lblTersedia.Name = "lblTersedia";
            lblTersedia.Size = new Size(137, 24);
            lblTersedia.TabIndex = 23;
            lblTersedia.Text = "Tersedia : 45";
            lblTersedia.Click += lblTersedia_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(193, 25);
            label4.Name = "label4";
            label4.Size = new Size(17, 24);
            label4.TabIndex = 22;
            label4.Text = "|";
            // 
            // lblTotalBuku
            // 
            lblTotalBuku.AutoSize = true;
            lblTotalBuku.BackColor = Color.Transparent;
            lblTotalBuku.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalBuku.ForeColor = Color.White;
            lblTotalBuku.Location = new Point(17, 25);
            lblTotalBuku.Name = "lblTotalBuku";
            lblTotalBuku.Size = new Size(170, 24);
            lblTotalBuku.TabIndex = 3;
            lblTotalBuku.Text = "Total Buku : 150 ";
            lblTotalBuku.Click += lblTotalBuku_Click;
            // 
            // dataGridBuku
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridBuku.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridBuku.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridBuku.ColumnHeadersHeight = 4;
            dataGridBuku.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridBuku.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridBuku.GridColor = Color.FromArgb(231, 229, 255);
            dataGridBuku.Location = new Point(17, 137);
            dataGridBuku.Name = "dataGridBuku";
            dataGridBuku.RowHeadersVisible = false;
            dataGridBuku.RowHeadersWidth = 51;
            dataGridBuku.Size = new Size(1091, 441);
            dataGridBuku.TabIndex = 7;
            dataGridBuku.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dataGridBuku.ThemeStyle.AlternatingRowsStyle.Font = null;
            dataGridBuku.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dataGridBuku.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dataGridBuku.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dataGridBuku.ThemeStyle.BackColor = Color.White;
            dataGridBuku.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dataGridBuku.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dataGridBuku.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridBuku.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dataGridBuku.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dataGridBuku.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridBuku.ThemeStyle.HeaderStyle.Height = 4;
            dataGridBuku.ThemeStyle.ReadOnly = false;
            dataGridBuku.ThemeStyle.RowsStyle.BackColor = Color.White;
            dataGridBuku.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBuku.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dataGridBuku.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridBuku.ThemeStyle.RowsStyle.Height = 29;
            dataGridBuku.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridBuku.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btnSearch
            // 
            btnSearch.BorderRadius = 15;
            btnSearch.CustomizableEdges = customizableEdges5;
            btnSearch.DisabledState.BorderColor = Color.DarkGray;
            btnSearch.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSearch.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSearch.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSearch.FillColor = Color.Black;
            btnSearch.Font = new Font("Inter", 10.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.ForeColor = Color.White;
            btnSearch.Image = (Image)resources.GetObject("btnSearch.Image");
            btnSearch.Location = new Point(779, 94);
            btnSearch.Name = "btnSearch";
            btnSearch.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnSearch.Size = new Size(151, 36);
            btnSearch.TabIndex = 6;
            btnSearch.Text = "Cari";
            btnSearch.Click += btnSearch_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(17, 94);
            label3.Name = "label3";
            label3.Size = new Size(112, 24);
            label3.TabIndex = 4;
            label3.Text = "Cari User :";
            // 
            // txtSearch
            // 
            txtSearch.BorderColor = Color.Silver;
            txtSearch.BorderRadius = 12;
            txtSearch.BorderThickness = 2;
            txtSearch.CustomizableEdges = customizableEdges7;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Inter", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.ForeColor = Color.Black;
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Location = new Point(135, 92);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Enter User Name";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtSearch.Size = new Size(638, 38);
            txtSearch.TabIndex = 5;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(33, 37, 41);
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1111, 70);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            // 
            btnRefresh.BorderRadius = 15;
            btnRefresh.CustomizableEdges = customizableEdges9;
            btnRefresh.DisabledState.BorderColor = Color.DarkGray;
            btnRefresh.DisabledState.CustomBorderColor = Color.DarkGray;
            btnRefresh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnRefresh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnRefresh.FillColor = Color.Black;
            btnRefresh.Font = new Font("Inter", 10.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Image = (Image)resources.GetObject("btnRefresh.Image");
            btnRefresh.Location = new Point(1041, 13);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnRefresh.Size = new Size(56, 45);
            btnRefresh.TabIndex = 21;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(17, 25);
            label1.Name = "label1";
            label1.Size = new Size(127, 24);
            label1.TabIndex = 3;
            label1.Text = "Daftar Buku";
            // 
            // Books
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1135, 732);
            Controls.Add(guna2Panel2);
            Name = "Books";
            Text = "Books";
            Load += Books_Load;
            guna2Panel2.ResumeLayout(false);
            guna2Panel2.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridBuku).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridBuku;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Label lblTotalBuku;
        private Label lblTersedia;
        private Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox comboKategori;
        private Guna.UI2.WinForms.Guna2Button btnTambah;
    }
}