using System;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmLogin : Form
    {
        private dbCustomer customerDb; // Instance of dbCustomer to interact with the database

        // Constructor
        public frmLogin()
        {
            InitializeComponent(); // This calls the Designer code
            customerDb = new dbCustomer(); // Initialize dbCustomer

            // Create database directory if it doesn't exist
            CreateDatabaseDirectory();
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
            // Set focus to username textbox when form loads
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // Enhanced input validation
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (string.IsNullOrWhiteSpace(username))
                    txtUsername.Focus();
                else
                    txtPassword.Focus();
                return;
            }

            // Validate username format
            if (!dbCustomer.IsValidUsername(username))
            {
                MessageBox.Show("Invalid username format. Username should be 3-20 characters with only letters, numbers, and underscores.",
                    "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            try
            {
                // Use dbCustomer to check user data from the database
                if (customerDb.Read(username)) // Reads the customer data using the username
                {
                    // SECURITY UPDATE: Use secure password verification instead of plain text comparison
                    if (customerDb.VerifyPassword(password)) // Changed from plain text comparison
                    {
                        MessageBox.Show($"Welcome back, {customerDb.data.USERNAME}!", "Login Successful",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Create proper user object and pass to Dashboard
                        var user = new User
                        {
                            Username = customerDb.data.USERNAME,
                            Role = "Admin" // You might want to add role field to database later
                        };

                        this.Hide();
                        var dashboard = new frmDashboard(user);
                        dashboard.FormClosed += (s, args) => this.Close(); // Close login when dashboard closes
                        dashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password. Please check your password and try again.",
                            "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPassword.Focus();
                        txtPassword.SelectAll(); // Select all text for easy retyping
                    }
                }
                else
                {
                    MessageBox.Show("Username not found. Please check your username or register for a new account.",
                        "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    txtUsername.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed due to an error: {ex.Message}\n\nPlease try again or contact support if the problem persists.",
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new frmRegister();
            registerForm.FormClosed += (s, args) => this.Show(); // Show login when register closes
            registerForm.Show();
        }

        // Optional: Add Enter key support for easier login
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e); // Trigger login when Enter is pressed in password field
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus(); // Move to password field when Enter is pressed in username field
            }
        }

        // Removed duplicate frmLogin_Load_1 method

        // Override form closing to ensure proper cleanup
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            customerDb = null; // Clean up database connection
            base.OnFormClosed(e);
        }
    }
}