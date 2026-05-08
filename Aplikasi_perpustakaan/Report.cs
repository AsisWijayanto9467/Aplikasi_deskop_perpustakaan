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
using WinFont = System.Drawing.Font;

namespace Aplikasi_perpustakaan
{
    public partial class Report : Form
    {
        private Koneksi koneksi;
        private DataTable dataPeminjaman;
        private DataTable dataPengembalian;
        private readonly object _lockPeminjaman = new object();
        private readonly object _lockPengembalian = new object();

        public Report()
        {
            InitializeComponent();
            koneksi = new Koneksi();

            if (this.dataGridPeminjaman != null)
            {
                this.dataGridPeminjaman.KeyDown += DataGrid_KeyDown;
            }

            if (this.dataGridPengembalian != null)
            {
                this.dataGridPengembalian.KeyDown += DataGrid_KeyDown;
            }

            if (this.btnSearch != null)
                this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            if (this.txtSearch != null)
                this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            if (this.btnLaporanPeminjaman != null)
                this.btnLaporanPeminjaman.Click += new System.EventHandler(this.btnLaporanPeminjaman_Click);
            if (this.dataGridPeminjaman != null)
                this.dataGridPeminjaman.CellClick += new DataGridViewCellEventHandler(this.dataGridPeminjaman_CellClick);

            if (this.btnSearchPengembalian != null)
                this.btnSearchPengembalian.Click += new System.EventHandler(this.btnSearchPengembalian_Click);
            if (this.txtSearchPengembalian != null)
                this.txtSearchPengembalian.TextChanged += new System.EventHandler(this.txtSearchPengembalian_TextChanged);
            if (this.btnLaporanPengembalian != null)
                this.btnLaporanPengembalian.Click += new System.EventHandler(this.btnLaporanPengembalian_Click);
            if (this.comboStatusDenda != null)
                this.comboStatusDenda.SelectedIndexChanged += new System.EventHandler(this.comboStatusDenda_SelectedIndexChanged);
            if (this.datePickerTanggalAwalPengembalian != null)
                this.datePickerTanggalAwalPengembalian.ValueChanged += new System.EventHandler(this.datePickerTanggalAwalPengembalian_ValueChanged);
            if (this.datePickerTanggalAkhirPengembalian != null)
                this.datePickerTanggalAkhirPengembalian.ValueChanged += new System.EventHandler(this.datePickerTanggalAkhirPengembalian_ValueChanged);
        }


        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            if (e.Control && e.KeyCode == Keys.C)
            {
                if (dgv.SelectedCells.Count == 1)
                {
                    object value = dgv.SelectedCells[0].Value;
                    if (value != null)
                    {
                        Clipboard.SetText(value.ToString());
                    }
                }
                else if (dgv.SelectedCells.Count > 1 || dgv.SelectedRows.Count > 0)
                {
                    dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                    DataObject data = dgv.GetClipboardContent();
                    if (data != null)
                    {
                        Clipboard.SetDataObject(data);
                    }
                }

                e.Handled = true;
            }
        }

        private async void Report_Load(object sender, EventArgs e)
        {
            SetupComboStatus();
            if (comboStatusDenda != null)
                SetupComboStatusDenda();

            StylingDataGridView();

            if (dataGridPengembalian != null)
                StylingDataGridPengembalian();
            await TampilDataPeminjaman();

            if (dataGridPengembalian != null)
                await TampilDataPengembalian();
        }

