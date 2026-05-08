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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

using Rectangle = System.Drawing.Rectangle;

namespace Aplikasi_perpustakaan
{
    public partial class DetailTransaction : Form
    {
        private Koneksi koneksi;
        private int idPeminjaman;
        private DataTable dataBukuDipinjam;
        private DataTable dataRincianDenda;

        public DetailTransaction(int idPeminjaman)
        {
            InitializeComponent();
            koneksi = new Koneksi();
            this.idPeminjaman = idPeminjaman;

            this.Load += new System.EventHandler(this.DetailTransaction_Load);

            this.AutoScroll = true;
            this.MinimumSize = new Size(800, 600);

            this.btnDownloadLaporan.Click += new System.EventHandler(this.btnDownloadLaporan_Click);
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.txtTotalDenda.ReadOnly = true;
            this.txtTotalDenda.BackColor = SystemColors.ControlLight;

            if (this.peringatanNoDenda != null)
            {
                this.peringatanNoDenda.Visible = false;
                this.peringatanNoDenda.Text = "✅ User tidak memiliki denda";
                this.peringatanNoDenda.ForeColor = Color.FromArgb(46, 204, 113); 
                this.peringatanNoDenda.Font = new System.Drawing.Font("Inter", 11f, System.Drawing.FontStyle.Bold);
            }
        }

        private async void DetailTransaction_Load(object sender, EventArgs e)
        {
            StylingDataGridBuku();
            StylingDataGridDenda();
            await LoadDetailTransaksi();
        }

        private async Task LoadDetailTransaksi()
        {
            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    await conn.OpenAsync();

                    string query = @"
                        SELECT 
                            p.kode_peminjaman,
                            p.tanggal_pinjam,
                            p.tanggal_jatuh_tempo,
                            p.status AS status_pinjam,
                            a.nama AS nama_anggota,
                            a.kode_anggota,
                            u.nama AS nama_petugas_pinjam,
                            pg.tanggal_kembali,
                            pg.terlambat,
                            pg.total_denda,
                            pg.status_denda,
                            up.nama AS nama_petugas_kembali
                        FROM peminjaman p
                        INNER JOIN anggota a ON p.id_anggota = a.id_anggota
                        INNER JOIN users u ON p.id_user = u.id_user
                        LEFT JOIN pengembalian pg ON p.id_peminjaman = pg.id_peminjaman
                        LEFT JOIN users up ON pg.id_user = up.id_user
                        WHERE p.id_peminjaman = @id_peminjaman";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_peminjaman", idPeminjaman);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            // Info Peminjaman
                            lblKodePeminjaman.Text = $"Kode Peminjaman : {reader["kode_peminjaman"]}";
                            lblNamaAnggota.Text = $"Nama Anggota : {reader["nama_anggota"]}";
                            lblKodeAnggota.Text = $"Kode Anggota : {reader["kode_anggota"]}";
                            lblTanggalPinjam.Text = $"Tanggal Pinjam : {Convert.ToDateTime(reader["tanggal_pinjam"]).ToString("dd MMMM yyyy")}";
                            lblTanggalTempo.Text = $"Tanggal Jatuh Tempo : {Convert.ToDateTime(reader["tanggal_jatuh_tempo"]).ToString("dd MMMM yyyy")}";

                            string status = reader["status_pinjam"].ToString();
                            string statusDisplay = status == "dipinjam" ? "📌 Masih Dipinjam" : "✅ Dikembalikan";
                            lblStatusPinjam.Text = $"Status Peminjaman : {statusDisplay}";
                            lblStatusPinjam.ForeColor = status == "dipinjam" ? Color.FromArgb(243, 156, 18) : Color.FromArgb(46, 204, 113);

