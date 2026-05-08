using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.WinForms;
using SkiaSharp;

namespace Aplikasi_perpustakaan
{
    public partial class Home : Form
    {
        Koneksi kon = new Koneksi();

        public Home()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.Home_Load);
        }

        private async void Home_Load(object sender, EventArgs e)
        {
            StylingAllDataGridViews();
            await LoadDashboardData();
            await LoadChartData();
        }

        // ==================== LOAD DASHBOARD DATA ====================
        private async Task LoadDashboardData()
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    // 1. Total Buku
                    string queryBuku = "SELECT COUNT(*) FROM buku";
                    using (MySqlCommand cmd = new MySqlCommand(queryBuku, conn))
                    {
                        object result = await cmd.ExecuteScalarAsync();
                        int totalBuku = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblTotalBuku.Text = totalBuku.ToString(); // Format: 120
                    }

                    // 2. Total Anggota
                    string queryAnggota = "SELECT COUNT(*) FROM anggota WHERE status = 'aktif'";
                    using (MySqlCommand cmd = new MySqlCommand(queryAnggota, conn))
                    {
                        object result = await cmd.ExecuteScalarAsync();
                        int totalAnggota = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblTotalAnggota.Text = totalAnggota.ToString(); // Format: 56
                    }

                    // 3. Total Denda Bulan Ini
                    string queryDenda = @"SELECT COALESCE(SUM(total_denda), 0) 
                                         FROM pengembalian 
                                         WHERE MONTH(tanggal_kembali) = MONTH(CURRENT_DATE()) 
                                         AND YEAR(tanggal_kembali) = YEAR(CURRENT_DATE())
                                         AND status_denda != 'lunas'";
                    using (MySqlCommand cmd = new MySqlCommand(queryDenda, conn))
                    {
                        object result = await cmd.ExecuteScalarAsync();
                        decimal totalDenda = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                        lblTotalDenda.Text = $"Rp {totalDenda:N0}"; // Format: Rp 1.500.000
                    }

                    // 4. Load Riwayat Peminjaman
                    await LoadRiwayatPeminjaman(conn);

                    // 5. Load Buku Populer
                    await LoadBukuPopuler(conn);

                    // 6. Load Pengembalian Terlambat
                    await LoadPengembalianTerlambat(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data dashboard: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== LOAD CHART DATA ====================
        private async Task LoadChartData()
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    // Query peminjaman per hari dalam bulan ini
                    string query = @"SELECT 
                                        DAY(tanggal_pinjam) as hari,
                                        COUNT(*) as jumlah_peminjaman
                                     FROM peminjaman 
                                     WHERE MONTH(tanggal_pinjam) = MONTH(CURRENT_DATE()) 
                                     AND YEAR(tanggal_pinjam) = YEAR(CURRENT_DATE())
                                     GROUP BY DAY(tanggal_pinjam)
                                     ORDER BY hari";

                    List<int> labels = new List<int>();
                    List<int> values = new List<int>();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                        {
                            // Isi semua hari dalam bulan
                            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                            for (int i = 1; i <= daysInMonth; i++)
                            {
                                labels.Add(i);
                                values.Add(0);
                            }

                            // Update dengan data aktual
                            while (await reader.ReadAsync())
                            {
                                int hari = reader.GetInt32("hari");
                                int jumlah = reader.GetInt32("jumlah_peminjaman");
                                if (hari >= 1 && hari <= daysInMonth)
                                {
                                    values[hari - 1] = jumlah;
                                }
                            }
                        }
                    }

                    // Update chart
                    UpdateChart(labels, values);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data chart: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateChart(List<int> labels, List<int> values)
        {
            // Konfigurasi Chart
            chartPenjualan.Series = new ISeries[]
            {
                new LineSeries<int>
                {
                    Name = "Peminjaman",
                    Values = values,
                    Stroke = new SolidColorPaint(SKColors.DodgerBlue, 3),
                    GeometryStroke = new SolidColorPaint(SKColors.DodgerBlue, 2),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    GeometrySize = 8,
                    Fill = new LinearGradientPaint(
                        new SKColor(30, 144, 255, 80),
                        new SKColor(30, 144, 255, 10),
                        new SKPoint(0.5f, 0),
                        new SKPoint(0.5f, 1)
                    ),
                    LineSmoothness = 0.3
                },
                new ColumnSeries<int>
                {
                    Name = "Total",
                    Values = values,
                    Stroke = null,
                    Fill = new SolidColorPaint(SKColors.LightSkyBlue),
                    MaxBarWidth = 20
                }
            };

            // Konfigurasi X Axis
            chartPenjualan.XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Tanggal",
                    NameTextSize = 14,
                    NamePaint = new SolidColorPaint(SKColors.DimGray),
                    Labels = labels.Select(l => l.ToString()).ToArray(),
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    MinStep = 1
                }
            };

            // Konfigurasi Y Axis
            chartPenjualan.YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Jumlah Peminjaman",
                    NameTextSize = 14,
                    NamePaint = new SolidColorPaint(SKColors.DimGray),
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    MinLimit = 0
                }
            };

            // Tooltip Position
            chartPenjualan.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Top;

            // Legend Position
            chartPenjualan.LegendPosition = LiveChartsCore.Measure.LegendPosition.Top;

            // Animasi
            chartPenjualan.AnimationsSpeed = TimeSpan.FromMilliseconds(800);
        }

        // ==================== LOAD RIWAYAT PEMINJAMAN ====================
        private async Task LoadRiwayatPeminjaman(MySqlConnection conn)
        {
            string query = @"SELECT 
                                p.kode_peminjaman AS 'Kode',
                                a.nama AS 'Nama Anggota',
                                b.judul AS 'Judul Buku',
                                DATE_FORMAT(p.tanggal_pinjam, '%d/%m/%Y') AS 'Tgl Pinjam',
                                DATE_FORMAT(p.tanggal_jatuh_tempo, '%d/%m/%Y') AS 'Jatuh Tempo',
                                CASE 
                                    WHEN p.status = 'dipinjam' THEN 'Dipinjam'
                                    WHEN p.status = 'dikembalikan' THEN 'Dikembalikan'
                                END AS 'Status'
                             FROM peminjaman p
                             JOIN anggota a ON p.id_anggota = a.id_anggota
                             JOIN detail_peminjaman dp ON p.id_peminjaman = dp.id_peminjaman
                             JOIN buku b ON dp.id_buku = b.id_buku
                             ORDER BY p.tanggal_pinjam DESC
                             LIMIT 50";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() => da.Fill(dt));
                    dataGridRiwayatPeminjaman.DataSource = dt;
                }
            }
        }

        // ==================== LOAD BUKU POPULER ====================
        private async Task LoadBukuPopuler(MySqlConnection conn)
        {
            string query = @"SELECT 
                                b.judul AS 'Judul Buku',
                                b.penulis AS 'Penulis',
                                COUNT(dp.id_buku) AS 'Total Dipinjam'
                             FROM detail_peminjaman dp
                             JOIN buku b ON dp.id_buku = b.id_buku
                             GROUP BY dp.id_buku
                             ORDER BY COUNT(dp.id_buku) DESC
                             LIMIT 10";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() => da.Fill(dt));
                    dataGridBukuPopuler.DataSource = dt;
                }
            }
        }

        // ==================== LOAD PENGEMBALIAN TERLAMBAT (HANYA KOLOM UTAMA) ====================
        private async Task LoadPengembalianTerlambat(MySqlConnection conn)
        {
            // Hanya ambil 5 kolom utama
            string query = @"SELECT 
                                a.nama AS 'Nama',
                                b.judul AS 'Judul Buku',
                                pb.terlambat AS 'Terlambat (Hari)',
                                CONCAT('Rp ', FORMAT(pb.total_denda, 0)) AS 'Denda',
                                CASE 
                                    WHEN pb.status_denda = 'belum_bayar' THEN 'Belum Bayar'
                                    WHEN pb.status_denda = 'lunas' THEN 'Lunas'
                                    ELSE 'Tidak Ada'
                                END AS 'Status'
                             FROM pengembalian pb
                             JOIN peminjaman p ON pb.id_peminjaman = p.id_peminjaman
                             JOIN anggota a ON p.id_anggota = a.id_anggota
                             JOIN detail_peminjaman dp ON p.id_peminjaman = dp.id_peminjaman
                             JOIN buku b ON dp.id_buku = b.id_buku
                             WHERE pb.terlambat > 0
                             ORDER BY pb.tanggal_kembali DESC
                             LIMIT 20";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    await Task.Run(() => da.Fill(dt));
                    dataGridPengembalianTerlambat.DataSource = dt;
                }
            }

            // Set lebar kolom khusus untuk tabel pengembalian terlambat (karena ukuran kecil)
            if (dataGridPengembalianTerlambat.Columns.Count >= 5)
            {
                dataGridPengembalianTerlambat.Columns["Nama"].Width = 120;
                dataGridPengembalianTerlambat.Columns["Judul Buku"].Width = 150;
                dataGridPengembalianTerlambat.Columns["Terlambat (Hari)"].Width = 80;
                dataGridPengembalianTerlambat.Columns["Denda"].Width = 100;
                dataGridPengembalianTerlambat.Columns["Status"].Width = 80;
            }
        }

        // ==================== STYLING DATAGRIDVIEW ====================
        private void StylingAllDataGridViews()
        {
            // Styling untuk semua DataGridView
            StyleDataGridView(dataGridRiwayatPeminjaman);
            StyleDataGridView(dataGridBukuPopuler);
            StyleDataGridView(dataGridPengembalianTerlambat);
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            // Basic Settings
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToResizeColumns = true; // Diubah ke true agar bisa di-resize manual
            dgv.AllowUserToResizeRows = false;

            // Display Settings
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.BackgroundColor = Color.White;

            // Row Style
            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            // Font
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            // Selection
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(65, 105, 225); // Royal Blue
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // Row Height
            dgv.RowTemplate.Height = 35;

            // Header Style
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72); // Navy Dark
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 0, 0);

            // Cell Padding
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
        }

        // ==================== CLICK EVENTS ====================
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void lblTotalDenda_Click(object sender, EventArgs e) { }
        private void lblTotalBuku_Click(object sender, EventArgs e) { }
        private void lblTotalAnggota_Click(object sender, EventArgs e) { }
        private void chartPenjualan_Load(object sender, EventArgs e) { }
        private void dataGridRiwayatPeminjaman_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridBukuPopuler_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridPengembalianTerlambat_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}