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
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAlamat_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {

        }

        // Tombol Auto untuk generate kode anggota
        private void btnAuto_Click(object sender, EventArgs e)
        {
            GenerateKodeAnggota();
        }

        // Method untuk generate kode anggota otomatis
        private void GenerateKodeAnggota()
        {
            string kodeBaru = GenerateKodeDariDatabase();
            txtCode.Text = kodeBaru;

            // Optional: Tampilkan notifikasi
            // MessageBox.Show($"Kode anggota berhasil digenerate: {kodeBaru}", "Informasi", 
            //    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Method untuk mengambil kode terakhir dari database dan generate kode baru
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

                    // Query untuk mengambil kode_anggota terakhir (urut berdasarkan id)
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

        // Method untuk generate kode berikutnya berdasarkan kode terakhir
        private string GenerateNextKode(string lastKode)
        {
            // Jika tidak ada data sama sekali, mulai dari A-001
            if (string.IsNullOrEmpty(lastKode))
            {
                return "A-001";
            }

            // Pisahkan prefix dan nomor
            // Contoh: "A-001" -> prefix = "A", number = 1
            string[] parts = lastKode.Split('-');

            if (parts.Length == 2 && parts[0] == "A")
            {
                if (int.TryParse(parts[1], out int lastNumber))
                {
                    int nextNumber = lastNumber + 1;
                    return $"A-{nextNumber:D3}"; // D3 membuat format 001, 002, 003, dst
                }
            }

            // Fallback jika format tidak sesuai
            return "A-001";
        }

        // Optional: Method untuk mengecek apakah kode sudah ada (validasi sebelum simpan)
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

        // Optional: Auto-generate saat form pertama kali dibuka
        private void Anggota_Load(object sender, EventArgs e)
        {
            // Auto generate kode saat form pertama dibuka
            GenerateKodeAnggota();
        }
    }
}
