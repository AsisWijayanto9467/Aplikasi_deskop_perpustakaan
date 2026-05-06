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

namespace Aplikasi_perpustakaan
{
    public partial class Anggota : Form
    {
        private Koneksi kon = new Koneksi();

        public Anggota()
        {
            InitializeComponent();
            btnUpdate.Visible = false;
            btnCancel.Visible = false;

            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.dataGridAnggota.CellClick += new DataGridViewCellEventHandler(this.dataGridAnggota_CellClick);

            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.txtNoHp.TextChanged += new System.EventHandler(this.txtNoHp_TextChanged);
        }

        private async void Anggota_Load(object sender, EventArgs e)
        {
            GenerateKodeAnggota();
            radioAktif.Checked = true;

            StylingDataGridView();
            await TampilAnggota();
        }

        private async Task TampilAnggota(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string query = @"SELECT id_anggota, kode_anggota, nama, alamat, no_hp, 
                                    DATE_FORMAT(tanggal_daftar, '%Y-%m-%d') AS tanggal_daftar, 
                                    status 
                                    FROM anggota";

                    MySqlCommand cmd;

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query += @" WHERE kode_anggota LIKE @keyword 
                                   OR nama LIKE @keyword 
                                   OR alamat LIKE @keyword 
                                   OR no_hp LIKE @keyword 
                                   OR status LIKE @keyword";
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    }
                    else
                    {
                        cmd = new MySqlCommand(query, conn);
                    }

