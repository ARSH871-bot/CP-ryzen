using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShippingManagementSystem.Models;

namespace ShippingManagementSystem.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        public Shipment GetById(int id)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM shipments WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapShipment(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipment By ID", false);
            }
            return null;
        }

        public List<Shipment> GetAll()
        {
            var shipments = new List<Shipment>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM shipments ORDER BY created_date DESC";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shipments.Add(MapShipment(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get All Shipments", false);
            }
            return shipments;
        }

        public List<Shipment> GetByStatus(string status)
        {
            var shipments = new List<Shipment>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM shipments WHERE status = @status ORDER BY created_date DESC";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                shipments.Add(MapShipment(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipments By Status", false);
            }
            return shipments;
        }

        public List<Shipment> GetByUser(int userId)
        {
            var shipments = new List<Shipment>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM shipments WHERE created_by = @userId ORDER BY created_date DESC";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                shipments.Add(MapShipment(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipments By User", false);
            }
            return shipments;
        }

        public int Create(Shipment shipment)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    // Generate tracking number if not provided
                    if (string.IsNullOrEmpty(shipment.TrackingNumber))
                    {
                        shipment.TrackingNumber = $"RYZEN{DateTime.Now:yyyyMMddHHmmss}";
                    }

                    string query = @"INSERT INTO shipments 
                                   (tracking_number, description, status, origin, destination, 
                                    date_shipped, estimated_arrival, created_by) 
                                   VALUES 
                                   (@trackingNumber, @description, @status, @origin, @destination, 
                                    @dateShipped, @estimatedArrival, @createdBy);
                                   SELECT LAST_INSERT_ID();";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@trackingNumber", shipment.TrackingNumber);
                        cmd.Parameters.AddWithValue("@description", shipment.Description);
                        cmd.Parameters.AddWithValue("@status", shipment.Status);
                        cmd.Parameters.AddWithValue("@origin", shipment.Origin ?? "Not specified");
                        cmd.Parameters.AddWithValue("@destination", shipment.Destination);
                        cmd.Parameters.AddWithValue("@dateShipped", shipment.DateShipped);
                        cmd.Parameters.AddWithValue("@estimatedArrival", shipment.EstimatedArrival);
                        cmd.Parameters.AddWithValue("@createdBy", shipment.CreatedBy);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Create Shipment", false);
                return 0;
            }
        }

        public bool Update(Shipment shipment)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = @"UPDATE shipments SET 
                                   description = @description,
                                   status = @status,
                                   origin = @origin,
                                   destination = @destination,
                                   date_shipped = @dateShipped,
                                   estimated_arrival = @estimatedArrival,
                                   actual_arrival = @actualArrival,
                                   weight = @weight,
                                   notes = @notes
                                   WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", shipment.Id);
                        cmd.Parameters.AddWithValue("@description", shipment.Description);
                        cmd.Parameters.AddWithValue("@status", shipment.Status);
                        cmd.Parameters.AddWithValue("@origin", shipment.Origin ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@destination", shipment.Destination);
                        cmd.Parameters.AddWithValue("@dateShipped", shipment.DateShipped);
                        cmd.Parameters.AddWithValue("@estimatedArrival", shipment.EstimatedArrival);
                        cmd.Parameters.AddWithValue("@actualArrival", shipment.ActualArrival ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@weight", shipment.Weight ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@notes", shipment.Notes ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Update Shipment", false);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "DELETE FROM shipments WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Delete Shipment", false);
                return false;
            }
        }

        public Dictionary<string, int> GetStatusCounts()
        {
            var counts = new Dictionary<string, int>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT status, COUNT(*) as count FROM shipments GROUP BY status";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            counts[reader.GetString("status")] = reader.GetInt32("count");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Status Counts", false);
            }
            return counts;
        }

        private Shipment MapShipment(MySqlDataReader reader)
        {
            return new Shipment
            {
                Id = reader.GetInt32("id"),
                TrackingNumber = reader.IsDBNull(reader.GetOrdinal("tracking_number")) ? null : reader.GetString("tracking_number"),
                Description = reader.GetString("description"),
                Status = reader.GetString("status"),
                Origin = reader.IsDBNull(reader.GetOrdinal("origin")) ? null : reader.GetString("origin"),
                Destination = reader.GetString("destination"),
                DateShipped = reader.GetDateTime("date_shipped"),
                EstimatedArrival = reader.GetDateTime("estimated_arrival"),
                ActualArrival = reader.IsDBNull(reader.GetOrdinal("actual_arrival")) ? (DateTime?)null : reader.GetDateTime("actual_arrival"),
                Weight = reader.IsDBNull(reader.GetOrdinal("weight")) ? (decimal?)null : reader.GetDecimal("weight"),
                Dimensions = reader.IsDBNull(reader.GetOrdinal("dimensions")) ? null : reader.GetString("dimensions"),
                ShippingCost = reader.IsDBNull(reader.GetOrdinal("shipping_cost")) ? (decimal?)null : reader.GetDecimal("shipping_cost"),
                Notes = reader.IsDBNull(reader.GetOrdinal("notes")) ? null : reader.GetString("notes"),
                CreatedBy = reader.GetInt32("created_by"),
                CreatedDate = reader.GetDateTime("created_date"),
                ModifiedDate = reader.GetDateTime("modified_date")
            };
        }
    }
}