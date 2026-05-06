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
            if (mainPanel.Controls.Count > 0)
            {
                mainPanel.Controls.Clear();
            }

            formBaru.TopLevel = false;
            formBaru.Dock = DockStyle.Fill;
            formBaru.FormBorderStyle = FormBorderStyle.None;

            mainPanel.Controls.Add(formBaru);
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

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Transaction());
            UpdateLabelInfo("Transaksi Peminjaman");
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Report());
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
    }
}
