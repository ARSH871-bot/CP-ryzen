using System;
using System.Collections.Generic;
using System.Data;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Database-driven user manager using MySQL DatabaseManager
    /// Compatible with existing User class definition
    /// </summary>
    public class UserManager
    {
        private readonly DatabaseManager dbManager;
        public string LastError { get; private set; } = "";

        public UserManager()
        {
            dbManager = new DatabaseManager(); // Use MySQL database instead of JSON files
        }

        /// <summary>
        /// Register user with enhanced password validation and database storage
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

                // Validate password strength
                if (!SecurityManager.ValidatePasswordStrength(password))
                {
                    LastError = SecurityManager.GetPasswordRequirements();
                    return false;
                }

                // Check if user exists in database
                if (UserExists(username))
                {
                    LastError = "Username already exists.";
                    return false;
                }

                // Check if email already exists (if provided)
                if (!string.IsNullOrEmpty(email) && EmailExists(email))
                {
                    LastError = "Email address already registered.";
                    return false;
                }

                // Hash password for secure storage
                string hashedPassword = SecurityManager.HashPassword(password);

                // Insert user into database
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
                    ErrorHandler.LogInfo($"User registered successfully in database: {username}", "UserManager");
                    return true;
                }
                else
                {
                    LastError = "Failed to save user data to database.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                LastError = "Registration failed due to system error.";
                ErrorHandler.HandleException(ex, "User Registration", false);
                return false;
            }
        }

        /// <summary>
        /// Authenticate user with database lookup and security features
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

                // Check if account is locked
                if (SecurityManager.IsAccountLocked(username))
                {
                    var remaining = SecurityManager.GetRemainingLockoutTime(username);
                    LastError = $"Account is locked. Try again in {remaining.Minutes} minutes and {remaining.Seconds} seconds.";
                    return null;
                }

                // Query database for user
                string sql = @"
                    SELECT Id, Username, PasswordHash, Email, Phone, Company, Role, IsActive, LastLogin,
                           FailedLoginAttempts, AccountLockedUntil
                    FROM Users 
                    WHERE Username = @username AND IsActive = 1";

                var parameters = new Dictionary<string, object>
                {
                    {"@username", username}
                };

                DataTable result = dbManager.ExecuteQuery(sql, parameters);

                if (result.Rows.Count == 0)
                {
                    LastError = "Invalid username or password.";
                    SecurityManager.RecordFailedAttempt(username);
                    return null;
                }

                DataRow row = result.Rows[0];
                string storedHash = row["PasswordHash"].ToString();

                // Verify password against stored hash
                bool isValidPassword = SecurityManager.VerifyPassword(password, storedHash);

                if (!isValidPassword)
                {
                    LastError = "Invalid username or password.";
                    SecurityManager.RecordFailedAttempt(username);

                    // Update failed login attempts in database
                    UpdateFailedLoginAttempts(Convert.ToInt32(row["Id"]));
                    return null;
                }

                // Clear failed attempts on successful login
                SecurityManager.ClearFailedAttempts(username);

                // Update last login time in database
                UpdateLastLogin(Convert.ToInt32(row["Id"]));

                // Create User object using existing User class definition
                var user = CreateUserFromDataRow(row);

                ErrorHandler.LogInfo($"User authenticated successfully from database: {username}", "UserManager");
                return user;
            }
            catch (Exception ex)
            {
                LastError = "Authentication failed due to system error.";
                ErrorHandler.HandleException(ex, "User Authentication", false);
                return null;
            }
        }

        /// <summary>
        /// Create User object from database row, compatible with existing User class
        /// </summary>
        private User CreateUserFromDataRow(DataRow row)
        {
            var user = new User();

            // Use reflection to safely set properties that exist in your User class
            var userType = typeof(User);

            // Set Username property
            var usernameProperty = userType.GetProperty("Username");
            if (usernameProperty != null)
            {
                usernameProperty.SetValue(user, row["Username"].ToString());
            }

            // Set Role property  
            var roleProperty = userType.GetProperty("Role");
            if (roleProperty != null)
            {
                roleProperty.SetValue(user, row["Role"].ToString());
            }

            // Try to set other properties if they exist
            TrySetProperty(user, "Email", row["Email"].ToString());
            TrySetProperty(user, "Phone", row["Phone"].ToString());
            TrySetProperty(user, "Company", row["Company"].ToString());

            // Try to set Id if it exists
            TrySetProperty(user, "Id", Convert.ToInt32(row["Id"]));

            return user;
        }

        /// <summary>
        /// Safely set property using reflection
        /// </summary>
        private void TrySetProperty(object obj, string propertyName, object value)
        {
            try
            {
                var property = obj.GetType().GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(obj, value);
                }
            }
            catch
            {
                // Ignore if property doesn't exist or can't be set
            }
        }

        #region Database Helper Methods

        /// <summary>
        /// Check if username exists in database
        /// </summary>
        private bool UserExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @username";
            var parameters = new Dictionary<string, object> { { "@username", username } };

            object result = dbManager.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Check if email exists in database
        /// </summary>
        private bool EmailExists(string email)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Email = @email AND Email != ''";
            var parameters = new Dictionary<string, object> { { "@email", email } };

            object result = dbManager.ExecuteScalar(sql, parameters);
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Update last login time in database
        /// </summary>
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
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Update Last Login", false);
            }
        }

        /// <summary>
        /// Update failed login attempts in database
        /// </summary>
        private void UpdateFailedLoginAttempts(int userId)
        {
            try
            {
                string sql = @"
                    UPDATE Users 
                    SET FailedLoginAttempts = FailedLoginAttempts + 1,
                        LastFailedLogin = @lastFailedLogin,
                        AccountLockedUntil = CASE 
                            WHEN FailedLoginAttempts + 1 >= 5 THEN DATE_ADD(NOW(), INTERVAL 30 MINUTE)
                            ELSE AccountLockedUntil
                        END
                    WHERE Id = @userId";

                var parameters = new Dictionary<string, object>
                {
                    {"@lastFailedLogin", DateTime.Now},
                    {"@userId", userId}
                };

                dbManager.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Update Failed Login Attempts", false);
            }
        }

        #endregion

        /// <summary>
        /// Test database connectivity
        /// </summary>
        public bool TestConnection()
        {
            return dbManager.TestConnection();
        }

        /// <summary>
        /// Get user by username for compatibility
        /// </summary>
        public User GetUser(string username)
        {
            try
            {
                string sql = "SELECT Id, Username, Email, Phone, Company, Role FROM Users WHERE Username = @username";
                var parameters = new Dictionary<string, object> { { "@username", username } };

                DataTable result = dbManager.ExecuteQuery(sql, parameters);

                if (result.Rows.Count > 0)
                {
                    return CreateUserFromDataRow(result.Rows[0]);
                }

                return null;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get User", false);
                return null;
            }
        }
    }
}