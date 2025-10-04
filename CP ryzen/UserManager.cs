using System;
using System.Collections.Generic;
using System.Data;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Database-driven user management - Clean implementation
    /// </summary>
    public class UserManager
    {
        private readonly DatabaseManager dbManager;
        public string LastError { get; private set; } = "";

        public UserManager()
        {
            dbManager = new DatabaseManager();
        }

        /// <summary>
        /// Register new user in database
        /// </summary>
        public bool RegisterUser(string username, string password, string email = "",
                               string phone = "", string company = "", string role = "Employee")
        {
            try
            {
                // Validate username
                if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
                {
                    LastError = "Username must be at least 3 characters long.";
                    return false;
                }

                // Validate password
                string passwordValidation = SecurityManager.ValidatePasswordDetailed(password);
                if (!string.IsNullOrEmpty(passwordValidation))
                {
                    LastError = passwordValidation;
                    return false;
                }

                // Check if user exists
                if (UserExists(username))
                {
                    LastError = "Username already exists.";
                    return false;
                }

                // Hash password
                string hashedPassword = SecurityManager.HashPassword(password);

                // Insert user
                string sql = @"
                    INSERT INTO Users (Username, PasswordHash, Email, Phone, Company, Role, CreatedDate)
                    VALUES (@username, @passwordHash, @email, @phone, @company, @role, @createdDate)";

                var parameters = new Dictionary<string, object>
                {
                    {"@username", username},
                    {"@passwordHash", hashedPassword},
                    {"@email", email ?? ""},
                    {"@phone", phone ?? ""},
                    {"@company", company ?? ""},
                    {"@role", role},
                    {"@createdDate", DateTime.Now}
                };

                int result = dbManager.ExecuteNonQuery(sql, parameters);

                if (result > 0)
                {
                    ErrorHandler.LogInfo($"User registered: {username}", "UserManager");
                    return true;
                }

                LastError = "Failed to save user.";
                return false;
            }
            catch (Exception ex)
            {
                LastError = "Registration failed.";
                ErrorHandler.HandleException(ex, "User Registration", false);
                return false;
            }
        }

        /// <summary>
        /// Authenticate user against database
        /// </summary>
        public User AuthenticateUser(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    LastError = "Username and password are required.";
                    return null;
                }

                // Check lockout
                if (SecurityManager.IsAccountLocked(username))
                {
                    var remaining = SecurityManager.GetRemainingLockoutTime(username);
                    LastError = $"Account locked. Try again in {remaining.Minutes}m {remaining.Seconds}s.";
                    return null;
                }

                // Query user
                string sql = @"
                    SELECT Id, Username, PasswordHash, Role, Email, Phone, Company
                    FROM Users 
                    WHERE Username = @username AND IsActive = 1";

                var parameters = new Dictionary<string, object> { { "@username", username } };
                DataTable result = dbManager.ExecuteQuery(sql, parameters);

                if (result.Rows.Count == 0)
                {
                    LastError = "Invalid username or password.";
                    SecurityManager.RecordFailedAttempt(username);
                    return null;
                }

                DataRow row = result.Rows[0];
                string storedHash = row["PasswordHash"].ToString();

                // Verify password
                if (!SecurityManager.VerifyPassword(password, storedHash))
                {
                    LastError = "Invalid username or password.";
                    SecurityManager.RecordFailedAttempt(username);
                    return null;
                }

                // Success - clear failed attempts
                SecurityManager.ClearFailedAttempts(username);
                UpdateLastLogin(Convert.ToInt32(row["Id"]));

                // Return User object
                return new User
                {
                    Username = row["Username"].ToString(),
                    Role = row["Role"].ToString()
                };
            }
            catch (Exception ex)
            {
                LastError = "Authentication failed.";
                ErrorHandler.HandleException(ex, "Authentication", false);
                return null;
            }
        }

        private bool UserExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            var parameters = new Dictionary<string, object> { { "@username", username } };
            object result = dbManager.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }

        private void UpdateLastLogin(int userId)
        {
            try
            {
                string sql = "UPDATE Users SET LastLogin = @lastLogin WHERE Id = @userId";
                var parameters = new Dictionary<string, object>
                {
                    {"@lastLogin", DateTime.Now},
                    {"@userId", userId}
                };
                dbManager.ExecuteNonQuery(sql, parameters);
            }
            catch { }
        }
    }
}