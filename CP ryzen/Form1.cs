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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use dbCustomer to check user data from the database
            if (customerDb.Read(username)) // Reads the customer data using the username
            {
                if (customerDb.data.PASSWORD == password) // Validate password
                {
                    MessageBox.Show("Login Successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Create proper user object and pass to Dashboard
                    var user = new User
                    {
                        Username = customerDb.data.USERNAME,
                        Role = "Admin" // You might want to add role field to database
                    };

                    this.Hide();
                    var dashboard = new frmDashboard(user);
                    dashboard.FormClosed += (s, args) => this.Close(); // Close login when dashboard closes
                    dashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"User not found.\nError: {customerDb.LastError}", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new frmRegister();
            registerForm.FormClosed += (s, args) => this.Show(); // Show login when register closes
            registerForm.Show();
        }
    }
}