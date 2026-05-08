namespace Aplikasi_perpustakaan
{
    partial class Home
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
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend skDefaultLegend1 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultLegend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            LiveChartsCore.Drawing.Padding padding1 = new LiveChartsCore.Drawing.Padding();
            LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip skDefaultTooltip1 = new LiveChartsCore.SkiaSharpView.SKCharts.SKDefaultTooltip();
            LiveChartsCore.Drawing.Padding padding2 = new LiveChartsCore.Drawing.Padding();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            chartPenjualan = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            label3 = new Label();
            guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            lblTotalBuku = new Label();
            label6 = new Label();
            guna2ShadowPanel4 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            guna2ShadowPanel3 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            lblTotalAnggota = new Label();
            label7 = new Label();
            guna2ShadowPanel5 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            lblTotalDenda = new Label();
            label5 = new Label();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            dataGridRiwayatPeminjaman = new Guna.UI2.WinForms.Guna2DataGridView();
            panel2 = new Panel();
            lblTitle = new Label();
            guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            dataGridBukuPopuler = new Guna.UI2.WinForms.Guna2DataGridView();
            panel1 = new Panel();
            label1 = new Label();
            guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            dataGridPengembalianTerlambat = new Guna.UI2.WinForms.Guna2DataGridView();
            panel3 = new Panel();
            label2 = new Label();
            mySqlCommand1 = new MySql.Data.MySqlClient.MySqlCommand();
            guna2ShadowPanel1.SuspendLayout();
            guna2ShadowPanel2.SuspendLayout();
            guna2ShadowPanel3.SuspendLayout();
            guna2ShadowPanel5.SuspendLayout();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridRiwayatPeminjaman).BeginInit();
            panel2.SuspendLayout();
            guna2Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridBukuPopuler).BeginInit();
            panel1.SuspendLayout();
            guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridPengembalianTerlambat).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // guna2ShadowPanel1
            // 
            guna2ShadowPanel1.BackColor = Color.Transparent;
            guna2ShadowPanel1.Controls.Add(chartPenjualan);
            guna2ShadowPanel1.Controls.Add(label3);
            guna2ShadowPanel1.FillColor = Color.White;
            guna2ShadowPanel1.Location = new Point(12, 12);
            guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            guna2ShadowPanel1.ShadowColor = Color.Black;
            guna2ShadowPanel1.Size = new Size(1111, 289);
            guna2ShadowPanel1.TabIndex = 0;
            // 
            // chartPenjualan
            // 
            chartPenjualan.AutoUpdateEnabled = true;
            chartPenjualan.ChartTheme = null;
            skDefaultLegend1.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultLegend1.Content = null;
            skDefaultLegend1.IsValid = false;
            skDefaultLegend1.Opacity = 1F;
            padding1.Bottom = 0F;
            padding1.Left = 0F;
            padding1.Right = 0F;
            padding1.Top = 0F;
            skDefaultLegend1.Padding = padding1;
            skDefaultLegend1.RemoveOnCompleted = false;
            skDefaultLegend1.RotateTransform = 0F;
            skDefaultLegend1.X = 0F;
            skDefaultLegend1.Y = 0F;
            chartPenjualan.Legend = skDefaultLegend1;
            chartPenjualan.Location = new Point(23, 45);
            chartPenjualan.MatchAxesScreenDataRatio = false;
            chartPenjualan.Name = "chartPenjualan";
            chartPenjualan.Size = new Size(1060, 220);
            chartPenjualan.TabIndex = 4;
            skDefaultTooltip1.AnimationsSpeed = TimeSpan.Parse("00:00:00.1500000");
            skDefaultTooltip1.Content = null;
            skDefaultTooltip1.IsValid = false;
            skDefaultTooltip1.Opacity = 1F;
            padding2.Bottom = 0F;
            padding2.Left = 0F;
            padding2.Right = 0F;
            padding2.Top = 0F;
            skDefaultTooltip1.Padding = padding2;
            skDefaultTooltip1.RemoveOnCompleted = false;
            skDefaultTooltip1.RotateTransform = 0F;
            skDefaultTooltip1.Wedge = 10;
            skDefaultTooltip1.X = 0F;
            skDefaultTooltip1.Y = 0F;
            chartPenjualan.Tooltip = skDefaultTooltip1;
            chartPenjualan.TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.Automatic;
            chartPenjualan.UpdaterThrottler = TimeSpan.Parse("00:00:00.0500000");
            chartPenjualan.Load += chartPenjualan_Load;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Black;
            label3.Location = new Point(23, 18);
            label3.Name = "label3";
            label3.Size = new Size(277, 24);
            label3.TabIndex = 3;
            label3.Text = "Grafik Peminjaman Bulanan";
            label3.Click += label3_Click;
            // 
            // guna2ShadowPanel2
            // 
            guna2ShadowPanel2.BackColor = Color.Transparent;
            guna2ShadowPanel2.Controls.Add(lblTotalBuku);
            guna2ShadowPanel2.Controls.Add(label6);
            guna2ShadowPanel2.Controls.Add(guna2ShadowPanel4);
            guna2ShadowPanel2.FillColor = Color.White;
            guna2ShadowPanel2.Location = new Point(12, 317);
            guna2ShadowPanel2.Name = "guna2ShadowPanel2";
            guna2ShadowPanel2.ShadowColor = Color.Black;
            guna2ShadowPanel2.Size = new Size(363, 146);
            guna2ShadowPanel2.TabIndex = 1;
            // 
            // lblTotalBuku
            // 
            lblTotalBuku.AutoSize = true;
            lblTotalBuku.BackColor = Color.Transparent;
            lblTotalBuku.Font = new Font("Inter", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalBuku.ForeColor = Color.Black;
            lblTotalBuku.Location = new Point(140, 61);
            lblTotalBuku.Name = "lblTotalBuku";
            lblTotalBuku.Size = new Size(79, 41);
            lblTotalBuku.TabIndex = 7;
            lblTotalBuku.Text = "120";
            lblTotalBuku.Click += lblTotalBuku_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Inter", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(128, 24);
            label6.Name = "label6";
            label6.Size = new Size(102, 21);
            label6.TabIndex = 6;
            label6.Text = "Total Buku";
            // 
            // guna2ShadowPanel4
            // 
            guna2ShadowPanel4.BackColor = Color.Transparent;
            guna2ShadowPanel4.FillColor = Color.White;
            guna2ShadowPanel4.Location = new Point(365, 0);
            guna2ShadowPanel4.Name = "guna2ShadowPanel4";
            guna2ShadowPanel4.ShadowColor = Color.Black;
            guna2ShadowPanel4.Size = new Size(370, 186);
            guna2ShadowPanel4.TabIndex = 2;
            // 
            // guna2ShadowPanel3
            // 
            guna2ShadowPanel3.BackColor = Color.Transparent;
            guna2ShadowPanel3.Controls.Add(lblTotalAnggota);
            guna2ShadowPanel3.Controls.Add(label7);
            guna2ShadowPanel3.FillColor = Color.White;
            guna2ShadowPanel3.Location = new Point(753, 317);
            guna2ShadowPanel3.Name = "guna2ShadowPanel3";
            guna2ShadowPanel3.ShadowColor = Color.Black;
            guna2ShadowPanel3.Size = new Size(363, 146);
            guna2ShadowPanel3.TabIndex = 2;
            // 
            // lblTotalAnggota
            // 
            lblTotalAnggota.AutoSize = true;
            lblTotalAnggota.BackColor = Color.Transparent;
            lblTotalAnggota.Font = new Font("Inter", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalAnggota.ForeColor = Color.Black;
            lblTotalAnggota.Location = new Point(158, 64);
            lblTotalAnggota.Name = "lblTotalAnggota";
            lblTotalAnggota.Size = new Size(62, 41);
            lblTotalAnggota.TabIndex = 8;
            lblTotalAnggota.Text = "56";
            lblTotalAnggota.Click += lblTotalAnggota_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Inter", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(124, 24);
            label7.Name = "label7";
            label7.Size = new Size(132, 21);
            label7.TabIndex = 5;
            label7.Text = "Total Anggota";
            // 
            // guna2ShadowPanel5
            // 
            guna2ShadowPanel5.BackColor = Color.Transparent;
            guna2ShadowPanel5.Controls.Add(lblTotalDenda);
            guna2ShadowPanel5.Controls.Add(label5);
            guna2ShadowPanel5.FillColor = Color.White;
            guna2ShadowPanel5.Location = new Point(383, 317);
            guna2ShadowPanel5.Name = "guna2ShadowPanel5";
            guna2ShadowPanel5.ShadowColor = Color.Black;
            guna2ShadowPanel5.Size = new Size(364, 146);
            guna2ShadowPanel5.TabIndex = 3;
            // 
            // lblTotalDenda
            // 
            lblTotalDenda.AutoSize = true;
            lblTotalDenda.BackColor = Color.Transparent;
            lblTotalDenda.Font = new Font("Inter", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalDenda.ForeColor = Color.Black;
            lblTotalDenda.Location = new Point(93, 64);
            lblTotalDenda.Name = "lblTotalDenda";
            lblTotalDenda.Size = new Size(164, 28);
            lblTotalDenda.TabIndex = 9;
            lblTotalDenda.Text = "Rp 1.500.000";
            lblTotalDenda.Click += lblTotalDenda_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Inter", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Black;
            label5.Location = new Point(124, 24);
            label5.Name = "label5";
            label5.Size = new Size(114, 21);
            label5.TabIndex = 5;
            label5.Text = "Total Denda";
            // 
            // guna2Panel1
            // 
            guna2Panel1.BackColor = Color.FromArgb(240, 242, 245);
            guna2Panel1.BorderColor = Color.Silver;
            guna2Panel1.BorderRadius = 10;
            guna2Panel1.BorderThickness = 1;
            guna2Panel1.Controls.Add(dataGridRiwayatPeminjaman);
            guna2Panel1.Controls.Add(panel2);
            guna2Panel1.CustomizableEdges = customizableEdges1;
            guna2Panel1.Location = new Point(12, 481);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Panel1.Size = new Size(1104, 271);
            guna2Panel1.TabIndex = 57;
            // 
            // dataGridRiwayatPeminjaman
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridRiwayatPeminjaman.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridRiwayatPeminjaman.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridRiwayatPeminjaman.ColumnHeadersHeight = 4;
            dataGridRiwayatPeminjaman.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridRiwayatPeminjaman.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridRiwayatPeminjaman.GridColor = Color.FromArgb(231, 229, 255);
            dataGridRiwayatPeminjaman.Location = new Point(13, 79);
            dataGridRiwayatPeminjaman.Name = "dataGridRiwayatPeminjaman";
            dataGridRiwayatPeminjaman.RowHeadersVisible = false;
            dataGridRiwayatPeminjaman.RowHeadersWidth = 51;
            dataGridRiwayatPeminjaman.Size = new Size(1079, 181);
            dataGridRiwayatPeminjaman.TabIndex = 2;
            dataGridRiwayatPeminjaman.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dataGridRiwayatPeminjaman.ThemeStyle.AlternatingRowsStyle.Font = null;
            dataGridRiwayatPeminjaman.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dataGridRiwayatPeminjaman.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dataGridRiwayatPeminjaman.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dataGridRiwayatPeminjaman.ThemeStyle.BackColor = Color.White;
            dataGridRiwayatPeminjaman.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dataGridRiwayatPeminjaman.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dataGridRiwayatPeminjaman.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridRiwayatPeminjaman.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dataGridRiwayatPeminjaman.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dataGridRiwayatPeminjaman.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridRiwayatPeminjaman.ThemeStyle.HeaderStyle.Height = 4;
            dataGridRiwayatPeminjaman.ThemeStyle.ReadOnly = false;
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.BackColor = Color.White;
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.Height = 29;
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridRiwayatPeminjaman.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridRiwayatPeminjaman.CellContentClick += dataGridRiwayatPeminjaman_CellContentClick;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(33, 37, 41);
            panel2.Controls.Add(lblTitle);
            panel2.Location = new Point(0, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(1101, 70);
            panel2.TabIndex = 1;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(23, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(175, 24);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "Aktifitas Terbaru";
            // 
            // guna2Panel2
            // 
            guna2Panel2.BackColor = Color.FromArgb(240, 242, 245);
            guna2Panel2.BorderColor = Color.Silver;
            guna2Panel2.BorderRadius = 10;
            guna2Panel2.BorderThickness = 1;
            guna2Panel2.Controls.Add(dataGridBukuPopuler);
            guna2Panel2.Controls.Add(panel1);
            guna2Panel2.CustomizableEdges = customizableEdges3;
            guna2Panel2.Location = new Point(12, 770);
            guna2Panel2.Name = "guna2Panel2";
            guna2Panel2.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Panel2.Size = new Size(548, 271);
            guna2Panel2.TabIndex = 58;
            // 
            // dataGridBukuPopuler
            // 
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridBukuPopuler.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dataGridBukuPopuler.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridBukuPopuler.ColumnHeadersHeight = 4;
            dataGridBukuPopuler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dataGridBukuPopuler.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridBukuPopuler.GridColor = Color.FromArgb(231, 229, 255);
            dataGridBukuPopuler.Location = new Point(13, 79);
            dataGridBukuPopuler.Name = "dataGridBukuPopuler";
            dataGridBukuPopuler.RowHeadersVisible = false;
            dataGridBukuPopuler.RowHeadersWidth = 51;
            dataGridBukuPopuler.Size = new Size(521, 181);
            dataGridBukuPopuler.TabIndex = 3;
            dataGridBukuPopuler.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dataGridBukuPopuler.ThemeStyle.AlternatingRowsStyle.Font = null;
            dataGridBukuPopuler.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dataGridBukuPopuler.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dataGridBukuPopuler.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dataGridBukuPopuler.ThemeStyle.BackColor = Color.White;
            dataGridBukuPopuler.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dataGridBukuPopuler.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dataGridBukuPopuler.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridBukuPopuler.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dataGridBukuPopuler.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dataGridBukuPopuler.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridBukuPopuler.ThemeStyle.HeaderStyle.Height = 4;
            dataGridBukuPopuler.ThemeStyle.ReadOnly = false;
            dataGridBukuPopuler.ThemeStyle.RowsStyle.BackColor = Color.White;
            dataGridBukuPopuler.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBukuPopuler.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dataGridBukuPopuler.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridBukuPopuler.ThemeStyle.RowsStyle.Height = 29;
            dataGridBukuPopuler.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridBukuPopuler.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridBukuPopuler.CellContentClick += dataGridBukuPopuler_CellContentClick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(33, 37, 41);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(0, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1101, 70);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(23, 25);
            label1.Name = "label1";
            label1.Size = new Size(170, 24);
            label1.TabIndex = 2;
            label1.Text = "Buku Terpopuler";
            // 
            // guna2Panel3
            // 
            guna2Panel3.BackColor = Color.FromArgb(240, 242, 245);
            guna2Panel3.BorderColor = Color.Silver;
            guna2Panel3.BorderRadius = 10;
            guna2Panel3.BorderThickness = 1;
            guna2Panel3.Controls.Add(dataGridPengembalianTerlambat);
            guna2Panel3.Controls.Add(panel3);
            guna2Panel3.CustomizableEdges = customizableEdges5;
            guna2Panel3.Location = new Point(568, 770);
            guna2Panel3.Name = "guna2Panel3";
            guna2Panel3.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Panel3.Size = new Size(548, 271);
            guna2Panel3.TabIndex = 59;
            // 
            // dataGridPengembalianTerlambat
            // 
            dataGridViewCellStyle7.BackColor = Color.White;
            dataGridPengembalianTerlambat.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = Color.White;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.True;
            dataGridPengembalianTerlambat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            dataGridPengembalianTerlambat.ColumnHeadersHeight = 4;
            dataGridPengembalianTerlambat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.White;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle9.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle9.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            dataGridPengembalianTerlambat.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridPengembalianTerlambat.GridColor = Color.FromArgb(231, 229, 255);
            dataGridPengembalianTerlambat.Location = new Point(15, 79);
            dataGridPengembalianTerlambat.Name = "dataGridPengembalianTerlambat";
            dataGridPengembalianTerlambat.RowHeadersVisible = false;
            dataGridPengembalianTerlambat.RowHeadersWidth = 51;
            dataGridPengembalianTerlambat.Size = new Size(521, 181);
            dataGridPengembalianTerlambat.TabIndex = 4;
            dataGridPengembalianTerlambat.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dataGridPengembalianTerlambat.ThemeStyle.AlternatingRowsStyle.Font = null;
            dataGridPengembalianTerlambat.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dataGridPengembalianTerlambat.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dataGridPengembalianTerlambat.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dataGridPengembalianTerlambat.ThemeStyle.BackColor = Color.White;
            dataGridPengembalianTerlambat.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dataGridPengembalianTerlambat.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dataGridPengembalianTerlambat.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridPengembalianTerlambat.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dataGridPengembalianTerlambat.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dataGridPengembalianTerlambat.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridPengembalianTerlambat.ThemeStyle.HeaderStyle.Height = 4;
            dataGridPengembalianTerlambat.ThemeStyle.ReadOnly = false;
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.BackColor = Color.White;
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.Height = 29;
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridPengembalianTerlambat.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridPengembalianTerlambat.CellContentClick += dataGridPengembalianTerlambat_CellContentClick;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(33, 37, 41);
            panel3.Controls.Add(label2);
            panel3.Location = new Point(0, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(1101, 70);
            panel3.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Inter", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(23, 25);
            label2.Name = "label2";
            label2.Size = new Size(233, 24);
            label2.TabIndex = 2;
            label2.Text = "Riwayat Pengembalian";
            // 
            // mySqlCommand1
            // 
            mySqlCommand1.CacheAge = 0;
            mySqlCommand1.Connection = null;
            mySqlCommand1.EnableCaching = false;
            mySqlCommand1.Transaction = null;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1135, 1053);
            Controls.Add(guna2Panel3);
            Controls.Add(guna2Panel2);
            Controls.Add(guna2Panel1);
            Controls.Add(guna2ShadowPanel5);
            Controls.Add(guna2ShadowPanel3);
            Controls.Add(guna2ShadowPanel2);
            Controls.Add(guna2ShadowPanel1);
            Name = "Home";
            Text = "Home";
            guna2ShadowPanel1.ResumeLayout(false);
            guna2ShadowPanel1.PerformLayout();
            guna2ShadowPanel2.ResumeLayout(false);
            guna2ShadowPanel2.PerformLayout();
            guna2ShadowPanel3.ResumeLayout(false);
            guna2ShadowPanel3.PerformLayout();
            guna2ShadowPanel5.ResumeLayout(false);
            guna2ShadowPanel5.PerformLayout();
            guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridRiwayatPeminjaman).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            guna2Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridBukuPopuler).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            guna2Panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridPengembalianTerlambat).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel2;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel4;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel3;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel5;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Panel panel2;
        private Label lblTitle;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Panel panel1;
        private Label label1;
        private Label label3;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridRiwayatPeminjaman;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridBukuPopuler;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2DataGridView dataGridPengembalianTerlambat;
        private Panel panel3;
        private Label label2;
        private Label label6;
        private Label label7;
        private Label label5;
        private Label lblTotalBuku;
        private Label lblTotalAnggota;
        private Label lblTotalDenda;
        private MySql.Data.MySqlClient.MySqlCommand mySqlCommand1;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart chartPenjualan;
    }
}