                            if (reader["tanggal_kembali"] != DBNull.Value)
                            {
                                lblTanggalPengembalian.Text = $"Tanggal Pengembalian : {Convert.ToDateTime(reader["tanggal_kembali"]).ToString("dd MMMM yyyy")}";
                                lblTerlambat.Text = $"Terlambat : {reader["terlambat"]} hari";
                                lblTotalDenda.Text = $"Total Denda : Rp {Convert.ToDecimal(reader["total_denda"]).ToString("N0")}";

                                string statusDenda = reader["status_denda"].ToString();
                                string statusDendaDisplay = statusDenda == "lunas" ? "✅ Lunas" :
                                                           statusDenda == "belum_bayar" ? "⚠️ Belum Bayar" : "✅ Tidak Ada Denda";
                                lblStatusDenda.Text = $"Status Denda : {statusDendaDisplay}";
                                lblStatusDenda.ForeColor = statusDenda == "belum_bayar" ? Color.Red : Color.Green;

                                lblNamaPetugas.Text = $"Petugas Pengembalian : {reader["nama_petugas_kembali"]}";

                                int terlambat = Convert.ToInt32(reader["terlambat"]);
                                decimal dendaTerlambat = terlambat * 2000;
                                lblDendaTerlambat.Text = $"Denda Keterlambatan : Rp {dendaTerlambat.ToString("N0")} ({terlambat} hari x Rp 2.000)";

                                decimal totalDenda = Convert.ToDecimal(reader["total_denda"]);
                                decimal dendaBukuHilang = totalDenda - dendaTerlambat;
                                if (dendaBukuHilang < 0) dendaBukuHilang = 0;
                                lblDendaBukuHilang.Text = $"Denda Buku Hilang/Tidak Kembali : Rp {dendaBukuHilang.ToString("N0")}";
                            }
                            else
                            {
                                lblTanggalPengembalian.Text = "Tanggal Pengembalian : -";
                                lblTerlambat.Text = "Terlambat : -";
                                lblDendaTerlambat.Text = "Denda Keterlambatan : -";
                                lblDendaBukuHilang.Text = "Denda Buku Hilang/Tidak Kembali : -";
                                lblTotalDenda.Text = "Total Denda : -";
                                lblStatusDenda.Text = "Status Denda : -";
                                lblNamaPetugas.Text = "Petugas Pengembalian : -";
                            }
                        }
                    }

                    string queryBuku = @"
                        SELECT 
                            b.kode_buku,
                            b.judul,
                            b.penulis,
                            b.penerbit,
                            dp.jumlah
                        FROM detail_peminjaman dp
                        INNER JOIN buku b ON dp.id_buku = b.id_buku
                        WHERE dp.id_peminjaman = @id_peminjaman
                        ORDER BY b.judul ASC";

                    MySqlCommand cmdBuku = new MySqlCommand(queryBuku, conn);
                    cmdBuku.Parameters.AddWithValue("@id_peminjaman", idPeminjaman);

                    MySqlDataAdapter daBuku = new MySqlDataAdapter(cmdBuku);
                    dataBukuDipinjam = new DataTable();
                    await Task.Run(() => daBuku.Fill(dataBukuDipinjam));

                    // Clear dulu sebelum set DataSource
                    dataGridBukuDipinjam.DataSource = null;
                    dataGridBukuDipinjam.Columns.Clear();

                    if (dataBukuDipinjam.Rows.Count > 0)
                    {
                        dataGridBukuDipinjam.DataSource = dataBukuDipinjam;

                        if (dataGridBukuDipinjam.Columns.Contains("kode_buku"))
                            dataGridBukuDipinjam.Columns["kode_buku"].HeaderText = "Kode Buku";
                        if (dataGridBukuDipinjam.Columns.Contains("judul"))
                            dataGridBukuDipinjam.Columns["judul"].HeaderText = "Judul Buku";
                        if (dataGridBukuDipinjam.Columns.Contains("penulis"))
                            dataGridBukuDipinjam.Columns["penulis"].HeaderText = "Penulis";
                        if (dataGridBukuDipinjam.Columns.Contains("penerbit"))
                            dataGridBukuDipinjam.Columns["penerbit"].HeaderText = "Penerbit";
                        if (dataGridBukuDipinjam.Columns.Contains("jumlah"))
                            dataGridBukuDipinjam.Columns["jumlah"].HeaderText = "Jumlah";

                        if (!dataGridBukuDipinjam.Columns.Contains("No"))
                        {
                            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                            colNo.Name = "No";
                            colNo.HeaderText = "No";
                            colNo.Width = 40;
                            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            dataGridBukuDipinjam.Columns.Insert(0, colNo);
                        }

                        for (int i = 0; i < dataGridBukuDipinjam.Rows.Count; i++)
                        {
                            dataGridBukuDipinjam.Rows[i].Cells["No"].Value = (i + 1).ToString();
                        }
                    }
                    else
                    {
                        DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                        colNo.Name = "No";
                        colNo.HeaderText = "No";
                        colNo.Width = 40;
                        dataGridBukuDipinjam.Columns.Add(colNo);

                        DataGridViewTextBoxColumn colInfo = new DataGridViewTextBoxColumn();
                        colInfo.Name = "Info";
                        colInfo.HeaderText = "Informasi";
                        colInfo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridBukuDipinjam.Columns.Add(colInfo);

                        dataGridBukuDipinjam.Rows.Add("", "Tidak ada data buku yang dipinjam");
                    }
                    string queryDenda = @"
                        SELECT 
                            pg.terlambat,
                            pg.total_denda,
                            pg.status_denda
                        FROM pengembalian pg
                        WHERE pg.id_peminjaman = @id_peminjaman";

                    MySqlCommand cmdDenda = new MySqlCommand(queryDenda, conn);
                    cmdDenda.Parameters.AddWithValue("@id_peminjaman", idPeminjaman);

                    MySqlDataAdapter daDenda = new MySqlDataAdapter(cmdDenda);
                    dataRincianDenda = new DataTable();
                    await Task.Run(() => daDenda.Fill(dataRincianDenda));

                    DataTable rincianDenda = new DataTable();
                    rincianDenda.Columns.Add("No", typeof(int));
                    rincianDenda.Columns.Add("Keterangan", typeof(string));
                    rincianDenda.Columns.Add("Jumlah", typeof(string));
                    rincianDenda.Columns.Add("Denda", typeof(string));

                    bool hasDenda = false;
                    decimal totalDendaValue = 0;

                    if (dataRincianDenda.Rows.Count > 0)
                    {
                        int terlambat = Convert.ToInt32(dataRincianDenda.Rows[0]["terlambat"]);
                        totalDendaValue = Convert.ToDecimal(dataRincianDenda.Rows[0]["total_denda"]);

                        if (totalDendaValue > 0)
                        {
                            hasDenda = true;
                            int no = 1;

                            if (terlambat > 0)
                            {
                                decimal dendaTerlambat = terlambat * 2000;
                                rincianDenda.Rows.Add(no++, $"Keterlambatan pengembalian ({terlambat} hari x Rp 2.000)", $"{terlambat} hari", $"Rp {dendaTerlambat:N0}");
                            }

                            decimal dendaBukuHilang = totalDendaValue - (terlambat * 2000);
                            if (dendaBukuHilang < 0) dendaBukuHilang = 0;
                            if (dendaBukuHilang > 0)
                            {
                                int jumlahBukuHilang = (int)(dendaBukuHilang / 50000);
                                if (jumlahBukuHilang < 1) jumlahBukuHilang = 1;
                                rincianDenda.Rows.Add(no++, $"Buku hilang/tidak kembali ({jumlahBukuHilang} eks x Rp 50.000)", $"{jumlahBukuHilang} buku", $"Rp {dendaBukuHilang:N0}");
                            }

                            if (rincianDenda.Rows.Count > 0)
                            {
                                rincianDenda.Rows.Add(0, "──────────────────────────────", "", "");
                                rincianDenda.Rows.Add(0, "TOTAL DENDA", "", $"Rp {totalDendaValue:N0}");
                            }
                        }
                    }

                    dataGridRincianDenda.DataSource = null;
                    dataGridRincianDenda.Columns.Clear();

                    if (hasDenda)
                    {
                        dataGridRincianDenda.DataSource = rincianDenda;

                        if (dataGridRincianDenda.Columns.Contains("No"))
                            dataGridRincianDenda.Columns["No"].Width = 40;
                        if (dataGridRincianDenda.Columns.Contains("Keterangan"))
                            dataGridRincianDenda.Columns["Keterangan"].Width = 300;
                        if (dataGridRincianDenda.Columns.Contains("Jumlah"))
                            dataGridRincianDenda.Columns["Jumlah"].Width = 100;
                        if (dataGridRincianDenda.Columns.Contains("Denda"))
                            dataGridRincianDenda.Columns["Denda"].Width = 150;

                        if (peringatanNoDenda != null)
                            peringatanNoDenda.Visible = false;

                        txtTotalDenda.Text = $"Rp {totalDendaValue:N0}";
                    }
                    else
                    {
                        DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                        colNo.Name = "No";
                        colNo.HeaderText = "No";
                        colNo.Width = 40;
                        colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dataGridRincianDenda.Columns.Add(colNo);

                        DataGridViewTextBoxColumn colKeterangan = new DataGridViewTextBoxColumn();
                        colKeterangan.Name = "Keterangan";
                        colKeterangan.HeaderText = "Keterangan";
                        colKeterangan.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridRincianDenda.Columns.Add(colKeterangan);

                        DataGridViewTextBoxColumn colJumlah = new DataGridViewTextBoxColumn();
                        colJumlah.Name = "Jumlah";
                        colJumlah.HeaderText = "Jumlah";
                        colJumlah.Width = 100;
                        colJumlah.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dataGridRincianDenda.Columns.Add(colJumlah);

                        DataGridViewTextBoxColumn colDenda = new DataGridViewTextBoxColumn();
                        colDenda.Name = "Denda";
                        colDenda.HeaderText = "Denda";
                        colDenda.Width = 150;
                        colDenda.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dataGridRincianDenda.Columns.Add(colDenda);

                        if (peringatanNoDenda != null)
                        {
                            peringatanNoDenda.Visible = true;
                        }

                        txtTotalDenda.Text = "Rp 0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat detail transaksi: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StylingDataGridBuku()
        {
            dataGridBukuDipinjam.ReadOnly = true;
            dataGridBukuDipinjam.AllowUserToAddRows = false;
            dataGridBukuDipinjam.AllowUserToDeleteRows = false;
            dataGridBukuDipinjam.AllowUserToOrderColumns = false;
            dataGridBukuDipinjam.AllowUserToResizeColumns = false;
            dataGridBukuDipinjam.AllowUserToResizeRows = false;
            dataGridBukuDipinjam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridBukuDipinjam.RowHeadersVisible = false;
            dataGridBukuDipinjam.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridBukuDipinjam.BorderStyle = BorderStyle.None;
            dataGridBukuDipinjam.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBukuDipinjam.BackgroundColor = Color.White;
            dataGridBukuDipinjam.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridBukuDipinjam.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridBukuDipinjam.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridBukuDipinjam.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular);
            dataGridBukuDipinjam.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridBukuDipinjam.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridBukuDipinjam.RowTemplate.Height = 32;
            dataGridBukuDipinjam.EnableHeadersVisualStyles = false;
            dataGridBukuDipinjam.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridBukuDipinjam.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridBukuDipinjam.ColumnHeadersHeight = 35;
            dataGridBukuDipinjam.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridBukuDipinjam.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridBukuDipinjam.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            dataGridBukuDipinjam.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void StylingDataGridDenda()
        {
            dataGridRincianDenda.ReadOnly = true;
            dataGridRincianDenda.AllowUserToAddRows = false;
            dataGridRincianDenda.AllowUserToDeleteRows = false;
            dataGridRincianDenda.AllowUserToOrderColumns = false;
            dataGridRincianDenda.AllowUserToResizeColumns = false;
            dataGridRincianDenda.AllowUserToResizeRows = false;
            dataGridRincianDenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridRincianDenda.RowHeadersVisible = false;
            dataGridRincianDenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridRincianDenda.BorderStyle = BorderStyle.None;
            dataGridRincianDenda.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridRincianDenda.BackgroundColor = Color.White;
            dataGridRincianDenda.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridRincianDenda.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridRincianDenda.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridRincianDenda.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular);
            dataGridRincianDenda.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridRincianDenda.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridRincianDenda.RowTemplate.Height = 30;
            dataGridRincianDenda.EnableHeadersVisualStyles = false;
            dataGridRincianDenda.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridRincianDenda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridRincianDenda.ColumnHeadersHeight = 35;
            dataGridRincianDenda.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridRincianDenda.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridRincianDenda.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
            dataGridRincianDenda.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void btnDownloadLaporan_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF File|*.pdf";
            saveDialog.Title = "Simpan Detail Transaksi";
            saveDialog.FileName = $"Detail_Transaksi_{lblKodePeminjaman.Text.Replace("Kode Peminjaman : ", "").Trim()}_{DateTime.Now:yyyyMMdd}.pdf";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                GeneratePDFDetail(saveDialog.FileName);
            }
        }

        private async void GeneratePDFDetail(string filePath)
        {
            try
            {
                btnDownloadLaporan.Enabled = false;
                btnDownloadLaporan.Text = "Generating...";

                await Task.Run(() =>
                {
                    Document doc = new Document(PageSize.A4, 30, 30, 40, 40);
                    PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                    doc.Open();

                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD);
                    titleFont.Color = BaseColor.BLACK;

                    iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
                    headerFont.Color = BaseColor.WHITE;

                    iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL);
                    normalFont.Color = BaseColor.BLACK;

                    iTextSharp.text.Font boldFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD);
                    boldFont.Color = BaseColor.BLACK;

                    Paragraph title = new Paragraph("DETAIL TRANSAKSI", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph("INFORMASI PEMINJAMAN", boldFont));
                    doc.Add(new Paragraph(lblKodePeminjaman.Text, normalFont));
                    doc.Add(new Paragraph(lblNamaAnggota.Text, normalFont));
                    doc.Add(new Paragraph(lblKodeAnggota.Text, normalFont));
                    doc.Add(new Paragraph(lblTanggalPinjam.Text, normalFont));
                    doc.Add(new Paragraph(lblTanggalTempo.Text, normalFont));
                    doc.Add(new Paragraph(lblStatusPinjam.Text, normalFont));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph("INFORMASI PENGEMBALIAN", boldFont));
                    doc.Add(new Paragraph(lblTanggalPengembalian.Text, normalFont));
                    doc.Add(new Paragraph(lblTerlambat.Text, normalFont));
                    doc.Add(new Paragraph(lblDendaTerlambat.Text, normalFont));
                    doc.Add(new Paragraph(lblDendaBukuHilang.Text, normalFont));
                    doc.Add(new Paragraph(lblTotalDenda.Text, normalFont));
                    doc.Add(new Paragraph(lblStatusDenda.Text, normalFont));
                    doc.Add(new Paragraph(lblNamaPetugas.Text, normalFont));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"TOTAL DENDA: {txtTotalDenda.Text}", boldFont));
                    doc.Add(new Paragraph("\n"));

                    doc.Add(new Paragraph($"Dicetak: {DateTime.Now:dd MMMM yyyy HH:mm} WIB", normalFont));
                    doc.Add(new Paragraph($"Petugas: {Program.NamaLengkap} ({Program.Role})", normalFont));

                    doc.Close();
                });

                MessageBox.Show($"Detail transaksi berhasil disimpan!\n\nLokasi: {filePath}",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat generate PDF: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownloadLaporan.Enabled = true;
                btnDownloadLaporan.Text = "Download Laporan";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = FindParentDashboard();

            if (dashboard != null)
            {
                Report reportForm = new Report();
                dashboard.TampilkanFormDiPanel(reportForm);
                dashboard.UpdateLabelInfoPublic("Laporan");
            }
            else
            {
                this.Close();
            }
        }

        private Dashboard FindParentDashboard()
        {
            Form parent = this.ParentForm;
            if (parent is Dashboard dashboard)
                return dashboard;

            foreach (Form form in Application.OpenForms)
            {
                if (form is Dashboard dash)
                    return dash;
            }
            return null;
        }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void lblKodePeminjaman_Click(object sender, EventArgs e) { }
        private void lblNamaAnggota_Click(object sender, EventArgs e) { }
        private void lblKodeAnggota_Click(object sender, EventArgs e) { }
        private void lblTanggalPinjam_Click(object sender, EventArgs e) { }
        private void lblTanggalTempo_Click(object sender, EventArgs e) { }
        private void lblStatusPinjam_Click(object sender, EventArgs e) { }
        private void lblTanggalPengembalian_Click(object sender, EventArgs e) { }
        private void lblTerlambat_Click(object sender, EventArgs e) { }
        private void lblDendaTerlambat_Click(object sender, EventArgs e) { }
        private void lblDendaBukuHilang_Click(object sender, EventArgs e) { }
        private void lblTotalDenda_Click(object sender, EventArgs e) { }
        private void lblStatusDenda_Click(object sender, EventArgs e) { }
        private void lblNamaPetugas_Click(object sender, EventArgs e) { }
        private void dataGridBukuDipinjam_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dataGridRincianDenda_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtTotalDenda_TextChanged(object sender, EventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void peringatanNoDenda_Click(object sender, EventArgs e) { }
    }
}