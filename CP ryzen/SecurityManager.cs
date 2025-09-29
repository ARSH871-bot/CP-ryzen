using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Simplified security manager without external dependencies
    /// </summary>
    public static class SecurityManager
    {
        private static readonly Dictionary<string, int> failedAttempts = new Dictionary<string, int>();
        private static readonly Dictionary<string, DateTime> lockoutTimes = new Dictionary<string, DateTime>();
        private static readonly int MaxFailedAttempts = 5;
        private static readonly int LockoutMinutes = 30;

        /// <summary>
        /// Simple password hashing using SHA256
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password + "SALT_2024"));
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// Verify password against hash
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == hash;
        }

        /// <summary>
        /// Check if account is locked
        /// </summary>
        public static bool IsAccountLocked(string username)
        {
            if (lockoutTimes.ContainsKey(username))
            {
                if (DateTime.Now < lockoutTimes[username])
                {
                    return true;
                }
                else
                {
                    // Lockout expired, clear it
                    lockoutTimes.Remove(username);
                    failedAttempts.Remove(username);
                }
            }
            return false;
        }

        /// <summary>
        /// Record failed login attempt
        /// </summary>
        public static void RecordFailedAttempt(string username)
        {
            if (!failedAttempts.ContainsKey(username))
            {
                failedAttempts[username] = 0;
            }

            failedAttempts[username]++;

            if (failedAttempts[username] >= MaxFailedAttempts)
            {
                lockoutTimes[username] = DateTime.Now.AddMinutes(LockoutMinutes);
                ErrorHandler.LogInfo($"Account locked for {username} due to failed attempts", "SecurityManager");
            }
        }

        /// <summary>
        /// Clear failed attempts on successful login
        /// </summary>
        public static void ClearFailedAttempts(string username)
        {
            failedAttempts.Remove(username);
            lockoutTimes.Remove(username);
        }

        /// <summary>
        /// Get remaining lockout time
        /// </summary>
        public static TimeSpan GetRemainingLockoutTime(string username)
        {
            if (lockoutTimes.ContainsKey(username))
            {
                var remaining = lockoutTimes[username] - DateTime.Now;
                return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
            }
            return TimeSpan.Zero;
        }

        /// <summary>
        /// Validate password strength
        /// </summary>
        public static bool ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 6)
                return false;

            bool hasUpper = false, hasLower = false, hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            return hasUpper && hasLower && hasDigit;
        }

        /// <summary>
        /// Get password requirements message
        /// </summary>
        public static string GetPasswordRequirements()
        {
            return "Password must be at least 6 characters long and contain:\n" +
                   "• At least one uppercase letter\n" +
                   "• At least one lowercase letter\n" +
                   "• At least one number";
        }

        /// <summary>
        /// Get failed attempts count for a user
        /// </summary>
        public static int GetFailedAttempts(string username)
        {
            return failedAttempts.ContainsKey(username) ? failedAttempts[username] : 0;
        }

        /// <summary>
        /// Check if password meets minimum requirements
        /// </summary>
        public static string ValidatePasswordDetailed(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Password is required");
                return string.Join("\n", errors);
            }

            if (password.Length < 6)
                errors.Add("Password must be at least 6 characters long");

            bool hasUpper = false, hasLower = false, hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                if (char.IsLower(c)) hasLower = true;
                if (char.IsDigit(c)) hasDigit = true;
            }

            if (!hasUpper) errors.Add("Password must contain at least one uppercase letter");
            if (!hasLower) errors.Add("Password must contain at least one lowercase letter");
            if (!hasDigit) errors.Add("Password must contain at least one number");

            return errors.Count > 0 ? string.Join("\n", errors) : "";
        }
    }
}