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
    public partial class Pengembalian : Form
    {
        private Koneksi koneksi;
        private int idPeminjamanAktif;

        private DataTable rincianDendaTable;
        private int totalDendaKeterlambatan = 0;
        private int totalDendaBukuHilang = 0;

        private bool isFormResetting = false;

        public Pengembalian()
        {
            InitializeComponent();
            koneksi = new Koneksi();
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            btnSave.Enabled = false;

            this.radioLunas.CheckedChanged += new System.EventHandler(this.radioLunas_CheckedChanged);
            this.radioBelumLunas.CheckedChanged += new System.EventHandler(this.radioBelumLunas_CheckedChanged);

            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnBatal.Click += new System.EventHandler(this.btnBatal_Click);
        }

        private void Pengembalian_Load(object sender, EventArgs e)
        {
            SetReadOnlyTextboxes();
            ClearFields();
            lblConsequence.Visible = false;
            StylingDataGridView();
            SetupRincianDendaGrid(); 

            txtTotalDenda.ReadOnly = true;
            txtTotalDenda.BackColor = SystemColors.ControlLight;
        }

        private void SetReadOnlyTextboxes()
        {
            txtCode.ReadOnly = true;
            txtPinjam.ReadOnly = true;
            txtNamaAnggota.ReadOnly = true;
            txtTempo.ReadOnly = true;

            txtCode.BackColor = SystemColors.ControlLight;
            txtPinjam.BackColor = SystemColors.ControlLight;
            txtNamaAnggota.BackColor = SystemColors.ControlLight;
            txtTempo.BackColor = SystemColors.ControlLight;
        }

        private void StylingDataGridView()
        {
            dataGridReturnBook.ReadOnly = false; 
            dataGridReturnBook.AllowUserToAddRows = false;
            dataGridReturnBook.AllowUserToDeleteRows = false;
            dataGridReturnBook.AllowUserToOrderColumns = false;

            dataGridReturnBook.AllowUserToResizeColumns = false;
            dataGridReturnBook.AllowUserToResizeRows = false;

            dataGridReturnBook.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridReturnBook.RowHeadersVisible = false;
            dataGridReturnBook.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridReturnBook.BorderStyle = BorderStyle.None;
            dataGridReturnBook.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridReturnBook.BackgroundColor = Color.White;

            dataGridReturnBook.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridReturnBook.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridReturnBook.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            dataGridReturnBook.DefaultCellStyle.Font = new Font("Inter", 9.5f, FontStyle.Regular);

            dataGridReturnBook.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridReturnBook.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridReturnBook.RowTemplate.Height = 35;

            dataGridReturnBook.EnableHeadersVisualStyles = false;
            dataGridReturnBook.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dataGridReturnBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridReturnBook.ColumnHeadersHeight = 38;

            dataGridReturnBook.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridReturnBook.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridReturnBook.ColumnHeadersDefaultCellStyle.Font = new Font("Inter", 9f, FontStyle.Bold);
            dataGridReturnBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridReturnBook.CellContentClick += new DataGridViewCellEventHandler(this.dataGridReturnBook_CellContentClick);
            dataGridReturnBook.CellValueChanged += new DataGridViewCellEventHandler(this.dataGridReturnBook_CellValueChanged);
            dataGridReturnBook.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.dataGridReturnBook_EditingControlShowing);

            this.btnReturnAll.Click += new System.EventHandler(this.btnReturnAll_Click);
        }

        private void SetupRincianDendaGrid()
        {
            DataGridRincianDenda.Columns.Clear();

            DataGridRincianDenda.Columns.Add("no", "No");
            DataGridRincianDenda.Columns["no"].Width = 40;
            DataGridRincianDenda.Columns["no"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridRincianDenda.Columns.Add("keterangan", "Keterangan");
            DataGridRincianDenda.Columns["keterangan"].Width = 250;

            DataGridRincianDenda.Columns.Add("jumlah", "Jumlah");
            DataGridRincianDenda.Columns["jumlah"].Width = 100;
            DataGridRincianDenda.Columns["jumlah"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            DataGridRincianDenda.Columns.Add("denda", "Denda (Rp)");
            DataGridRincianDenda.Columns["denda"].Width = 120;
            DataGridRincianDenda.Columns["denda"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            DataGridRincianDenda.Columns["denda"].DefaultCellStyle.Format = "N0";

            DataGridRincianDenda.ReadOnly = true;
            DataGridRincianDenda.AllowUserToAddRows = false;
            DataGridRincianDenda.AllowUserToDeleteRows = false;
            DataGridRincianDenda.AllowUserToOrderColumns = false;
            DataGridRincianDenda.AllowUserToResizeColumns = false;
            DataGridRincianDenda.AllowUserToResizeRows = false;
            DataGridRincianDenda.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridRincianDenda.RowHeadersVisible = false;
            DataGridRincianDenda.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridRincianDenda.BorderStyle = BorderStyle.None;
            DataGridRincianDenda.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DataGridRincianDenda.BackgroundColor = Color.White;

            DataGridRincianDenda.RowsDefaultCellStyle.BackColor = Color.White;
            DataGridRincianDenda.RowsDefaultCellStyle.ForeColor = Color.Black;
            DataGridRincianDenda.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);

            DataGridRincianDenda.DefaultCellStyle.Font = new Font("Inter", 9f, FontStyle.Regular);
            DataGridRincianDenda.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            DataGridRincianDenda.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridRincianDenda.RowTemplate.Height = 30;
            DataGridRincianDenda.EnableHeadersVisualStyles = false;
            DataGridRincianDenda.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            DataGridRincianDenda.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DataGridRincianDenda.ColumnHeadersHeight = 35;

            DataGridRincianDenda.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            DataGridRincianDenda.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            DataGridRincianDenda.ColumnHeadersDefaultCellStyle.Font = new Font("Inter", 9f, FontStyle.Bold);
            DataGridRincianDenda.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridRincianDenda.CellContentClick += new DataGridViewCellEventHandler(this.DataGridRincianDenda_CellContentClick);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (isFormResetting) return; 

            string kodePeminjaman = txtKode.Text.Trim();

            if (string.IsNullOrEmpty(kodePeminjaman))
            {
                if (!isFormResetting) 
                {
                    MessageBox.Show("Silakan masukkan kode peminjaman terlebih dahulu!",
                        "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKode.Focus();
                }
                return;
            }

            CariDataPeminjaman(kodePeminjaman);
        }

        private void CariDataPeminjaman(string kodePeminjaman)
        {
            MySqlConnection conn = null;

            try
            {
                conn = koneksi.GetConn();
                conn.Open();

                string query = @"
                    SELECT 
                        p.id_peminjaman,
                        p.kode_peminjaman,
                        p.tanggal_pinjam,
                        p.tanggal_jatuh_tempo,
                        p.status,
                        a.kode_anggota,
                        a.nama AS nama_anggota
                    FROM peminjaman p
                    INNER JOIN anggota a ON p.id_anggota = a.id_anggota
                    WHERE p.kode_peminjaman = @kode_peminjaman
                    AND p.status = 'dipinjam'";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kode_peminjaman", kodePeminjaman);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    idPeminjamanAktif = Convert.ToInt32(reader["id_peminjaman"]);

                    txtCode.Text = reader["kode_peminjaman"].ToString();
                    txtNamaAnggota.Text = reader["nama_anggota"].ToString();

                    DateTime tanggalPinjam = Convert.ToDateTime(reader["tanggal_pinjam"]);
                    txtPinjam.Text = tanggalPinjam.ToString("dd/MM/yyyy");

                    DateTime tanggalJatuhTempo = Convert.ToDateTime(reader["tanggal_jatuh_tempo"]);
                    txtTempo.Text = tanggalJatuhTempo.ToString("dd/MM/yyyy");

                    CekKeterlambatan(tanggalJatuhTempo);

                    reader.Close();

                    LoadDataBuku(idPeminjamanAktif);
                }
                else
                {
                    ClearFields();
                    lblConsequence.Visible = false;
                    dataGridReturnBook.Rows.Clear();
                    dataGridReturnBook.Columns.Clear();
                    lblTotalEksemplar.Text = "";
                    MessageBox.Show("Data peminjaman tidak ditemukan atau sudah dikembalikan!\n\n" +
                        "Pastikan kode peminjaman benar dan buku masih berstatus dipinjam.",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtKode.Focus();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mencari data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void LoadDataBuku(int idPeminjaman)
        {
            MySqlConnection conn = null;

            try
            {
                conn = koneksi.GetConn();
                conn.Open();

                string query = @"
            SELECT 
                dp.id_detail,
                b.kode_buku,
                b.judul,
                b.penulis,
                b.penerbit,
                b.tahun_terbit,
                dp.jumlah
            FROM detail_peminjaman dp
            INNER JOIN buku b ON dp.id_buku = b.id_buku
            WHERE dp.id_peminjaman = @id_peminjaman
            ORDER BY b.judul ASC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_peminjaman", idPeminjaman);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                SetupDataGridColumns();

                dataGridReturnBook.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dataGridReturnBook.Rows.Add();
                    DataGridViewRow dgvRow = dataGridReturnBook.Rows[rowIndex];

                    dgvRow.Cells["chkPilih"].Value = false;
                    dgvRow.Cells["id_detail"].Value = Convert.ToInt32(row["id_detail"]); 
                    dgvRow.Cells["kode_buku"].Value = row["kode_buku"].ToString();
                    dgvRow.Cells["judul"].Value = row["judul"].ToString();
                    dgvRow.Cells["penulis"].Value = row["penulis"].ToString();
                    dgvRow.Cells["penerbit"].Value = row["penerbit"].ToString();
                    dgvRow.Cells["tahun_terbit"].Value = row["tahun_terbit"].ToString();
                    dgvRow.Cells["jumlah_pinjam"].Value = Convert.ToInt32(row["jumlah"]); 
                    dgvRow.Cells["dikembalikan"].Value = 0; 
                }
                UpdateTotalEksemplar();
                HitungDanTampilkanDenda();

                btnReturnAll.Enabled = dataGridReturnBook.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat memuat data buku: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void SetupDataGridColumns()
        {
            dataGridReturnBook.Columns.Clear();

            DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
            chkColumn.Name = "chkPilih";
            chkColumn.HeaderText = "Pilih";
            chkColumn.Width = 50;
            chkColumn.ReadOnly = false;
            chkColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            chkColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            chkColumn.ThreeState = false; 
            dataGridReturnBook.Columns.Add(chkColumn);

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.Name = "id_detail";
            idColumn.HeaderText = "ID Detail";
            idColumn.Visible = false;
            idColumn.ValueType = typeof(int);
            dataGridReturnBook.Columns.Add(idColumn);

            dataGridReturnBook.Columns.Add("kode_buku", "Kode Buku");
            dataGridReturnBook.Columns.Add("judul", "Judul Buku");
            dataGridReturnBook.Columns.Add("penulis", "Penulis");
            dataGridReturnBook.Columns.Add("penerbit", "Penerbit");
            dataGridReturnBook.Columns.Add("tahun_terbit", "Tahun");

            DataGridViewTextBoxColumn jumlahPinjamColumn = new DataGridViewTextBoxColumn();
            jumlahPinjamColumn.Name = "jumlah_pinjam";
            jumlahPinjamColumn.HeaderText = "Jumlah Pinjam";
            jumlahPinjamColumn.ValueType = typeof(int); 
            dataGridReturnBook.Columns.Add(jumlahPinjamColumn);

            DataGridViewTextBoxColumn dikembalikanColumn = new DataGridViewTextBoxColumn();
            dikembalikanColumn.Name = "dikembalikan";
            dikembalikanColumn.HeaderText = "Dikembalikan";
            dikembalikanColumn.ReadOnly = false;
            dikembalikanColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dikembalikanColumn.Width = 90;
            dikembalikanColumn.ValueType = typeof(int);
            dikembalikanColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridReturnBook.Columns.Add(dikembalikanColumn);
        }

        private void UpdateTotalEksemplar()
        {
            int totalDikembalikan = 0;

            foreach (DataGridViewRow row in dataGridReturnBook.Rows)
            {
                if (row.Cells["chkPilih"].Value != null &&
                    Convert.ToBoolean(row.Cells["chkPilih"].Value) == true)
                {
                    if (row.Cells["dikembalikan"].Value != null)
                    {
                        try
                        {
                            totalDikembalikan += Convert.ToInt32(row.Cells["dikembalikan"].Value);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            lblTotalEksemplar.Text = $"Total Eksemplar Dikembalikan: {totalDikembalikan}";
        }

        private void HitungDanTampilkanDenda()
        {
            if (dataGridReturnBook.Rows.Count == 0 || idPeminjamanAktif == 0)
            {
                DataGridRincianDenda.Rows.Clear();
                txtTotalDenda.Text = "Rp 0";
                return;
            }

            DataGridRincianDenda.Rows.Clear();

            totalDendaKeterlambatan = 0;
            totalDendaBukuHilang = 0;
            int noUrut = 1;

            if (lblConsequence.Visible && lblConsequence.Text.Contains("Terlambat"))
            {
                DateTime hariIni = DateTime.Now.Date;

                DateTime tanggalJatuhTempo;
                if (DateTime.TryParseExact(txtTempo.Text, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out tanggalJatuhTempo))
                {
                    if (hariIni > tanggalJatuhTempo)
                    {
                        TimeSpan selisih = hariIni - tanggalJatuhTempo;
                        int hariTerlambat = selisih.Days;

                        totalDendaKeterlambatan = hariTerlambat * 2000;

                        DataGridRincianDenda.Rows.Add(
                            noUrut++,
                            $"Keterlambatan pengembalian ({hariTerlambat} hari x Rp 2.000)",
                            $"{hariTerlambat} hari",
                            totalDendaKeterlambatan
                        );
                    }
                }
            }
            else if (lblConsequence.Visible && lblConsequence.Text.Contains("Belum jatuh tempo"))
            {
            }

            foreach (DataGridViewRow row in dataGridReturnBook.Rows)
            {
                int jumlahPinjam = 0;
                int jumlahDikembalikan = 0;

                try
                {
                    jumlahPinjam = Convert.ToInt32(row.Cells["jumlah_pinjam"].Value);
                }
                catch { jumlahPinjam = 0; }

                if (row.Cells["chkPilih"].Value != null &&
                    Convert.ToBoolean(row.Cells["chkPilih"].Value) == true)
                {
                    try
                    {
                        jumlahDikembalikan = Convert.ToInt32(row.Cells["dikembalikan"].Value);
                    }
                    catch { jumlahDikembalikan = 0; }

                    int bukuHilang = jumlahPinjam - jumlahDikembalikan;

                    if (bukuHilang > 0)
                    {
                        int dendaPerBuku = 50000;
                        int totalDendaBuku = bukuHilang * dendaPerBuku;
                        totalDendaBukuHilang += totalDendaBuku;

                        DataGridRincianDenda.Rows.Add(
                            noUrut++,
                            $"Buku hilang/tidak kembali: {row.Cells["judul"].Value} ({bukuHilang} eks x Rp 50.000)",
                            $"{bukuHilang} buku",
                            totalDendaBuku
                        );
                    }
                }
                else
                {
                    if (jumlahPinjam > 0)
                    {
                        int dendaPerBuku = 50000;
                        int totalDendaBuku = jumlahPinjam * dendaPerBuku;
                        totalDendaBukuHilang += totalDendaBuku;

                        DataGridRincianDenda.Rows.Add(
                            noUrut++,
                            $"Buku tidak dikembalikan: {row.Cells["judul"].Value} ({jumlahPinjam} eks x Rp 50.000)",
                            $"{jumlahPinjam} buku",
                            totalDendaBuku
                        );
                    }
                }
            }

            int grandTotal = totalDendaKeterlambatan + totalDendaBukuHilang;

            txtTotalDenda.Text = $"Rp {grandTotal:N0}";

            if (DataGridRincianDenda.Rows.Count > 0)
            {
                int separatorIndex = DataGridRincianDenda.Rows.Add();
                DataGridRincianDenda.Rows[separatorIndex].Cells["no"].Value = "";
                DataGridRincianDenda.Rows[separatorIndex].Cells["keterangan"].Value = "──────────────────────────────";
                DataGridRincianDenda.Rows[separatorIndex].Cells["jumlah"].Value = "";
                DataGridRincianDenda.Rows[separatorIndex].Cells["denda"].Value = "";
                DataGridRincianDenda.Rows[separatorIndex].DefaultCellStyle.BackColor = Color.LightGray;
                DataGridRincianDenda.Rows[separatorIndex].ReadOnly = true;

                int totalIndex = DataGridRincianDenda.Rows.Add();
                DataGridRincianDenda.Rows[totalIndex].Cells["no"].Value = "";
                DataGridRincianDenda.Rows[totalIndex].Cells["keterangan"].Value = "TOTAL DENDA";
                DataGridRincianDenda.Rows[totalIndex].Cells["jumlah"].Value = "";
                DataGridRincianDenda.Rows[totalIndex].Cells["denda"].Value = grandTotal;
                DataGridRincianDenda.Rows[totalIndex].DefaultCellStyle.Font = new Font("Inter", 9.5f, FontStyle.Bold);
                DataGridRincianDenda.Rows[totalIndex].DefaultCellStyle.ForeColor = Color.Red;
                DataGridRincianDenda.Rows[totalIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240);
                DataGridRincianDenda.Rows[totalIndex].ReadOnly = true;
            }
        }

        private void CekKeterlambatan(DateTime tanggalJatuhTempo)
        {
            DateTime hariIni = DateTime.Now.Date;

            if (hariIni > tanggalJatuhTempo)
            {
                TimeSpan selisih = hariIni - tanggalJatuhTempo;
                int hariTerlambat = selisih.Days;

                int denda = hariTerlambat * 2000;

                lblConsequence.Text = $"Terlambat : ⚠️ {hariTerlambat} hari (Denda Rp {denda:N0})";
                lblConsequence.ForeColor = Color.Red;
                lblConsequence.Visible = true;
            }
            else
            {
                lblConsequence.Text = "✅ Belum jatuh tempo (Tidak ada denda)";
                lblConsequence.ForeColor = Color.Green;
                lblConsequence.Visible = true;
            }
        }

        private void ClearFields()
        {
            isFormResetting = true; 

            try
            {
                txtKode.Clear();
                txtCode.Clear();
                txtNamaAnggota.Clear();
                txtPinjam.Clear();
                txtTempo.Clear();
                dataGridReturnBook.Rows.Clear();
                dataGridReturnBook.Columns.Clear();
                lblTotalEksemplar.Text = "";
                idPeminjamanAktif = 0;
                DataGridRincianDenda.Rows.Clear();
                txtTotalDenda.Text = "";
                radioLunas.Checked = false;
                radioBelumLunas.Checked = false;
                btnSave.Enabled = false;
                lblConsequence.Visible = false; 
            }
            finally
            {
                isFormResetting = false; 
                txtKode.Focus(); 
            }
        }

        private void dataGridReturnBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 &&
                dataGridReturnBook.Columns[e.ColumnIndex].Name == "chkPilih" &&
                e.RowIndex >= 0)
            {
                dataGridReturnBook.CommitEdit(DataGridViewDataErrorContexts.Commit);

                DataGridViewRow row = dataGridReturnBook.Rows[e.RowIndex];
                bool isChecked = Convert.ToBoolean(row.Cells["chkPilih"].Value);

                if (isChecked)
                {
                    row.Cells["dikembalikan"].Value = Convert.ToInt32(row.Cells["jumlah_pinjam"].Value);
                }
                else
                {
                    row.Cells["dikembalikan"].Value = 0; 
                }

                UpdateTotalEksemplar();
                HitungDanTampilkanDenda();
            }
        }



        private void dataGridReturnBook_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridReturnBook.Columns[e.ColumnIndex].Name == "dikembalikan")
                {
                    DataGridViewRow row = dataGridReturnBook.Rows[e.RowIndex];
                    int nilai = 0;

                    if (row.Cells["dikembalikan"].Value != null)
                    {
                        try
                        {
                            nilai = Convert.ToInt32(row.Cells["dikembalikan"].Value);
                        }
                        catch
                        {
                            nilai = 0;
                            row.Cells["dikembalikan"].Value = 0;
                        }
                    }

                    int maxValue = Convert.ToInt32(row.Cells["jumlah_pinjam"].Value);

                    if (nilai > maxValue)
                    {
                        MessageBox.Show($"Jumlah dikembalikan tidak boleh melebihi jumlah pinjam ({maxValue})!",
                            "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        row.Cells["dikembalikan"].Value = maxValue;
                    }
                    else if (nilai < 0)
                    {
                        row.Cells["dikembalikan"].Value = 0;
                    }

                    if (Convert.ToInt32(row.Cells["dikembalikan"].Value) > 0)
                    {
                        row.Cells["chkPilih"].Value = true;
                    }
                    else
                    {
                        row.Cells["chkPilih"].Value = false;
                    }

                    UpdateTotalEksemplar();
                    HitungDanTampilkanDenda();
                }
            }
        }

        private void dataGridReturnBook_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dataGridReturnBook.CurrentCell.ColumnIndex >= 0 &&
                dataGridReturnBook.Columns[dataGridReturnBook.CurrentCell.ColumnIndex].Name == "dikembalikan")
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress -= new KeyPressEventHandler(NumericTextBox_KeyPress);
                    tb.KeyPress += new KeyPressEventHandler(NumericTextBox_KeyPress);
                }
            }
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void btnReturnAll_Click(object sender, EventArgs e)
        {
            if (dataGridReturnBook.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data buku untuk dikembalikan!",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Apakah Anda yakin ingin mencentang semua buku?\n" +
                "Semua buku akan dianggap dikembalikan dengan jumlah sesuai pinjaman.",
                "Konfirmasi Return All",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridReturnBook.Rows)
                {
                    row.Cells["chkPilih"].Value = true;
                    row.Cells["dikembalikan"].Value = Convert.ToInt32(row.Cells["jumlah_pinjam"].Value);
                }

                UpdateTotalEksemplar();
                HitungDanTampilkanDenda();

                MessageBox.Show("Semua buku telah dicentang untuk dikembalikan.",
                    "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void radioBelumLunas_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBelumLunas.Checked)
            {
                btnSave.Enabled = true;
                btnSave.Text = "Simpan Pengembalian (Denda Belum Lunas)";
            }
        }

        private void radioLunas_CheckedChanged(object sender, EventArgs e)
        {
            if (radioLunas.Checked)
            {
                btnSave.Enabled = true;
                btnSave.Text = "Simpan Pengembalian (Denda Lunas)";
            }
        }

        private void DataGridRincianDenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (dataGridReturnBook.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data buku untuk diproses!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!radioLunas.Checked && !radioBelumLunas.Checked)
            {
                MessageBox.Show("Silakan pilih status pembayaran denda (Lunas/Belum Lunas)!",
                    "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string statusDenda = radioLunas.Checked ? "LUNAS" : "BELUM LUNAS";
            string pesanKonfirmasi = $"Apakah Anda yakin ingin menyimpan data pengembalian ini?\n\n" +
                                     $"Kode Peminjaman: {txtCode.Text}\n" +
                                     $"Nama Anggota: {txtNamaAnggota.Text}\n" +
                                     $"Total Denda: {txtTotalDenda.Text}\n" +
                                     $"Status Denda: {statusDenda}\n\n" +
                                     $"Lanjutkan?";

            DialogResult result = MessageBox.Show(pesanKonfirmasi,
                "Konfirmasi Pengembalian",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SimpanPengembalian();
            }
        }

        private void SimpanPengembalian()
        {
            MySqlConnection conn = null;
            MySqlTransaction transaction = null;

            try
            {
                conn = koneksi.GetConn();
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. Update status peminjaman menjadi 'dikembalikan'
                string updatePeminjaman = @"
            UPDATE peminjaman 
            SET status = 'dikembalikan' 
            WHERE id_peminjaman = @id_peminjaman";

                MySqlCommand cmdPeminjaman = new MySqlCommand(updatePeminjaman, conn, transaction);
                cmdPeminjaman.Parameters.AddWithValue("@id_peminjaman", idPeminjamanAktif);
                cmdPeminjaman.ExecuteNonQuery();

                // ============================================================
                // TAMBAHKAN BAGIAN INI: 2. Insert ke tabel pengembalian
                // ============================================================

                // Hitung total denda dari txtTotalDenda
                decimal totalDenda = 0;
                string dendaText = txtTotalDenda.Text.Replace("Rp ", "").Replace(".", "").Replace(",", "");
                decimal.TryParse(dendaText, out totalDenda);

                // Hitung hari terlambat
                int hariTerlambat = 0;
                if (lblConsequence.Visible && lblConsequence.Text.Contains("Terlambat"))
                {
                    DateTime hariIni = DateTime.Now.Date;
                    DateTime tanggalJatuhTempo;
                    if (DateTime.TryParseExact(txtTempo.Text, "dd/MM/yyyy", null,
                        System.Globalization.DateTimeStyles.None, out tanggalJatuhTempo))
                    {
                        if (hariIni > tanggalJatuhTempo)
                        {
                            TimeSpan selisih = hariIni - tanggalJatuhTempo;
                            hariTerlambat = selisih.Days;
                        }
                    }
                }

                // Tentukan status denda
                string statusDendaValue;
                if (totalDenda == 0)
                    statusDendaValue = "tidak_ada";
                else if (radioLunas.Checked)
                    statusDendaValue = "lunas";
                else
                    statusDendaValue = "belum_bayar";

                // Query insert pengembalian
                string insertPengembalian = @"
            INSERT INTO pengembalian 
            (id_peminjaman, tanggal_kembali, terlambat, total_denda, status_denda, id_user) 
            VALUES 
            (@id_peminjaman, @tanggal_kembali, @terlambat, @total_denda, @status_denda, @id_user)";

                MySqlCommand cmdPengembalian = new MySqlCommand(insertPengembalian, conn, transaction);
                cmdPengembalian.Parameters.AddWithValue("@id_peminjaman", idPeminjamanAktif);
                cmdPengembalian.Parameters.AddWithValue("@tanggal_kembali", DateTime.Now.Date);
                cmdPengembalian.Parameters.AddWithValue("@terlambat", hariTerlambat);
                cmdPengembalian.Parameters.AddWithValue("@total_denda", totalDenda);
                cmdPengembalian.Parameters.AddWithValue("@status_denda", statusDendaValue);
                cmdPengembalian.Parameters.AddWithValue("@id_user", Program.UserId);
                cmdPengembalian.ExecuteNonQuery();

                // ============================================================
                // AKHIR TAMBAHAN
                // ============================================================

                // 3. Update stok buku yang dikembalikan
                foreach (DataGridViewRow row in dataGridReturnBook.Rows)
                {
                    if (row.Cells["chkPilih"].Value != null &&
                        Convert.ToBoolean(row.Cells["chkPilih"].Value) == true)
                    {
                        int jumlahDikembalikan = Convert.ToInt32(row.Cells["dikembalikan"].Value);
                        string kodeBuku = row.Cells["kode_buku"].Value.ToString();

                        if (jumlahDikembalikan > 0)
                        {
                            string updateStok = @"
                        UPDATE buku 
                        SET stok = stok + @jumlah 
                        WHERE kode_buku = @kode_buku";

                            MySqlCommand cmdStok = new MySqlCommand(updateStok, conn, transaction);
                            cmdStok.Parameters.AddWithValue("@jumlah", jumlahDikembalikan);
                            cmdStok.Parameters.AddWithValue("@kode_buku", kodeBuku);
                            cmdStok.ExecuteNonQuery();
                        }
                    }
                }

                // Commit transaction
                transaction.Commit();

                // Tampilkan pesan sukses
                string statusDenda = radioLunas.Checked ? "LUNAS" : "BELUM LUNAS";
                string pesanSukses = $"✅ Pengembalian berhasil disimpan!\n\n" +
                                    $"Kode: {txtCode.Text}\n" +
                                    $"Anggota: {txtNamaAnggota.Text}\n" +
                                    $"Total Denda: {txtTotalDenda.Text}\n" +
                                    $"Status: {statusDenda}";

                MessageBox.Show(pesanSukses, "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetFormBatal();

            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                MessageBox.Show("Error saat menyimpan data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void ResetFormBatal()
        {
            isFormResetting = true;

            txtKode.Clear();
            txtCode.Clear();
            txtNamaAnggota.Clear();
            txtPinjam.Clear();
            txtTempo.Clear();
            dataGridReturnBook.Rows.Clear();
            dataGridReturnBook.Columns.Clear();
            lblTotalEksemplar.Text = "";
            idPeminjamanAktif = 0;
            DataGridRincianDenda.Rows.Clear();
            txtTotalDenda.Text = "";
            radioLunas.Checked = false;
            radioBelumLunas.Checked = false;
            btnSave.Enabled = false;
            lblConsequence.Visible = false;

            isFormResetting = false;
            txtKode.Focus();
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            ResetFormBatal();
        }

        private void txtKode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch_Click(sender, e);
                e.Handled = true;
            }
        }

        private void txtKode_TextChanged(object sender, EventArgs e) { }
        private void txtCode_TextChanged(object sender, EventArgs e) { }
        private void txtPinjam_TextChanged(object sender, EventArgs e) { }
        private void txtNamaAnggota_TextChanged(object sender, EventArgs e) { }
        private void txtTempo_TextChanged(object sender, EventArgs e) { }
        private void lblConsequence_Click(object sender, EventArgs e) { }
        private void lblTotalEksemplar_Click(object sender, EventArgs e) { }
        private void radioAdmin_CheckedChanged(object sender, EventArgs e) { }
        private void txtTotalDenda_TextChanged(object sender, EventArgs e) { }

    }
}