        private void SetupComboStatus()
        {
            comboStatus.Items.Clear();
            comboStatus.Items.Add("Semua");
            comboStatus.Items.Add("Dipinjam");
            comboStatus.Items.Add("Dikembalikan");
            comboStatus.SelectedIndex = 0;
            comboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async Task TampilDataPeminjaman(string keyword = "", string statusFilter = "Semua", DateTime? tglAwal = null, DateTime? tglAkhir = null)
        {
            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    await conn.OpenAsync();

                    string query = @"
                        SELECT 
                            p.id_peminjaman,
                            p.kode_peminjaman,
                            a.nama AS nama_anggota,
                            p.tanggal_pinjam,
                            p.tanggal_jatuh_tempo,
                            p.status,
                            COALESCE(pg.total_denda, 0) AS total_denda,
                            COALESCE(pg.terlambat, 0) AS terlambat,
                            COALESCE(pg.status_denda, 'tidak_ada') AS status_denda
                        FROM peminjaman p
                        INNER JOIN anggota a ON p.id_anggota = a.id_anggota
                        LEFT JOIN pengembalian pg ON p.id_peminjaman = pg.id_peminjaman
                        WHERE 1=1";

                    List<MySqlParameter> parameters = new List<MySqlParameter>();

                    if (tglAwal.HasValue)
                    {
                        query += " AND p.tanggal_pinjam >= @tglAwal";
                        parameters.Add(new MySqlParameter("@tglAwal", tglAwal.Value));
                    }
                    if (tglAkhir.HasValue)
                    {
                        query += " AND p.tanggal_pinjam <= @tglAkhir";
                        parameters.Add(new MySqlParameter("@tglAkhir", tglAkhir.Value));
                    }

                    if (statusFilter != "Semua")
                    {
                        query += " AND p.status = @status";
                        parameters.Add(new MySqlParameter("@status", statusFilter.ToLower()));
                    }

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query += @" AND (p.kode_peminjaman LIKE @keyword 
                                   OR a.nama LIKE @keyword 
                                   OR a.kode_anggota LIKE @keyword)";
                        parameters.Add(new MySqlParameter("@keyword", "%" + keyword + "%"));
                    }

