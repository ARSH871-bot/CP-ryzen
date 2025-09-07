using System;
using System.IO;
using System.Threading.Tasks;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Complete email management system with comprehensive notification methods
    /// Simulates email sending with detailed logging for development
    /// </summary>
    public static class EmailManager
    {
        private static readonly string LogDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private static readonly string EmailLogFile = Path.Combine(LogDirectory, "email_log.txt");
        private static readonly bool IsSimulationMode = true; // Set to false for production SMTP

        static EmailManager()
        {
            try
            {
                Directory.CreateDirectory(LogDirectory);
            }
            catch
            {
                // Continue without logging if directory creation fails
            }
        }

        #region User Registration and Welcome Emails

        /// <summary>
        /// Send welcome email to new registered users
        /// </summary>
        public static async Task<bool> SendWelcomeEmail(string toEmail, string username, string role, string company)
        {
            try
            {
                string subject = "Welcome to Ryzen Shipping Management System";
                string body = GenerateWelcomeEmailBody(username, role, company);

                await SimulateEmailSending(toEmail, subject, body);
                LogEmail("Welcome", toEmail, subject, "Registration");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Welcome Email", false);
                return false;
            }
        }

        /// <summary>
        /// Generate welcome email HTML content
        /// </summary>
        private static string GenerateWelcomeEmailBody(string username, string role, string company)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; margin: 20px;'>
                    <h2 style='color: #2c3e50;'>Welcome to Ryzen Shipping Management!</h2>
                    
                    <p>Dear {username},</p>
                    
                    <p>Your account has been successfully created with the following details:</p>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Username:</strong> {username}</p>
                        <p><strong>Role:</strong> {role}</p>
                        <p><strong>Company:</strong> {(string.IsNullOrEmpty(company) ? "Not specified" : company)}</p>
                        <p><strong>Account Created:</strong> {DateTime.Now:MMM dd, yyyy 'at' HH:mm}</p>
                    </div>
                    
                    <h3 style='color: #34495e;'>Getting Started</h3>
                    <ul>
                        <li>Log in to your dashboard to manage shipments</li>
                        <li>Track packages in real-time</li>
                        <li>Generate comprehensive reports</li>
                        <li>Set up notifications and preferences</li>
                    </ul>
                    
                    <h3 style='color: #34495e;'>Security Features</h3>
                    <p>Your account includes enhanced security features:</p>
                    <ul>
                        <li>Encrypted password storage</li>
                        <li>Account lockout protection</li>
                        <li>Login attempt monitoring</li>
                        <li>Comprehensive audit trails</li>
                    </ul>
                    
                    <div style='background-color: #e8f4fd; padding: 15px; border-left: 4px solid #3498db; margin: 20px 0;'>
                        <p><strong>Support:</strong> If you need assistance, please contact our support team.</p>
                    </div>
                    
                    <p>Thank you for choosing Ryzen Shipping Management System!</p>
                    
                    <hr style='margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        This is an automated message from Ryzen Shipping Management System.<br>
                        Please do not reply to this email.
                    </p>
                </body>
                </html>";
        }

        #endregion

        #region Shipment Notification Emails

        /// <summary>
        /// Send email notification when a new shipment is created
        /// </summary>
        public static async Task<bool> SendShipmentCreatedEmail(string toEmail, Shipment shipment)
        {
            try
            {
                string subject = $"New Shipment Created - #{shipment.ID}";
                string body = GenerateShipmentCreatedEmailBody(shipment);

                await SimulateEmailSending(toEmail, subject, body);
                LogEmail("Shipment Created", toEmail, subject, "Shipment Management");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Shipment Created Email", false);
                return false;
            }
        }

        /// <summary>
        /// Send email notification when shipment status is updated
        /// </summary>
        public static async Task<bool> SendShipmentStatusUpdateEmail(string toEmail, Shipment shipment)
        {
            try
            {
                string subject = $"Shipment Status Update - #{shipment.ID}";
                string body = GenerateStatusUpdateEmailBody(shipment);

                await SimulateEmailSending(toEmail, subject, body);
                LogEmail("Status Update", toEmail, subject, "Shipment Management");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Status Update Email", false);
                return false;
            }
        }

        /// <summary>
        /// Send delivery confirmation email
        /// </summary>
        public static async Task<bool> SendDeliveryConfirmationEmail(string toEmail, Shipment shipment)
        {
            try
            {
                string subject = $"Package Delivered - #{shipment.ID}";
                string body = GenerateDeliveryConfirmationEmailBody(shipment);

                await SimulateEmailSending(toEmail, subject, body);
                LogEmail("Delivery Confirmation", toEmail, subject, "Shipment Management");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Delivery Confirmation Email", false);
                return false;
            }
        }

        /// <summary>
        /// Send general shipment update email
        /// </summary>
        public static async Task<bool> SendShipmentUpdatedEmail(string toEmail, Shipment shipment)
        {
            try
            {
                string subject = $"Shipment Information Updated - #{shipment.ID}";
                string body = GenerateShipmentUpdateEmailBody(shipment);

                await SimulateEmailSending(toEmail, subject, body);
                LogEmail("Shipment Updated", toEmail, subject, "Shipment Management");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Send Shipment Update Email", false);
                return false;
            }
        }

        /// <summary>
        /// Legacy method - redirects to SendShipmentStatusUpdateEmail
        /// </summary>
        public static async Task<bool> SendStatusUpdateEmail(string toEmail, Shipment shipment, string oldStatus)
        {
            return await SendShipmentStatusUpdateEmail(toEmail, shipment);
        }

        /// <summary>
        /// Legacy method - redirects to SendDeliveryConfirmationEmail
        /// </summary>
        public static async Task<bool> SendDeliveryEmail(string toEmail, Shipment shipment)
        {
            return await SendDeliveryConfirmationEmail(toEmail, shipment);
        }

        #endregion

        #region Email Body Generators

        /// <summary>
        /// Generate shipment creation email content
        /// </summary>
        private static string GenerateShipmentCreatedEmailBody(Shipment shipment)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; margin: 20px;'>
                    <h2 style='color: #27ae60;'>New Shipment Created</h2>
                    
                    <p>A new shipment has been created and assigned to you:</p>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Shipment ID:</td><td>#{shipment.ID}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Description:</td><td>{shipment.Description}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Status:</td><td><span style='color: #f39c12; font-weight: bold;'>{shipment.Status}</span></td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Destination:</td><td>{shipment.Destination}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Date Shipped:</td><td>{shipment.DateShipped:MMM dd, yyyy}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Est. Arrival:</td><td>{shipment.EstimatedArrival:MMM dd, yyyy}</td></tr>
                        </table>
                    </div>
                    
                    <p>Please log into the system to view full details and manage this shipment.</p>
                    
                    <div style='background-color: #e8f4fd; padding: 15px; border-left: 4px solid #3498db; margin: 20px 0;'>
                        <p><strong>Next Steps:</strong></p>
                        <ul>
                            <li>Review shipment details in your dashboard</li>
                            <li>Update tracking information as needed</li>
                            <li>Monitor progress and update status</li>
                        </ul>
                    </div>
                    
                    <hr style='margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        Automated notification from Ryzen Shipping Management System<br>
                        Generated on {DateTime.Now:MMM dd, yyyy 'at' HH:mm}
                    </p>
                </body>
                </html>";
        }

        /// <summary>
        /// Generate status update email content
        /// </summary>
        private static string GenerateStatusUpdateEmailBody(Shipment shipment)
        {
            string statusColor = GetStatusColor(shipment.Status);
            string statusMessage = GetStatusMessage(shipment.Status);

            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; margin: 20px;'>
                    <h2 style='color: #3498db;'>Shipment Status Update</h2>
                    
                    <p>The status of your shipment has been updated:</p>
                    
                    <div style='background-color: {statusColor}; color: white; padding: 15px; border-radius: 5px; margin: 20px 0; text-align: center;'>
                        <h3 style='margin: 0; font-size: 24px;'>{shipment.Status.ToUpper()}</h3>
                    </div>
                    
                    {statusMessage}
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Shipment ID:</td><td>#{shipment.ID}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Description:</td><td>{shipment.Description}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Destination:</td><td>{shipment.Destination}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Updated:</td><td>{DateTime.Now:MMM dd, yyyy 'at' HH:mm}</td></tr>
                        </table>
                    </div>
                    
                    <p>Log into your dashboard for complete tracking details and updates.</p>
                    
                    <hr style='margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        Automated status update from Ryzen Shipping Management System
                    </p>
                </body>
                </html>";
        }

        /// <summary>
        /// Generate delivery confirmation email content
        /// </summary>
        private static string GenerateDeliveryConfirmationEmailBody(Shipment shipment)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; margin: 20px;'>
                    <h2 style='color: #27ae60;'>📦 Package Delivered Successfully!</h2>
                    
                    <p>Great news! Your shipment has been delivered successfully.</p>
                    
                    <div style='background-color: #d4edda; border: 1px solid #c3e6cb; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <h3 style='color: #155724; margin-top: 0;'>✅ Delivery Confirmed</h3>
                        <p style='color: #155724; margin-bottom: 0;'>Your package was delivered on {DateTime.Now:MMM dd, yyyy 'at' HH:mm}</p>
                    </div>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <h4>Shipment Details:</h4>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Shipment ID:</td><td>#{shipment.ID}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Description:</td><td>{shipment.Description}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Delivered To:</td><td>{shipment.Destination}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Original Ship Date:</td><td>{shipment.DateShipped:MMM dd, yyyy}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Delivery Date:</td><td>{DateTime.Now:MMM dd, yyyy}</td></tr>
                        </table>
                    </div>
                    
                    <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Need Support?</strong> If you have any questions about this delivery or need assistance, please contact our customer service team.</p>
                    </div>
                    
                    <p>Thank you for using Ryzen Shipping Management System!</p>
                    
                    <hr style='margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        Delivery confirmation from Ryzen Shipping Management System<br>
                        Generated on {DateTime.Now:MMM dd, yyyy 'at' HH:mm}
                    </p>
                </body>
                </html>";
        }

        /// <summary>
        /// Generate general shipment update email content
        /// </summary>
        private static string GenerateShipmentUpdateEmailBody(Shipment shipment)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; margin: 20px;'>
                    <h2 style='color: #8e44ad;'>Shipment Information Updated</h2>
                    
                    <p>The information for your shipment has been updated:</p>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <table style='width: 100%; border-collapse: collapse;'>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Shipment ID:</td><td>#{shipment.ID}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Description:</td><td>{shipment.Description}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Current Status:</td><td><span style='color: #e67e22; font-weight: bold;'>{shipment.Status}</span></td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Destination:</td><td>{shipment.Destination}</td></tr>
                            <tr><td style='padding: 5px 0; font-weight: bold;'>Updated:</td><td>{DateTime.Now:MMM dd, yyyy 'at' HH:mm}</td></tr>
                        </table>
                    </div>
                    
                    <p>Please review the updated information in your dashboard.</p>
                    
                    <div style='background-color: #e8f4fd; padding: 15px; border-left: 4px solid #3498db; margin: 20px 0;'>
                        <p><strong>What's Next:</strong> Check your dashboard for the latest tracking information and any additional updates.</p>
                    </div>
                    
                    <hr style='margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        Automated update from Ryzen Shipping Management System
                    </p>
                </body>
                </html>";
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get color based on shipment status
        /// </summary>
        private static string GetStatusColor(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return "#f39c12";
                case "in transit":
                    return "#3498db";
                case "out for delivery":
                    return "#e67e22";
                case "delivered":
                    return "#27ae60";
                case "cancelled":
                    return "#e74c3c";
                default:
                    return "#95a5a6";
            }
        }

        /// <summary>
        /// Get status-specific message
        /// </summary>
        private static string GetStatusMessage(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return "<p><strong>Status:</strong> Your shipment is being prepared for dispatch.</p>";
                case "in transit":
                    return "<p><strong>Status:</strong> Your shipment is on its way to the destination.</p>";
                case "out for delivery":
                    return "<p><strong>Status:</strong> Your shipment is out for delivery and will arrive soon!</p>";
                case "delivered":
                    return "<p><strong>Status:</strong> Your shipment has been successfully delivered!</p>";
                case "cancelled":
                    return "<p><strong>Status:</strong> This shipment has been cancelled. Please contact support for more information.</p>";
                default:
                    return "<p><strong>Status:</strong> Shipment status has been updated.</p>";
            }
        }

        /// <summary>
        /// Simulate email sending with delay
        /// </summary>
        private static async Task SimulateEmailSending(string toEmail, string subject, string body)
        {
            if (IsSimulationMode)
            {
                // Simulate network delay
                await Task.Delay(new Random().Next(100, 500));

                // Log detailed email content for testing
                LogDetailedEmail(toEmail, subject, body);
            }
            else
            {
                // TODO: Implement real SMTP sending for production
                throw new NotImplementedException("Production SMTP sending not yet implemented");
            }
        }

        /// <summary>
        /// Log email sending attempt
        /// </summary>
        private static void LogEmail(string emailType, string toEmail, string subject, string context)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] EMAIL SENT - {emailType}\n" +
                                 $"To: {toEmail}\n" +
                                 $"Subject: {subject}\n" +
                                 $"Context: {context}\n" +
                                 new string('-', 80) + "\n";

                File.AppendAllText(EmailLogFile, logEntry);
            }
            catch
            {
                // Continue if logging fails
            }
        }

        /// <summary>
        /// Log detailed email content for testing
        /// </summary>
        private static void LogDetailedEmail(string toEmail, string subject, string body)
        {
            try
            {
                string detailedLogEntry = $"\n{new string('=', 100)}\n" +
                                        $"EMAIL SIMULATION - {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                                        $"To: {toEmail}\n" +
                                        $"Subject: {subject}\n" +
                                        $"Body:\n{body}\n" +
                                        $"{new string('=', 100)}\n\n";

                File.AppendAllText(EmailLogFile, detailedLogEntry);
            }
            catch
            {
                // Continue if logging fails
            }
        }

        #endregion

        #region Configuration Methods

        /// <summary>
        /// Check if email system is in simulation mode
        /// </summary>
        public static bool IsInSimulationMode()
        {
            return IsSimulationMode;
        }

        /// <summary>
        /// Get email log file path for external access
        /// </summary>
        public static string GetEmailLogPath()
        {
            return EmailLogFile;
        }

        #endregion
    }
}