using System.Collections.Generic;
using ShippingManagementSystem.Models;

namespace ShippingManagementSystem.Repositories
{
    public interface IShipmentRepository
    {
        Shipment GetById(int id);
        List<Shipment> GetAll();
        List<Shipment> GetByStatus(string status);
        List<Shipment> GetByUser(int userId);
        int Create(Shipment shipment);
        bool Update(Shipment shipment);
        bool Delete(int id);
        Dictionary<string, int> GetStatusCounts();
    }
}