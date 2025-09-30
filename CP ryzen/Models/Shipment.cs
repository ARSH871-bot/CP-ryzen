using System;

namespace ShippingManagementSystem.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DateShipped { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public DateTime? ActualArrival { get; set; }
        public decimal? Weight { get; set; }
        public string Dimensions { get; set; }
        public decimal? ShippingCost { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        // Legacy property - fully settable for backward compatibility
        public string Role { get; set; }

        // Make ID settable, synced with Id
        private int _id;
        public int ID
        {
            get => _id > 0 ? _id : Id;
            set { _id = value; Id = value; }
        }

        public override string ToString()
        {
            return $"ID {Id}: {Description} - {Status}";
        }
    }
}