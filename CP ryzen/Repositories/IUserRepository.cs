using System.Collections.Generic;
using ShippingManagementSystem.Models;

namespace ShippingManagementSystem.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        User GetByUsername(string username);
        List<User> GetAll();
        int Create(User user);
        bool Update(User user);
        bool Delete(int id);
        bool UsernameExists(string username);
    }
}