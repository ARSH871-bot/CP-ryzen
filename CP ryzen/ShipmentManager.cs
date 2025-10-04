using System;
using System.Collections.Generic;
using System.Data;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Database-driven shipment management - Fixed signatures
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
        /// Create shipment - matches frmManageShipments expected signature
        /// </summary>
        public int CreateShipment(string description, string destination, string origin,
                                 DateTime dateShipped, DateTime estimatedArrival, int createdBy,
                                 string status = "Pending")
        {
            try
            {
                string trackingNumber = GenerateTrackingNumber();

                string sql = @"
                    INSERT INTO Shipments (Description, Status, Destination, Origin, DateShipped, 
                                         EstimatedArrival, CreatedBy, TrackingNumber, CreatedDate)
                    VALUES (@description, @status, @destination, @origin, @dateShipped, 
                            @estimatedArrival, @createdBy, @trackingNumber, @createdDate);
                    SELECT LAST_INSERT_ID();";

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

                object result = dbManager.ExecuteScalar(sql, parameters);

                if (result != null)
                {
                    int shipmentId = Convert.ToInt32(result);
                    ErrorHandler.LogInfo($"Shipment created: ID {shipmentId}, Tracking: {trackingNumber}", "ShipmentManager");
                    return shipmentId;
                }

                LastError = "Failed to create shipment.";
                return -1;
            }
            catch (Exception ex)
            {
                LastError = "Shipment creation failed.";
                ErrorHandler.HandleException(ex, "Create Shipment", false);
                return -1;
            }
        }

        /// <summary>
        /// Update shipment - matches frmManageShipments expected signature
        /// </summary>
        public bool UpdateShipment(int shipmentId, string description, string status,
                                 string destination, DateTime dateShipped, DateTime estimatedArrival,
                                 string role = null)
        {
            try
            {
                string sql = @"
                    UPDATE Shipments 
                    SET Description = @description, 
                        Status = @status, 
                        Destination = @destination,
                        DateShipped = @dateShipped, 
                        EstimatedArrival = @estimatedArrival,
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

                // Set actual arrival if delivered
                if (status.ToLower() == "delivered")
                {
                    sql = sql.Replace("ModifiedDate = @modifiedDate",
                                    "ModifiedDate = @modifiedDate, ActualArrival = @actualArrival");
                    parameters.Add("@actualArrival", DateTime.Now);
                }

                int result = dbManager.ExecuteNonQuery(sql, parameters);

                if (result > 0)
                {
                    ErrorHandler.LogInfo($"Shipment updated: ID {shipmentId}", "ShipmentManager");
                    return true;
                }

                LastError = "Failed to update shipment.";
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Update failed.";
                ErrorHandler.HandleException(ex, "Update Shipment", false);
                return false;
            }
        }

        /// <summary>
        /// Get all shipments with optional filtering
        /// </summary>
        public List<Shipment> GetAllShipments(string roleFilter = null)
        {
            try
            {
                string sql = @"
                    SELECT s.*, u.Role
                    FROM Shipments s
                    LEFT JOIN Users u ON s.CreatedBy = u.Id
                    WHERE 1=1";

                var parameters = new Dictionary<string, object>();

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
                    shipments.Add(MapRowToShipment(row));
                }

                return shipments;
            }
            catch (Exception ex)
            {
                LastError = "Failed to load shipments.";
                ErrorHandler.HandleException(ex, "Get Shipments", false);
                return new List<Shipment>();
            }
        }

        /// <summary>
        /// Delete shipment
        /// </summary>
        public bool DeleteShipment(int shipmentId)
        {
            try
            {
                string sql = "DELETE FROM Shipments WHERE Id = @shipmentId";
                var parameters = new Dictionary<string, object> { { "@shipmentId", shipmentId } };

                int result = dbManager.ExecuteNonQuery(sql, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                LastError = "Delete failed.";
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
                string sql = "SELECT s.*, u.Role FROM Shipments s LEFT JOIN Users u ON s.CreatedBy = u.Id WHERE s.TrackingNumber = @trackingNumber";
                var parameters = new Dictionary<string, object> { { "@trackingNumber", trackingNumber } };

                DataTable result = dbManager.ExecuteQuery(sql, parameters);

                if (result.Rows.Count > 0)
                {
                    return MapRowToShipment(result.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get by Tracking", false);
                return null;
            }
        }

        private Shipment MapRowToShipment(DataRow row)
        {
            return new Shipment
            {
                ID = Convert.ToInt32(row["Id"]),
                Description = row["Description"].ToString(),
                Status = row["Status"].ToString(),
                Destination = row["Destination"].ToString(),
                Origin = row["Origin"]?.ToString(),
                DateShipped = Convert.ToDateTime(row["DateShipped"]),
                EstimatedArrival = Convert.ToDateTime(row["EstimatedArrival"]),
                TrackingNumber = row["TrackingNumber"]?.ToString(),
                Role = row["Role"]?.ToString() ?? "Employee",
                CreatedDate = Convert.ToDateTime(row["CreatedDate"])
            };
        }

        private string GenerateTrackingNumber()
        {
            return $"RYZ{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }
    }
}