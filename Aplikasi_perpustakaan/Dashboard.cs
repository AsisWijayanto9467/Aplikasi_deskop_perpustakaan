using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Timer = System.Windows.Forms.Timer;

namespace Aplikasi_perpustakaan
{
    public partial class Dashboard : Form
    {
        private string namaUser;
        private string roleUser;
        private Form currentForm;
        private Timer timerJam;

        public Dashboard(string nama, string role)
        {
            InitializeComponent();
            this.namaUser = nama;
            this.roleUser = role;

            InitializeTimer();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            AturHakAkses();
            TampilkanForm(new Home());
            UpdateLabelInfo("Dashboard");
            UpdateUserInfo();
        }

        // TIMER & USER INFO 
        private void InitializeTimer()
        {
            timerJam = new Timer();
            timerJam.Interval = 1000;
            timerJam.Tick += TimerJam_Tick;
            timerJam.Start();
        }

        private void TimerJam_Tick(object sender, EventArgs e)
        {
            if (lblTimer != null)
            {
                lblTimer.Text = DateTime.Now.ToString("HH:mm:ss") + " WIB";
            }
        }

        private void UpdateUserInfo()
        {
            if (lblRoleAndName != null)
            {
                string roleCapitalized = roleUser.Substring(0, 1).ToUpper() + roleUser.Substring(1).ToLower();

                lblRoleAndName.Text = $"{roleCapitalized} - {namaUser}";
            }
        }

        // HAK AKSES 
        private void AturHakAkses()
        {
            btnBooks.Visible = true;
            btnMember.Visible = true;
            btnReport.Visible = true;

            if (roleUser.ToLower() == "petugas")
            {
                btnBooks.Visible = false;
                btnMember.Visible = false;
            }
        }

        private void TampilkanForm(Form formBaru)
        {
            if (mainPanel.Controls.Count > 0)
            {
                mainPanel.Controls.Clear();
            }

            formBaru.TopLevel = false;
            formBaru.Dock = DockStyle.None;
            formBaru.FormBorderStyle = FormBorderStyle.None;

            int maxBottom = 0;
            foreach (Control ctrl in formBaru.Controls)
            {
                int bottom = ctrl.Bottom + ctrl.Margin.Bottom + ctrl.Padding.Bottom;
                if (bottom > maxBottom)
                    maxBottom = bottom;
            }

            int formHeight = Math.Max(maxBottom + 50, formBaru.MinimumSize.Height);
            int formWidth = mainPanel.Width - 30;

            formBaru.Size = new Size(formWidth, formHeight);

            mainPanel.AutoScroll = true;
            mainPanel.AutoScrollMinSize = new Size(0, 0);
            mainPanel.VerticalScroll.Visible = false;

            if (formHeight > mainPanel.Height)
            {
                mainPanel.AutoScrollMinSize = new Size(0, formHeight);
                mainPanel.VerticalScroll.Visible = true;
            }

            mainPanel.Controls.Add(formBaru);
            formBaru.Location = new Point(10, 0);
            formBaru.Show();

            currentForm = formBaru;
        }

        private void UpdateLabelInfo(string halaman)
        {
            lblInfo.Text = $"📌 Halaman : {halaman}";
        }


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

        private void btnPeminjaman_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Peminjaman());
            UpdateLabelInfo("Peminjaman");
        }

        private void btnPengembalian_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Pengembalian());
            UpdateLabelInfo("Pengembalian");
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            TampilkanForm(new Report());
            UpdateLabelInfo("Laporan");
        }

        public void TampilkanFormDiPanel(Form formBaru)
        {
            TampilkanForm(formBaru);
        }

        public void UpdateLabelInfoPublic(string halaman)
        {
            UpdateLabelInfo(halaman);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Apakah Anda yakin ingin logout?",
                "Konfirmasi Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                if (timerJam != null)
                {
                    timerJam.Stop();
                    timerJam.Dispose();
                }

                Login formLogin = new Login();
                formLogin.Show();
                this.Close();
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
        private void lblInfo_Click(object sender, EventArgs e) { }
        private void mainPanel_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void lblTimer_Click(object sender, EventArgs e) { }
        private void lblRoleAndName_Click(object sender, EventArgs e) { }
    }
}