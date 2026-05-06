using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace Aplikasi_perpustakaan
{
    public partial class FormBooks : Form
    {
        private BookService _bookService;
        private bool _isGenerating = false;
        private string _selectedImagePath = "";
        private bool _isEditMode = false;
        private int _editBookId = 0;
        private Koneksi kon = new Koneksi();
        private bool _isLoadingData = false;

        public FormBooks()
        {
            InitializeComponent();
            _bookService = new BookService();
            _isEditMode = false;

            toolTip1 = new ToolTip();
            toolTip1.ToolTipTitle = "Info Cover";
            toolTip1.ShowAlways = true;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 200;

            SetupPictureBoxDragDrop();
            LoadComboBoxData();

            SetupCreateMode();
        }

        public FormBooks(int bookId) : this()
        {
            _isEditMode = true;
            _editBookId = bookId;
            SetupEditMode();
        }

        private void SetupCreateMode()
        {
            this.Text = "Tambah Buku Baru";
            lblTitle.Text = "Tambah Buku Baru";
            btnSave.Text = "Simpan Buku";
        }

        // SETUP MODE EDIT
        private void SetupEditMode()
        {
            this.Text = "Edit Buku";
            lblTitle.Text = "Edit Buku";
            btnSave.Text = "Update Buku";

            LoadBookData(_editBookId);
        }

        private void LoadBookData(int bookId)
        {
            _isLoadingData = true; 

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();

                    string query = @"SELECT * FROM buku WHERE id_buku = @id_buku";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_buku", bookId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtKode.Text = reader["kode_buku"].ToString();
                            txtBarcode.Text = reader["barcode"]?.ToString() ?? "";
                            txtJudul.Text = reader["judul"].ToString();
                            txtPenulis.Text = reader["penulis"].ToString();
                            txtPenerbit.Text = reader["penerbit"].ToString();

                            string kategori = reader["kategori"].ToString();
                            if (!string.IsNullOrEmpty(kategori) && comboKategori.Items.Contains(kategori))
                            {
                                comboKategori.SelectedItem = kategori;
                            }

                            string tahun = reader["tahun_terbit"].ToString();
                            if (!string.IsNullOrEmpty(tahun))
                            {
                                int tahunIndex = comboTahunTerbit.FindStringExact(tahun);
                                if (tahunIndex >= 0)
                                    comboTahunTerbit.SelectedIndex = tahunIndex;
                            }

                            if (int.TryParse(reader["stok"].ToString(), out int stok))
                            {
                                txtStok.Text = stok.ToString();
                            }

                            string lokasiRak = reader["lokasi_rak"].ToString();
                            if (!string.IsNullOrEmpty(lokasiRak))
                            {
                                int rakIndex = comboLokasiRak.FindStringExact(lokasiRak);
                                if (rakIndex >= 0)
                                    comboLokasiRak.SelectedIndex = rakIndex;
                            }

                            string coverPath = reader["cover_buku"].ToString();
                            if (!string.IsNullOrEmpty(coverPath) && File.Exists(coverPath))
                            {
                                _selectedImagePath = coverPath;
                                using (var stream = new FileStream(coverPath, FileMode.Open, FileAccess.Read))
                                {
                                    pictureBox.Image = Image.FromStream(stream);
                                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                                pictureBox.BorderStyle = BorderStyle.FixedSingle;
                                toolTip1.SetToolTip(pictureBox, $"Cover: {Path.GetFileName(coverPath)}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data buku: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingData = false; 
            }
        }


        private void LoadComboBoxData()
        {
            LoadKategoriData();
            LoadTahunTerbitData();
            LoadLokasiRakData();
        }

        private void LoadKategoriData()
        {
            comboKategori.Items.Clear();

            comboKategori.Items.Add("-- Pilih Kategori --");

            string[] kategoriList = new string[]
            {
                "Pendidikan",
                "Novel",
                "Komik",
                "Teknologi & Komputer",
                "Sains & Matematika",
                "Sejarah",
                "Agama & Spiritual",
                "Bisnis & Ekonomi",
                "Kesehatan",
                "Hukum",
                "Filsafat",
                "Bahasa",
                "Seni & Desain",
                "Biografi",
                "Ensiklopedia",
                "Kamus",
                "Majalah",
                "Jurnal",
                "Fiksi Ilmiah",
                "Fantasi",
                "Horor",
                "Romantis",
                "Petualangan",
                "Anak-anak",
                "Lainnya"
            };

            comboKategori.Items.AddRange(kategoriList);

            comboKategori.SelectedIndex = 0;

            comboKategori.DropDownStyle = ComboBoxStyle.DropDownList; 
        }

        private void LoadTahunTerbitData()
        {
            comboTahunTerbit.Items.Clear();

            comboTahunTerbit.Items.Add("-- Pilih Tahun --");

            int tahunSekarang = DateTime.Now.Year;
            int tahunMinimal = 1777;

            for (int tahun = tahunSekarang; tahun >= tahunMinimal; tahun--)
            {
                comboTahunTerbit.Items.Add(tahun.ToString());
            }

            comboTahunTerbit.SelectedIndex = 0;

            comboTahunTerbit.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadLokasiRakData()
        {
            comboLokasiRak.Items.Clear();

            comboLokasiRak.Items.Add("-- Pilih Lokasi Rak --");

            string[] rakLetters = new string[] { "A", "B", "C", "D", "E" };

            foreach (string letter in rakLetters)
            {
                for (int i = 1; i <= 5; i++)
                {
                    comboLokasiRak.Items.Add($"Rak {letter}-{i}");
                }
            }

            string[] lokasiTambahan = new string[]
            {
                "Lemari 1",
                "Lemari 2",
                "Lemari 3",
                "Gudang Arsip",
                "Ruang Baca"
            };
            comboLokasiRak.Items.AddRange(lokasiTambahan);

            comboLokasiRak.SelectedIndex = 0;

            comboLokasiRak.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboKategori.SelectedIndex <= 0)
                return;

            string selectedKategori = comboKategori.SelectedItem.ToString();
            Console.WriteLine($"Kategori dipilih: {selectedKategori}");
        }

        private void comboTahunTerbit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboTahunTerbit.SelectedIndex <= 0)
                return;
            string selectedTahun = comboTahunTerbit.SelectedItem.ToString();
            Console.WriteLine($"Tahun dipilih: {selectedTahun}");
        }

        private void comboLokasiRak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboLokasiRak.SelectedIndex <= 0)
                return;

            string selectedLokasi = comboLokasiRak.SelectedItem.ToString();
            Console.WriteLine($"Lokasi dipilih: {selectedLokasi}");
        }

        private void SetupPictureBoxDragDrop()
        {
            pictureBox.AllowDrop = true;

            pictureBox.DragEnter += PictureBox_DragEnter;

            pictureBox.DragDrop += PictureBox_DragDrop;
        }

        // EVENT: DRAG ENTER PICTUREBOX
        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                string extension = Path.GetExtension(files[0]).ToLower();
                if (extension == ".jpg" || extension == ".jpeg" ||
                    extension == ".png" || extension == ".bmp" || extension == ".gif")
                {
                    e.Effect = DragDropEffects.Copy; 
                }
                else
                {
                    e.Effect = DragDropEffects.None; 
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // EVENT: DRAG DROP PICTUREBOX
        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];

                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length > 2 * 1024 * 1024) 
                    {
                        MessageBox.Show("Ukuran file maksimal 2MB!",
                            "File Terlalu Besar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    LoadImageToPictureBox(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat gambar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // BUTTON: UPLOAD GAMBAR
        private void btnUploadGambar_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "File Gambar|*.jpg;*.jpeg;*.png;*.bmp;*.gif|" +
                                            "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                                            "PNG (*.png)|*.png|" +
                                            "BMP (*.bmp)|*.bmp|" +
                                            "GIF (*.gif)|*.gif";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.Title = "Pilih Gambar Cover Buku";
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length > 2 * 1024 * 1024) // 2MB
                        {
                            MessageBox.Show("Ukuran file maksimal 2MB!",
                                "File Terlalu Besar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        LoadImageToPictureBox(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal upload gambar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadImageToPictureBox(string filePath)
        {
            try
            {
                _selectedImagePath = filePath;

                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    pictureBox.Image = Image.FromStream(stream);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom; 
                }

                pictureBox.BorderStyle = BorderStyle.FixedSingle;

                toolTip1.SetToolTip(pictureBox, $"Cover: {Path.GetFileName(filePath)}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Gagal memuat gambar: {ex.Message}");
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Apakah Anda ingin menghapus gambar cover?",
                "Hapus Cover",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearPictureBox();
            }
        }

        private void ClearPictureBox()
        {
            pictureBox.Image = null;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            _selectedImagePath = "";
            toolTip1.SetToolTip(pictureBox, "Drag & Drop gambar di sini atau klik Upload");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void ClearAllFields()
        {
            foreach (Control control in this.Controls)
            {
                ClearControl(control);
            }

            txtKode.Focus();
        }

        private void ClearControl(Control control)
        {
            // Clear TextBox
            if (control is TextBox txt)
            {
                txt.Clear();
            }
            else if (control is ComboBox cmb)
            {
                cmb.SelectedIndex = 0;
            }
            else if (control is NumericUpDown nud)
            {
                nud.Value = nud.Minimum;
            }
            else if (control is PictureBox pic)
            {
                pic.Image = null;
                pic.BorderStyle = BorderStyle.FixedSingle;
                _selectedImagePath = "";
            }
            else if (control is RichTextBox rtb)
            {
                rtb.Clear();
            }
            else if (control.HasChildren)
            {
                foreach (Control childControl in control.Controls)
                {
                    ClearControl(childControl);
                }
            }
        }

        private void txtKode_TextChanged(object sender, EventArgs e)
        {
            if (_isLoadingData || _isGenerating || string.IsNullOrWhiteSpace(txtKode.Text))
                return;

            CekKodeDiDatabase();
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            if (_isLoadingData || _isGenerating || string.IsNullOrWhiteSpace(txtBarcode.Text))
                return;

            CekBarcodeDiDatabase();
        }

        // BUTTON: GENERATE KODE
        private void btnGenerateKode_Click(object sender, EventArgs e)
        {
            try
            {
                _isGenerating = true;
                Cursor = Cursors.WaitCursor;

                string kodeBaru = _bookService.GenerateBookCode();
                txtKode.Text = kodeBaru;

                TampilkanStatusGenerate(btnGenerateKode, "✓ Kode tergenerate!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal generate kode: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _isGenerating = false;
            }
        }

        // BUTTON: GENERATE BARCODE
        private void btnGenerateBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                _isGenerating = true;
                Cursor = Cursors.WaitCursor;

                string barcodeBaru = _bookService.GenerateBarcode();
                txtBarcode.Text = barcodeBaru;

                TampilkanStatusGenerate(btnGenerateBarcode, "✓ Barcode tergenerate!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal generate barcode: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                _isGenerating = false;
            }
        }

        private async void TampilkanStatusGenerate(Control button, string pesan)
        {
            string originalText = button.Text;
            button.Text = pesan;
            await Task.Delay(1500);
            button.Text = originalText;
        }

        private void CekKodeDiDatabase()
        {
            try
            {
                string kode = txtKode.Text.Trim();

                if (_isEditMode)
                {
                    string originalKode = GetOriginalKodeBuku();
                    if (kode == originalKode)
                        return;
                }

                if (_bookService.IsCodeExists(kode))
                {
                    DialogResult result = MessageBox.Show(
                        $"Kode '{kode}' sudah digunakan oleh buku lain!\n\nKlik Yes untuk generate kode baru otomatis.",
                        "Kode Duplikat",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        _isGenerating = true;
                        string kodeBaru = _bookService.GenerateBookCode();
                        txtKode.Text = kodeBaru;
                        _isGenerating = false;
                        TampilkanStatusGenerate(btnGenerateKode, "✓ Kode baru dibuat!");
                    }
                    else
                    {
                        txtKode.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cek kode: {ex.Message}");
            }
        }

        private void CekBarcodeDiDatabase()
        {
            try
            {
                string barcode = txtBarcode.Text.Trim();
                if (_isEditMode)
                {
                    string originalBarcode = GetOriginalBarcode();
                    if (barcode == originalBarcode)
                        return;
                }

                if (_bookService.IsBarcodeExists(barcode))
                {
                    DialogResult result = MessageBox.Show(
                        $"Barcode '{barcode}' sudah digunakan oleh buku lain!\n\nKlik Yes untuk generate barcode baru otomatis.",
                        "Barcode Duplikat",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        _isGenerating = true;
                        string barcodeBaru = _bookService.GenerateBarcode();
                        txtBarcode.Text = barcodeBaru;
                        _isGenerating = false;
                        TampilkanStatusGenerate(btnGenerateBarcode, "✓ Barcode baru dibuat!");
                    }
                    else
                    {
                        txtBarcode.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cek barcode: {ex.Message}");
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }

        private void guna2HtmlLabel6_Click(object sender, EventArgs e) { }

        private void guna2Button4_Click(object sender, EventArgs e) { }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPenulis_TextChanged(object sender, EventArgs e) { }

        private void btnPenerbit_TextChanged(object sender, EventArgs e) { }

        private void txtStok_TextChanged(object sender, EventArgs e) { }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                DialogResult confirmResult = MessageBox.Show(
                    _isEditMode ? "Apakah Anda yakin ingin mengupdate buku ini?" : "Apakah Anda yakin ingin menyimpan buku ini?",
                    _isEditMode ? "Konfirmasi Update" : "Konfirmasi Simpan",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;

                string savedCoverPath = "";
                if (!string.IsNullOrEmpty(_selectedImagePath) && File.Exists(_selectedImagePath))
                {
                    if (_selectedImagePath.Contains("Covers"))
                    {
                        savedCoverPath = _selectedImagePath;
                    }
                    else
                    {
                        savedCoverPath = _bookService.SaveCoverImage(_selectedImagePath);
                    }
                }

                string kodeBuku = txtKode.Text.Trim();
                string barcode = txtBarcode.Text.Trim();
                string judul = txtJudul.Text.Trim();
                string penulis = txtPenulis.Text.Trim();
                string penerbit = txtPenerbit.Text.Trim();
                string tahunTerbit = comboTahunTerbit.SelectedIndex > 0 ? comboTahunTerbit.SelectedItem.ToString() : "";
                string kategori = comboKategori.SelectedIndex > 0 ? comboKategori.SelectedItem.ToString() : "";
                int stok = 0;
                int.TryParse(txtStok.Text.Trim(), out stok);
                string lokasiRak = comboLokasiRak.SelectedIndex > 0 ? comboLokasiRak.SelectedItem.ToString() : "";

                bool success;

                if (_isEditMode)
                {
                    success = UpdateBook(_editBookId, kodeBuku, barcode, judul, penulis, penerbit,
                        tahunTerbit, kategori, stok, lokasiRak, savedCoverPath);
                }
                else
                {
                    success = _bookService.SaveBook(kodeBuku, barcode, judul, penulis, penerbit,
                        tahunTerbit, kategori, stok, lokasiRak, savedCoverPath);
                }

                if (success)
                {
                    MessageBox.Show(
                        _isEditMode ? $"Buku '{judul}' berhasil diupdate!" : $"Buku '{judul}' berhasil disimpan!",
                        "✅ Berhasil",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gagal menyimpan buku. Silakan coba lagi.",
                        "❌ Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan:\n\n{ex.Message}",
                    "❌ Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // UPDATE BUKU
        private bool UpdateBook(int idBuku, string kodeBuku, string barcode, string judul,
            string penulis, string penerbit, string tahunTerbit, string kategori,
            int stok, string lokasiRak, string coverPath)
        {
            using (MySqlConnection conn = kon.GetConn())
            {
                try
                {
                    conn.Open();

                    string query = @"UPDATE buku SET 
                        kode_buku = @kode_buku,
                        barcode = @barcode,
                        judul = @judul,
                        penulis = @penulis,
                        penerbit = @penerbit,
                        tahun_terbit = @tahun_terbit,
                        kategori = @kategori,
                        stok = @stok,
                        lokasi_rak = @lokasi_rak,
                        cover_buku = @cover_buku
                    WHERE id_buku = @id_buku";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kode_buku", kodeBuku);
                    cmd.Parameters.AddWithValue("@barcode", string.IsNullOrEmpty(barcode) ? DBNull.Value : (object)barcode);
                    cmd.Parameters.AddWithValue("@judul", judul);
                    cmd.Parameters.AddWithValue("@penulis", penulis);
                    cmd.Parameters.AddWithValue("@penerbit", penerbit);
                    cmd.Parameters.AddWithValue("@tahun_terbit", tahunTerbit);
                    cmd.Parameters.AddWithValue("@kategori", kategori);
                    cmd.Parameters.AddWithValue("@stok", stok);
                    cmd.Parameters.AddWithValue("@lokasi_rak", string.IsNullOrEmpty(lokasiRak) ? DBNull.Value : (object)lokasiRak);
                    cmd.Parameters.AddWithValue("@cover_buku", string.IsNullOrEmpty(coverPath) ? DBNull.Value : (object)coverPath);
                    cmd.Parameters.AddWithValue("@id_buku", idBuku);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error updating book: {ex.Message}");
                }
            }
        }

        // VALIDASI FORM SEBELUM SIMPAN (DIPERBAIKI)
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtKode.Text))
            {
                MessageBox.Show("Kode buku wajib diisi!\n\nKlik tombol Generate untuk membuat kode otomatis.",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtJudul.Text))
            {
                MessageBox.Show("Judul buku wajib diisi!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtJudul.Focus();
                return false;
            }
            if (txtJudul.Text.Trim().Length < 3)
            {
                MessageBox.Show("Judul buku minimal 3 karakter!",
                    "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtJudul.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPenulis.Text))
            {
                MessageBox.Show("Nama penulis wajib diisi!",
                    " Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPenulis.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPenerbit.Text))
            {
                MessageBox.Show("Nama penerbit wajib diisi!",
                    " Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPenerbit.Focus();
                return false;
            }

            if (comboKategori.SelectedIndex <= 0)
            {
                MessageBox.Show("Pilih kategori buku!",
                    " Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboKategori.Focus();
                return false;
            }

            if (comboTahunTerbit.SelectedIndex <= 0)
            {
                MessageBox.Show("Pilih tahun terbit buku!",
                    " Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboTahunTerbit.Focus();
                return false;
            }

            int stokValue;
            if (!int.TryParse(txtStok.Text.Trim(), out stokValue))
            {
                MessageBox.Show("Stok harus berupa angka!\n\nContoh: 5, 10, 100",
                    " Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStok.Focus();
                return false;
            }

            if (stokValue < 0)
            {
                MessageBox.Show("Stok tidak boleh kurang dari 0!",
                    " Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStok.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtKode.Text))
            {
                string originalKode = GetOriginalKodeBuku();

                if (txtKode.Text.Trim() != originalKode)
                {
                    if (_bookService.IsCodeExists(txtKode.Text.Trim()))
                    {
                        DialogResult result = MessageBox.Show(
                            $"Kode '{txtKode.Text}' sudah digunakan oleh buku lain!\n\nKlik OK untuk generate kode baru.",
                            " Kode Duplikat",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.OK)
                        {
                            _isGenerating = true;
                            txtKode.Text = _bookService.GenerateBookCode();
                            _isGenerating = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                string originalBarcode = GetOriginalBarcode();

                if (txtBarcode.Text.Trim() != originalBarcode)
                {
                    if (_bookService.IsBarcodeExists(txtBarcode.Text.Trim()))
                    {
                        DialogResult result = MessageBox.Show(
                            $"Barcode '{txtBarcode.Text}' sudah digunakan oleh buku lain!\n\nKlik OK untuk generate barcode baru.",
                            " Barcode Duplikat",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.OK)
                        {
                            _isGenerating = true;
                            txtBarcode.Text = _bookService.GenerateBarcode();
                            _isGenerating = false;
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private string GetOriginalKodeBuku()
        {
            if (!_isEditMode || _editBookId == 0)
                return ""; 

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();
                    string query = "SELECT kode_buku FROM buku WHERE id_buku = @id_buku";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_buku", _editBookId);
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
            catch
            {
                return "";
            }
        }

        private string GetOriginalBarcode()
        {
            if (!_isEditMode || _editBookId == 0)
                return ""; 

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    conn.Open();
                    string query = "SELECT barcode FROM buku WHERE id_buku = @id_buku";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_buku", _editBookId);
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "";
                }
            }
            catch
            {
                return "";
            }
        }
    }
}