using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShippingManagementSystem.Models; // If User is in Models folder

namespace ShippingManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetById(int id)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM users WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapUser(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get User By ID", false);
            }
            return null;
        }

        public User GetByUsername(string username)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM users WHERE username = @username";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return MapUser(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get User By Username", false);
            }
            return null;
        }

        public List<User> GetAll()
        {
            var users = new List<User>();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT * FROM users ORDER BY created_date DESC";
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get All Users", false);
            }
            return users;
        }

        public int Create(User user)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = @"INSERT INTO users (username, password_hash, email, phone, company, role) 
                                   VALUES (@username, @passwordHash, @email, @phone, @company, @role);
                                   SELECT LAST_INSERT_ID();";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                        cmd.Parameters.AddWithValue("@email", user.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@phone", user.Phone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@company", user.Company ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@role", user.Role);

                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Create User", false);
                return 0;
            }
        }

        public bool Update(User user)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = @"UPDATE users SET 
                                   email = @email, 
                                   phone = @phone, 
                                   company = @company, 
                                   role = @role,
                                   is_active = @isActive
                                   WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", user.Id);
                        cmd.Parameters.AddWithValue("@email", user.Email ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@phone", user.Phone ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@company", user.Company ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@role", user.Role);
                        cmd.Parameters.AddWithValue("@isActive", user.IsActive);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Update User", false);
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "DELETE FROM users WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Delete User", false);
                return false;
            }
        }

        public bool UsernameExists(string username)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Check Username Exists", false);
                return false;
            }
        }

        private User MapUser(MySqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32("id"),
                Username = reader.GetString("username"),
                PasswordHash = reader.GetString("password_hash"),
                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone"),
                Company = reader.IsDBNull(reader.GetOrdinal("company")) ? null : reader.GetString("company"),
                Role = reader.GetString("role"),
                IsActive = reader.GetBoolean("is_active"),
                CreatedDate = reader.GetDateTime("created_date"),
                ModifiedDate = reader.GetDateTime("modified_date")
            };
        }
    }
}