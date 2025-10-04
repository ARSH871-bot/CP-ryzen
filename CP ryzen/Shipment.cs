using System;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Shipment data model - single definition for entire application
    /// </summary>
    public class Shipment
    {
        // Primary Key
        public int ID { get; set; }

        // Core shipment information
        public string Description { get; set; }
        public string Status { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }

        // Dates
        public DateTime DateShipped { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }

        // Tracking
        public string TrackingNumber { get; set; }

        // Additional details
        public decimal? Weight { get; set; }
        public string Dimensions { get; set; }
        public decimal? ShippingCost { get; set; }
        public string Notes { get; set; }

        // User assignment (for compatibility with UI)
        public string Role { get; set; }

        // Audit fields
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? AssignedUserId { get; set; }

        // Display helpers
        public override string ToString()
        {
            return $"#{ID}: {Description} - {Status}";
        }
    }
}