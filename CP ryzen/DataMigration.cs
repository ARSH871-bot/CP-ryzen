using Newtonsoft.Json;
using ShippingManagementSystem.Models;
using ShippingManagementSystem.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace ShippingManagementSystem
{
    public class DataMigration
    {
        private readonly string dbFolder = Path.Combine(Directory.GetCurrentDirectory(), "DB");
        private readonly UserRepository userRepository;

        public DataMigration()
        {
            userRepository = new UserRepository();
        }

        public bool MigrateUsersFromJson()
        {
            try
            {
                string customerFile = Path.Combine(dbFolder, "customer.json");

                if (!File.Exists(customerFile))
                {
                    ErrorHandler.LogInfo("No customer.json found to migrate", "Migration");
                    return true;
                }

                string json = File.ReadAllText(customerFile);
                var jsonUsers = JsonConvert.DeserializeObject<List<JsonUser>>(json);

                if (jsonUsers == null || jsonUsers.Count == 0)
                {
                    return true;
                }

                int migrated = 0;
                foreach (var jsonUser in jsonUsers)
                {
                    // Check if user already exists
                    if (userRepository.UsernameExists(jsonUser.USERNAME))
                    {
                        continue;
                    }

                    var user = new User
                    {
                        Username = jsonUser.USERNAME,
                        PasswordHash = jsonUser.PASSWORD, // Already hashed from JSON
                        Email = jsonUser.EMAIL,
                        Phone = jsonUser.PHONE,
                        Company = jsonUser.COMPANY,
                        Role = "Employee" // Default role
                    };

                    int userId = userRepository.Create(user);
                    if (userId > 0)
                    {
                        migrated++;
                        ErrorHandler.LogInfo($"Migrated user: {user.Username}", "Migration");
                    }
                }

                ErrorHandler.ShowInfo($"Successfully migrated {migrated} users from JSON to database", "Migration Complete");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Migrate Users", true);
                return false;
            }
        }

        // Helper class for JSON deserialization
        private class JsonUser
        {
            public string USERNAME { get; set; }
            public string PASSWORD { get; set; }
            public string EMAIL { get; set; }
            public string PHONE { get; set; }
            public string COMPANY { get; set; }
        }
    }
}