                    using (cmd)
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            await Task.Run(() => da.Fill(dt));

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    UpdateDataGrid(dt);
                                }));
                            }
                            else
                            {
                                UpdateDataGrid(dt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data anggota: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGrid(DataTable dt)
        {
            dataGridAnggota.DataSource = dt;

            // Set header text untuk setiap kolom
            if (dt.Columns.Contains("id_anggota"))
                dataGridAnggota.Columns["id_anggota"].HeaderText = "ID Anggota";
            if (dt.Columns.Contains("kode_anggota"))
                dataGridAnggota.Columns["kode_anggota"].HeaderText = "Kode Anggota";
            if (dt.Columns.Contains("nama"))
                dataGridAnggota.Columns["nama"].HeaderText = "Nama Lengkap";
            if (dt.Columns.Contains("alamat"))
                dataGridAnggota.Columns["alamat"].HeaderText = "Alamat";
            if (dt.Columns.Contains("no_hp"))
                dataGridAnggota.Columns["no_hp"].HeaderText = "No. HP";
            if (dt.Columns.Contains("tanggal_daftar"))
                dataGridAnggota.Columns["tanggal_daftar"].HeaderText = "Tanggal Daftar";
            if (dt.Columns.Contains("status"))
                dataGridAnggota.Columns["status"].HeaderText = "Status";

            // Update total anggota
            lblTotalAnggota.Text = "Total Anggota: " + dt.Rows.Count.ToString();

            // Buat kolom aksi (Update/Delete buttons)
            BuatKolomAksi();
        }

        private void BuatKolomAksi()
        {
            // Sembunyikan kolom ID
            if (dataGridAnggota.Columns.Contains("id_anggota"))
            {
                dataGridAnggota.Columns["id_anggota"].Visible = false;
            }

            // Tambahkan kolom No jika belum ada
            if (!dataGridAnggota.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                colNo.Name = "No";
                colNo.HeaderText = "No";
                colNo.Width = 40;
                colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridAnggota.Columns.Insert(0, colNo);
            }

            // Atur auto size untuk kolom nama
            if (dataGridAnggota.Columns.Contains("nama"))
            {
                dataGridAnggota.Columns["nama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Hapus kolom aksi yang lama jika ada
            if (dataGridAnggota.Columns.Contains("btnUpdate"))
                dataGridAnggota.Columns.Remove("btnUpdate");
            if (dataGridAnggota.Columns.Contains("btnDelete"))
                dataGridAnggota.Columns.Remove("btnDelete");
            if (dataGridAnggota.Columns.Contains("Action"))
                dataGridAnggota.Columns.Remove("Action");

            // Tambahkan kolom Action baru
            DataGridViewTextBoxColumn colAction = new DataGridViewTextBoxColumn();
            colAction.Name = "Action";
            colAction.HeaderText = "Action";
            colAction.Width = 150;
            colAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridAnggota.Columns.Add(colAction);
        }

        private void StylingDataGridView()
        {
            dataGridAnggota.ReadOnly = true;
            dataGridAnggota.AllowUserToAddRows = false;
            dataGridAnggota.AllowUserToDeleteRows = false;
            dataGridAnggota.AllowUserToOrderColumns = false;

            dataGridAnggota.AllowUserToResizeColumns = false;
            dataGridAnggota.AllowUserToResizeRows = false;

            dataGridAnggota.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridAnggota.RowHeadersVisible = false;
            dataGridAnggota.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridAnggota.BorderStyle = BorderStyle.None;
            dataGridAnggota.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridAnggota.BackgroundColor = Color.White;

            dataGridAnggota.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridAnggota.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridAnggota.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            dataGridAnggota.DefaultCellStyle.Font = new Font("Inter", 9.5f, FontStyle.Regular);

            dataGridAnggota.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridAnggota.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridAnggota.RowTemplate.Height = 35;

            dataGridAnggota.EnableHeadersVisualStyles = false;
            dataGridAnggota.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dataGridAnggota.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridAnggota.ColumnHeadersHeight = 38;

            dataGridAnggota.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridAnggota.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridAnggota.ColumnHeadersDefaultCellStyle.Font = new Font("Inter", 9f, FontStyle.Bold);
            dataGridAnggota.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            // Event untuk menampilkan nomor urut dan menggambar button
            dataGridAnggota.RowPostPaint -= DataGridAnggota_RowPostPaint;
            dataGridAnggota.RowPostPaint += DataGridAnggota_RowPostPaint;

            dataGridAnggota.CellPainting -= DataGridAnggota_CellPainting;
            dataGridAnggota.CellPainting += DataGridAnggota_CellPainting;
        }

        private void DataGridAnggota_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dataGridAnggota.Rows[e.RowIndex].Cells["No"] != null)
            {
                dataGridAnggota.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
            }
        }

        private void DataGridAnggota_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridAnggota.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                int buttonWidth = 60;
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
                    using (SolidBrush brush = new SolidBrush(Color.DodgerBlue))
                    {
                        e.Graphics.FillRectangle(brush, rectUpdate);
                    }
                    TextRenderer.DrawText(e.Graphics, "Update", new Font("Segoe UI", 8f, FontStyle.Bold),
                        rectUpdate, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    using (SolidBrush brush = new SolidBrush(Color.Crimson))
                    {
                        e.Graphics.FillRectangle(brush, rectDelete);
                    }
                    TextRenderer.DrawText(e.Graphics, "Delete", new Font("Segoe UI", 8f, FontStyle.Bold),
                        rectDelete, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                e.Handled = true;
            }
        }

        // ==================== SEARCH METHODS ====================

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                await TampilAnggota(txtSearch.Text.Trim());
            }
            else
            {
                await TampilAnggota();
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await TampilAnggota(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await TampilAnggota();
        }

        // ==================== CELL CLICK (Untuk tombol Update/Delete) ====================

        private async void dataGridAnggota_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridAnggota.Columns[e.ColumnIndex].Name == "Action")
            {
                Point mousePosition = dataGridAnggota.PointToClient(Cursor.Position);
                Rectangle cellRect = dataGridAnggota.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int relativeX = mousePosition.X - cellRect.X;

                string idAnggota = dataGridAnggota.Rows[e.RowIndex].Cells["id_anggota"].Value?.ToString();
                string namaAnggota = dataGridAnggota.Rows[e.RowIndex].Cells["nama"].Value?.ToString();
                string kodeAnggota = dataGridAnggota.Rows[e.RowIndex].Cells["kode_anggota"].Value?.ToString();
                string alamat = dataGridAnggota.Rows[e.RowIndex].Cells["alamat"].Value?.ToString();
                string noHp = dataGridAnggota.Rows[e.RowIndex].Cells["no_hp"].Value?.ToString();
                string tanggalDaftar = dataGridAnggota.Rows[e.RowIndex].Cells["tanggal_daftar"].Value?.ToString();
                string status = dataGridAnggota.Rows[e.RowIndex].Cells["status"].Value?.ToString();

                if (string.IsNullOrEmpty(idAnggota))
                {
                    MessageBox.Show("Data anggota tidak valid!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int buttonWidth = 60;
                int margin = 5;

                int updateStart = margin;
                int updateEnd = margin + buttonWidth;
                int deleteStart = updateEnd + margin;
                int deleteEnd = deleteStart + buttonWidth;

                if (relativeX >= updateStart && relativeX <= updateEnd)
                {
                    // Masuk mode edit
                    MasukModeEdit(idAnggota, kodeAnggota, namaAnggota, alamat, noHp, tanggalDaftar, status);
                }
                else if (relativeX >= deleteStart && relativeX <= deleteEnd)
                {
                    // Konfirmasi dan hapus data
                    DialogResult dialogResult = MessageBox.Show(
                        $"Apakah Anda yakin ingin menghapus anggota '{namaAnggota}'?\n\nData yang dihapus tidak dapat dikembalikan!",
                        "Konfirmasi Hapus",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        await HapusAnggota(idAnggota);
                    }
                }
            }
        }

        // ==================== MODE EDIT ====================

        private void MasukModeEdit(string idAnggota, string kodeAnggota, string nama, string alamat, string noHp, string tanggalDaftar, string status)
        {
            // Simpan ID anggota ke Tag txtCode untuk digunakan saat update
            txtCode.Tag = idAnggota;

            // Isi form dengan data yang akan diedit
            txtCode.Text = kodeAnggota;
            txtFullName.Text = nama;
            txtAlamat.Text = alamat;
            txtNoHp.Text = noHp;

            // Parse tanggal daftar
            if (DateTime.TryParse(tanggalDaftar, out DateTime tglDaftar))
            {
                DatePickerTanggalDaftar.Value = tglDaftar;
            }
            else
            {
                DatePickerTanggalDaftar.Value = DateTime.Now;
            }

            // Set status radio button
            if (status.ToLower() == "aktif")
            {
                radioAktif.Checked = true;
                radioNonAktif.Checked = false;
            }
            else
            {
                radioNonAktif.Checked = true;
                radioAktif.Checked = false;
            }

            // Sembunyikan tombol Simpan/Clear, tampilkan tombol Update/Cancel
            btnSimpan.Visible = false;
            btnClear.Visible = false;
            btnUpdate.Visible = true;
            btnCancel.Visible = true;

            // Disable tombol Auto dan txtCode agar kode tidak bisa diubah saat edit
            btnAuto.Enabled = false;
            txtCode.ReadOnly = true;

            txtFullName.Focus();
        }

        private void KembaliModeTambah()
        {
            // Hapus ID yang tersimpan
            txtCode.Tag = null;

            // Clear semua field
            ClearAllFields();

            // Kembalikan tombol seperti semula
            btnSimpan.Visible = true;
            btnClear.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;

            // Enable kembali tombol Auto dan txtCode
            btnAuto.Enabled = true;
            txtCode.ReadOnly = false;

            txtFullName.Focus();
        }

        // ==================== UPDATE ANGGOTA ====================

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // Validasi input
            if (!ValidateAllInputs())
            {
                return;
            }

            string idAnggota = txtCode.Tag?.ToString();

            if (string.IsNullOrEmpty(idAnggota))
            {
                MessageBox.Show("Data anggota tidak valid!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Konfirmasi update
            DialogResult dialogResult = MessageBox.Show(
                "Apakah Anda yakin ingin mengupdate data anggota ini?",
                "Konfirmasi Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult == DialogResult.Yes)
            {
                await UpdateAnggota(idAnggota);
            }
        }

        private async Task UpdateAnggota(string idAnggota)
        {
            try
            {
                btnUpdate.Enabled = false;
                string originalText = btnUpdate.Text;
                btnUpdate.Text = "...Loading";

                string nama = txtFullName.Text.Trim();
                string alamat = txtAlamat.Text.Trim();
                string noHp = txtNoHp.Text.Trim();
                string tglDaftar = DatePickerTanggalDaftar.Value.ToString("yyyy-MM-dd");
                string status = radioAktif.Checked ? "aktif" : "nonaktif";

                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string query = @"UPDATE anggota 
                                    SET nama = @nama, 
                                        alamat = @alamat, 
                                        no_hp = @noHp, 
                                        tanggal_daftar = @tglDaftar, 
                                        status = @status 
                                    WHERE id_anggota = @idAnggota";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@alamat", alamat);
                        cmd.Parameters.AddWithValue("@noHp", noHp);
                        cmd.Parameters.AddWithValue("@tglDaftar", tglDaftar);
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@idAnggota", idAnggota);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data anggota berhasil diupdate!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Kembali ke mode tambah
                            KembaliModeTambah();

                            // Refresh data grid
                            await TampilAnggota(txtSearch.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("Gagal mengupdate data anggota. Data mungkin tidak ditemukan.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem: " + ex.Message,
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUpdate.Enabled = true;
                btnUpdate.Text = "Update";
            }
        }

        // ==================== CANCEL EDIT ====================

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bool hasChanges = !string.IsNullOrWhiteSpace(txtFullName.Text) ||
                             !string.IsNullOrWhiteSpace(txtAlamat.Text) ||
                             !string.IsNullOrWhiteSpace(txtNoHp.Text);

            if (hasChanges)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Apakah Anda yakin ingin membatalkan edit?\nPerubahan yang belum disimpan akan hilang.",
                    "Konfirmasi Batal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            KembaliModeTambah();
        }

        // ==================== HAPUS ANGGOTA ====================

        private async Task HapusAnggota(string idAnggota)
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string query = "DELETE FROM anggota WHERE id_anggota = @idAnggota";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idAnggota", idAnggota);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Anggota berhasil dihapus!",
                                "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Refresh data grid
                            await TampilAnggota(txtSearch.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("Anggota tidak ditemukan atau gagal dihapus.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Cek jika error karena foreign key constraint
                if (ex.Number == 1451)
                {
                    MessageBox.Show("Anggota tidak dapat dihapus karena masih memiliki data terkait (misalnya data peminjaman).",
                        "Hapus Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Terjadi kesalahan database: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem: " + ex.Message,
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== EXISTING METHODS ====================

        // Tombol Auto untuk generate kode anggota
        private void btnAuto_Click(object sender, EventArgs e)
        {
            GenerateKodeAnggota();
        }

        private void GenerateKodeAnggota()
        {
            string kodeBaru = GenerateKodeDariDatabase();
            txtCode.Text = kodeBaru;
        }

        private string GenerateKodeDariDatabase()
        {
            string kodeTerakhir = GetLastKodeAnggota();
            string kodeBaru = GenerateNextKode(kodeTerakhir);
            return kodeBaru;
        }

        // Method untuk mengambil kode anggota terakhir dari database
        private string GetLastKodeAnggota()
        {
            string lastKode = null;

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();
                    string query = "SELECT kode_anggota FROM anggota ORDER BY id_anggota DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lastKode = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error mengambil data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return lastKode;
        }

        private string GenerateNextKode(string lastKode)
        {
            if (string.IsNullOrEmpty(lastKode))
            {
                return "A-001";
            }

            string[] parts = lastKode.Split('-');

            if (parts.Length == 2 && parts[0] == "A")
            {
                if (int.TryParse(parts[1], out int lastNumber))
                {
                    int nextNumber = lastNumber + 1;
                    return $"A-{nextNumber:D3}";
                }
            }

            return "A-001";
        }

        private bool IsKodeExist(string kode)
        {
            bool exists = false;

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM anggota WHERE kode_anggota = @kode";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kode", kode);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return exists;
        }

        // Method untuk validasi semua input
        private bool ValidateAllInputs()
        {
            // Validasi kode anggota
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Kode anggota tidak boleh kosong! Silakan klik tombol Auto untuk generate kode.",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            // Validasi nama lengkap
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Nama lengkap harus diisi!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            // Validasi panjang nama minimal
            if (txtFullName.Text.Trim().Length < 3)
            {
                MessageBox.Show("Nama lengkap minimal 3 karakter!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return false;
            }

            // Validasi alamat
            if (string.IsNullOrWhiteSpace(txtAlamat.Text))
            {
                MessageBox.Show("Alamat harus diisi!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlamat.Focus();
                return false;
            }

            // Validasi panjang alamat minimal
            if (txtAlamat.Text.Trim().Length < 5)
            {
                MessageBox.Show("Alamat terlalu pendek! Minimal 5 karakter.",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAlamat.Focus();
                return false;
            }

            // Validasi nomor HP
            if (string.IsNullOrWhiteSpace(txtNoHp.Text))
            {
                MessageBox.Show("Nomor HP harus diisi!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNoHp.Focus();
                return false;
            }

            // Validasi format nomor HP (hanya angka)
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtNoHp.Text.Trim(), @"^[\d]+$"))
            {
                MessageBox.Show("Nomor HP hanya boleh berisi angka!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNoHp.Focus();
                return false;
            }

            // Validasi panjang nomor HP
            if (txtNoHp.Text.Trim().Length < 10 || txtNoHp.Text.Trim().Length > 13)
            {
                MessageBox.Show("Nomor HP harus antara 10-13 digit!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNoHp.Focus();
                return false;
            }

            // Validasi tanggal daftar
            if (DatePickerTanggalDaftar.Value > DateTime.Now)
            {
                MessageBox.Show("Tanggal daftar tidak boleh lebih dari tanggal hari ini!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DatePickerTanggalDaftar.Focus();
                return false;
            }

            // Validasi status harus dipilih
            if (!radioAktif.Checked && !radioNonAktif.Checked)
            {
                MessageBox.Show("Status keanggotaan harus dipilih!",
                    "Validasi Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool SaveAnggotaData(string kode, string nama, string alamat, string noHp, string tglDaftar, string status)
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();

                    string query = @"INSERT INTO anggota 
                (kode_anggota, nama, alamat, no_hp, tanggal_daftar, status) 
                VALUES 
                (@kode, @nama, @alamat, @noHp, @tglDaftar, @status)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kode", kode);
                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@alamat", alamat);
                        cmd.Parameters.AddWithValue("@noHp", noHp);
                        cmd.Parameters.AddWithValue("@tglDaftar", tglDaftar);
                        cmd.Parameters.AddWithValue("@status", status);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("Kode anggota sudah ada dalam database! Silakan generate kode baru.",
                        "Duplicate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    throw;
                }
                return false;
            }
        }

        private void ClearAllFields()
        {
            GenerateKodeAnggota();

            txtFullName.Clear();
            txtAlamat.Clear();
            txtNoHp.Clear();

            DatePickerTanggalDaftar.Value = DateTime.Now;

            radioAktif.Checked = true;
            radioNonAktif.Checked = false;

            // Jangan clear txtSearch agar data tetap terfilter
            // txtSearch.Clear();

            txtFullName.Focus();
        }

        private async void btnSimpan_Click(object sender, EventArgs e)
        {
            if (!ValidateAllInputs())
            {
                return;
            }

            if (IsKodeExist(txtCode.Text.Trim()))
            {
                MessageBox.Show("Kode anggota sudah ada! Silakan generate kode baru dengan mengklik tombol Auto.",
                    "Kode Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return;
            }

            string kode = txtCode.Text.Trim();
            string nama = txtFullName.Text.Trim();
            string alamat = txtAlamat.Text.Trim();
            string noHp = txtNoHp.Text.Trim();
            string tglDaftar = DatePickerTanggalDaftar.Value.ToString("yyyy-MM-dd");
            string status = radioAktif.Checked ? "aktif" : "nonaktif";

            DialogResult confirmResult = MessageBox.Show(
                "Apakah Anda yakin ingin menyimpan data anggota ini?\n\n" +
                $"Kode: {kode}\n" +
                $"Nama: {nama}\n" +
                $"Alamat: {alamat}\n" +
                $"No. HP: {noHp}\n" +
                $"Tanggal Daftar: {DatePickerTanggalDaftar.Value:dd MMMM yyyy}\n" +
                $"Status: {(radioAktif.Checked ? "Aktif" : "Nonaktif")}",
                "Konfirmasi Penyimpanan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            btnSimpan.Enabled = false;
            string originalText = btnSimpan.Text;
            btnSimpan.Text = "...Loading";

            try
            {
                bool success = await Task.Run(() => SaveAnggotaData(kode, nama, alamat, noHp, tglDaftar, status));

                if (success)
                {
                    MessageBox.Show("Data anggota berhasil disimpan!",
                        "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearAllFields();

                    // Refresh data grid setelah simpan
                    await TampilAnggota(txtSearch.Text.Trim());
                }
                else
                {
                    MessageBox.Show("Gagal menyimpan data anggota!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan sistem: {ex.Message}",
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSimpan.Enabled = true;
                btnSimpan.Text = originalText;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            bool hasData = !string.IsNullOrWhiteSpace(txtFullName.Text) ||
                          !string.IsNullOrWhiteSpace(txtAlamat.Text) ||
                          !string.IsNullOrWhiteSpace(txtNoHp.Text) ||
                          radioNonAktif.Checked;

            if (hasData)
            {
                DialogResult result = MessageBox.Show(
                    "Apakah Anda yakin ingin membersihkan semua field?\nSemua data yang belum disimpan akan hilang.",
                    "Konfirmasi Clear",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ClearAllFields();
                }
            }
            else
            {
                ClearAllFields();
            }
        }

        private void txtNoHp_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNoHp.Text))
            {
                string filtered = System.Text.RegularExpressions.Regex.Replace(txtNoHp.Text, "[^0-9]", "");
                if (filtered != txtNoHp.Text)
                {
                    txtNoHp.Text = filtered;
                    txtNoHp.SelectionStart = txtNoHp.Text.Length;
                }
            }
        }

        // ==================== EMPTY EVENT HANDLERS ====================

        private void txtAlamat_TextChanged(object sender, EventArgs e) { }
        private void txtCode_TextChanged(object sender, EventArgs e) { }
        private void txtFullName_TextChanged(object sender, EventArgs e) { }
        private void DatePickerTanggalDaftar_ValueChanged(object sender, EventArgs e) { }
        private void radioAktif_CheckedChanged(object sender, EventArgs e) { }
        private void radioNonAktif_CheckedChanged(object sender, EventArgs e) { }
        private void lblTotalAnggota_Click(object sender, EventArgs e) { }

    }
}