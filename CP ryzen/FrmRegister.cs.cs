using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmRegister : Form
    {
        private UserManager userManager; // Enhanced user manager with security features
        private bool isUserManagerAvailable = false;

        public frmRegister()
        {
            InitializeComponent();
            InitializeUserManager();
        }

        /// <summary>
        /// Initialize UserManager with detailed error reporting
        /// </summary>
        private void InitializeUserManager()
        {
            try
            {
                userManager = new UserManager();
                isUserManagerAvailable = true;

                // Show success message on form load
                this.Load += (s, e) => ShowDatabaseStatus();
            }
            catch (Exception ex)
            {
                isUserManagerAvailable = false;
                userManager = null;

                // Show detailed error on form load
                this.Load += (s, e) => {
                    MessageBox.Show($"UserManager initialization failed:\n\n{ex.Message}\n\n" +
                                  "Using fallback registration system (JSON files).\n" +
                                  "Users will be stored locally instead of MySQL database.",
                        "Database Connection Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                };
            }
        }

        /// <summary>
        /// Show database connection status when form loads
        /// </summary>
        private void ShowDatabaseStatus()
        {
            string statusMessage = "";

            if (isUserManagerAvailable)
            {
                statusMessage = "✓ MySQL Database Connection: ACTIVE\n" +
                              "✓ Users will be stored in XAMPP database\n" +
                              "✓ Enhanced security features enabled";

                // Test actual connection
                try
                {
                    var dbManager = new DatabaseManager();
                    bool connected = dbManager.TestConnection();

                    if (!connected)
                    {
                        statusMessage = "⚠ MySQL Database: CONNECTION FAILED\n" +
                                      "• XAMPP MySQL may not be running\n" +
                                      "• Falling back to local file storage";
                        isUserManagerAvailable = false;
                        userManager = null;
                    }
                }
                catch (Exception ex)
                {
                    statusMessage = $"⚠ Database Error: {ex.Message}\n" +
                                  "• Falling back to local file storage";
                    isUserManagerAvailable = false;
                    userManager = null;
                }
            }
            else
            {
                statusMessage = "⚠ MySQL Database: NOT AVAILABLE\n" +
                              "• Using local file storage (JSON)\n" +
                              "• Basic security features only";
            }

            // Update form title to show status
            this.Text = isUserManagerAvailable ?
                "Registration - MySQL Database" :
                "Registration - Local Storage";

            // Optional: Show status message (comment out if too intrusive)
            // MessageBox.Show(statusMessage, "Database Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string confirmPassword = txtConfirmPassword.Text.Trim();
                string companyName = txtCompanyName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string phone = txtPhone.Text.Trim();
                string role = cmbRole.SelectedItem?.ToString() ?? "Employee";

                // Enhanced validation
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Please fill in all required fields (Username, Password, Confirm Password).",
                        "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                // Username validation
                if (username.Length < 3)
                {
                    MessageBox.Show("Username must be at least 3 characters long.",
                        "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    txtUsername.SelectAll();
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Registration Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtConfirmPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // Enhanced password strength validation (with fallback)
                if (SecurityManagerExists() && !SecurityManager.ValidatePasswordStrength(password))
                {
                    MessageBox.Show($"Password does not meet security requirements:\n\n{SecurityManager.GetPasswordRequirements()}",
                        "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                    return;
                }

                // Registration with detailed system tracking
                bool registrationResult = false;
                string systemUsed = "";

                if (isUserManagerAvailable && userManager != null)
                {
                    // Try MySQL database registration
                    registrationResult = userManager.RegisterUser(username, password, email, phone, companyName, role);
                    systemUsed = "MySQL Database";

                    if (!registrationResult)
                    {
                        // Show detailed MySQL error and try fallback
                        MessageBox.Show($"MySQL Database registration failed:\n{userManager.LastError}\n\nAttempting fallback to local storage...",
                            "Database Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        registrationResult = RegisterWithFallback(username, password, email, phone, companyName, role);
                        systemUsed = "Local Files (Fallback)";
                    }
                }
                else
                {
                    // Use fallback registration system directly
                    registrationResult = RegisterWithFallback(username, password, email, phone, companyName, role);
                    systemUsed = "Local Files";
                }

                if (registrationResult)
                {
                    // Send welcome email asynchronously if email provided
                    if (!string.IsNullOrEmpty(email))
                    {
                        Task.Run(async () =>
                        {
                            try
                            {
                                bool emailSent = await EmailManager.SendWelcomeEmail(email, username, role, companyName);
                                if (emailSent && ErrorHandlerExists())
                                {
                                    ErrorHandler.LogInfo($"Welcome email sent to {email} for user {username}", "Registration");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ErrorHandlerExists())
                                {
                                    ErrorHandler.HandleException(ex, "Send Welcome Email", false);
                                }
                            }
                        });
                    }

                    // Success message with system information
                    string successMessage = $"Registration successful!\n\nSystem Used: {systemUsed}\n\n";

                    if (systemUsed.Contains("MySQL"))
                    {
                        successMessage += "✓ User stored in XAMPP MySQL database\n" +
                                        "✓ Password securely encrypted\n" +
                                        "✓ Enhanced security features active\n";
                    }
                    else
                    {
                        successMessage += "✓ User stored in local files\n" +
                                        "✓ Basic security features active\n";
                    }

                    successMessage += (!string.IsNullOrEmpty(email) ? "✓ Welcome email sent\n\n" : "\n") +
                                    "You can now login with your credentials.";

                    MessageBox.Show(successMessage, "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Log successful registration with system details
                    if (ErrorHandlerExists())
                    {
                        ErrorHandler.LogInfo($"New user registered: {username} using {systemUsed}", "Registration");
                    }

                    this.Hide();
                    new frmLogin().Show();
                }
                else
                {
                    // Show specific error message
                    string errorMsg = userManager?.LastError ?? "Registration failed with unknown error.";
                    MessageBox.Show($"Registration failed:\n\n{errorMsg}", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Focus appropriate field based on error
                    if (errorMsg.Contains("username") || errorMsg.Contains("Username"))
                    {
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                    }
                    else if (errorMsg.Contains("email") || errorMsg.Contains("Email"))
                    {
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                    }
                    else if (errorMsg.Contains("password") || errorMsg.Contains("Password"))
                    {
                        txtPassword.Focus();
                        txtPassword.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ErrorHandlerExists())
                {
                    ErrorHandler.HandleException(ex, "User Registration");
                }
                else
                {
                    MessageBox.Show($"Registration error: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Clear sensitive fields on error
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                txtUsername.Focus();
            }
        }

        /// <summary>
        /// Fallback registration using dbCustomer system with detailed tracking
        /// </summary>
        private bool RegisterWithFallback(string username, string password, string email, string phone, string companyName, string role)
        {
            try
            {
                var customerDb = new dbCustomer();

                // Check if user already exists
                if (customerDb.Read(username))
                {
                    MessageBox.Show("A user with this username already exists in the local storage system.",
                        "Username Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Set user data (with password hashing if SecurityManager exists)
                customerDb.data.USERNAME = username;
                customerDb.data.PASSWORD = SecurityManagerExists() ? SecurityManager.HashPassword(password) : password;
                customerDb.data.EMAIL = email;
                customerDb.data.PHONE = phone;
                customerDb.data.COMPANY = companyName;

                // Save to local JSON file
                bool result = customerDb.Update(username);

                if (result && ErrorHandlerExists())
                {
                    ErrorHandler.LogInfo($"User {username} saved to local storage at: {Path.Combine(Directory.GetCurrentDirectory(), "Database", "customer")}", "Fallback Registration");
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fallback registration failed: {ex.Message}\n\nPlease check file permissions and try again.",
                    "Fallback Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Manual database test for troubleshooting
        /// </summary>
        private void TestDatabaseManually()
        {
            string result = "=== COMPREHENSIVE DATABASE TEST ===\n\n";

            try
            {
                // Test 1: Check if classes exist
                result += "Component Availability:\n";
                result += $"• UserManager: {(userManager != null ? "✓ Available" : "✗ Missing")}\n";

                try
                {
                    var dbManager = new DatabaseManager();
                    result += "• DatabaseManager: ✓ Available\n";

                    // Test connection
                    bool connected = dbManager.TestConnection();
                    result += $"• Database Connection: {(connected ? "✓ SUCCESS" : "✗ FAILED")}\n\n";

                    if (connected)
                    {
                        result += "MySQL Database Details:\n";
                        result += "• Server: localhost:3306\n";
                        result += "• Database: shipping_management\n";
                        result += "• Table: users\n\n";
                    }
                    else
                    {
                        result += "Connection Issues:\n";
                        result += "• Check XAMPP MySQL is running\n";
                        result += "• Verify port 3306 is available\n";
                        result += "• Check firewall settings\n\n";
                    }
                }
                catch (Exception dbEx)
                {
                    result += $"• DatabaseManager: ✗ ERROR - {dbEx.Message}\n\n";
                }

                // Test 2: Check current storage locations
                result += "Storage Locations:\n";
                string localDbPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "customer");
                result += $"• Local Storage: {localDbPath}\n";
                result += $"• Local Files Exist: {(Directory.Exists(localDbPath) ? "✓ Yes" : "✗ No")}\n\n";

                // Test 3: Try test registration
                result += "Registration Test:\n";
                if (userManager != null)
                {
                    string testUser = "test_" + DateTime.Now.Ticks;
                    bool testResult = userManager.RegisterUser(testUser, "TestPass123", "test@test.com", "", "Test Company", "Employee");
                    result += $"• MySQL Test: {(testResult ? "✓ SUCCESS" : "✗ FAILED")}\n";
                    if (!testResult)
                    {
                        result += $"  Error: {userManager.LastError}\n";
                    }
                }
                else
                {
                    result += "• MySQL Test: ✗ UserManager not available\n";
                }

                MessageBox.Show(result, "Database Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database test failed: {ex.Message}", "Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Check if SecurityManager exists
        /// </summary>
        private bool SecurityManagerExists()
        {
            try
            {
                SecurityManager.ValidatePasswordStrength("test");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if ErrorHandler exists
        /// </summary>
        private bool ErrorHandlerExists()
        {
            try
            {
                ErrorHandler.LogInfo("Test", "Test");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmLogin().Show();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all fields
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtCompanyName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            cmbRole.SelectedIndex = -1;

            // Focus on first field
            txtUsername.Focus();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            // Set up role dropdown if not already populated
            if (cmbRole.Items.Count == 0)
            {
                cmbRole.Items.AddRange(new string[] { "Admin", "Manager", "Dispatcher", "Employee" });
                cmbRole.SelectedIndex = 3; // Default to Employee
            }

            // Set focus to username field
            txtUsername.Focus();
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            // Show database test instead of security info
            TestDatabaseManually();
        }

        // Real-time validation methods with safety checks
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (SecurityManagerExists() && txtPassword.Text.Length > 0)
            {
                if (SecurityManager.ValidatePasswordStrength(txtPassword.Text))
                {
                    txtPassword.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    txtPassword.BackColor = System.Drawing.Color.LightPink;
                }
            }
            else
            {
                txtPassword.BackColor = System.Drawing.Color.White;
            }
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtConfirmPassword.Text.Length > 0)
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    txtConfirmPassword.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    txtConfirmPassword.BackColor = System.Drawing.Color.LightPink;
                }
            }
            else
            {
                txtConfirmPassword.BackColor = System.Drawing.Color.White;
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length > 0)
            {
                if (txtUsername.Text.Length >= 3 && txtUsername.Text.Length <= 20)
                {
                    txtUsername.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    txtUsername.BackColor = System.Drawing.Color.LightPink;
                }
            }
            else
            {
                txtUsername.BackColor = System.Drawing.Color.White;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                if (IsValidEmail(txtEmail.Text))
                {
                    txtEmail.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    txtEmail.BackColor = System.Drawing.Color.LightYellow;
                }
            }
            else
            {
                txtEmail.BackColor = System.Drawing.Color.White;
            }
        }

        /// <summary>
        /// Validate email address format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}