namespace Aplikasi_perpustakaan
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 

        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string NamaLengkap { get; set; }
        public static string Role { get; set; }

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            UserId = 1;                    // id_user dari database
            Username = "admin1";           // username
            NamaLengkap = "Ahmad Soebardjo"; // nama lengkap
            Role = "admin";                // role

            Application.Run(new Dashboard(NamaLengkap, Role));
        }
    }
}