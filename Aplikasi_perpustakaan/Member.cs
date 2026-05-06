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
    public partial class Member : Form
    {
        Koneksi kon = new Koneksi();

        public Member()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;

            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.dataGridUser.CellClick += new DataGridViewCellEventHandler(this.dataGridUser_CellClick);
        }

        private async void Member_Load(object sender, EventArgs e)
        {
            StylingDataGridView();
            await TampilUser();
        }

        private async Task TampilUser(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string query = "SELECT id_user, username, nama, role, DATE_FORMAT(created_at, '%Y-%m-%d') AS tanggal_dibuat FROM users";

                    MySqlCommand cmd;

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        query += " WHERE username LIKE @keyword OR nama LIKE @keyword OR role LIKE @keyword";
                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    }
                    else
                    {
                        cmd = new MySqlCommand(query, conn);
                    }

                    using (cmd)
                    {
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            await Task.Run(() => da.Fill(dt));

                            if (this.InvokeRequired)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    UpdateDataGrid(dt);
                                }));
                            }
                            else
                            {
                                UpdateDataGrid(dt);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGrid(DataTable dt)
        {
            dataGridUser.DataSource = dt;

            if (dt.Columns.Contains("id_user"))
                dataGridUser.Columns["id_user"].HeaderText = "ID User";
            if (dt.Columns.Contains("username"))
                dataGridUser.Columns["username"].HeaderText = "Username";
            if (dt.Columns.Contains("nama"))
                dataGridUser.Columns["nama"].HeaderText = "Nama Lengkap";
            if (dt.Columns.Contains("role"))
                dataGridUser.Columns["role"].HeaderText = "Role";
            if (dt.Columns.Contains("tanggal_dibuat"))
                dataGridUser.Columns["tanggal_dibuat"].HeaderText = "Tanggal Dibuat";

            lblTotalUser.Text = "Total User: " + dt.Rows.Count.ToString();

            BuatKolomAksi();
        }

        private void BuatKolomAksi()
        {
            if (dataGridUser.Columns.Contains("id_user"))
            {
                dataGridUser.Columns["id_user"].Visible = false;
            }

            if (!dataGridUser.Columns.Contains("No"))
            {
                DataGridViewTextBoxColumn colNo = new DataGridViewTextBoxColumn();
                colNo.Name = "No";
                colNo.HeaderText = "No";
                colNo.Width = 40;
                colNo.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                colNo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridUser.Columns.Insert(0, colNo);
            }

            if (dataGridUser.Columns.Contains("nama"))
            {
                dataGridUser.Columns["nama"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dataGridUser.Columns.Contains("btnUpdate")) dataGridUser.Columns.Remove("btnUpdate");
            if (dataGridUser.Columns.Contains("btnDelete")) dataGridUser.Columns.Remove("btnDelete");
            if (dataGridUser.Columns.Contains("Action")) dataGridUser.Columns.Remove("Action");

            DataGridViewTextBoxColumn colAction = new DataGridViewTextBoxColumn();
            colAction.Name = "Action";
            colAction.HeaderText = "Action";
            colAction.Width = 150;
            colAction.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridUser.Columns.Add(colAction);
        }

        private void StylingDataGridView()
        {
            dataGridUser.ReadOnly = true;
            dataGridUser.AllowUserToAddRows = false;
            dataGridUser.AllowUserToDeleteRows = false;
            dataGridUser.AllowUserToOrderColumns = false;

            dataGridUser.AllowUserToResizeColumns = false;
            dataGridUser.AllowUserToResizeRows = false;

            dataGridUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridUser.RowHeadersVisible = false;
            dataGridUser.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUser.BorderStyle = BorderStyle.None;
            dataGridUser.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridUser.BackgroundColor = Color.White;

            dataGridUser.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridUser.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridUser.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            dataGridUser.DefaultCellStyle.Font = new Font("Inter", 9.5f, FontStyle.Regular);

            dataGridUser.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridUser.DefaultCellStyle.SelectionForeColor = Color.White;

            dataGridUser.RowTemplate.Height = 35;

            dataGridUser.EnableHeadersVisualStyles = false;
            dataGridUser.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dataGridUser.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridUser.ColumnHeadersHeight = 38;

            dataGridUser.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridUser.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridUser.ColumnHeadersDefaultCellStyle.Font = new Font("Inter", 9f, FontStyle.Bold);
            dataGridUser.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dataGridUser.RowPostPaint -= DataGridUser_RowPostPaint;
            dataGridUser.RowPostPaint += DataGridUser_RowPostPaint;

            dataGridUser.CellPainting -= DataGridUser_CellPainting;
            dataGridUser.CellPainting += DataGridUser_CellPainting;
        }

        private void DataGridUser_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dataGridUser.Rows[e.RowIndex].Cells["No"] != null)
            {
                dataGridUser.Rows[e.RowIndex].Cells["No"].Value = (e.RowIndex + 1).ToString();
            }
        }

        private void DataGridUser_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridUser.Columns[e.ColumnIndex].Name == "Action" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                int buttonWidth = 60;
                int buttonHeight = 24;
                int margin = 5;

                int updateX = e.CellBounds.Left + margin;
                int updateY = e.CellBounds.Top + (e.CellBounds.Height - buttonHeight) / 2;
                Rectangle rectUpdate = new Rectangle(updateX, updateY, buttonWidth, buttonHeight);

                int deleteX = updateX + buttonWidth + margin;
                int deleteY = updateY;
                Rectangle rectDelete = new Rectangle(deleteX, deleteY, buttonWidth, buttonHeight);

                if (deleteX + buttonWidth <= e.CellBounds.Right)
                {
                    using (SolidBrush brush = new SolidBrush(Color.DodgerBlue))
                    {
                        e.Graphics.FillRectangle(brush, rectUpdate);
                    }
                    TextRenderer.DrawText(e.Graphics, "Update", new Font("Segoe UI", 8f, FontStyle.Bold),
                        rectUpdate, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

                    using (SolidBrush brush = new SolidBrush(Color.Crimson))
                    {
                        e.Graphics.FillRectangle(brush, rectDelete);
                    }
                    TextRenderer.DrawText(e.Graphics, "Delete", new Font("Segoe UI", 8f, FontStyle.Bold),
                        rectDelete, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }

                e.Handled = true;
            }
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                await TampilUser(txtSearch.Text.Trim());
            }
            else
            {
                await TampilUser();
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await TampilUser(txtSearch.Text.Trim());
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            await TampilUser();
        }


        private async void dataGridUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridUser.Columns[e.ColumnIndex].Name == "Action")
            {
                Point mousePosition = dataGridUser.PointToClient(Cursor.Position);
                Rectangle cellRect = dataGridUser.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int relativeX = mousePosition.X - cellRect.X;

                string idUser = dataGridUser.Rows[e.RowIndex].Cells["id_user"].Value?.ToString();
                string namaUser = dataGridUser.Rows[e.RowIndex].Cells["nama"].Value?.ToString();
                string username = dataGridUser.Rows[e.RowIndex].Cells["username"].Value?.ToString();
                string role = dataGridUser.Rows[e.RowIndex].Cells["role"].Value?.ToString();

                if (string.IsNullOrEmpty(idUser))
                {
                    MessageBox.Show("Data user tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int buttonWidth = 60;
                int margin = 5;

                int updateStart = margin;
                int updateEnd = margin + buttonWidth;
                int deleteStart = updateEnd + margin;
                int deleteEnd = deleteStart + buttonWidth;

                if (relativeX >= updateStart && relativeX <= updateEnd)
                {
                    MasukModeEdit(idUser, namaUser, username, role);
                }
                else if (relativeX >= deleteStart && relativeX <= deleteEnd)
                {
                    DialogResult dialogResult = MessageBox.Show(
                        $"Apakah Anda yakin ingin menghapus user '{namaUser}'?\n\nData yang dihapus tidak dapat dikembalikan!",
                        "Konfirmasi Hapus",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (dialogResult == DialogResult.Yes)
                    {
                        await HapusUser(idUser);
                    }
                }
            }
        }

        private void MasukModeEdit(string idUser, string nama, string username, string role)
        {
            txtName.Tag = idUser; 

            txtName.Text = nama;
            txtUsername.Text = username;
            txtPassword.Text = ""; 
            txtPassword.PlaceholderText = "Isi password baru (kosongkan jika tidak diubah)";

            if (role.ToLower() == "admin")
            {
                radioAdmin.Checked = true;
                radioPetugas.Checked = false;
            }
            else
            {
                radioPetugas.Checked = true;
                radioAdmin.Checked = false;
            }

            btnSimpan.Visible = false;
            btnClear.Visible = false;
            btnUpdate.Visible = true;
            btnCancel.Visible = true;

            lblTitle.Text = "Edit User";

            txtName.Focus();
        }

        private void KembaliModeTambah()
        {
            ClearForm();

            txtName.Tag = null;

            btnSimpan.Visible = true;
            btnClear.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;

            lblTitle.Text = "Form Input User";

            txtPassword.PlaceholderText = "Enter your password";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Nama dan Username wajib diisi!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string idUser = txtName.Tag?.ToString();

            if (string.IsNullOrEmpty(idUser))
            {
                MessageBox.Show("Data user tidak valid!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dialogResult = MessageBox.Show(
                "Apakah Anda yakin ingin mengupdate data user ini?",
                "Konfirmasi Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dialogResult == DialogResult.Yes)
            {
                UpdateUser(idUser);
            }
        }

        private async void UpdateUser(string idUser)
        {
            try
            {
                btnUpdate.Enabled = false;
                btnCancel.Enabled = false;

                string roleSelected = radioAdmin.Checked ? "admin" : "petugas";

                using (MySqlConnection conn = kon.GetConn())
                {
                    await conn.OpenAsync();

                    string query;
                    MySqlCommand cmd;

                    if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);
                        query = "UPDATE users SET username = @username, password = @password, nama = @nama, role = @role WHERE id_user = @id_user";

                        cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                    }
                    else
                    {
                        query = "UPDATE users SET username = @username, nama = @nama, role = @role WHERE id_user = @id_user";

                        cmd = new MySqlCommand(query, conn);
                    }

                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@nama", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@role", roleSelected);
                    cmd.Parameters.AddWithValue("@id_user", idUser);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data user berhasil diupdate!", "Sukses",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        KembaliModeTambah();

                        await TampilUser(txtSearch.Text.Trim());
                    }
                    else
                    {
                        MessageBox.Show("Gagal mengupdate data user.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("Username sudah digunakan! Silakan cari username lain.",
                        "Error Username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Terjadi kesalahan database: " + ex.Message,
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem: " + ex.Message,
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUpdate.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text) ||
                !string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Apakah Anda yakin ingin membatalkan edit?\nPerubahan yang belum disimpan akan hilang.",
                    "Konfirmasi Batal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            KembaliModeTambah();
        }



        private async Task HapusUser(string idUser)
        {
            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    string query = "DELETE FROM users WHERE id_user = @id_user";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id_user", idUser);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await TampilUser(txtSearch.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("User tidak ditemukan atau gagal dihapus.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Terjadi kesalahan database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Semua data wajib diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSimpan.Enabled = false;
            string originalText = btnSimpan.Text;
            btnSimpan.Text = "...Loading";

            string roleSelected = radioAdmin.Checked ? "admin" : "petugas";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(txtPassword.Text);

            try
            {
                using (MySqlConnection conn = kon.GetConn())
                {
                    string query = "INSERT INTO users (username, password, nama, role) VALUES (@username, @password, @nama, @role)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@nama", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@role", roleSelected);

                        await conn.OpenAsync();
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User baru berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();

                            // Refresh data grid untuk menampilkan data terbaru
                            await TampilUser(txtSearch.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("Gagal menyimpan data user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("Username sudah digunakan! Silakan cari username lain.", "Error Username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Terjadi kesalahan database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan sistem: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSimpan.Enabled = true;
                btnSimpan.Text = originalText;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();

            radioPetugas.Checked = true;
            radioAdmin.Checked = false;
            ckPassword.Checked = false;
            txtPassword.UseSystemPasswordChar = true;

            txtName.Focus();
        }

        private void ckPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !ckPassword.Checked;
        }




        private void lblTotalUser_Click(object sender, EventArgs e)
        {

        }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel6_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }





        private void radioAdmin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioPetugas_CheckedChanged(object sender, EventArgs e)
        {

        }

        
    }
}
