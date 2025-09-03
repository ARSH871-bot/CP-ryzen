using System;
using System.IO;
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
                // Initialize application settings
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Create necessary directories for the database
                InitializeApplicationDirectories();

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

        /// <summary>
        /// Creates the necessary directory structure for the application database
        /// </summary>
        private static void InitializeApplicationDirectories()
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string databasePath = Path.Combine(currentDirectory, "Database");
                string customerPath = Path.Combine(databasePath, "customer");
                string shipmentsPath = Path.Combine(databasePath, "shipments");
                string reportsPath = Path.Combine(currentDirectory, "Reports");
                string logsPath = Path.Combine(currentDirectory, "Logs");

                // Create directories if they don't exist
                Directory.CreateDirectory(customerPath);
                Directory.CreateDirectory(shipmentsPath);
                Directory.CreateDirectory(reportsPath);
                Directory.CreateDirectory(logsPath);

                // Create a default admin user if no users exist
                CreateDefaultAdminUser(customerPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating application directories:\n{ex.Message}",
                              "Initialization Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Creates a default admin user if no users exist in the system
        /// </summary>
        private static void CreateDefaultAdminUser(string customerPath)
        {
            try
            {
                string adminFilePath = Path.Combine(customerPath, "admin.txt");

                // Only create if admin user doesn't exist
                if (!File.Exists(adminFilePath))
                {
                    var defaultAdmin = new
                    {
                        USERNAME = "admin",
                        PASSWORD = "admin123", // In production, this should be hashed
                        COMPANY = "Ryzen Shipping Management",
                        EMAIL = "admin@ryzen.com",
                        PHONE = "+64-9-000-0000"
                    };

                    string jsonContent = System.Text.Json.JsonSerializer.Serialize(defaultAdmin, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    File.WriteAllText(adminFilePath, jsonContent);
                }
            }
            catch (Exception ex)
            {
                // Don't show error for default user creation failure
                // Just log it if logging is available
                Console.WriteLine($"Could not create default admin user: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles thread exceptions
        /// </summary>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            LogException(e.Exception);
            MessageBox.Show($"An unexpected error occurred:\n\n{e.Exception.Message}\n\nThe application will continue running.",
                          "Application Error",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles unhandled domain exceptions
        /// </summary>
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

        /// <summary>
        /// Logs exceptions to a file for debugging purposes
        /// </summary>
        private static void LogException(Exception ex)
        {
            try
            {
                string logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                string logFile = Path.Combine(logPath, $"error_log_{DateTime.Now:yyyyMMdd}.txt");

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {ex.Message}\n" +
                                $"Stack Trace: {ex.StackTrace}\n" +
                                new string('-', 80) + "\n";

                File.AppendAllText(logFile, logEntry);
            }
            catch
            {
                // If logging fails, we can't do much about it
                // In a production environment, you might want to use a more robust logging framework
            }
        }
    }
}