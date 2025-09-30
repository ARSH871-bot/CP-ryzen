using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ShippingManagementSystem
{
    public class DatabaseConnection
    {
        private static string connectionString;

        static DatabaseConnection()
        {
            string server = ConfigurationManager.AppSettings["DbServer"] ?? "localhost";
            string database = ConfigurationManager.AppSettings["DbName"] ?? "ryzen_shipping";
            string username = ConfigurationManager.AppSettings["DbUsername"] ?? "root";
            string password = ConfigurationManager.AppSettings["DbPassword"] ?? "";

            connectionString = $"Server={server};Database={database};Uid={username};Pwd={password};CharSet=utf8mb4;";
        }

        public static MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database Connection", true);
                throw;
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}