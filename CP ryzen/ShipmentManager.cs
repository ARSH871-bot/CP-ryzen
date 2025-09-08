using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Database-driven shipment management using MySQL
    /// Replaces in-memory List<Shipment> with proper database operations
    /// </summary>
    public class ShipmentManager
    {
        private readonly DatabaseManager dbManager;
        public string LastError { get; private set; } = "";

        public ShipmentManager()
        {
            dbManager = new DatabaseManager();
        }

        /// <summary>
        /// Create a new shipment in the database
        /// </summary>
        public bool CreateShipment(string description, string destination, string origin,
                                 DateTime dateShipped, DateTime estimatedArrival, int createdBy,
                                 string status = "Pending")
        {
            try
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
                    LastError = "Estimated arrival cannot be before shipped date.";
                    return false;
                }

                // Generate unique tracking number
                string trackingNumber = GenerateTrackingNumber();

                string sql = @"
                    INSERT INTO Shipments (Description, Status, Destination, Origin, DateShipped, 
                                         EstimatedArrival, CreatedBy, TrackingNumber, CreatedDate)
                    VALUES (@description, @status, @destination, @origin, @dateShipped, 
                            @estimatedArrival, @createdBy, @trackingNumber, @createdDate)";

                var parameters = new Dictionary<string, object>
                {
                    {"@description", description},
                    {"@status", status},
                    {"@destination", destination},
                    {"@origin", origin ?? ""},
                    {"@dateShipped", dateShipped},
                    {"@estimatedArrival", estimatedArrival},
                    {"@createdBy", createdBy},
                    {"@trackingNumber", trackingNumber},
                    {"@createdDate", DateTime.Now}
                };

                int result = dbManager.ExecuteNonQuery(sql, parameters);

                if (result > 0)
                {
                    ErrorHandler.LogInfo($"Shipment created successfully: {trackingNumber}", "ShipmentManager");
                    return true;
                }
                else
                {
                    LastError = "Failed to create shipment.";
                    return false;
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
        /// Get all shipments with optional filtering
        /// </summary>
        public List<Shipment> GetAllShipments(string statusFilter = null, string roleFilter = null)
        {
            try
            {
                string sql = @"
                    SELECT s.Id, s.Description, s.Status, s.Destination, s.Origin, 
                           s.DateShipped, s.EstimatedArrival, s.ActualArrival, s.TrackingNumber,
                           s.Weight, s.Dimensions, s.ShippingCost, s.Notes, s.CreatedDate,
                           u.Username as CreatedByName, u.Role
                    FROM Shipments s
                    LEFT JOIN Users u ON s.CreatedBy = u.Id
                    WHERE 1=1";

                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    sql += " AND s.Status = @status";
                    parameters.Add("@status", statusFilter);
                }

                if (!string.IsNullOrEmpty(roleFilter) && roleFilter != "All")
                {
                    sql += " AND u.Role = @role";
                    parameters.Add("@role", roleFilter);
                }

                sql += " ORDER BY s.CreatedDate DESC";

                DataTable result = dbManager.ExecuteQuery(sql, parameters);
                var shipments = new List<Shipment>();

                foreach (DataRow row in result.Rows)
                {
                    shipments.Add(CreateShipmentFromDataRow(row));
                }

                return shipments;
            }
            catch (Exception ex)
            {
                LastError = "Failed to load shipments.";
                ErrorHandler.HandleException(ex, "Get All Shipments", false);
                return new List<Shipment>();
            }
        }

        /// <summary>
        /// Update an existing shipment
        /// </summary>
        public bool UpdateShipment(int shipmentId, string description, string status,
                                 string destination, DateTime dateShipped, DateTime estimatedArrival,
                                 string role = null)
        {
            try
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
                    LastError = "Estimated arrival cannot be before shipped date.";
                    return false;
                }

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

                // Set actual arrival if status is "Delivered"
                if (status.ToLower() == "delivered")
                {
                    sql = sql.Replace("ModifiedDate = @modifiedDate",
                                    "ModifiedDate = @modifiedDate, ActualArrival = @actualArrival");
                    parameters.Add("@actualArrival", DateTime.Now);
                }

                int result = dbManager.ExecuteNonQuery(sql, parameters);

                if (result > 0)
                {
                    ErrorHandler.LogInfo($"Shipment updated successfully: ID {shipmentId}", "ShipmentManager");
                    return true;
                }
                else
                {
                    LastError = "Failed to update shipment.";
                    return false;
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
        /// Delete a shipment
        /// </summary>
        public bool DeleteShipment(int shipmentId)
        {
            try
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
                    LastError = "Failed to delete shipment.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                LastError = "Shipment deletion failed due to system error.";
                ErrorHandler.HandleException(ex, "Delete Shipment", false);
                return false;
            }
        }

        /// <summary>
        /// Get shipment by tracking number
        /// </summary>
        public Shipment GetShipmentByTrackingNumber(string trackingNumber)
        {
            try
            {
                string sql = @"
                    SELECT s.Id, s.Description, s.Status, s.Destination, s.Origin, 
                           s.DateShipped, s.EstimatedArrival, s.ActualArrival, s.TrackingNumber,
                           s.Weight, s.Dimensions, s.ShippingCost, s.Notes, s.CreatedDate,
                           u.Username as CreatedByName, u.Role
                    FROM Shipments s
                    LEFT JOIN Users u ON s.CreatedBy = u.Id
                    WHERE s.TrackingNumber = @trackingNumber";

                var parameters = new Dictionary<string, object>
                {
                    {"@trackingNumber", trackingNumber}
                };

                DataTable result = dbManager.ExecuteQuery(sql, parameters);

                if (result.Rows.Count > 0)
                {
                    return CreateShipmentFromDataRow(result.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipment by Tracking Number", false);
                return null;
            }
        }

        /// <summary>
        /// Get shipments by status for dashboard statistics
        /// </summary>
        public Dictionary<string, int> GetShipmentsByStatus()
        {
            try
            {
                string sql = @"
                    SELECT Status, COUNT(*) as Count 
                    FROM Shipments 
                    GROUP BY Status";

                DataTable result = dbManager.ExecuteQuery(sql);
                var statusCounts = new Dictionary<string, int>();

                foreach (DataRow row in result.Rows)
                {
                    statusCounts[row["Status"].ToString()] = Convert.ToInt32(row["Count"]);
                }

                return statusCounts;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipments by Status", false);
                return new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// Create sample shipments for demonstration (only if no shipments exist)
        /// </summary>
        public void CreateSampleShipmentsIfEmpty(int userId)
        {
            try
            {
                // Check if any shipments exist
                string countSql = "SELECT COUNT(*) FROM Shipments";
                object countResult = dbManager.ExecuteScalar(countSql);

                if (Convert.ToInt32(countResult) == 0)
                {
                    // Create sample shipments
                    CreateShipment("Electronics to NY", "New York", "California",
                                 DateTime.Now.AddDays(-5), DateTime.Now.AddDays(3), userId, "In Transit");

                    CreateShipment("Furniture to CA", "California", "Texas",
                                 DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-2), userId, "Delivered");

                    CreateShipment("Clothing to TX", "Texas", "New York",
                                 DateTime.Now.AddDays(-1), DateTime.Now.AddDays(7), userId, "Pending");

                    ErrorHandler.LogInfo("Sample shipments created", "ShipmentManager");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Create Sample Shipments", false);
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Generate unique tracking number
        /// </summary>
        private string GenerateTrackingNumber()
        {
            string prefix = "RYZ";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string random = new Random().Next(1000, 9999).ToString();
            return $"{prefix}{timestamp}{random}";
        }

        /// <summary>
        /// Create Shipment object from database row, compatible with existing Shipment class
        /// </summary>
        private Shipment CreateShipmentFromDataRow(DataRow row)
        {
            var shipment = new Shipment();

            // Use reflection to safely set properties that exist in your Shipment class
            TrySetProperty(shipment, "ID", Convert.ToInt32(row["Id"]));
            TrySetProperty(shipment, "Id", Convert.ToInt32(row["Id"]));
            TrySetProperty(shipment, "Description", row["Description"].ToString());
            TrySetProperty(shipment, "Status", row["Status"].ToString());
            TrySetProperty(shipment, "Destination", row["Destination"].ToString());
            TrySetProperty(shipment, "DateShipped", Convert.ToDateTime(row["DateShipped"]));
            TrySetProperty(shipment, "EstimatedArrival", Convert.ToDateTime(row["EstimatedArrival"]));
            TrySetProperty(shipment, "Role", row["Role"]?.ToString() ?? "Employee");
            TrySetProperty(shipment, "TrackingNumber", row["TrackingNumber"].ToString());

            return shipment;
        }

        /// <summary>
        /// Safely set property using reflection
        /// </summary>
        private void TrySetProperty(object obj, string propertyName, object value)
        {
            try
            {
                var property = obj.GetType().GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(obj, value);
                }
            }
            catch
            {
                // Ignore if property doesn't exist or can't be set
            }
        }

        #endregion

        /// <summary>
        /// Test database connectivity
        /// </summary>
        public bool TestConnection()
        {
            return dbManager.TestConnection();
        }
    }
}