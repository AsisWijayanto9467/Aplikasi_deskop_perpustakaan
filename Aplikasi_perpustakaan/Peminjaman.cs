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
    public partial class Peminjaman : Form
    {
        private Koneksi koneksi;
        private DataTable keranjangTable;

        public Peminjaman()
        {
            InitializeComponent();
            koneksi = new Koneksi();
            TampilkanNamaPetugas();
            SetupForm();
            LoadComboAnggota();
            LoadComboBuku();
            InitializeKeranjang();

            this.Load += Peminjaman_Load;
        }

        private void Peminjaman_Load(object sender, EventArgs e)
        {
            StylingDataGridKeranjang();
        }

        private void SetupForm()
        {
            txtPetugas.Enabled = false;
            txtPetugas.ReadOnly = true;
            txtPetugas.BackColor = Color.WhiteSmoke;

            txtKodePinjaman.ReadOnly = true;
            txtKodePinjaman.BackColor = Color.WhiteSmoke;

            txtJumlah.Text = "1";
            txtJumlah.TextAlign = HorizontalAlignment.Center;

            comboAnggota.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBuku.DropDownStyle = ComboBoxStyle.DropDownList;

            dataGridKeranjangBuku.ReadOnly = true;
            dataGridKeranjangBuku.AllowUserToAddRows = false;
            dataGridKeranjangBuku.AllowUserToDeleteRows = false;
            dataGridKeranjangBuku.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }


        private void TampilkanNamaPetugas()
        {
            txtPetugas.Text = Program.NamaLengkap;
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            txtKodePinjaman.Text = GenerateKodePeminjaman();
        }

        private string GenerateKodePeminjaman()
        {
            string kodePinjaman = "";
            bool isUnique = false;
            int maxAttempts = 10;
            int attempt = 0;

            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    conn.Open();

                    while (!isUnique && attempt < maxAttempts)
                    {
                        attempt++;

                        string tanggal = DateTime.Now.ToString("yyyyMMdd");
                        string randomPart = GenerateRandomString(4);

                        kodePinjaman = $"PJM-{tanggal}-{randomPart}";

                        if (!IsKodeExists(conn, kodePinjaman))
                        {
                            isUnique = true;
                        }
                    }

                    if (!isUnique)
                    {
                        string timestamp = DateTime.Now.ToString("HHmmssfff");
                        kodePinjaman = $"PJM-{DateTime.Now:yyyyMMdd}-{timestamp}";

                        if (IsKodeExists(conn, kodePinjaman))
                        {
                            MessageBox.Show("Gagal generate kode unik! Silakan coba lagi.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return "";
                        }
                    }
                }

                return kodePinjaman;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saat generate kode: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool IsKodeExists(MySqlConnection conn, string kode)
        {
            string query = "SELECT COUNT(*) FROM peminjaman WHERE kode_peminjaman = @kode";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@kode", kode);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        private void LoadComboAnggota()
        {
            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    conn.Open();

                    string query = @"SELECT id_anggota, kode_anggota, nama, no_hp 
                                   FROM anggota 
                                   WHERE status = 'aktif' 
                                   ORDER BY nama ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataRow defaultRow = dt.NewRow();
                            defaultRow["id_anggota"] = 0;
                            defaultRow["kode_anggota"] = "";
                            defaultRow["nama"] = "-- Pilih Anggota --";
                            defaultRow["no_hp"] = "";
                            dt.Rows.InsertAt(defaultRow, 0);

                            comboAnggota.DataSource = dt;
                            comboAnggota.DisplayMember = "nama";
                            comboAnggota.ValueMember = "id_anggota";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data anggota: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadComboBuku()
        {
            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    conn.Open();
                    string query = @"SELECT 
                                id_buku, 
                                kode_buku, 
                                judul, 
                                penulis,
                                penerbit,        -- ⬅️ TAMBAHKAN INI
                                stok,
                                lokasi_rak,
                                CONCAT(judul, ' | Stok: ', stok, ' | Kode: ', kode_buku) AS display_text
                           FROM buku 
                           WHERE stok > 0 
                           ORDER BY judul ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataRow defaultRow = dt.NewRow();
                            defaultRow["id_buku"] = 0;
                            defaultRow["kode_buku"] = "";
                            defaultRow["judul"] = "";
                            defaultRow["penulis"] = "";
                            defaultRow["penerbit"] = "";
                            defaultRow["stok"] = 0;
                            defaultRow["lokasi_rak"] = "";
                            defaultRow["display_text"] = "-- Pilih Buku --";
                            dt.Rows.InsertAt(defaultRow, 0);

                            comboBuku.DataSource = dt;
                            comboBuku.DisplayMember = "display_text";
                            comboBuku.ValueMember = "id_buku";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data buku: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboAnggota_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboAnggota.SelectedIndex > 0)
            {
                DataRowView row = (DataRowView)comboAnggota.SelectedItem;

                string infoAnggota = $"Kode: {row["kode_anggota"]}\n" +
                                    $"Nama: {row["nama"]}\n" +
                                    $"No HP: {row["no_hp"]}";
            }
        }

        private void comboBuku_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBuku.SelectedIndex > 0) 
            {
                DataRowView row = (DataRowView)comboBuku.SelectedItem;

                int stok = Convert.ToInt32(row["stok"]);
                string infoBuku = $"Judul: {row["judul"]}\n" +
                                 $"Penulis: {row["penulis"]}\n" +
                                 $"Kode: {row["kode_buku"]}\n" +
                                 $"Stok Tersedia: {stok}\n" +
                                 $"Lokasi: {row["lokasi_rak"]}";

                if (stok > 0)
                {
                }
            }
        }

        private void InitializeKeranjang()
        {
            keranjangTable = new DataTable();
            keranjangTable.Columns.Add("id_buku", typeof(int));
            keranjangTable.Columns.Add("kode_buku", typeof(string));
            keranjangTable.Columns.Add("judul", typeof(string));
            keranjangTable.Columns.Add("penulis", typeof(string));
            keranjangTable.Columns.Add("penerbit", typeof(string));
            keranjangTable.Columns.Add("stok_tersedia", typeof(int));
            keranjangTable.Columns.Add("jumlah_pinjam", typeof(int));

            dataGridKeranjangBuku.DataSource = keranjangTable;

        }

        private void StylingDataGridKeranjang()
        {
            dataGridKeranjangBuku.ReadOnly = true;
            dataGridKeranjangBuku.AllowUserToAddRows = false;
            dataGridKeranjangBuku.AllowUserToDeleteRows = false;
            dataGridKeranjangBuku.AllowUserToOrderColumns = false;
            dataGridKeranjangBuku.AllowUserToResizeColumns = false;
            dataGridKeranjangBuku.AllowUserToResizeRows = false;

            dataGridKeranjangBuku.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridKeranjangBuku.RowHeadersVisible = false;
            dataGridKeranjangBuku.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridKeranjangBuku.BorderStyle = BorderStyle.None;
            dataGridKeranjangBuku.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridKeranjangBuku.BackgroundColor = Color.White;

            dataGridKeranjangBuku.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridKeranjangBuku.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridKeranjangBuku.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            dataGridKeranjangBuku.DefaultCellStyle.Font = new Font("Inter", 9.5f, FontStyle.Regular);
            dataGridKeranjangBuku.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridKeranjangBuku.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridKeranjangBuku.RowTemplate.Height = 35;

            dataGridKeranjangBuku.EnableHeadersVisualStyles = false;
            dataGridKeranjangBuku.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridKeranjangBuku.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridKeranjangBuku.ColumnHeadersHeight = 38;

            dataGridKeranjangBuku.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridKeranjangBuku.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridKeranjangBuku.ColumnHeadersDefaultCellStyle.Font = new Font("Inter", 9f, FontStyle.Bold);
            dataGridKeranjangBuku.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            if (dataGridKeranjangBuku.Columns.Contains("id_buku"))
                dataGridKeranjangBuku.Columns["id_buku"].Visible = false;

            if (dataGridKeranjangBuku.Columns.Contains("kode_buku"))
                dataGridKeranjangBuku.Columns["kode_buku"].HeaderText = "Kode Buku";
            if (dataGridKeranjangBuku.Columns.Contains("judul"))
                dataGridKeranjangBuku.Columns["judul"].HeaderText = "Judul Buku";
            if (dataGridKeranjangBuku.Columns.Contains("penulis"))
                dataGridKeranjangBuku.Columns["penulis"].HeaderText = "Penulis";
            if (dataGridKeranjangBuku.Columns.Contains("penerbit"))
                dataGridKeranjangBuku.Columns["penerbit"].HeaderText = "Penerbit";
            if (dataGridKeranjangBuku.Columns.Contains("stok_tersedia"))
                dataGridKeranjangBuku.Columns["stok_tersedia"].HeaderText = "Stok";
            if (dataGridKeranjangBuku.Columns.Contains("jumlah_pinjam"))
                dataGridKeranjangBuku.Columns["jumlah_pinjam"].HeaderText = "Jumlah";

            if (dataGridKeranjangBuku.Columns.Contains("kode_buku"))
                dataGridKeranjangBuku.Columns["kode_buku"].Width = 100;
            if (dataGridKeranjangBuku.Columns.Contains("judul"))
                dataGridKeranjangBuku.Columns["judul"].Width = 200;
            if (dataGridKeranjangBuku.Columns.Contains("penulis"))
                dataGridKeranjangBuku.Columns["penulis"].Width = 120;
            if (dataGridKeranjangBuku.Columns.Contains("penerbit"))
                dataGridKeranjangBuku.Columns["penerbit"].Width = 120;
            if (dataGridKeranjangBuku.Columns.Contains("stok_tersedia"))
                dataGridKeranjangBuku.Columns["stok_tersedia"].Width = 60;
            if (dataGridKeranjangBuku.Columns.Contains("jumlah_pinjam"))
                dataGridKeranjangBuku.Columns["jumlah_pinjam"].Width = 60;

            if (!dataGridKeranjangBuku.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                colNo.Name = "No";
                colNo.HeaderText = "No";
                colNo.Width = 40;
                colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridKeranjangBuku.Columns.Insert(0, colNo);
            }

            if (!dataGridKeranjangBuku.Columns.Contains("Action"))
            {
                DataGridViewTextBoxColumn colAction = new DataGridViewTextBoxColumn();
                colAction.Name = "Action";
                colAction.HeaderText = "Action";
                colAction.Width = 80;
                colAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridKeranjangBuku.Columns.Add(colAction);
            }

            dataGridKeranjangBuku.RowPostPaint -= DataGridKeranjang_RowPostPaint;
            dataGridKeranjangBuku.RowPostPaint += DataGridKeranjang_RowPostPaint;

            dataGridKeranjangBuku.CellPainting -= DataGridKeranjang_CellPainting;
            dataGridKeranjangBuku.CellPainting += DataGridKeranjang_CellPainting;

            dataGridKeranjangBuku.CellClick -= DataGridKeranjang_CellClick;
            dataGridKeranjangBuku.CellClick += DataGridKeranjang_CellClick;
        }

        private void DataGridKeranjang_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dataGridKeranjangBuku.Rows[e.RowIndex].Cells["No"] != null)
            {
                dataGridKeranjangBuku.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
            }
        }

        private void DataGridKeranjang_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridKeranjangBuku.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                int buttonWidth = 70;
                int buttonHeight = 24;

                int deleteX = e.CellBounds.Left + (e.CellBounds.Width - buttonWidth) / 2;
                int deleteY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;
                Rectangle rectDelete = new Rectangle(deleteX, deleteY, buttonWidth, buttonHeight);

                using (SolidBrush brush = new SolidBrush(Color.Crimson))
                {
                    e.Graphics.FillRectangle(brush, rectDelete);
                }

                TextRenderer.DrawText(e.Graphics, "Hapus", new Font("Inter", 8f, FontStyle.Bold),
                    rectDelete, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                e.Handled = true;
            }
        }

        private void DataGridKeranjang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridKeranjangBuku.Columns[e.ColumnIndex].Name == "Action")
            {
                string judulBuku = dataGridKeranjangBuku.Rows[e.RowIndex].Cells["judul"].Value?.ToString();

                DialogResult dialogResult = MessageBox.Show(
                    $"Apakah Anda yakin ingin menghapus buku '{judulBuku}' dari keranjang?",
                    "Konfirmasi Hapus",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (dialogResult == DialogResult.Yes)
                {
                    HapusDariKeranjang(e.RowIndex);
                }
            }
        }

        private void HapusDariKeranjang(int rowIndex)
        {
            try
            {
                keranjangTable.Rows[rowIndex].Delete();
                keranjangTable.AcceptChanges();

                UpdateTotalBuku();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error menghapus buku: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridKeranjangBuku_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void txtJumlah_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtJumlah.Text))
            {
                if (!int.TryParse(txtJumlah.Text, out int jumlah))
                {
                    txtJumlah.Text = "1";
                }
                else if (jumlah < 1)
                {
                    txtJumlah.Text = "1";
                }
            }
        }

        private void btnTambahBuku_Click(object sender, EventArgs e)
        {
            if (comboBuku.SelectedIndex <= 0)
            {
                MessageBox.Show("Silakan pilih buku terlebih dahulu!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBuku.Focus();
                return;
            }

            if (!int.TryParse(txtJumlah.Text, out int jumlahPinjam) || jumlahPinjam <= 0)
            {
                MessageBox.Show("Jumlah pinjam tidak valid!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtJumlah.Focus();
                return;
            }

            DataRowView selectedBuku = (DataRowView)comboBuku.SelectedItem;
            int idBuku = Convert.ToInt32(selectedBuku["id_buku"]);
            int stokTersedia = Convert.ToInt32(selectedBuku["stok"]);

            DataRow[] existingRows = keranjangTable.Select($"id_buku = {idBuku}");
            if (existingRows.Length > 0)
            {
                int jumlahDiKeranjang = Convert.ToInt32(existingRows[0]["jumlah_pinjam"]);
                int totalJumlah = jumlahDiKeranjang + jumlahPinjam;

                if (totalJumlah > stokTersedia)
                {
                    MessageBox.Show($"Stok tidak mencukupi!\n\n" +
                        $"Stok tersedia: {stokTersedia}\n" +
                        $"Di keranjang: {jumlahDiKeranjang}\n" +
                        $"Mau ditambah: {jumlahPinjam}\n" +
                        $"Total: {totalJumlah} (melebihi stok!)",
                        "Stok Tidak Cukup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                existingRows[0]["jumlah_pinjam"] = totalJumlah;
                keranjangTable.AcceptChanges();

            }
            else
            {
                if (jumlahPinjam > stokTersedia)
                {
                    MessageBox.Show($"Stok buku tidak mencukupi!\n" +
                        $"Stok tersedia: {stokTersedia}\n" +
                        $"Diminta: {jumlahPinjam}",
                        "Stok Tidak Cukup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow newRow = keranjangTable.NewRow();
                newRow["id_buku"] = idBuku;
                newRow["kode_buku"] = selectedBuku["kode_buku"];
                newRow["judul"] = selectedBuku["judul"];
                newRow["penulis"] = selectedBuku["penulis"];
                newRow["penerbit"] = selectedBuku["penerbit"];
                newRow["stok_tersedia"] = stokTersedia;
                newRow["jumlah_pinjam"] = jumlahPinjam;

                keranjangTable.Rows.Add(newRow);

            }

            UpdateTotalBuku();

            comboBuku.SelectedIndex = 0;
            txtJumlah.Text = "1";
        }

        private void UpdateTotalBuku()
        {
            int totalJenisBuku = keranjangTable.Rows.Count;
            int totalEksemplar = 0;

            foreach (DataRow row in keranjangTable.Rows)
            {
                totalEksemplar += Convert.ToInt32(row["jumlah_pinjam"]);
            }

            lblTotalBuku.Text = $"Jenis Buku: {totalJenisBuku}";
            lblEksemplarBuku.Text = $"Total Eksemplar: {totalEksemplar}";
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            btnSave.Enabled = false;
            string originalText = btnSave.Text;
            btnSave.Text = "...Loading";

            try
            {
                using (MySqlConnection conn = koneksi.GetConn())
                {
                    await conn.OpenAsync();

                    using (MySqlTransaction transaction = await conn.BeginTransactionAsync())
                    {
                        try
                        {
                            if (await IsKodeExistsAsync(conn, txtKodePinjaman.Text))
                            {
                                MessageBox.Show("Kode peminjaman sudah digunakan!\nSilakan generate kode baru.",
                                    "Duplikasi Kode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string queryPeminjaman = @"INSERT INTO peminjaman 
                        (kode_peminjaman, id_anggota, id_user, tanggal_pinjam, tanggal_jatuh_tempo, status) 
                        VALUES 
                        (@kode, @idAnggota, @idUser, @tglPinjam, @tglJatuhTempo, @status);
                        SELECT LAST_INSERT_ID();";

                            long idPeminjaman = 0;

                            using (MySqlCommand cmd = new MySqlCommand(queryPeminjaman, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@kode", txtKodePinjaman.Text);
                                cmd.Parameters.AddWithValue("@idAnggota", comboAnggota.SelectedValue);
                                cmd.Parameters.AddWithValue("@idUser", Program.UserId);
                                cmd.Parameters.AddWithValue("@tglPinjam", DatePickerTanggalDaftar.Value.Date);
                                cmd.Parameters.AddWithValue("@tglJatuhTempo", datePickerTempo.Value.Date);

                                string status = radioDipinjam.Checked ? "dipinjam" : "dikembalikan";
                                cmd.Parameters.AddWithValue("@status", status);

                                idPeminjaman = Convert.ToInt64(await cmd.ExecuteScalarAsync());
                            }

                            foreach (DataRow row in keranjangTable.Rows)
                            {
                                int idBuku = Convert.ToInt32(row["id_buku"]);
                                int jumlah = Convert.ToInt32(row["jumlah_pinjam"]);

                                string queryDetail = @"INSERT INTO detail_peminjaman 
                            (id_peminjaman, id_buku, jumlah) 
                            VALUES 
                            (@idPeminjaman, @idBuku, @jumlah)";

                                using (MySqlCommand cmd = new MySqlCommand(queryDetail, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@idPeminjaman", idPeminjaman);
                                    cmd.Parameters.AddWithValue("@idBuku", idBuku);
                                    cmd.Parameters.AddWithValue("@jumlah", jumlah);
                                    await cmd.ExecuteNonQueryAsync();
                                }

                                string queryUpdateStok = @"UPDATE buku 
                                                  SET stok = stok - @jumlah 
                                                  WHERE id_buku = @idBuku 
                                                  AND stok >= @jumlah";

                                using (MySqlCommand cmd = new MySqlCommand(queryUpdateStok, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@jumlah", jumlah);
                                    cmd.Parameters.AddWithValue("@idBuku", idBuku);

                                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                                    if (rowsAffected == 0)
                                    {
                                        throw new Exception($"Stok buku '{row["judul"]}' tidak mencukupi!");
                                    }
                                }
                            }

                            await transaction.CommitAsync();

                            MessageBox.Show("Peminjaman berhasil disimpan!", "Sukses",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ResetForm();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            MessageBox.Show($"Gagal menyimpan peminjaman!\n\nError: {ex.Message}\n\n" +
                                "Semua perubahan telah dibatalkan.",
                                "Error Transaksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error koneksi database: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = originalText;
            }
        }

        private async Task<bool> IsKodeExistsAsync(MySqlConnection conn, string kode)
        {
            string query = "SELECT COUNT(*) FROM peminjaman WHERE kode_peminjaman = @kode";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@kode", kode);
                object result = await cmd.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtKodePinjaman.Text))
            {
                MessageBox.Show("Harap generate kode peminjaman terlebih dahulu!\nKlik tombol 'Auto' untuk generate kode.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnAuto.Focus();
                return false;
            }
            if (comboAnggota.SelectedIndex <= 0)
            {
                MessageBox.Show("Harap pilih anggota terlebih dahulu!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboAnggota.Focus();
                return false;
            }

            if (keranjangTable.Rows.Count == 0)
            {
                MessageBox.Show("Keranjang buku masih kosong!\nTambahkan buku terlebih dahulu.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBuku.Focus();
                return false;
            }

            if (datePickerTempo.Value.Date <= DatePickerTanggalDaftar.Value.Date)
            {
                MessageBox.Show("Tanggal jatuh tempo harus lebih besar dari tanggal pinjam!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                datePickerTempo.Focus();
                return false;
            }

            if (!radioDipinjam.Checked && !radioDikembalikan.Checked)
            {
                MessageBox.Show("Harap pilih status peminjaman!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ResetForm()
        {
            txtKodePinjaman.Clear();
            comboAnggota.SelectedIndex = 0;
            comboBuku.SelectedIndex = 0;
            txtJumlah.Text = "1";
            keranjangTable.Clear();

            UpdateTotalBuku();

            DatePickerTanggalDaftar.Value = DateTime.Now;
            datePickerTempo.Value = DateTime.Now.AddDays(7);
            radioDipinjam.Checked = false;
            radioDikembalikan.Checked = false;

            btnAuto.Focus();
        }

        private void btnRefreshAll_Click(object sender, EventArgs e)
        {
            txtKodePinjaman.Clear();
            comboAnggota.SelectedIndex = 0;
            comboBuku.SelectedIndex = 0;
            txtJumlah.Text = "1";
            keranjangTable.Clear();

            UpdateTotalBuku();

            DatePickerTanggalDaftar.Value = DateTime.Now;
            datePickerTempo.Value = DateTime.Now.AddDays(7);
            radioDipinjam.Checked = false;
            radioDikembalikan.Checked = false;

            btnAuto.Focus();
        }

        private void DatePickerTanggalDaftar_ValueChanged(object sender, EventArgs e)
        {
            if (datePickerTempo.Value <= DatePickerTanggalDaftar.Value)
            {
                datePickerTempo.Value = DatePickerTanggalDaftar.Value.AddDays(7);
            }
        }

        private void datePickerTempo_ValueChanged(object sender, EventArgs e)
        {
            if (datePickerTempo.Value.Date < DatePickerTanggalDaftar.Value.Date)
            {
                MessageBox.Show("Tanggal jatuh tempo tidak boleh kurang dari tanggal pinjam!",
                    "Validasi Tanggal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                datePickerTempo.Value = DatePickerTanggalDaftar.Value.AddDays(7);
            }
        }

        private void radioDipinjam_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioDikembalikan_CheckedChanged(object sender, EventArgs e)
        {
        }












        private void lblTotalBuku_Click(object sender, EventArgs e)
        {

        }

        private void lblEksemplarBuku_Click(object sender, EventArgs e)
        {

        }

        private void txtKodePinjaman_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridBuku_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {

        }


        private void guna2Panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void txtPetugas_TextChanged(object sender, EventArgs e)
        {

        }

        
    }

}
