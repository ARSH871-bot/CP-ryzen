using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Enhanced shipment management system with comprehensive email notifications
    /// Supports both database and fallback modes with complete email integration
    /// </summary>
    public class ShipmentManager
    {
        private readonly DatabaseManager dbManager;
        public string LastError { get; private set; } = "";
        private readonly bool isDatabaseMode;

        public ShipmentManager()
        {
            try
            {
                dbManager = new DatabaseManager();
                // Test connection on initialization
                if (dbManager.TestConnection())
                {
                    isDatabaseMode = true;
                    ErrorHandler.LogInfo("ShipmentManager initialized in database mode", "ShipmentManager");
                }
                else
                {
                    throw new Exception("Database connection failed");
                }
            }
            catch (Exception ex)
            {
                isDatabaseMode = false;
                dbManager = null;
                ErrorHandler.LogInfo("ShipmentManager running in fallback mode without database", "ShipmentManager");
            }
        }

        /// <summary>
        /// Create a new shipment with comprehensive email notification
        /// </summary>
        public bool CreateShipment(string description, string destination, string origin,
                                 DateTime dateShipped, DateTime estimatedArrival, int createdBy,
                                 string status = "Pending", int assignedUserId = 0, decimal weight = 0,
                                 string dimensions = "", decimal shippingCost = 0, string notes = "")
        {
            try
            {
                // Validate input
                if (!ValidateShipmentInput(description, destination, dateShipped, estimatedArrival))
                {
                    return false;
                }

                if (isDatabaseMode)
                {
                    return CreateShipmentInDatabase(description, destination, origin, dateShipped,
                        estimatedArrival, createdBy, status, assignedUserId, weight, dimensions, shippingCost, notes);
                }
                else
                {
                    return CreateShipmentInMemory(description, destination, origin, dateShipped,
                        estimatedArrival, createdBy, status);
                }
            }
            catch (Exception ex)
            {
                LastError = "Shipment creation failed due to system error.";
                ErrorHandler.HandleException(ex, "Create Shipment", false);
                return false;
            }
        }

        /// <summary>
        /// Validate shipment input data
        /// </summary>
        private bool ValidateShipmentInput(string description, string destination, DateTime dateShipped, DateTime estimatedArrival)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                LastError = "Description is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                LastError = "Destination is required.";
                return false;
            }

            if (estimatedArrival < dateShipped)
            {
                LastError = "Estimated arrival date cannot be before shipped date.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create shipment in database with enhanced email notification
        /// </summary>
        private bool CreateShipmentInDatabase(string description, string destination, string origin,
                                            DateTime dateShipped, DateTime estimatedArrival, int createdBy,
                                            string status, int assignedUserId, decimal weight,
                                            string dimensions, decimal shippingCost, string notes)
        {
            // Generate unique tracking number
            string trackingNumber = GenerateTrackingNumber();

            string sql = @"
                INSERT INTO Shipments (Description, Status, Destination, Origin, DateShipped, 
                                     EstimatedArrival, AssignedUserId, CreatedBy, TrackingNumber,
                                     Weight, Dimensions, ShippingCost, Notes, CreatedDate, ModifiedDate)
                VALUES (@description, @status, @destination, @origin, @dateShipped, 
                        @estimatedArrival, @assignedUserId, @createdBy, @trackingNumber,
                        @weight, @dimensions, @shippingCost, @notes, @createdDate, @modifiedDate);
                SELECT LAST_INSERT_ID();";

            var parameters = new Dictionary<string, object>
            {
                {"@description", description},
                {"@status", status},
                {"@destination", destination},
                {"@origin", origin ?? ""},
                {"@dateShipped", dateShipped},
                {"@estimatedArrival", estimatedArrival},
                {"@assignedUserId", assignedUserId > 0 ? (object)assignedUserId : DBNull.Value},
                {"@createdBy", createdBy},
                {"@trackingNumber", trackingNumber},
                {"@weight", weight},
                {"@dimensions", dimensions ?? ""},
                {"@shippingCost", shippingCost},
                {"@notes", notes ?? ""},
                {"@createdDate", DateTime.Now},
                {"@modifiedDate", DateTime.Now}
            };

            object result = dbManager.ExecuteScalar(sql, parameters);

            if (result != null && int.TryParse(result.ToString(), out int shipmentId))
            {
                ErrorHandler.LogInfo($"Shipment created successfully: {trackingNumber} (ID: {shipmentId})", "ShipmentManager");

                // Send comprehensive email notifications
                SendShipmentCreatedNotificationAsync(shipmentId, trackingNumber, description, destination,
                    status, dateShipped, estimatedArrival, createdBy, assignedUserId);

                return true;
            }
            else
            {
                LastError = "Failed to create shipment in database.";
                return false;
            }
        }

        /// <summary>
        /// Create shipment in memory (fallback mode) with email notification
        /// </summary>
        private bool CreateShipmentInMemory(string description, string destination, string origin,
                                          DateTime dateShipped, DateTime estimatedArrival, int createdBy, string status)
        {
            // Generate a unique ID for in-memory mode
            int shipmentId = new Random().Next(1000, 9999);
            string trackingNumber = GenerateTrackingNumber();

            ErrorHandler.LogInfo($"Shipment created in memory mode: {trackingNumber} (ID: {shipmentId})", "ShipmentManager");

            // Send email notification for in-memory shipment
            SendShipmentCreatedNotificationAsync(shipmentId, trackingNumber, description, destination,
                status, dateShipped, estimatedArrival, createdBy, 0);

            return true;
        }

        /// <summary>
        /// Get all shipments with optional filtering
        /// </summary>
        public List<Shipment> GetAllShipments(string statusFilter = null)
        {
            try
            {
                if (isDatabaseMode)
                {
                    return GetShipmentsFromDatabase(statusFilter);
                }
                else
                {
                    return GetFallbackShipments(statusFilter);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get All Shipments", false);
                return new List<Shipment>();
            }
        }

        /// <summary>
        /// Get shipments from database with enhanced data mapping
        /// </summary>
        private List<Shipment> GetShipmentsFromDatabase(string statusFilter)
        {
            string sql = @"
                SELECT s.Id, s.Description, s.Status, s.Destination, s.Origin, 
                       s.DateShipped, s.EstimatedArrival, s.ActualArrival,
                       s.AssignedUserId, s.CreatedBy, s.TrackingNumber,
                       s.Weight, s.Dimensions, s.ShippingCost, s.Notes,
                       s.CreatedDate, s.ModifiedDate,
                       u1.Username as AssignedUserName, u1.Email as AssignedUserEmail,
                       u2.Username as CreatedByUserName, u2.Email as CreatedByUserEmail,
                       COALESCE(u1.Role, 'Employee') as Role
                FROM Shipments s
                LEFT JOIN Users u1 ON s.AssignedUserId = u1.Id
                LEFT JOIN Users u2 ON s.CreatedBy = u2.Id";

            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                sql += " WHERE s.Status = @status";
                parameters.Add("@status", statusFilter);
            }

            sql += " ORDER BY s.CreatedDate DESC";

            DataTable result = dbManager.ExecuteQuery(sql, parameters);
            var shipments = new List<Shipment>();

            foreach (DataRow row in result.Rows)
            {
                shipments.Add(new Shipment
                {
                    ID = Convert.ToInt32(row["Id"]),
                    Description = row["Description"].ToString(),
                    Status = row["Status"].ToString(),
                    Destination = row["Destination"].ToString(),
                    DateShipped = Convert.ToDateTime(row["DateShipped"]),
                    EstimatedArrival = Convert.ToDateTime(row["EstimatedArrival"]),
                    Role = row["Role"]?.ToString() ?? "Employee"
                });
            }

            return shipments;
        }

        /// <summary>
        /// Get fallback shipments for testing when database is unavailable
        /// </summary>
        private List<Shipment> GetFallbackShipments(string statusFilter)
        {
            var shipments = new List<Shipment>
            {
                new Shipment
                {
                    ID = 1,
                    Description = "Electronics to NY",
                    Status = "In Transit",
                    Destination = "New York",
                    DateShipped = DateTime.Now.AddDays(-5),
                    EstimatedArrival = DateTime.Now.AddDays(3),
                    Role = "Dispatcher"
                },
                new Shipment
                {
                    ID = 2,
                    Description = "Furniture to CA",
                    Status = "Delivered",
                    Destination = "California",
                    DateShipped = DateTime.Now.AddDays(-10),
                    EstimatedArrival = DateTime.Now.AddDays(-2),
                    Role = "Manager"
                },
                new Shipment
                {
                    ID = 3,
                    Description = "Clothing to TX",
                    Status = "Pending",
                    Destination = "Texas",
                    DateShipped = DateTime.Now.AddDays(-1),
                    EstimatedArrival = DateTime.Now.AddDays(7),
                    Role = "Employee"
                }
            };

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                shipments = shipments.Where(s => s.Status == statusFilter).ToList();
            }

            return shipments;
        }

        /// <summary>
        /// Update shipment with comprehensive email notification
        /// </summary>
        public bool UpdateShipment(int shipmentId, string description, string status,
                                 string destination, DateTime dateShipped, DateTime estimatedArrival,
                                 string role = "Employee")
        {
            try
            {
                if (!ValidateShipmentInput(description, destination, dateShipped, estimatedArrival))
                {
                    return false;
                }

                if (isDatabaseMode)
                {
                    return UpdateShipmentInDatabase(shipmentId, description, status, destination,
                        dateShipped, estimatedArrival, role);
                }
                else
                {
                    ErrorHandler.LogInfo($"Shipment update simulated in memory mode: ID {shipmentId}", "ShipmentManager");

                    // Send simulated email notification
                    SendStatusChangeNotificationAsync(shipmentId, description, destination, status, "Pending");

                    return true;
                }
            }
            catch (Exception ex)
            {
                LastError = "Shipment update failed due to system error.";
                ErrorHandler.HandleException(ex, "Update Shipment", false);
                return false;
            }
        }

        /// <summary>
        /// Update shipment in database with enhanced status change notification
        /// </summary>
        private bool UpdateShipmentInDatabase(int shipmentId, string description, string status,
                                            string destination, DateTime dateShipped, DateTime estimatedArrival, string role)
        {
            // Get current shipment data for comparison
            string getCurrentSql = @"
                SELECT Status, Description, Destination, TrackingNumber
                FROM Shipments WHERE Id = @shipmentId";
            var getCurrentParams = new Dictionary<string, object> { { "@shipmentId", shipmentId } };
            DataTable currentData = dbManager.ExecuteQuery(getCurrentSql, getCurrentParams);

            if (currentData.Rows.Count == 0)
            {
                LastError = "Shipment not found.";
                return false;
            }

            string oldStatus = currentData.Rows[0]["Status"].ToString();
            string oldDescription = currentData.Rows[0]["Description"].ToString();
            string trackingNumber = currentData.Rows[0]["TrackingNumber"].ToString();

            string sql = @"
                UPDATE Shipments 
                SET Description = @description, Status = @status, Destination = @destination,
                    DateShipped = @dateShipped, EstimatedArrival = @estimatedArrival,
                    ModifiedDate = @modifiedDate
                WHERE Id = @shipmentId";

            var parameters = new Dictionary<string, object>
            {
                {"@description", description},
                {"@status", status},
                {"@destination", destination},
                {"@dateShipped", dateShipped},
                {"@estimatedArrival", estimatedArrival},
                {"@modifiedDate", DateTime.Now},
                {"@shipmentId", shipmentId}
            };

            int result = dbManager.ExecuteNonQuery(sql, parameters);

            if (result > 0)
            {
                ErrorHandler.LogInfo($"Shipment updated successfully: ID {shipmentId}, Tracking: {trackingNumber}", "ShipmentManager");

                // Send appropriate email notifications
                bool statusChanged = !string.IsNullOrEmpty(oldStatus) && oldStatus != status;
                bool significantChange = oldDescription != description || statusChanged;

                if (significantChange)
                {
                    if (statusChanged)
                    {
                        SendStatusChangeNotificationAsync(shipmentId, description, destination, status, oldStatus);
                    }
                    else
                    {
                        SendShipmentUpdateNotificationAsync(shipmentId, description, destination, status);
                    }
                }

                return true;
            }
            else
            {
                LastError = "No changes were made to the shipment.";
                return false;
            }
        }

        /// <summary>
        /// Delete shipment from database
        /// </summary>
        public bool DeleteShipment(int shipmentId)
        {
            try
            {
                if (isDatabaseMode)
                {
                    string sql = "DELETE FROM Shipments WHERE Id = @shipmentId";
                    var parameters = new Dictionary<string, object>
                    {
                        {"@shipmentId", shipmentId}
                    };

                    int result = dbManager.ExecuteNonQuery(sql, parameters);

                    if (result > 0)
                    {
                        ErrorHandler.LogInfo($"Shipment deleted successfully: ID {shipmentId}", "ShipmentManager");
                        return true;
                    }
                    else
                    {
                        LastError = "Shipment not found.";
                        return false;
                    }
                }
                else
                {
                    ErrorHandler.LogInfo($"Shipment deletion simulated in memory mode: ID {shipmentId}", "ShipmentManager");
                    return true;
                }
            }
            catch (Exception ex)
            {
                LastError = "Shipment deletion failed due to system error.";
                ErrorHandler.HandleException(ex, "Delete Shipment", false);
                return false;
            }
        }

        #region Enhanced Email Notification Methods

        /// <summary>
        /// Send comprehensive shipment creation notification
        /// </summary>
        private async void SendShipmentCreatedNotificationAsync(int shipmentId, string trackingNumber,
            string description, string destination, string status, DateTime dateShipped,
            DateTime estimatedArrival, int createdBy, int assignedUserId)
        {
            await Task.Run(async () =>
            {
                try
                {
                    // Get email addresses for notifications
                    var emailAddresses = GetUserEmails(createdBy, assignedUserId);

                    if (emailAddresses.Count > 0)
                    {
                        var shipment = new Shipment
                        {
                            ID = shipmentId,
                            Description = description,
                            Status = status,
                            Destination = destination,
                            DateShipped = dateShipped,
                            EstimatedArrival = estimatedArrival
                        };

                        // Send creation email to all relevant users
                        foreach (string email in emailAddresses)
                        {
                            bool emailSent = await EmailManager.SendShipmentCreatedEmail(email, shipment);
                            if (emailSent)
                            {
                                ErrorHandler.LogInfo($"Shipment creation email sent to {email} for tracking {trackingNumber}", "ShipmentManager");
                            }
                        }
                    }
                    else
                    {
                        ErrorHandler.LogWarning($"No email addresses found for shipment creation notification: {trackingNumber}", "ShipmentManager");
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleException(ex, "Send Shipment Created Notification", false);
                }
            });
        }

        /// <summary>
        /// Send status change notification with enhanced details
        /// </summary>
        private async void SendStatusChangeNotificationAsync(int shipmentId, string description,
            string destination, string newStatus, string oldStatus)
        {
            await Task.Run(async () =>
            {
                try
                {
                    // Get related user emails
                    var emailAddresses = GetShipmentUserEmails(shipmentId);

                    if (emailAddresses.Count > 0)
                    {
                        var shipment = new Shipment
                        {
                            ID = shipmentId,
                            Description = description,
                            Status = newStatus,
                            Destination = destination
                        };

                        foreach (string email in emailAddresses)
                        {
                            // Send status update email
                            bool statusEmailSent = await EmailManager.SendShipmentStatusUpdateEmail(email, shipment);
                            if (statusEmailSent)
                            {
                                ErrorHandler.LogInfo($"Status change email sent to {email} for shipment {shipmentId}: {oldStatus} → {newStatus}", "ShipmentManager");
                            }

                            // Send delivery confirmation if status is "Delivered"
                            if (newStatus.ToLower() == "delivered")
                            {
                                bool deliveryEmailSent = await EmailManager.SendDeliveryConfirmationEmail(email, shipment);
                                if (deliveryEmailSent)
                                {
                                    ErrorHandler.LogInfo($"Delivery confirmation email sent to {email} for shipment {shipmentId}", "ShipmentManager");
                                }
                            }
                        }
                    }
                    else
                    {
                        ErrorHandler.LogWarning($"No email addresses found for status change notification: shipment {shipmentId}", "ShipmentManager");
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleException(ex, "Send Status Change Notification", false);
                }
            });
        }

        /// <summary>
        /// Send general shipment update notification
        /// </summary>
        private async void SendShipmentUpdateNotificationAsync(int shipmentId, string description,
            string destination, string status)
        {
            await Task.Run(async () =>
            {
                try
                {
                    var emailAddresses = GetShipmentUserEmails(shipmentId);

                    if (emailAddresses.Count > 0)
                    {
                        var shipment = new Shipment
                        {
                            ID = shipmentId,
                            Description = description,
                            Status = status,
                            Destination = destination
                        };

                        foreach (string email in emailAddresses)
                        {
                            bool emailSent = await EmailManager.SendShipmentUpdatedEmail(email, shipment);
                            if (emailSent)
                            {
                                ErrorHandler.LogInfo($"Shipment update email sent to {email} for shipment {shipmentId}", "ShipmentManager");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleException(ex, "Send Shipment Update Notification", false);
                }
            });
        }

        /// <summary>
        /// Get user emails for notification with enhanced error handling
        /// </summary>
        private List<string> GetUserEmails(int createdBy, int assignedUserId)
        {
            var emails = new List<string>();

            try
            {
                if (isDatabaseMode && dbManager != null)
                {
                    string sql = @"
                        SELECT DISTINCT Email FROM Users 
                        WHERE (Id = @createdBy OR Id = @assignedUserId) 
                        AND Email IS NOT NULL AND Email != '' AND IsActive = 1";

                    var parameters = new Dictionary<string, object>
                    {
                        {"@createdBy", createdBy},
                        {"@assignedUserId", assignedUserId}
                    };

                    DataTable result = dbManager.ExecuteQuery(sql, parameters);

                    foreach (DataRow row in result.Rows)
                    {
                        string email = row["Email"].ToString().Trim();
                        if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
                        {
                            emails.Add(email);
                        }
                    }
                }
                else
                {
                    // Fallback for testing - use demo emails
                    emails.Add("demo@ryzenshipment.com");
                    emails.Add("test@example.com");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get User Emails", false);
            }

            return emails;
        }

        /// <summary>
        /// Get shipment-related user emails with enhanced filtering
        /// </summary>
        private List<string> GetShipmentUserEmails(int shipmentId)
        {
            var emails = new List<string>();

            try
            {
                if (isDatabaseMode && dbManager != null)
                {
                    string sql = @"
                        SELECT DISTINCT u.Email 
                        FROM Users u
                        INNER JOIN Shipments s ON (u.Id = s.CreatedBy OR u.Id = s.AssignedUserId)
                        WHERE s.Id = @shipmentId 
                        AND u.Email IS NOT NULL AND u.Email != '' AND u.IsActive = 1";

                    var parameters = new Dictionary<string, object> { { "@shipmentId", shipmentId } };
                    DataTable result = dbManager.ExecuteQuery(sql, parameters);

                    foreach (DataRow row in result.Rows)
                    {
                        string email = row["Email"].ToString().Trim();
                        if (!string.IsNullOrEmpty(email) && IsValidEmail(email))
                        {
                            emails.Add(email);
                        }
                    }
                }
                else
                {
                    // Fallback for testing
                    emails.Add("demo@ryzenshipment.com");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipment User Emails", false);
            }

            return emails;
        }

        /// <summary>
        /// Validate email address format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Generate unique tracking number with enhanced format
        /// </summary>
        private string GenerateTrackingNumber()
        {
            string prefix = "RYZ";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string random = new Random().Next(1000, 9999).ToString();
            return $"{prefix}{timestamp}{random}";
        }

        /// <summary>
        /// Get shipment statistics for dashboard
        /// </summary>
        public Dictionary<string, int> GetShipmentStatistics()
        {
            var stats = new Dictionary<string, int>();

            try
            {
                var allShipments = GetAllShipments();

                stats["Total"] = allShipments.Count;
                stats["Pending"] = allShipments.Count(s => s.Status == "Pending");
                stats["InTransit"] = allShipments.Count(s => s.Status == "In Transit");
                stats["Delivered"] = allShipments.Count(s => s.Status == "Delivered");
                stats["Cancelled"] = allShipments.Count(s => s.Status == "Cancelled");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipment Statistics", false);
            }

            return stats;
        }

        /// <summary>
        /// Test database connectivity
        /// </summary>
        public bool TestConnection()
        {
            return isDatabaseMode && dbManager?.TestConnection() == true;
        }

        #endregion
    }
}