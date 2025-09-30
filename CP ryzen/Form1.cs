using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmLogin : Form
    {
        private dbCustomer customerDb; // Instance of dbCustomer to interact with the database

        public frmLogin()
        {
            InitializeComponent();
            customerDb = new dbCustomer();
            CreateDatabaseDirectory();
            AddTestButtons(); // Add email testing buttons
        }

        private void CreateDatabaseDirectory()
        {
            try
            {
                string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "customer");
                Directory.CreateDirectory(dbPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating database directory: {ex.Message}", "Setup Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check if account is locked (if SecurityManager exists)
                if (SecurityManagerExists() && SecurityManager.IsAccountLocked(username))
                {
                    var remaining = SecurityManager.GetRemainingLockoutTime(username);
                    MessageBox.Show($"Account is locked due to multiple failed login attempts.\n\n" +
                                  $"Please try again in {remaining.Minutes} minutes and {remaining.Seconds} seconds.",
                        "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Read user data from database
                if (customerDb.Read(username))
                {
                    bool isValidPassword = false;

                    // Check if password is hashed (new accounts) or plain text (legacy accounts)
                    string storedPassword = customerDb.data.PASSWORD;

                    if (SecurityManagerExists())
                    {
                        // Try to verify as hashed password first (new accounts)
                        if (storedPassword.Length > 20 && storedPassword.Contains("="))
                        {
                            // This looks like a hashed password
                            isValidPassword = SecurityManager.VerifyPassword(password, storedPassword);
                        }
                        else
                        {
                            // Plain text password (legacy account) - upgrade it
                            if (storedPassword == password)
                            {
                                isValidPassword = true;
                                // Upgrade to hashed password
                                UpgradePasswordSecurity(username, password);
                            }
                        }
                    }
                    else
                    {
                        // Fallback to plain text comparison if SecurityManager doesn't exist
                        isValidPassword = (storedPassword == password);
                    }

                    if (isValidPassword)
                    {
                        // Clear failed attempts on successful login
                        if (SecurityManagerExists())
                        {
                            SecurityManager.ClearFailedAttempts(username);
                        }

                        MessageBox.Show("Login Successful!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Create user object and navigate to dashboard
                        var user = new User
                        {
                            Username = customerDb.data.USERNAME,
                            Role = "Admin" // Default role - you can enhance this later
                        };

                        // Log successful login
                        if (ErrorHandlerExists())
                        {
                            ErrorHandler.LogInfo($"User logged in: {username}", "Login");
                        }

                        this.Hide();
                        var dashboard = new frmDashboard(user);
                        dashboard.FormClosed += (s, args) => this.Close();
                        dashboard.Show();
                    }
                    else
                    {
                        // Record failed attempt
                        if (SecurityManagerExists())
                        {
                            SecurityManager.RecordFailedAttempt(username);
                        }

                        MessageBox.Show("Invalid password.", "Login Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Clear password field for security
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
                else
                {
                    // Record failed attempt for non-existent user
                    if (SecurityManagerExists())
                    {
                        SecurityManager.RecordFailedAttempt(username);
                    }

                    MessageBox.Show($"User not found.\nError: {customerDb.LastError}", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtPassword.Clear();
                    txtUsername.Focus();
                }
            }
            catch (Exception ex)
            {
                // Use ErrorHandler if available, otherwise show basic error
                if (ErrorHandlerExists())
                {
                    ErrorHandler.HandleException(ex, "Login Process");
                }
                else
                {
                    MessageBox.Show($"Login error: {ex.Message}", "System Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                txtPassword.Clear();
                txtUsername.Focus();
            }
        }

        #region Email Testing Methods

        /// <summary>
        /// Add temporary test buttons for email troubleshooting
        /// </summary>
        private void AddTestButtons()
        {
            try
            {
                Button emailTestBtn = new Button();
                emailTestBtn.Text = "Test Email";
                emailTestBtn.Location = new Point(10, 10);
                emailTestBtn.Size = new Size(80, 30);
                emailTestBtn.BackColor = System.Drawing.Color.LightBlue;
                emailTestBtn.Click += (s, e) => TestEmailSystem();
                this.Controls.Add(emailTestBtn);

                Button fsTestBtn = new Button();
                fsTestBtn.Text = "Test Files";
                fsTestBtn.Location = new Point(100, 10);
                fsTestBtn.Size = new Size(80, 30);
                fsTestBtn.BackColor = System.Drawing.Color.LightGreen;
                fsTestBtn.Click += (s, e) => TestFileSystemAccess();
                this.Controls.Add(fsTestBtn);

                Button systemTestBtn = new Button();
                systemTestBtn.Text = "Test All";
                systemTestBtn.Location = new Point(190, 10);
                systemTestBtn.Size = new Size(80, 30);
                systemTestBtn.BackColor = System.Drawing.Color.LightYellow;
                systemTestBtn.Click += (s, e) => TestSystems();
                this.Controls.Add(systemTestBtn);
            }
            catch (Exception ex)
            {
                // Don't break the form if test buttons fail to create
                MessageBox.Show($"Failed to create test buttons: {ex.Message}", "Warning");
            }
        }

        /// <summary>
        /// Comprehensive email system test
        /// </summary>
        private void TestEmailSystem()
        {
            try
            {
                string result = "=== EMAIL SYSTEM TEST ===\n\n";

                // Test 1: Check if EmailManager exists
                try
                {
                    bool isSimulation = EmailManager.IsInSimulationMode();
                    result += $"✓ EmailManager: Available\n";
                    result += $"✓ Simulation Mode: {isSimulation}\n\n";
                }
                catch (Exception ex)
                {
                    result += $"✗ EmailManager: FAILED - {ex.Message}\n\n";
                    MessageBox.Show(result, "Email Test Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Test 2: Check log directory
                string logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                bool logDirExists = Directory.Exists(logPath);
                result += $"Log Directory: {(logDirExists ? "✓ Exists" : "✗ Missing")}\n";
                result += $"Path: {logPath}\n\n";

                // Test 3: Test email sending
                result += "Testing email send...\n";
                MessageBox.Show(result + "Starting async email test...", "Email Test - Step 1", MessageBoxButtons.OK);

                Task.Run(async () =>
                {
                    try
                    {
                        bool emailSent = await EmailManager.SendWelcomeEmail("test@example.com", "TestUser", "Employee", "Test Company");

                        this.BeginInvoke(new Action(() =>
                        {
                            result += $"Email Send Result: {(emailSent ? "✓ SUCCESS" : "✗ FAILED")}\n\n";

                            // Test 4: Check if log file was created
                            string emailLogFile = Path.Combine(logPath, "email_log.txt");
                            bool logFileExists = File.Exists(emailLogFile);
                            result += $"Email Log File: {(logFileExists ? "✓ Created" : "✗ Missing")}\n";

                            if (logFileExists)
                            {
                                try
                                {
                                    string logContent = File.ReadAllText(emailLogFile);
                                    result += $"Log Size: {logContent.Length} characters\n";

                                    if (logContent.Length > 0)
                                    {
                                        string recentLog = logContent.Length > 300 ?
                                            logContent.Substring(logContent.Length - 300) : logContent;
                                        result += $"\nRecent Log Content:\n{recentLog}";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result += $"✗ Error reading log: {ex.Message}";
                                }
                            }

                            MessageBox.Show(result, "Email Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show($"Async Email Test Failed:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                                "Email Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Email Test Setup Failed:\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Email Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Test file system access for logging
        /// </summary>
        private void TestFileSystemAccess()
        {
            try
            {
                string result = "=== FILE SYSTEM TEST ===\n\n";
                string testDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                string testFile = Path.Combine(testDir, "test.txt");

                // Test directory creation
                try
                {
                    Directory.CreateDirectory(testDir);
                    result += "✓ Directory creation: SUCCESS\n";
                }
                catch (Exception ex)
                {
                    result += $"✗ Directory creation: FAILED - {ex.Message}\n";
                }

                // Test file writing
                try
                {
                    File.WriteAllText(testFile, $"Test file created at {DateTime.Now}");
                    result += "✓ File writing: SUCCESS\n";
                }
                catch (Exception ex)
                {
                    result += $"✗ File writing: FAILED - {ex.Message}\n";
                }

                // Test file reading
                try
                {
                    string content = File.ReadAllText(testFile);
                    result += $"✓ File reading: SUCCESS\n";
                    result += $"Content: {content}\n";
                }
                catch (Exception ex)
                {
                    result += $"✗ File reading: FAILED - {ex.Message}\n";
                }

                // Clean up
                try
                {
                    if (File.Exists(testFile))
                    {
                        File.Delete(testFile);
                        result += "✓ File cleanup: SUCCESS\n";
                    }
                }
                catch (Exception ex)
                {
                    result += $"✗ File cleanup: FAILED - {ex.Message}\n";
                }

                result += $"\nCurrent Directory: {Directory.GetCurrentDirectory()}\n";
                result += $"Logs Path: {testDir}\n";

                MessageBox.Show(result, "File System Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File System Test Failed:\n{ex.Message}",
                    "File System Test Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Upgrade plain text password to hashed password
        /// </summary>
        private void UpgradePasswordSecurity(string username, string plainTextPassword)
        {
            try
            {
                if (SecurityManagerExists())
                {
                    customerDb.data.PASSWORD = SecurityManager.HashPassword(plainTextPassword);
                    customerDb.Update(username);

                    if (ErrorHandlerExists())
                    {
                        ErrorHandler.LogInfo($"Upgraded password security for user: {username}", "Login");
                    }
                }
            }
            catch (Exception ex)
            {
                // Don't break login if password upgrade fails
                if (ErrorHandlerExists())
                {
                    ErrorHandler.HandleException(ex, "Password Security Upgrade", false);
                }
            }
        }

        /// <summary>
        /// Check if SecurityManager class exists and is available
        /// </summary>
        private bool SecurityManagerExists()
        {
            try
            {
                // Try to access SecurityManager - if it doesn't exist, this will throw an exception
                var test = SecurityManager.ValidatePasswordStrength("test");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if ErrorHandler class exists and is available
        /// </summary>
        private bool ErrorHandlerExists()
        {
            try
            {
                // Try to access ErrorHandler - if it doesn't exist, this will throw an exception
                ErrorHandler.LogInfo("Test", "Test");
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Event Handlers

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new frmRegister();
            registerForm.FormClosed += (s, args) => this.Show();
            registerForm.Show();
        }

        // Add keyboard navigation support
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        #endregion

        #region System Testing

        /// <summary>
        /// Comprehensive system test
        /// </summary>
        private void TestSystems()
        {
            string testResults = "=== SYSTEM COMPONENT TEST ===\n\n";

            // Test database connection
            try
            {
                customerDb.Read("nonexistent");
                testResults += "✓ Database system: Working\n";
            }
            catch (Exception ex)
            {
                testResults += $"✗ Database system: ERROR - {ex.Message}\n";
            }

            // Test SecurityManager
            testResults += SecurityManagerExists() ? "✓ SecurityManager: Available\n" : "✗ SecurityManager: Not found\n";

            // Test ErrorHandler
            testResults += ErrorHandlerExists() ? "✓ ErrorHandler: Available\n" : "✗ ErrorHandler: Not found\n";

            // Test EmailManager
            try
            {
                bool emailTest = EmailManager.IsInSimulationMode();
                testResults += $"✓ EmailManager: Available (Simulation: {emailTest})\n";
            }
            catch (Exception ex)
            {
                testResults += $"✗ EmailManager: ERROR - {ex.Message}\n";
            }

            // Test current directory and permissions
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                testResults += $"✓ Current Directory: {currentDir}\n";

                // Test if we can write to current directory
                string testFile = Path.Combine(currentDir, "temp_test.txt");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                testResults += "✓ Write permissions: Available\n";
            }
            catch (Exception ex)
            {
                testResults += $"✗ Directory access: ERROR - {ex.Message}\n";
            }

            testResults += "\n=== RECOMMENDED ACTIONS ===\n";

            if (!SecurityManagerExists())
                testResults += "• Create SecurityManager.cs file\n";
            if (!ErrorHandlerExists())
                testResults += "• Create ErrorHandler.cs file\n";

            try
            {
                EmailManager.IsInSimulationMode();
            }
            catch
            {
                testResults += "• Create EmailManager.cs file\n";
            }

            MessageBox.Show(testResults, "System Test Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void btnTestDatabase_Click(object sender, EventArgs e)
        {
            new frmDatabaseTest().ShowDialog();
        }
    }
}