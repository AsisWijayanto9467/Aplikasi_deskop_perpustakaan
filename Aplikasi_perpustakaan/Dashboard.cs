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

        public Dashboard(string nama, string role)
        {
            InitializeComponent();
            this.namaUser = nama;
            this.roleUser = role;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            AturHakAkses();
        }

        private void AturHakAkses()
        {
            if (roleUser == "petugas")
            {
                MessageBox.Show("Komponen khusus admin telah disembunyikan.", "Info Akses");
            }
            else if (roleUser == "admin")
            {
            }
        }


        private void guna2Button7_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

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

        private void btnMember_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {

        }


        private void btnBooks_Click(object sender, EventArgs e)
        {

        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {

        }

        private void btnReport_Click(object sender, EventArgs e)
        {

        }
    }
}
