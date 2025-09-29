using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Collections.Concurrent;
using System.Configuration;

namespace ShippingManagementSystem
{
    public static class EmailManager
    {
        private static readonly string LogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private static readonly string EmailLogFile = Path.Combine(LogDirectory, "email_log.txt");
        private static readonly ConcurrentQueue<EmailQueueItem> EmailQueue = new ConcurrentQueue<EmailQueueItem>();
        private static bool _isProcessingQueue = false;

        private static readonly bool IsSimulationMode = GetConfigValue("EmailSimulationMode", "true").ToLower() == "true";
        private static readonly string SmtpServer = GetConfigValue("SmtpServer", "smtp.gmail.com");
        private static readonly int SmtpPort = int.Parse(GetConfigValue("SmtpPort", "587"));
        private static readonly bool EnableSsl = GetConfigValue("EnableSsl", "true").ToLower() == "true";
        private static readonly string SenderEmail = GetConfigValue("SenderEmail", "noreply@ryzenshipment.com");
        private static readonly string SenderPassword = GetConfigValue("SenderPassword", "");
        private static readonly string SenderName = GetConfigValue("SenderName", "Ryzen Shipping Management");
        private static readonly string CompanyName = GetConfigValue("CompanyName", "Ryzen Shipping Management");
        private static readonly string SupportEmail = GetConfigValue("SupportEmail", "support@ryzenshipment.com");

        static EmailManager()
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);
                if (!IsSimulationMode)
                {
                    StartEmailQueueProcessor();
                }
            }
            catch { }
        }

        public static async Task<bool> SendWelcomeEmail(string toEmail, string username, string role, string company)
        {
            try
            {
                if (!IsValidEmail(toEmail)) return false;

                string subject = $"Welcome to {CompanyName}";
                string body = GenerateWelcomeEmailBody(username, role, company);

                return await SendEmailAsync(toEmail, subject, body, EmailPriority.Normal, "Welcome");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Welcome Email", false);
                return false;
            }
        }

        public static async Task<bool> SendShipmentCreatedEmail(string toEmail, Shipment shipment)
        {
            try
            {
                if (!IsValidEmail(toEmail)) return false;

                string subject = $"New Shipment Created - #{shipment.ID}";
                string body = GenerateShipmentCreatedEmailBody(shipment);

                return await SendEmailAsync(toEmail, subject, body, EmailPriority.Normal, "Shipment Created");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Shipment Created Email", false);
                return false;
            }
        }

        private static async Task<bool> SendEmailAsync(string toEmail, string subject, string body, EmailPriority priority, string context)
        {
            try
            {
                var emailItem = new EmailQueueItem
                {
                    ToEmail = toEmail,
                    Subject = subject,
                    Body = body,
                    Priority = priority,
                    Context = context,
                    QueuedAt = DateTime.Now,
                    Attempts = 0
                };

                if (IsSimulationMode)
                {
                    return await SimulateEmailSending(emailItem);
                }
                else
                {
                    EmailQueue.Enqueue(emailItem);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, $"Queue Email - {context}", false);
                return false;
            }
        }

        private static async Task<bool> SimulateEmailSending(EmailQueueItem emailItem)
        {
            await Task.Delay(new Random().Next(100, 500));
            LogDetailedEmail(emailItem.ToEmail, emailItem.Subject, emailItem.Body);
            LogEmail(emailItem.Context, emailItem.ToEmail, emailItem.Subject, "SIMULATION", true);
            return true;
        }

        private static void StartEmailQueueProcessor()
        {
            if (_isProcessingQueue) return;
            _isProcessingQueue = true;
            Task.Run(ProcessEmailQueue);
        }

        private static async Task ProcessEmailQueue()
        {
            while (_isProcessingQueue)
            {
                try
                {
                    if (EmailQueue.TryDequeue(out EmailQueueItem emailItem))
                    {
                        using (var client = new SmtpClient(SmtpServer, SmtpPort))
                        {
                            client.EnableSsl = EnableSsl;
                            client.Credentials = new NetworkCredential(SenderEmail, SenderPassword);

                            using (var message = new MailMessage())
                            {
                                message.From = new MailAddress(SenderEmail, SenderName);
                                message.To.Add(emailItem.ToEmail);
                                message.Subject = emailItem.Subject;
                                message.Body = emailItem.Body;
                                message.IsBodyHtml = true;

                                await client.SendMailAsync(message);
                                LogEmail(emailItem.Context, emailItem.ToEmail, emailItem.Subject, "SMTP", true);
                            }
                        }
                    }
                    else
                    {
                        await Task.Delay(1000);
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleException(ex, "Email Queue Processing", false);
                    await Task.Delay(5000);
                }
            }
        }

        private static string GenerateWelcomeEmailBody(string username, string role, string company)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2 style='color: #27ae60;'>Welcome to {CompanyName}!</h2>
                    <p>Hello {username},</p>
                    <p>Your account has been successfully created.</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Username:</strong> {username}</p>
                        <p><strong>Role:</strong> {role}</p>
                        <p><strong>Company:</strong> {(string.IsNullOrEmpty(company) ? "Not specified" : company)}</p>
                    </div>
                    <p>Thank you for choosing {CompanyName}!</p>
                </body>
                </html>";
        }

        private static string GenerateShipmentCreatedEmailBody(Shipment shipment)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2 style='color: #3498db;'>New Shipment Created</h2>
                    <p>A new shipment has been created:</p>
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px;'>
                        <p><strong>ID:</strong> #{shipment.ID}</p>
                        <p><strong>Description:</strong> {shipment.Description}</p>
                        <p><strong>Status:</strong> {shipment.Status}</p>
                        <p><strong>Destination:</strong> {shipment.Destination}</p>
                    </div>
                </body>
                </html>";
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        private static string GetConfigValue(string key, string defaultValue)
        {
            try
            {
                return ConfigurationManager.AppSettings[key] ?? defaultValue;
            }
            catch { return defaultValue; }
        }

        private static void LogEmail(string type, string to, string subject, string method, bool success, string error = "")
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {(success ? "SUCCESS" : "FAILED")} ({method}) - {type}\n" +
                                 $"To: {to}\nSubject: {subject}\n{(success ? "" : $"Error: {error}\n")}" +
                                 new string('-', 80) + "\n";
                File.AppendAllText(EmailLogFile, logEntry);
            }
            catch { }
        }

        private static void LogDetailedEmail(string to, string subject, string body)
        {
            try
            {
                string logEntry = $"\n{new string('=', 100)}\nEMAIL SIMULATION - {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                                 $"To: {to}\nSubject: {subject}\nBody:\n{body}\n{new string('=', 100)}\n\n";
                File.AppendAllText(EmailLogFile, logEntry);
            }
            catch { }
        }
    }

    internal class EmailQueueItem
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailPriority Priority { get; set; }
        public string Context { get; set; }
        public DateTime QueuedAt { get; set; }
        public int Attempts { get; set; }
    }

    public enum EmailPriority
    {
        Low = 0,
        Normal = 1,
        High = 2
    }
}