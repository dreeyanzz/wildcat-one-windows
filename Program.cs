namespace wildcat_one_windows
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Show disclaimer notice before login
            using (var notice = new NoticeForm())
            {
                if (notice.ShowDialog() != DialogResult.OK)
                    return; // User closed without acknowledging
            }

            Application.Run(new LoginForm());
        }
    }
}
