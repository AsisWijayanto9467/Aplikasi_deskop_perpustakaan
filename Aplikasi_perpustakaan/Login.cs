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
    public partial class Login : Form
    {
        Koneksi kon = new Koneksi();
        MySqlCommand cmd;
        MySqlDataReader dr;

        public Login()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Color warnaAsli = btnSubmit.FillColor;
            btnSubmit.Enabled = false;
            btnSubmit.Text = "...loading";
            btnSubmit.FillColor = Color.LightSkyBlue;

            MySqlConnection conn = kon.GetConn();

            try
            {
                conn.Open();

                string query = "SELECT id_user, username, nama, role FROM users WHERE username = @username AND password = SHA2(@password, 256)";

                cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    Program.UserId = Convert.ToInt32(dr["id_user"]);
                    Program.Username = dr["username"].ToString();
                    Program.NamaLengkap = dr["nama"].ToString();
                    Program.Role = dr["role"].ToString();

                    string namaUser = Program.NamaLengkap;
                    string roleUser = Program.Role;

                    MessageBox.Show($"Selamat Datang, {namaUser}! Anda login sebagai {roleUser}.",
                        "Login Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // BUKA DASHBOARD
                    Dashboard dash = new Dashboard(namaUser, roleUser);
                    dash.Show();

                    // TUTUP LOGIN FORM
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username atau Password salah!", "Gagal Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan koneksi: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSubmit.Enabled = true;
                btnSubmit.Text = "Submit";
                btnSubmit.FillColor = warnaAsli;

                if (dr != null && !dr.IsClosed) dr.Close();
                if (conn != null && conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void ckPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (ckPassword.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = false;

                ckPassword.Text = "Keep Password";
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;

                ckPassword.Text = "Show Password";
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }



















































        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
