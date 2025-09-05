using System;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Centralized error handling and logging system
    /// </summary>
    public static class ErrorHandler
    {
        private static readonly string LogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private static readonly string LogFileName = "application_log.txt";
        private static readonly string LogFilePath = Path.Combine(LogDirectory, LogFileName);

        static ErrorHandler()
        {
            try
            {
                // Ensure log directory exists
                Directory.CreateDirectory(LogDirectory);
            }
            catch
            {
                // If we can't create log directory, continue without logging
            }
        }

        /// <summary>
        /// Handle and log an exception with user-friendly message
        /// </summary>
        public static void HandleException(Exception ex, string context = "", bool showToUser = true)
        {
            try
            {
                LogError(ex, context);

                if (showToUser)
                {
                    ShowUserFriendlyError(ex, context);
                }
            }
            catch
            {
                // Fallback if error handling itself fails
                if (showToUser)
                {
                    MessageBox.Show("An unexpected error occurred. Please restart the application.",
                        "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Log error to file
        /// </summary>
        private static void LogError(Exception ex, string context)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR in {context}\n" +
                                 $"Message: {ex.Message}\n" +
                                 $"Type: {ex.GetType().Name}\n" +
                                 $"Stack Trace: {ex.StackTrace}\n" +
                                 new string('-', 80) + "\n";

                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // If logging fails, don't crash the application
            }
        }

        /// <summary>
        /// Show user-friendly error message
        /// </summary>
        private static void ShowUserFriendlyError(Exception ex, string context)
        {
            string userMessage = GetUserFriendlyMessage(ex, context);
            string title = GetErrorTitle(ex);

            MessageBox.Show(userMessage, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Convert technical exceptions to user-friendly messages
        /// </summary>
        private static string GetUserFriendlyMessage(Exception ex, string context)
        {
            switch (ex)
            {
                case FileNotFoundException _:
                    return "A required file could not be found. Please check your installation.";

                case DirectoryNotFoundException _:
                    return "A required directory could not be found. The application will attempt to create it.";

                case UnauthorizedAccessException _:
                    return "The application doesn't have permission to access a required file or folder. Please run as administrator or check file permissions.";

                case System.Net.WebException _:
                    return "Network connection error. Please check your internet connection and try again.";

                case ArgumentException argEx when argEx.Message.Contains("password"):
                    return "Invalid password format. Please check your password and try again.";

                case ArgumentException argEx when argEx.Message.Contains("username"):
                    return "Invalid username format. Please check your username and try again.";

                case ArgumentException _:
                    return "Invalid input provided. Please check your entries and try again.";

                case InvalidOperationException _:
                    return $"Unable to complete the operation in {context}. Please try again.";

                case NullReferenceException _:
                    return $"A required component is not available in {context}. Please restart the application.";

                default:
                    return $"An unexpected error occurred in {context}. The error has been logged. Please try again or contact support if the problem persists.";
            }
        }

        /// <summary>
        /// Get appropriate error dialog title
        /// </summary>
        private static string GetErrorTitle(Exception ex)
        {
            switch (ex)
            {
                case FileNotFoundException _:
                case DirectoryNotFoundException _:
                    return "File System Error";

                case UnauthorizedAccessException _:
                    return "Permission Error";

                case System.Net.WebException _:
                    return "Network Error";

                case ArgumentException _:
                    return "Input Error";

                case InvalidOperationException _:
                    return "Operation Error";

                default:
                    return "Application Error";
            }
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public static void LogInfo(string message, string context = "")
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO in {context}: {message}\n";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // If logging fails, don't crash the application
            }
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        public static void LogWarning(string message, string context = "")
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] WARNING in {context}: {message}\n";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch
            {
                // If logging fails, don't crash the application
            }
        }

        /// <summary>
        /// Show confirmation dialog with error handling
        /// </summary>
        public static DialogResult ShowConfirmation(string message, string title = "Confirm Action")
        {
            try
            {
                return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            catch (Exception ex)
            {
                LogError(ex, "ShowConfirmation");
                return DialogResult.No; // Default to safe option
            }
        }

        /// <summary>
        /// Show information message with error handling
        /// </summary>
        public static void ShowInfo(string message, string title = "Information")
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogError(ex, "ShowInfo");
            }
        }

        /// <summary>
        /// Show warning message with error handling
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            try
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogError(ex, "ShowWarning");
            }
        }

        /// <summary>
        /// Safely execute an action with error handling
        /// </summary>
        public static void SafeExecute(Action action, string context = "Unknown Operation")
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                HandleException(ex, context);
            }
        }

        /// <summary>
        /// Safely execute a function with error handling and return default value on error
        /// </summary>
        public static T SafeExecute<T>(Func<T> func, T defaultValue, string context = "Unknown Operation")
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                HandleException(ex, context);
                return defaultValue;
            }
        }
    }
}