                    query += " ORDER BY p.tanggal_pinjam DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        dataPeminjaman = new DataTable();
                        await Task.Run(() => da.Fill(dataPeminjaman));

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => UpdateDataGridPeminjaman(dataPeminjaman)));
                        }
                        else
                        {
                            UpdateDataGridPeminjaman(dataPeminjaman);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data peminjaman: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGridPeminjaman(DataTable dt)
        {
            if (dataGridPeminjaman == null) return;

            lock (_lockPeminjaman)
            {
                try
                {
                    dataGridPeminjaman.DataSource = null;
                    dataGridPeminjaman.Columns.Clear();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                        colNo.Name = "No";
                        colNo.HeaderText = "No";
                        colNo.Width = 50;
                        dataGridPeminjaman.Columns.Add(colNo);

                        DataGridViewTextBoxColumn colInfo = new DataGridViewTextBoxColumn();
                        colInfo.Name = "Info";
                        colInfo.HeaderText = "Informasi";
                        colInfo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridPeminjaman.Columns.Add(colInfo);

                        dataGridPeminjaman.Rows.Add("", "Tidak ada data peminjaman");
                        return;
                    }

                    dataGridPeminjaman.DataSource = dt;

                    if (dataGridPeminjaman.Columns.Contains("id_peminjaman"))
                        dataGridPeminjaman.Columns["id_peminjaman"].Visible = false;
                    if (dataGridPeminjaman.Columns.Contains("terlambat"))
                        dataGridPeminjaman.Columns["terlambat"].Visible = false;
                    if (dataGridPeminjaman.Columns.Contains("status_denda"))
                        dataGridPeminjaman.Columns["status_denda"].Visible = false;

                    if (dataGridPeminjaman.Columns.Contains("kode_peminjaman"))
                        dataGridPeminjaman.Columns["kode_peminjaman"].HeaderText = "Kode Peminjaman";
                    if (dataGridPeminjaman.Columns.Contains("nama_anggota"))
                        dataGridPeminjaman.Columns["nama_anggota"].HeaderText = "Nama Anggota";
                    if (dataGridPeminjaman.Columns.Contains("tanggal_pinjam"))
                    {
                        dataGridPeminjaman.Columns["tanggal_pinjam"].HeaderText = "Tanggal Pinjam";
                        dataGridPeminjaman.Columns["tanggal_pinjam"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                    if (dataGridPeminjaman.Columns.Contains("tanggal_jatuh_tempo"))
                    {
                        dataGridPeminjaman.Columns["tanggal_jatuh_tempo"].HeaderText = "Jatuh Tempo";
                        dataGridPeminjaman.Columns["tanggal_jatuh_tempo"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                    if (dataGridPeminjaman.Columns.Contains("status"))
                        dataGridPeminjaman.Columns["status"].HeaderText = "Status";
                    if (dataGridPeminjaman.Columns.Contains("total_denda"))
                    {
                        dataGridPeminjaman.Columns["total_denda"].HeaderText = "Denda";
                        dataGridPeminjaman.Columns["total_denda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridPeminjaman.Columns["total_denda"].DefaultCellStyle.Format = "Rp #,##0";
                    }

                    BuatKolomAksiPeminjaman();

                    for (int i = 0; i < dataGridPeminjaman.Rows.Count; i++)
                    {
                        if (dataGridPeminjaman.Columns.Contains("No"))
                        {
                            dataGridPeminjaman.Rows[i].Cells["No"].Value = (i + 1).ToString();
                        }

                        if (dataGridPeminjaman.Columns.Contains("status"))
                        {
                            string status = dataGridPeminjaman.Rows[i].Cells["status"].Value?.ToString();
                            if (status == "dipinjam")
                            {
                                dataGridPeminjaman.Rows[i].Cells["status"].Style.ForeColor = Color.FromArgb(243, 156, 18);
                                dataGridPeminjaman.Rows[i].Cells["status"].Style.Font = new WinFont("Inter", 9f, FontStyle.Bold);
                            }
                            else if (status == "dikembalikan")
                            {
                                dataGridPeminjaman.Rows[i].Cells["status"].Style.ForeColor = Color.FromArgb(46, 204, 113);
                                dataGridPeminjaman.Rows[i].Cells["status"].Style.Font = new WinFont("Inter", 9f, FontStyle.Bold);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("UpdateDataGridPeminjaman error: " + ex.Message);
                }
            }
        }

        private void BuatKolomAksiPeminjaman()
        {
            if (!dataGridPeminjaman.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                colNo.Name = "No";
                colNo.HeaderText = "No";
                colNo.Width = 40;
                colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridPeminjaman.Columns.Insert(0, colNo);
            }

            if (dataGridPeminjaman.Columns.Contains("Action"))
                dataGridPeminjaman.Columns.Remove("Action");

            DataGridViewTextBoxColumn colAction = new DataGridViewTextBoxColumn();
            colAction.Name = "Action";
            colAction.HeaderText = "Action";
            colAction.Width = 120;
            colAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridPeminjaman.Columns.Add(colAction);
        }

        private void StylingDataGridView()
        {
            dataGridPeminjaman.ReadOnly = true;
            dataGridPeminjaman.AllowUserToAddRows = false;
            dataGridPeminjaman.AllowUserToDeleteRows = false;
            dataGridPeminjaman.AllowUserToOrderColumns = false;
            dataGridPeminjaman.AllowUserToResizeColumns = false;
            dataGridPeminjaman.AllowUserToResizeRows = false;
            dataGridPeminjaman.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridPeminjaman.RowHeadersVisible = false;
            dataGridPeminjaman.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPeminjaman.BorderStyle = BorderStyle.None;
            dataGridPeminjaman.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridPeminjaman.BackgroundColor = Color.White;
            dataGridPeminjaman.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridPeminjaman.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridPeminjaman.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridPeminjaman.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Regular);
            dataGridPeminjaman.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridPeminjaman.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridPeminjaman.RowTemplate.Height = 35;
            dataGridPeminjaman.EnableHeadersVisualStyles = false;
            dataGridPeminjaman.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridPeminjaman.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridPeminjaman.ColumnHeadersHeight = 38;
            dataGridPeminjaman.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridPeminjaman.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridPeminjaman.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            dataGridPeminjaman.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridPeminjaman.CellPainting -= dataGridPeminjaman_CellPainting;
            dataGridPeminjaman.CellPainting += dataGridPeminjaman_CellPainting;
        }

        private void dataGridPeminjaman_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Render button Action
            if (e.ColumnIndex >= 0 && dataGridPeminjaman.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                int buttonWidth = 90;
                int buttonHeight = 26;
                int detailX = e.CellBounds.Left + (e.CellBounds.Width - buttonWidth) / 2;
                int detailY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;
                Rectangle rectDetail = new Rectangle(detailX, detailY, buttonWidth, buttonHeight);

                using (SolidBrush brush = new SolidBrush(Color.FromArgb(93, 173, 226)))
                {
                    e.Graphics.FillRectangle(brush, rectDetail);
                }

                TextRenderer.DrawText(e.Graphics, "Detail",
                    new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold),
                    rectDetail, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void dataGridPeminjaman_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridPeminjaman.Columns[e.ColumnIndex].Name == "Action")
            {
                Point mousePosition = dataGridPeminjaman.PointToClient(Cursor.Position);
                Rectangle cellRect = dataGridPeminjaman.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int relativeX = mousePosition.X - cellRect.X;

                int buttonWidth = 90;
                int detailStart = (cellRect.Width - buttonWidth) / 2;
                int detailEnd = detailStart + buttonWidth;

                if (relativeX >= detailStart && relativeX <= detailEnd)
                {
                    int idPeminjaman = Convert.ToInt32(dataGridPeminjaman.Rows[e.RowIndex].Cells["id_peminjaman"].Value);
                    TampilkanDetailTransaksi(idPeminjaman);
                }
            }
        }

        private async void TampilkanDetailTransaksi(int idPeminjaman)
        {
            Dashboard dashboard = FindParentDashboard();

            if (dashboard != null)
            {
                DetailTransaction detailForm = new DetailTransaction(idPeminjaman);
                dashboard.TampilkanFormDiPanel(detailForm);

                dashboard.UpdateLabelInfoPublic($"Detail Transaksi #{idPeminjaman}");
            }
            else
            {
                DetailTransaction detailForm = new DetailTransaction(idPeminjaman);
                detailForm.ShowDialog();
                await TampilDataPeminjaman();
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

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            await FilterDataPeminjaman();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await FilterDataPeminjaman();
        }

        private async void comboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            await FilterDataPeminjaman();
        }

        private async void datePickerTanggalAwal_ValueChanged(object sender, EventArgs e)
        {
            await FilterDataPeminjaman();
        }

        private async void datePickerTanggalAkhir_ValueChanged(object sender, EventArgs e)
        {
            await FilterDataPeminjaman();
        }

        private async Task FilterDataPeminjaman()
        {
            string keyword = txtSearch?.Text?.Trim() ?? "";
            string statusFilter = comboStatus?.SelectedItem?.ToString() ?? "Semua";
            DateTime? tglAwal = datePickerTanggalAwal?.Value.Date;
            DateTime? tglAkhir = datePickerTanggalAkhir?.Value.Date;

            await TampilDataPeminjaman(keyword, statusFilter, tglAwal, tglAkhir);
        }
        private async void btnLaporanPeminjaman_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF File|*.pdf";
            saveDialog.Title = "Simpan Laporan Peminjaman";
            saveDialog.FileName = $"Laporan_Peminjaman_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                await GeneratePDFPeminjaman(saveDialog.FileName);
            }
        }

        private async Task GeneratePDFPeminjaman(string filePath)
        {
            try
            {
                btnLaporanPeminjaman.Enabled = false;
                btnLaporanPeminjaman.Text = "Generating...";

                if (dataPeminjaman == null || dataPeminjaman.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data peminjaman untuk dicetak!", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await Task.Run(() =>
                {
                    Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
                    PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                    doc.Open();

                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    titleFont.Color = new BaseColor(20, 25, 72);

                    iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.BOLD);
                    headerFont.Color = BaseColor.WHITE;

                    iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL);
                    normalFont.Color = BaseColor.BLACK;

                    Paragraph title = new Paragraph("LAPORAN PEMINJAMAN BUKU", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);

                    doc.Add(new Paragraph("\n"));

                    Paragraph info = new Paragraph();
                    info.Add(new Chunk("Periode: ", new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD)));
                    info.Add(new Chunk($"{datePickerTanggalAwal.Value:dd/MM/yyyy} - {datePickerTanggalAkhir.Value:dd/MM/yyyy}\n", normalFont));
                    info.Add(new Chunk("Dicetak: ", new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD)));
                    info.Add(new Chunk($"{DateTime.Now:dd MMMM yyyy HH:mm} WIB\n", normalFont));
                    info.Add(new Chunk("Petugas: ", new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD)));
                    info.Add(new Chunk($"{Program.NamaLengkap} ({Program.Role})\n\n", normalFont));
                    doc.Add(info);

                    PdfPTable table = new PdfPTable(7);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 5f, 15f, 20f, 12f, 12f, 12f, 12f });

                    string[] headers = { "No", "Kode", "Anggota", "Tgl Pinjam", "Jth Tempo", "Status", "Denda" };
                    foreach (string header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                        cell.BackgroundColor = new BaseColor(20, 25, 72);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 6;
                        table.AddCell(cell);
                    }

                    int no = 1;
                    foreach (DataRow row in dataPeminjaman.Rows)
                    {
                        decimal denda = 0;
                        if (row["total_denda"] != DBNull.Value)
                        {
                            decimal.TryParse(row["total_denda"].ToString(), out denda);
                        }

                        table.AddCell(new Phrase(no.ToString(), normalFont));
                        table.AddCell(new Phrase(row["kode_peminjaman"].ToString(), normalFont));
                        table.AddCell(new Phrase(row["nama_anggota"].ToString(), normalFont));
                        table.AddCell(new Phrase(Convert.ToDateTime(row["tanggal_pinjam"]).ToString("dd/MM/yy"), normalFont));
                        table.AddCell(new Phrase(Convert.ToDateTime(row["tanggal_jatuh_tempo"]).ToString("dd/MM/yy"), normalFont));
                        table.AddCell(new Phrase(row["status"].ToString(), normalFont));
                        table.AddCell(new Phrase("Rp " + denda.ToString("N0"), normalFont));
                        no++;
                    }

                    doc.Add(table);

                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph($"Total Peminjaman: {dataPeminjaman.Rows.Count} transaksi",
                        new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD)));

                    doc.Add(new Paragraph("\n\n© 2026 Perpustakaan Digital - Generated by System", normalFont));
                    doc.Close();
                });

                MessageBox.Show($"Laporan peminjaman berhasil disimpan!\n\nLokasi: {filePath}",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat generate PDF: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLaporanPeminjaman.Enabled = true;
                btnLaporanPeminjaman.Text = "Download Laporan";
            }
        }

        private void SetupComboStatusDenda()
        {
            comboStatusDenda.Items.Clear();
            comboStatusDenda.Items.Add("Semua");
            comboStatusDenda.Items.Add("tidak_ada");
            comboStatusDenda.Items.Add("belum_bayar");
            comboStatusDenda.Items.Add("lunas");
            comboStatusDenda.SelectedIndex = 0;
            comboStatusDenda.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async Task TampilDataPengembalian(string keyword = "", string statusDenda = "Semua", DateTime? tglAwal = null, DateTime? tglAkhir = null)
        {
            if (dataGridPengembalian == null) return;

            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    await conn.OpenAsync();

                    string query = @"
                        SELECT 
                            pg.id_pengembalian,
                            pg.id_peminjaman,
                            p.kode_peminjaman,
                            a.nama AS nama_anggota,
                            pg.tanggal_kembali,
                            pg.terlambat,
                            pg.total_denda,
                            pg.status_denda,
                            u.nama AS nama_petugas
                        FROM pengembalian pg
                        INNER JOIN peminjaman p ON pg.id_peminjaman = p.id_peminjaman
                        INNER JOIN anggota a ON p.id_anggota = a.id_anggota
                        INNER JOIN users u ON pg.id_user = u.id_user
                        WHERE 1=1";

                    List<MySqlParameter> parameters = new List<MySqlParameter>();

                    if (tglAwal.HasValue)
                    {
                        query += " AND pg.tanggal_kembali >= @tglAwal";
                        parameters.Add(new MySqlParameter("@tglAwal", tglAwal.Value));
                    }
                    if (tglAkhir.HasValue)
                    {
                        query += " AND pg.tanggal_kembali <= @tglAkhir";
                        parameters.Add(new MySqlParameter("@tglAkhir", tglAkhir.Value));
                    }

                    if (statusDenda != "Semua")
                    {
                        query += " AND pg.status_denda = @statusDenda";
                        parameters.Add(new MySqlParameter("@statusDenda", statusDenda));
                    }

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query += @" AND (p.kode_peminjaman LIKE @keyword 
                                   OR a.nama LIKE @keyword 
                                   OR u.nama LIKE @keyword)";
                        parameters.Add(new MySqlParameter("@keyword", "%" + keyword + "%"));
                    }

                    query += " ORDER BY pg.tanggal_kembali DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dtPengembalian = new DataTable();
                        await Task.Run(() => da.Fill(dtPengembalian));

                        dataPengembalian = dtPengembalian;

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => UpdateDataGridPengembalian(dataPengembalian)));
                        }
                        else
                        {
                            UpdateDataGridPengembalian(dataPengembalian);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data pengembalian: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGridPengembalian(DataTable dt)
        {
            if (dataGridPengembalian == null) return;

            lock (_lockPengembalian)
            {
                try
                {
                    dataGridPengembalian.DataSource = null;
                    dataGridPengembalian.Columns.Clear();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                        colNo.Name = "No";
                        colNo.HeaderText = "No";
                        colNo.Width = 50;
                        dataGridPengembalian.Columns.Add(colNo);

                        DataGridViewTextBoxColumn colInfo = new DataGridViewTextBoxColumn();
                        colInfo.Name = "Info";
                        colInfo.HeaderText = "Informasi";
                        colInfo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        dataGridPengembalian.Columns.Add(colInfo);

                        dataGridPengembalian.Rows.Add("", "Tidak ada data pengembalian");
                        return;
                    }

                    dataGridPengembalian.DataSource = dt;

                    if (dataGridPengembalian.Columns.Contains("id_pengembalian"))
                        dataGridPengembalian.Columns["id_pengembalian"].Visible = false;
                    if (dataGridPengembalian.Columns.Contains("id_peminjaman"))
                        dataGridPengembalian.Columns["id_peminjaman"].Visible = false;

                    if (dataGridPengembalian.Columns.Contains("kode_peminjaman"))
                        dataGridPengembalian.Columns["kode_peminjaman"].HeaderText = "Kode Peminjaman";
                    if (dataGridPengembalian.Columns.Contains("nama_anggota"))
                        dataGridPengembalian.Columns["nama_anggota"].HeaderText = "Nama Anggota";
                    if (dataGridPengembalian.Columns.Contains("tanggal_kembali"))
                    {
                        dataGridPengembalian.Columns["tanggal_kembali"].HeaderText = "Tanggal Kembali";
                        dataGridPengembalian.Columns["tanggal_kembali"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    }
                    if (dataGridPengembalian.Columns.Contains("terlambat"))
                    {
                        dataGridPengembalian.Columns["terlambat"].HeaderText = "Terlambat (Hari)";
                        dataGridPengembalian.Columns["terlambat"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridPengembalian.Columns.Contains("total_denda"))
                    {
                        dataGridPengembalian.Columns["total_denda"].HeaderText = "Total Denda";
                        dataGridPengembalian.Columns["total_denda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dataGridPengembalian.Columns["total_denda"].DefaultCellStyle.Format = "Rp #,##0";
                    }
                    if (dataGridPengembalian.Columns.Contains("status_denda"))
                    {
                        dataGridPengembalian.Columns["status_denda"].HeaderText = "Status Denda";
                        dataGridPengembalian.Columns["status_denda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    if (dataGridPengembalian.Columns.Contains("nama_petugas"))
                        dataGridPengembalian.Columns["nama_petugas"].HeaderText = "Petugas";

                    if (!dataGridPengembalian.Columns.Contains("No"))
                    {
                        DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                        colNo.Name = "No";
                        colNo.HeaderText = "No";
                        colNo.Width = 50;
                        colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        colNo.ReadOnly = true;
                        dataGridPengembalian.Columns.Insert(0, colNo);
                    }
                    for (int i = 0; i < dataGridPengembalian.Rows.Count; i++)
                    {
                        if (dataGridPengembalian.Columns.Contains("No"))
                        {
                            dataGridPengembalian.Rows[i].Cells["No"].Value = (i + 1).ToString();
                        }

                        if (dataGridPengembalian.Columns.Contains("status_denda"))
                        {
                            string status = dataGridPengembalian.Rows[i].Cells["status_denda"].Value?.ToString();
                            if (status == "belum_bayar")
                            {
                                dataGridPengembalian.Rows[i].Cells["status_denda"].Style.ForeColor = Color.FromArgb(231, 76, 60);
                                dataGridPeminjaman.Rows[i].Cells["status"].Style.Font = new WinFont("Inter", 9f, FontStyle.Bold);
                            }
                            else if (status == "lunas")
                            {
                                dataGridPengembalian.Rows[i].Cells["status_denda"].Style.ForeColor = Color.FromArgb(46, 204, 113);
                            }
                            else if (status == "tidak_ada")
                            {
                                dataGridPengembalian.Rows[i].Cells["status_denda"].Style.ForeColor = Color.Gray;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("UpdateDataGridPengembalian error: " + ex.Message);
                }
            }
        }

        private void StylingDataGridPengembalian()
        {
            dataGridPengembalian.ReadOnly = true;
            dataGridPengembalian.AllowUserToAddRows = false;
            dataGridPengembalian.AllowUserToDeleteRows = false;
            dataGridPengembalian.AllowUserToOrderColumns = false;
            dataGridPengembalian.AllowUserToResizeColumns = false;
            dataGridPengembalian.AllowUserToResizeRows = false;
            dataGridPengembalian.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridPengembalian.RowHeadersVisible = false;
            dataGridPengembalian.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPengembalian.BorderStyle = BorderStyle.None;
            dataGridPengembalian.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridPengembalian.BackgroundColor = Color.White;
            dataGridPengembalian.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridPengembalian.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridPengembalian.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dataGridPengembalian.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Regular);
            dataGridPengembalian.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridPengembalian.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridPengembalian.RowTemplate.Height = 35;
            dataGridPengembalian.EnableHeadersVisualStyles = false;
            dataGridPengembalian.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridPengembalian.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridPengembalian.ColumnHeadersHeight = 38;
            dataGridPengembalian.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridPengembalian.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridPengembalian.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5f, System.Drawing.FontStyle.Bold);
            dataGridPengembalian.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private async void txtSearchPengembalian_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            await FilterDataPengembalian();
        }

        private async void btnSearchPengembalian_Click(object sender, EventArgs e)
        {
            await FilterDataPengembalian();
        }

        private async void comboStatusDenda_SelectedIndexChanged(object sender, EventArgs e)
        {
            await FilterDataPengembalian();
        }

        private async void datePickerTanggalAwalPengembalian_ValueChanged(object sender, EventArgs e)
        {
            await FilterDataPengembalian();
        }

        private async void datePickerTanggalAkhirPengembalian_ValueChanged(object sender, EventArgs e)
        {
            await FilterDataPengembalian();
        }

        private async Task FilterDataPengembalian()
        {
            string keyword = txtSearchPengembalian?.Text?.Trim() ?? "";
            string statusDenda = comboStatusDenda?.SelectedItem?.ToString() ?? "Semua";
            DateTime? tglAwal = datePickerTanggalAwalPengembalian?.Value.Date;
            DateTime? tglAkhir = datePickerTanggalAkhirPengembalian?.Value.Date;

            await TampilDataPengembalian(keyword, statusDenda, tglAwal, tglAkhir);
        }

        private async void btnLaporanPengembalian_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PDF File|*.pdf";
            saveDialog.Title = "Simpan Laporan Pengembalian";
            saveDialog.FileName = $"Laporan_Pengembalian_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                await GeneratePDFPengembalian(saveDialog.FileName);
            }
        }

        private async Task GeneratePDFPengembalian(string filePath)
        {
            try
            {
                btnLaporanPengembalian.Enabled = false;
                btnLaporanPengembalian.Text = "Generating...";

                if (dataPengembalian == null || dataPengembalian.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data pengembalian untuk dicetak!", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await Task.Run(() =>
                {
                    Document doc = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
                    PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                    doc.Open();

                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Font titleFont = new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD);
                    titleFont.Color = new BaseColor(20, 25, 72);

                    iTextSharp.text.Font headerFont = new iTextSharp.text.Font(baseFont, 11, iTextSharp.text.Font.BOLD);
                    headerFont.Color = BaseColor.WHITE;

                    iTextSharp.text.Font normalFont = new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.NORMAL);
                    normalFont.Color = BaseColor.BLACK;

                    Paragraph title = new Paragraph("LAPORAN PENGEMBALIAN BUKU", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    doc.Add(title);

                    doc.Add(new Paragraph("\n"));

                    Paragraph info = new Paragraph();
                    info.Add(new Chunk("Periode: ", new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD)));
                    info.Add(new Chunk($"{datePickerTanggalAwalPengembalian.Value:dd/MM/yyyy} - {datePickerTanggalAkhirPengembalian.Value:dd/MM/yyyy}\n", normalFont));
                    info.Add(new Chunk("Dicetak: ", new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD)));
                    info.Add(new Chunk($"{DateTime.Now:dd MMMM yyyy HH:mm} WIB\n", normalFont));
                    info.Add(new Chunk("Petugas: ", new iTextSharp.text.Font(baseFont, 9, iTextSharp.text.Font.BOLD)));
                    info.Add(new Chunk($"{Program.NamaLengkap} ({Program.Role})\n\n", normalFont));
                    doc.Add(info);

                    PdfPTable table = new PdfPTable(7);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 5f, 15f, 18f, 12f, 10f, 12f, 12f });

                    string[] headers = { "No", "Kode", "Anggota", "Tgl Kembali", "Terlambat", "Denda", "Status" };
                    foreach (string header in headers)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(header, headerFont));
                        cell.BackgroundColor = new BaseColor(20, 25, 72);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.Padding = 6;
                        table.AddCell(cell);
                    }

                    int no = 1;
                    foreach (DataRow row in dataPengembalian.Rows)
                    {
                        decimal denda = 0;
                        if (row["total_denda"] != DBNull.Value)
                        {
                            decimal.TryParse(row["total_denda"].ToString(), out denda);
                        }

                        string statusDenda = row["status_denda"].ToString();
                        string statusDisplay = statusDenda switch
                        {
                            "lunas" => "Lunas",
                            "belum_bayar" => "Belum Bayar",
                            "tidak_ada" => "Tidak Ada",
                            _ => statusDenda
                        };

                        table.AddCell(new Phrase(no.ToString(), normalFont));
                        table.AddCell(new Phrase(row["kode_peminjaman"].ToString(), normalFont));
                        table.AddCell(new Phrase(row["nama_anggota"].ToString(), normalFont));
                        table.AddCell(new Phrase(Convert.ToDateTime(row["tanggal_kembali"]).ToString("dd/MM/yy"), normalFont));
                        table.AddCell(new Phrase(row["terlambat"].ToString() + " hari", normalFont));
                        table.AddCell(new Phrase("Rp " + denda.ToString("N0"), normalFont));
                        table.AddCell(new Phrase(statusDisplay, normalFont));
                        no++;
                    }

                    doc.Add(table);

                    doc.Add(new Paragraph("\n"));
                    doc.Add(new Paragraph($"Total Pengembalian: {dataPengembalian.Rows.Count} transaksi",
                        new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD)));

                    doc.Add(new Paragraph("\n\n© 2026 Perpustakaan Digital - Generated by System", normalFont));
                    doc.Close();
                });

                MessageBox.Show($"Laporan pengembalian berhasil disimpan!\n\nLokasi: {filePath}",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat generate PDF: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLaporanPengembalian.Enabled = true;
                btnLaporanPengembalian.Text = "Download Laporan";
            }
        }

        private void dataGridPengembalian_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void dataGridPeminjaman_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}