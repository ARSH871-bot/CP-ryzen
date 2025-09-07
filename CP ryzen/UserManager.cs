using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Simplified user manager using existing infrastructure
    /// </summary>
    public class UserManager
    {
        private dbCustomer customerDb;
        public string LastError { get; private set; } = "";

        public UserManager()
        {
            customerDb = new dbCustomer();
        }

        /// <summary>
        /// Register user with enhanced password validation
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

                // Check if user exists
                if (customerDb.Read(username))
                {
                    LastError = "Username already exists.";
                    return false;
                }

                // Set user data with hashed password
                customerDb.data.USERNAME = username;
                customerDb.data.PASSWORD = SecurityManager.HashPassword(password);
                customerDb.data.EMAIL = email;
                customerDb.data.PHONE = phone;
                customerDb.data.COMPANY = company;

                // Save to database
                if (customerDb.Update(username))
                {
                    ErrorHandler.LogInfo($"User registered: {username}", "UserManager");
                    return true;
                }
                else
                {
                    LastError = "Failed to save user data.";
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
        /// Authenticate user with security features
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

                // Read user data
                if (!customerDb.Read(username))
                {
                    LastError = "Invalid username or password.";
                    SecurityManager.RecordFailedAttempt(username);
                    return null;
                }

                // Verify password
                bool isValidPassword = false;

                // Check if password is hashed (newer accounts) or plain text (legacy)
                if (customerDb.data.PASSWORD.Contains("=") || customerDb.data.PASSWORD.Length > 20)
                {
                    // Hashed password
                    isValidPassword = SecurityManager.VerifyPassword(password, customerDb.data.PASSWORD);
                }
                else
                {
                    // Legacy plain text password - upgrade it
                    if (customerDb.data.PASSWORD == password)
                    {
                        isValidPassword = true;
                        // Upgrade to hashed password
                        customerDb.data.PASSWORD = SecurityManager.HashPassword(password);
                        customerDb.Update(username);
                        ErrorHandler.LogInfo($"Upgraded password security for: {username}", "UserManager");
                    }
                }

                if (!isValidPassword)
                {
                    LastError = "Invalid username or password.";
                    SecurityManager.RecordFailedAttempt(username);
                    return null;
                }

                // Clear failed attempts on successful login
                SecurityManager.ClearFailedAttempts(username);

                // Create and return user object
                var user = new User
                {
                    Username = customerDb.data.USERNAME,
                    Role = "Admin" // Default role
                };

                ErrorHandler.LogInfo($"User authenticated: {username}", "UserManager");
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
        /// Verify password for compatibility
        /// </summary>
        public bool VerifyPassword(string password)
        {
            return SecurityManager.VerifyPassword(password, customerDb.data.PASSWORD);
        }

        /// <summary>
        /// Read user for compatibility
        /// </summary>
        public bool Read(string username)
        {
            return customerDb.Read(username);
        }
    }
}