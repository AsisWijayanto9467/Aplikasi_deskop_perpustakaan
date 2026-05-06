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
    public partial class Books : Form
    {
        public Books()
        {
            InitializeComponent();
        }

        private void Books_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }


        private void btnTambah_Click(object sender, EventArgs e)
        {
            FormBooks formTambah = new FormBooks();
            formTambah.ShowDialog();
        }
    }
}
