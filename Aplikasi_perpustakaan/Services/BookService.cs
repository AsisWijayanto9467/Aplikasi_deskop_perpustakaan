using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace Aplikasi_perpustakaan
{
    class BookService
    {
        private Koneksi _koneksi;

        public BookService()
        {
            _koneksi = new Koneksi();
        }

        // ========================================
        // GENERATE KODE BUKU (B-001, B-002, ...)
        // ========================================
        public string GenerateBookCode()
        {
            string newCode = "B-001"; // Default jika tabel kosong

            using (MySqlConnection conn = _koneksi.GetConn())
            {
                try
                {
                    conn.Open();

                    // Ambil kode_buku terakhir yang berawalan "B-"
                    string query = "SELECT kode_buku FROM buku WHERE kode_buku LIKE 'B-%' ORDER BY id_buku DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string lastCode = result.ToString(); // Contoh: "B-005"
                            string numberPart = lastCode.Replace("B-", "");

                            if (int.TryParse(numberPart, out int lastNumber))
                            {
                                int nextNumber = lastNumber + 1;
                                newCode = $"B-{nextNumber:D3}"; // Format 3 digit: B-006
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error generate kode buku: {ex.Message}");
                }
            }

            return newCode;
        }

        // ========================================
        // GENERATE BARCODE UNIK (Format: 978-XXXXXX-XXX-X)
        // ========================================
        public string GenerateBarcode()
        {
            string newBarcode = "";
            bool isUnique = false;
            int maxAttempts = 100; // Maksimal percobaan generate
            int attempt = 0;

            using (MySqlConnection conn = _koneksi.GetConn())
            {
                try
                {
                    conn.Open();

                    while (!isUnique && attempt < maxAttempts)
                    {
                        // Generate barcode random
                        newBarcode = GenerateRandomBarcode();

                        // Cek apakah barcode sudah ada di database
                        string checkQuery = "SELECT COUNT(*) FROM buku WHERE barcode = @barcode";
                        using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@barcode", newBarcode);
                            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                            if (count == 0)
                            {
                                isUnique = true;
                            }
                        }

                        attempt++;
                    }

                    if (!isUnique)
                    {
                        throw new Exception("Gagal generate barcode unik setelah 100 percobaan");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error generate barcode: {ex.Message}");
                }
            }

            return newBarcode;
        }

        // ========================================
        // HELPER: Generate Barcode Random
        // ========================================
        private string GenerateRandomBarcode()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            // Prefix ISBN standar (978 atau 979)
            string prefix = "978";

            // 6 digit random untuk publisher + title
            for (int i = 0; i < 6; i++)
            {
                sb.Append(random.Next(0, 10));
            }
            string middle = sb.ToString();

            // 3 digit random untuk title extension
            sb.Clear();
            for (int i = 0; i < 3; i++)
            {
                sb.Append(random.Next(0, 10));
            }
            string titleExt = sb.ToString();

            // Hitung check digit
            string partialISBN = prefix + middle + titleExt;
            int checkDigit = CalculateCheckDigit(partialISBN);

            // Format final: 978-XXXXXX-XXX-X
            return $"{prefix}-{middle}-{titleExt}-{checkDigit}";
        }

        // ========================================
        // HELPER: Hitung Check Digit ISBN
        // ========================================
        private int CalculateCheckDigit(string isbn)
        {
            int sum = 0;
            for (int i = 0; i < isbn.Length; i++)
            {
                int digit = int.Parse(isbn[i].ToString());
                sum += digit * (i + 1);
            }

            int checkDigit = sum % 11;
            return checkDigit == 10 ? 0 : checkDigit;
        }

        // ========================================
        // CEK KODE SUDAH ADA DI DATABASE
        // ========================================
        public bool IsCodeExists(string kodeBuku)
        {
            using (MySqlConnection conn = _koneksi.GetConn())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM buku WHERE kode_buku = @kode";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@kode", kodeBuku);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        // ========================================
        // CEK BARCODE SUDAH ADA DI DATABASE
        // ========================================
        public bool IsBarcodeExists(string barcode)
        {
            using (MySqlConnection conn = _koneksi.GetConn())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM buku WHERE barcode = @barcode";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@barcode", barcode);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        // ========================================
        // SIMPAN BUKU KE DATABASE
        // ========================================
        public bool SaveBook(string kodeBuku, string barcode, string judul,
            string penulis, string penerbit, string tahunTerbit, string kategori,
            int stok, string lokasiRak, string coverPath)
        {
            using (MySqlConnection conn = _koneksi.GetConn())
            {
                try
                {
                    conn.Open();

                    // Query INSERT
                    string query = @"INSERT INTO buku 
                (kode_buku, barcode, judul, penulis, penerbit, tahun_terbit, 
                 kategori, stok, lokasi_rak, cover_buku, created_at) 
                VALUES 
                (@kode_buku, @barcode, @judul, @penulis, @penerbit, @tahun_terbit, 
                 @kategori, @stok, @lokasi_rak, @cover_buku, NOW())";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parameter
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

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0; // True jika berhasil insert
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error saving book: {ex.Message}");
                }
            }
        }

        // ========================================
        // SIMPAN GAMBAR COVER KE FOLDER APLIKASI
        // ========================================
        public string SaveCoverImage(string sourceFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath))
                    return "";

                // Tentukan folder penyimpanan
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string coverFolder = Path.Combine(appDataPath, "AplikasiPerpustakaan", "Covers");

                // Buat folder jika belum ada
                if (!Directory.Exists(coverFolder))
                {
                    Directory.CreateDirectory(coverFolder);
                }

                // Generate nama file unik berdasarkan timestamp
                string fileExtension = Path.GetExtension(sourceFilePath);
                string newFileName = $"cover_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}{fileExtension}";
                string destinationPath = Path.Combine(coverFolder, newFileName);

                // Copy file ke folder Covers
                File.Copy(sourceFilePath, destinationPath, true);

                // Return relative path atau full path
                return destinationPath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving cover image: {ex.Message}");
            }
        }
    }
}