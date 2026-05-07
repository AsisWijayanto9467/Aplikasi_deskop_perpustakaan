using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aplikasi_perpustakaan
{
    public partial class Dashboard : Form
    {
        private string namaUser;
        private string roleUser;
        private Form currentForm;

        public Dashboard(string nama, string role)
        {
            InitializeComponent();
            this.namaUser = nama;
            this.roleUser = role;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            AturHakAkses();
            TampilkanForm(new Home());
            UpdateLabelInfo("Dashboard");
        }

        private void AturHakAkses()
        {
            if (roleUser == "petugas")
            {
                MessageBox.Show("Komponen khusus admin telah disembunyikan.", "Info Akses");
                btnReport.Visible = false;
            }
            else if (roleUser == "admin")
            {
            }
        }

        private void TampilkanForm(Form formBaru)
        {
            // HAPUS KONTEN LAMA
            if (mainPanel.Controls.Count > 0)
            {
                mainPanel.Controls.Clear();
            }

            // 🔥 SETTING FORM AGAR BISA MASUK KE PANEL
            formBaru.TopLevel = false;
            formBaru.Dock = DockStyle.None;
            formBaru.FormBorderStyle = FormBorderStyle.None;

            // 🔥 HITUNG TINGGI FORM SECARA OTOMATIS
            int maxBottom = 0;
            foreach (Control ctrl in formBaru.Controls)
            {
                int bottom = ctrl.Bottom + ctrl.Margin.Bottom + ctrl.Padding.Bottom;
                if (bottom > maxBottom)
                    maxBottom = bottom;
            }

            // Tambah padding 50px untuk keamanan
            int formHeight = Math.Max(maxBottom + 50, formBaru.MinimumSize.Height);
            int formWidth = mainPanel.Width - 30; // Kurangi sedikit untuk margin

            formBaru.Size = new Size(formWidth, formHeight);

            // 🔥 SETTING SCROLL PANEL
            mainPanel.AutoScroll = true;
            mainPanel.AutoScrollMinSize = new Size(0, 0);
            mainPanel.VerticalScroll.Visible = false;

            // 🔥 AKTIFKAN SCROLL JIKA FORM LEBIH TINGGI DARI PANEL
            if (formHeight > mainPanel.Height)
            {
                mainPanel.AutoScrollMinSize = new Size(0, formHeight);
                mainPanel.VerticalScroll.Visible = true;
            }

            mainPanel.Controls.Add(formBaru);
            formBaru.Location = new Point(10, 0); // Beri sedikit margin kiri
            formBaru.Show();

            currentForm = formBaru;
        }

        private void UpdateLabelInfo(string halaman)
        {
            lblInfo.Text = $"📌 Halaman : {halaman}";
        }

        // ========== EVENT CLICK UNTUK SETIAP TOMBOL ==========

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Home());
            UpdateLabelInfo("Dashboard");
        }

        private void btnMember_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Member());
            UpdateLabelInfo("Member");
        }

        private void btnAnggota_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Anggota());
            UpdateLabelInfo("Anggota");
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Books());
            UpdateLabelInfo("Books / Buku");
        }


        private void btnReport_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Report());
            UpdateLabelInfo("Laporan");
        }

        private void btnPeminjaman_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Peminjaman());
            UpdateLabelInfo("Laporan");
        }

        private void btnPengembalian_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Pengembalian());
            UpdateLabelInfo("Laporan");
        }

        private void btnDenda_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Denda());
            UpdateLabelInfo("Laporan");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Login formLogin = new Login();
                formLogin.Show();
                this.Close();
            }
        }

        // ========== METHOD LAIN YANG SUDAH ADA ==========

        private void guna2Button7_Click(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void lblInfo_Click(object sender, EventArgs e) { }

        private void mainPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
