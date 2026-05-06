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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Books));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            dataGridUser = new Guna.UI2.WinForms.Guna2DataGridView();
            btnSearch = new Guna.UI2.WinForms.Guna2Button();
            label3 = new Label();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            lblTotalBuku = new Label();
            label4 = new Label();
            lblTersedia = new Label();
            btnTambah = new Guna.UI2.WinForms.Guna2Button();
            comboKategori = new Guna.UI2.WinForms.Guna2ComboBox();
            guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridUser).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
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
            guna2Panel2.Controls.Add(dataGridUser);
            guna2Panel2.Controls.Add(btnSearch);
            guna2Panel2.Controls.Add(label3);
            guna2Panel2.Controls.Add(txtSearch);
            guna2Panel2.Controls.Add(panel1);
            guna2Panel2.CustomizableEdges = customizableEdges13;
            guna2Panel2.Location = new Point(12, 12);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges14;
            guna2Panel2.Size = new Size(1111, 708);
            guna2Panel2.TabIndex = 5;
            // 
            // dataGridUser
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridUser.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridUser.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridUser.ColumnHeadersHeight = 4;
            dataGridUser.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridUser.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridUser.GridColor = Color.FromArgb(231, 229, 255);
            dataGridUser.Location = new Point(17, 137);
            dataGridUser.Name = "dataGridUser";
            dataGridUser.RowHeadersVisible = false;
            dataGridUser.RowHeadersWidth = 51;
            dataGridUser.Size = new Size(1091, 441);
            dataGridUser.TabIndex = 7;
            dataGridUser.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dataGridUser.ThemeStyle.AlternatingRowsStyle.Font = null;
            dataGridUser.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dataGridUser.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dataGridUser.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dataGridUser.ThemeStyle.BackColor = Color.White;
            dataGridUser.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dataGridUser.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dataGridUser.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridUser.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dataGridUser.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dataGridUser.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridUser.ThemeStyle.HeaderStyle.Height = 4;
            dataGridUser.ThemeStyle.ReadOnly = false;
            dataGridUser.ThemeStyle.RowsStyle.BackColor = Color.White;
            dataGridUser.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridUser.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dataGridUser.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridUser.ThemeStyle.RowsStyle.Height = 29;
            dataGridUser.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridUser.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btnSearch
            // 
            btnSearch.BorderRadius = 15;
            btnSearch.CustomizableEdges = customizableEdges7;
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
            btnSearch.ShadowDecoration.CustomizableEdges = customizableEdges8;
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
            txtSearch.CustomizableEdges = customizableEdges9;
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
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtSearch.Size = new Size(638, 38);
            txtSearch.TabIndex = 5;
            // 
            // btnRefresh
            // 
            btnRefresh.BorderRadius = 15;
            btnRefresh.CustomizableEdges = customizableEdges11;
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
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnRefresh.Size = new Size(56, 45);
            btnRefresh.TabIndex = 21;
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
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(33, 37, 41);
            panel2.Controls.Add(lblTersedia);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(guna2Button1);
            panel2.Controls.Add(lblTotalBuku);
            panel2.Location = new Point(0, 638);
            panel2.Name = "panel2";
            panel2.Size = new Size(1111, 70);
            panel2.TabIndex = 22;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 15;
            guna2Button1.CustomizableEdges = customizableEdges5;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.Black;
            guna2Button1.Font = new Font("Inter", 10.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Image = (Image)resources.GetObject("guna2Button1.Image");
            guna2Button1.Location = new Point(1041, 13);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button1.Size = new Size(56, 45);
            guna2Button1.TabIndex = 21;
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
            comboKategori.Font = new Font("Segoe UI", 10F);
            comboKategori.ForeColor = Color.FromArgb(68, 88, 112);
            comboKategori.ItemHeight = 30;
            comboKategori.Location = new Point(936, 94);
            comboKategori.Name = "comboKategori";
            comboKategori.ShadowDecoration.CustomizableEdges = customizableEdges2;
            comboKategori.Size = new Size(161, 36);
            comboKategori.TabIndex = 25;
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
            ((System.ComponentModel.ISupportInitialize)dataGridUser).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridUser;
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Panel panel1;
        private Label label1;
        private Panel panel2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Label lblTotalBuku;
        private Label lblTersedia;
        private Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox comboKategori;
        private Guna.UI2.WinForms.Guna2Button btnTambah;
    }
}