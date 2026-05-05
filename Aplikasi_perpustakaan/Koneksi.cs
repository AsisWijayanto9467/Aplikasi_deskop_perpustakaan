using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace Aplikasi_perpustakaan
{
    class Koneksi
    {
        public MySqlConnection GetConn()
        {
            string str = "server=localhost;port=3306;database=deskop_perpustakaan;uid=root;pwd=;";
            MySqlConnection conn = new MySqlConnection(str);
            return conn;
        }
    }
}
