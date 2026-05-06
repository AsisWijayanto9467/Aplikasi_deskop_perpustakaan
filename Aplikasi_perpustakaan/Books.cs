using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Aplikasi_perpustakaan
{
    public partial class Books : Form
    {
        Koneksi kon = new Koneksi();
        private bool _isLoading = false;

        public Books()
        {
            InitializeComponent();


            this.dataGridBuku.CellClick += new DataGridViewCellEventHandler(this.dataGridBuku_CellClick);
        }

        private async void Books_Load(object sender, EventArgs e)
        {
            StylingDataGridView();
            LoadKategoriFilter();
            await TampilBuku();
        }

        private void LoadKategoriFilter()
        {
            comboKategori.Items.Clear();
            comboKategori.Items.Add("-- Semua Kategori --");

            string[] kategoriList = new string[]
            {
                "Pendidikan", "Novel", "Komik", "Teknologi & Komputer",
                "Sains & Matematika", "Sejarah", "Agama & Spiritual",
                "Bisnis & Ekonomi", "Kesehatan", "Hukum", "Filsafat",
                "Bahasa", "Seni & Desain", "Biografi", "Ensiklopedia",
                "Kamus", "Majalah", "Jurnal", "Fiksi Ilmiah", "Fantasi",
                "Horor", "Romantis", "Petualangan", "Anak-anak", "Lainnya"
            };

            comboKategori.Items.AddRange(kategoriList);
            comboKategori.SelectedIndex = 0;
            comboKategori.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private async Task TampilBuku(string keyword = "", string kategori = "")
        {
            if (_isLoading) return;
            _isLoading = true;

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string query = @"SELECT 
                        id_buku,
                        kode_buku,
                        barcode,
                        judul,
                        penulis,
                        penerbit,
                        tahun_terbit,
                        kategori,
                        stok,
                        lokasi_rak,
                        cover_buku,
                        created_at
                    FROM buku WHERE 1=1";

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query += @" AND (kode_buku LIKE @keyword 
                                  OR judul LIKE @keyword 
                                  OR penulis LIKE @keyword 
                                  OR penerbit LIKE @keyword
                                  OR barcode LIKE @keyword)";
                    }

                    if (!string.IsNullOrWhiteSpace(kategori) && kategori != "-- Semua Kategori --")
                    {
                        query += " AND kategori = @kategori";
                    }

                    query += " ORDER BY id_buku DESC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(kategori) && kategori != "-- Semua Kategori --")
                    {
                        cmd.Parameters.AddWithValue("@kategori", kategori);
                    }

                    using (cmd)
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            await Task.Run(() => da.Fill(dt));

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() => UpdateDataGrid(dt)));
                            }
                            else
                            {
                                UpdateDataGrid(dt);
                            }

                            await UpdateStatistik();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task UpdateStatistik()
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string queryTotal = "SELECT COUNT(*) FROM buku";
                    MySqlCommand cmdTotal = new MySqlCommand(queryTotal, conn);
                    int totalBuku = Convert.ToInt32(await cmdTotal.ExecuteScalarAsync());

                    string queryTersedia = "SELECT COUNT(*) FROM buku WHERE stok > 0";
                    MySqlCommand cmdTersedia = new MySqlCommand(queryTersedia, conn);
                    int bukuTersedia = Convert.ToInt32(await cmdTersedia.ExecuteScalarAsync());

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            lblTotalBuku.Text = $"Total Buku : {totalBuku}";
                            lblTersedia.Text = $"Tersedia : {bukuTersedia}";
                        }));
                    }
                    else
                    {
                        lblTotalBuku.Text = $"Total Buku : {totalBuku}";
                        lblTersedia.Text = $"Tersedia : {bukuTersedia}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error update statistik: {ex.Message}");
            }
        }

        private void UpdateDataGrid(DataTable dt)
        {
            dataGridBuku.DataSource = null;
            dataGridBuku.Rows.Clear();
            dataGridBuku.Columns.Clear();

            dataGridBuku.DataSource = dt;
            if (dataGridBuku.Columns.Contains("id_buku"))
                dataGridBuku.Columns["id_buku"].Visible = false;
            if (dataGridBuku.Columns.Contains("created_at"))
                dataGridBuku.Columns["created_at"].Visible = false;
            if (dataGridBuku.Columns.Contains("lokasi_rak"))
                dataGridBuku.Columns["lokasi_rak"].Visible = false;
            if (dataGridBuku.Columns.Contains("barcode"))
                dataGridBuku.Columns["barcode"].Visible = false;

            if (dataGridBuku.Columns.Contains("kode_buku"))
                dataGridBuku.Columns["kode_buku"].HeaderText = "Kode";
            if (dataGridBuku.Columns.Contains("judul"))
                dataGridBuku.Columns["judul"].HeaderText = "Judul Buku";
            if (dataGridBuku.Columns.Contains("penulis"))
                dataGridBuku.Columns["penulis"].HeaderText = "Penulis";
            if (dataGridBuku.Columns.Contains("penerbit"))
                dataGridBuku.Columns["penerbit"].HeaderText = "Penerbit";
            if (dataGridBuku.Columns.Contains("tahun_terbit"))
                dataGridBuku.Columns["tahun_terbit"].HeaderText = "Tahun";
            if (dataGridBuku.Columns.Contains("kategori"))
                dataGridBuku.Columns["kategori"].HeaderText = "Kategori";
            if (dataGridBuku.Columns.Contains("stok"))
                dataGridBuku.Columns["stok"].HeaderText = "Stok";
            if (dataGridBuku.Columns.Contains("kode_buku"))
                dataGridBuku.Columns["kode_buku"].Width = 70;
            if (dataGridBuku.Columns.Contains("judul"))
                dataGridBuku.Columns["judul"].Width = 160;
            if (dataGridBuku.Columns.Contains("stok"))
            {
                dataGridBuku.Columns["stok"].Width = 50;
                dataGridBuku.Columns["stok"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            if (dataGridBuku.Columns.Contains("tahun_terbit"))
            {
                dataGridBuku.Columns["tahun_terbit"].Width = 55;
                dataGridBuku.Columns["tahun_terbit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            TambahKolomCover();

            BuatKolomAksi();

            IsiKolomCover(dt);

            dataGridBuku.RowTemplate.Height = 70;
        }

        // ✅ TAMBAH KOLOM COVER
        private void TambahKolomCover()
        {
            if (dataGridBuku.Columns.Contains("Cover"))
                dataGridBuku.Columns.Remove("Cover");

            if (dataGridBuku.Columns.Contains("cover_buku"))
                dataGridBuku.Columns["cover_buku"].Visible = false;

            DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
            imgColumn.Name = "Cover";
            imgColumn.HeaderText = "Cover";
            imgColumn.Width = 100;
            imgColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom; 
            imgColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int insertPosition = 3; 
            if (dataGridBuku.Columns.Count > insertPosition)
                dataGridBuku.Columns.Insert(insertPosition, imgColumn);
            else
                dataGridBuku.Columns.Add(imgColumn);
        }

        private void IsiKolomCover(DataTable dt)
        {
            if (!dataGridBuku.Columns.Contains("Cover"))
                return;

            int coverColumnIndex = dataGridBuku.Columns["Cover"].Index;

            foreach (DataGridViewRow row in dataGridBuku.Rows)
            {
                if (row.Index < dt.Rows.Count)
                {
                    string coverPath = dt.Rows[row.Index]["cover_buku"]?.ToString();

                    if (!string.IsNullOrEmpty(coverPath) && File.Exists(coverPath))
                    {
                        try
                        {
                            using (FileStream fs = new FileStream(coverPath, FileMode.Open, FileAccess.Read))
                            {
                                Image coverImage = Image.FromStream(fs);
                                Image resizedImage = new Bitmap(coverImage, new Size(90, 80));
                                row.Cells[coverColumnIndex].Value = resizedImage;
                            }
                        }
                        catch
                        {
                            row.Cells[coverColumnIndex].Value = CreateDefaultCoverImage();
                        }
                    }
                    else
                    {
                        row.Cells[coverColumnIndex].Value = CreateDefaultCoverImage();
                    }
                }
            }
        }

        private Image CreateDefaultCoverImage()
        {
            Bitmap bmp = new Bitmap(90, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(233, 236, 239));
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (Font font = new Font("Segoe UI", 8))
                {
                    string text = "📚\nNo Cover";
                    SizeF textSize = g.MeasureString(text, font);
                    float x = (bmp.Width - textSize.Width) / 2;
                    float y = (bmp.Height - textSize.Height) / 2;

                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(108, 117, 125)))
                    {
                        g.DrawString(text, font, brush, x, y);
                    }
                }
            }
            return bmp;
        }

        private void BuatKolomAksi()
        {
            if (dataGridBuku.Columns.Contains("No"))
                dataGridBuku.Columns.Remove("No");
            if (dataGridBuku.Columns.Contains("Action"))
                dataGridBuku.Columns.Remove("Action");

            DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
            colNo.Name = "No";
            colNo.HeaderText = "No";
            colNo.Width = 40;
            colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridBuku.Columns.Insert(0, colNo);

            DataGridViewTextBoxColumn colAction = new DataGridViewTextBoxColumn();
            colAction.Name = "Action";
            colAction.HeaderText = "Action";
            colAction.Width = 150; 
            colAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridBuku.Columns.Add(colAction);
        }

        private void StylingDataGridView()
        {
            dataGridBuku.ReadOnly = true;
            dataGridBuku.AllowUserToAddRows = false;
            dataGridBuku.AllowUserToDeleteRows = false;
            dataGridBuku.AllowUserToOrderColumns = false;
            dataGridBuku.AllowUserToResizeColumns = false;
            dataGridBuku.AllowUserToResizeRows = false;

            dataGridBuku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridBuku.RowHeadersVisible = false;
            dataGridBuku.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridBuku.BorderStyle = BorderStyle.None;
            dataGridBuku.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridBuku.BackgroundColor = Color.White;

            dataGridBuku.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridBuku.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridBuku.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            dataGridBuku.DefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Regular);

            dataGridBuku.DefaultCellStyle.SelectionBackColor = Color.FromArgb(94, 148, 255);
            dataGridBuku.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridBuku.RowTemplate.Height = 85;

            dataGridBuku.EnableHeadersVisualStyles = false;
            dataGridBuku.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dataGridBuku.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridBuku.ColumnHeadersHeight = 38;

            dataGridBuku.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(33, 42, 57);
            dataGridBuku.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridBuku.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dataGridBuku.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridBuku.RowPostPaint -= DataGridBuku_RowPostPaint;
            dataGridBuku.RowPostPaint += DataGridBuku_RowPostPaint;

            dataGridBuku.CellPainting -= DataGridBuku_CellPainting;
            dataGridBuku.CellPainting += DataGridBuku_CellPainting;
        }

        private void DataGridBuku_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dataGridBuku.Rows[e.RowIndex].Cells["No"] != null)
            {
                dataGridBuku.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
            }
        }

        private void DataGridBuku_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridBuku.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                int buttonWidth = 58;
                int buttonHeight = 24;
                int margin = 5;

                int updateX = e.CellBounds.Left + margin;
                int updateY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;
                Rectangle rectUpdate = new Rectangle(updateX, updateY, buttonWidth, buttonHeight);

                int deleteX = updateX + buttonWidth + margin;
                int deleteY = updateY;
                Rectangle rectDelete = new Rectangle(deleteX, deleteY, buttonWidth, buttonHeight);

                if (deleteX + buttonWidth <= e.CellBounds.Right)
                {
                    using (GraphicsPath path = GetRoundedRect(rectUpdate, 4))
                    {
                        using (SolidBrush brush = new SolidBrush(Color.DodgerBlue))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    TextRenderer.DrawText(e.Graphics, "Update", new Font("Segoe UI", 8f, FontStyle.Bold), rectUpdate, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    using (GraphicsPath path = GetRoundedRect(rectDelete, 4))
                    {
                        using (SolidBrush brush = new SolidBrush(Color.Crimson))
                        {
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.FillPath(brush, path);
                        }
                    }
                    TextRenderer.DrawText(e.Graphics, "Delete", new Font("Segoe UI", 8f, FontStyle.Bold),
                        rectDelete, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                e.Handled = true;
            }
        }
        // HELPER: ROUNDED RECTANGLE
        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private async void dataGridBuku_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridBuku.Columns[e.ColumnIndex].Name == "Action")
            {
                Point mousePosition = dataGridBuku.PointToClient(Cursor.Position);
                Rectangle cellRect = dataGridBuku.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int relativeX = mousePosition.X - cellRect.X;

                string idBuku = dataGridBuku.Rows[e.RowIndex].Cells["id_buku"].Value?.ToString();
                string judulBuku = dataGridBuku.Rows[e.RowIndex].Cells["judul"].Value?.ToString();

                if (string.IsNullOrEmpty(idBuku))
                {
                    MessageBox.Show("Data buku tidak valid!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int buttonWidth = 58;
                int margin = 5;

                int updateStart = margin;
                int updateEnd = margin + buttonWidth;
                int deleteStart = updateEnd + margin;
                int deleteEnd = deleteStart + buttonWidth;

                if (relativeX >= updateStart && relativeX <= updateEnd)
                {
                    int bookId = Convert.ToInt32(idBuku);
                    FormBooks formEdit = new FormBooks(bookId); 

                    if (formEdit.ShowDialog() == DialogResult.OK)
                    {
                        string keyword = txtSearch.Text.Trim();
                        string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
                        await TampilBuku(keyword, kategori);
                    }
                }
                else if (relativeX >= deleteStart && relativeX <= deleteEnd)
                {
                    DialogResult dialogResult = MessageBox.Show(
                        $"Apakah Anda yakin ingin menghapus buku '{judulBuku}'?\n\nData yang dihapus tidak dapat dikembalikan!",
                        "Konfirmasi Hapus",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        await HapusBuku(idBuku);
                    }
                }
            }
        }

        // HAPUS BUKU
        private async Task HapusBuku(string idBuku)
        {
            try
            {
                string coverPath = "";

                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();
                    string queryCover = "SELECT cover_buku FROM buku WHERE id_buku = @id_buku";
                    MySqlCommand cmdCover = new MySqlCommand(queryCover, conn);
                    cmdCover.Parameters.AddWithValue("@id_buku", idBuku);
                    object result = await cmdCover.ExecuteScalarAsync();
                    if (result != null && result != DBNull.Value)
                    {
                        coverPath = result.ToString();
                    }
                }

                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM buku WHERE id_buku = @id_buku";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_buku", idBuku);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        if (!string.IsNullOrEmpty(coverPath) && File.Exists(coverPath))
                        {
                            try { File.Delete(coverPath); } catch { }
                        }

                        MessageBox.Show("Buku berhasil dihapus!", "Sukses",MessageBoxButtons.OK, MessageBoxIcon.Information);

                        string keyword = txtSearch.Text.Trim();
                        string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
                        await TampilBuku(keyword, kategori);
                    }
                    else
                    {
                        MessageBox.Show("Buku tidak ditemukan atau gagal dihapus.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // SEARCH
        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);
            string keyword = txtSearch.Text.Trim();
            string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
            await TampilBuku(keyword, kategori);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
            await TampilBuku(keyword, kategori);
        }

        private async void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
            await TampilBuku(keyword, kategori);
        }

        // TOMBOL TAMBAH & REFRESH
        private async void btnTambah_Click(object sender, EventArgs e)
        {
            FormBooks formTambah = new FormBooks();

            if (formTambah.ShowDialog() == DialogResult.OK)
            {
                string keyword = txtSearch.Text.Trim();
                string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
                await TampilBuku(keyword, kategori);
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            comboKategori.SelectedIndex = 0;
            await TampilBuku();
        }

        private void lblTotalBuku_Click(object sender, EventArgs e) { }
        private void lblTersedia_Click(object sender, EventArgs e) { }
    }
}