using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Password validation and security requirements
    /// </summary>
    public static class PasswordValidator
    {
        // Password requirements configuration
        private static readonly int MinLength = 8;
        private static readonly int MaxLength = 128;
        private static readonly int RequiredUppercase = 1;
        private static readonly int RequiredLowercase = 1;
        private static readonly int RequiredNumbers = 1;
        private static readonly int RequiredSpecialChars = 1;

        // Common weak passwords to reject
        private static readonly HashSet<string> CommonPasswords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "password", "123456", "password123", "admin", "administrator",
            "qwerty", "abc123", "welcome", "letmein", "monkey",
            "1234567890", "password1", "123456789", "welcome123"
        };

        /// <summary>
        /// Validate password meets security requirements
        /// </summary>
        public static PasswordValidationResult ValidatePassword(string password, string username = "")
        {
            var result = new PasswordValidationResult
            {
                IsValid = true,
                Errors = new List<string>(),
                Warnings = new List<string>(),
                Strength = PasswordStrength.Weak
            };

            if (string.IsNullOrEmpty(password))
            {
                result.IsValid = false;
                result.Errors.Add("Password is required");
                return result;
            }

            // Length validation
            if (password.Length < MinLength)
            {
                result.IsValid = false;
                result.Errors.Add($"Password must be at least {MinLength} characters long");
            }

            if (password.Length > MaxLength)
            {
                result.IsValid = false;
                result.Errors.Add($"Password cannot exceed {MaxLength} characters");
            }

            // Character requirements
            if (!HasMinimumCharacterTypes(password))
            {
                result.IsValid = false;
                result.Errors.Add("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character");
            }

            // Common password check
            if (IsCommonPassword(password))
            {
                result.IsValid = false;
                result.Errors.Add("Password is too common and easily guessed");
            }

            // Username similarity check
            if (!string.IsNullOrEmpty(username) && IsPasswordSimilarToUsername(password, username))
            {
                result.IsValid = false;
                result.Errors.Add("Password cannot be similar to username");
            }

            // Sequential or repeated characters
            if (HasSequentialOrRepeatedChars(password))
            {
                result.Warnings.Add("Password contains sequential or repeated characters which may reduce security");
            }

            // Calculate password strength
            result.Strength = CalculatePasswordStrength(password);

            // Add strength warnings
            if (result.Strength == PasswordStrength.Weak && result.IsValid)
            {
                result.Warnings.Add("Password strength is weak. Consider using a longer password with more variety");
            }
            else if (result.Strength == PasswordStrength.Fair && result.IsValid)
            {
                result.Warnings.Add("Password strength is fair. Consider adding more complexity for better security");
            }

            return result;
        }

        /// <summary>
        /// Check if password has minimum required character types
        /// </summary>
        private static bool HasMinimumCharacterTypes(string password)
        {
            int uppercaseCount = 0;
            int lowercaseCount = 0;
            int numberCount = 0;
            int specialCharCount = 0;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) uppercaseCount++;
                else if (char.IsLower(c)) lowercaseCount++;
                else if (char.IsDigit(c)) numberCount++;
                else if (IsSpecialCharacter(c)) specialCharCount++;
            }

            return uppercaseCount >= RequiredUppercase &&
                   lowercaseCount >= RequiredLowercase &&
                   numberCount >= RequiredNumbers &&
                   specialCharCount >= RequiredSpecialChars;
        }

        /// <summary>
        /// Check if character is a valid special character
        /// </summary>
        private static bool IsSpecialCharacter(char c)
        {
            return "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c);
        }

        /// <summary>
        /// Check if password is in common passwords list
        /// </summary>
        private static bool IsCommonPassword(string password)
        {
            return CommonPasswords.Contains(password);
        }

        /// <summary>
        /// Check if password is too similar to username
        /// </summary>
        private static bool IsPasswordSimilarToUsername(string password, string username)
        {
            if (string.IsNullOrEmpty(username))
                return false;

            // Case-insensitive comparison
            string lowerPassword = password.ToLower();
            string lowerUsername = username.ToLower();

            // Direct match
            if (lowerPassword == lowerUsername)
                return true;

            // Password contains username or vice versa
            if (lowerPassword.Contains(lowerUsername) || lowerUsername.Contains(lowerPassword))
                return true;

            // Reversed username
            if (lowerPassword == new string(lowerUsername.Reverse().ToArray()))
                return true;

            return false;
        }

        /// <summary>
        /// Check for sequential or repeated characters
        /// </summary>
        private static bool HasSequentialOrRepeatedChars(string password)
        {
            int sequentialCount = 0;
            int repeatedCount = 0;

            for (int i = 0; i < password.Length - 2; i++)
            {
                // Check for sequential characters (abc, 123, etc.)
                if (password[i] + 1 == password[i + 1] && password[i + 1] + 1 == password[i + 2])
                {
                    sequentialCount++;
                }

                // Check for repeated characters (aaa, 111, etc.)
                if (password[i] == password[i + 1] && password[i + 1] == password[i + 2])
                {
                    repeatedCount++;
                }
            }

            // Consider it problematic if more than 1 occurrence of either pattern
            return sequentialCount > 1 || repeatedCount > 1;
        }

        /// <summary>
        /// Calculate password strength based on various factors
        /// </summary>
        private static PasswordStrength CalculatePasswordStrength(string password)
        {
            int score = 0;

            // Length scoring
            if (password.Length >= 8) score += 1;
            if (password.Length >= 12) score += 1;
            if (password.Length >= 16) score += 1;

            // Character variety scoring
            if (Regex.IsMatch(password, @"[a-z]")) score += 1;
            if (Regex.IsMatch(password, @"[A-Z]")) score += 1;
            if (Regex.IsMatch(password, @"[0-9]")) score += 1;
            if (Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]")) score += 1;

            // Complexity bonus
            if (HasGoodMixOfCharacters(password)) score += 1;
            if (!HasSequentialOrRepeatedChars(password)) score += 1;

            // Convert score to strength
            if (score <= 3) return PasswordStrength.Weak;
            if (score <= 6) return PasswordStrength.Fair;
            if (score <= 8) return PasswordStrength.Good;
            return PasswordStrength.Strong;
        }

        /// <summary>
        /// Check if password has good mix of different character types
        /// </summary>
        private static bool HasGoodMixOfCharacters(string password)
        {
            int charTypeCount = 0;
            if (Regex.IsMatch(password, @"[a-z]")) charTypeCount++;
            if (Regex.IsMatch(password, @"[A-Z]")) charTypeCount++;
            if (Regex.IsMatch(password, @"[0-9]")) charTypeCount++;
            if (Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{}|;:,.<>?]")) charTypeCount++;

            return charTypeCount >= 3;
        }

        /// <summary>
        /// Generate password strength indicator text
        /// </summary>
        public static string GetStrengthText(PasswordStrength strength)
        {
            switch (strength)
            {
                case PasswordStrength.Weak:
                    return "Weak - Use a longer password with more variety";
                case PasswordStrength.Fair:
                    return "Fair - Consider adding more complexity";
                case PasswordStrength.Good:
                    return "Good - Password meets security requirements";
                case PasswordStrength.Strong:
                    return "Strong - Excellent password security";
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Get password requirements text for user guidance
        /// </summary>
        public static string GetPasswordRequirements()
        {
            return $"Password Requirements:\n" +
                   $"• Minimum {MinLength} characters long\n" +
                   $"• At least {RequiredUppercase} uppercase letter (A-Z)\n" +
                   $"• At least {RequiredLowercase} lowercase letter (a-z)\n" +
                   $"• At least {RequiredNumbers} number (0-9)\n" +
                   $"• At least {RequiredSpecialChars} special character (!@#$%^&*)\n" +
                   $"• Cannot be a common password\n" +
                   $"• Cannot be similar to your username";
        }
    }

    /// <summary>
    /// Password validation result container
    /// </summary>
    public class PasswordValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public PasswordStrength Strength { get; set; }

        public string GetFormattedMessage()
        {
            var messages = new List<string>();

            if (Errors.Count > 0)
            {
                messages.Add("Errors:");
                messages.AddRange(Errors.Select(e => "• " + e));
            }

            if (Warnings.Count > 0)
            {
                messages.Add("Warnings:");
                messages.AddRange(Warnings.Select(w => "• " + w));
            }

            return string.Join("\n", messages);
        }
    }

    /// <summary>
    /// Password strength levels
    /// </summary>
    public enum PasswordStrength
    {
        Weak,
        Fair,
        Good,
        Strong
    }
}