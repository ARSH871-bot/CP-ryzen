using System;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Set up global exception handling
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                // Start the application with the Login form
                Application.Run(new frmLogin());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A critical error occurred while starting the application:\n\n{ex.Message}",
                              "Application Startup Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show($"An unexpected error occurred:\n\n{e.Exception.Message}\n\nThe application will continue running.",
                          "Application Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                LogException(ex);
                MessageBox.Show($"A critical error occurred:\n\n{ex.Message}\n\nThe application will now close.",
                              "Critical Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private static void LogException(Exception ex)
        {
            try
            {
                string logPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Logs");
                System.IO.Directory.CreateDirectory(logPath);
                string logFile = System.IO.Path.Combine(logPath, $"error_log_{DateTime.Now:yyyyMMdd}.txt");

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}\n" +
                                $"Stack Trace: {ex.StackTrace}\n" +
                                new string('-', 80) + "\n";

                System.IO.File.AppendAllText(logFile, logEntry);
            }
            catch
            {
                // If logging fails, we can't do much about it
            }
        }
    }
}