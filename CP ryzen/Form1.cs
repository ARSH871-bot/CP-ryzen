using System;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmLogin : Form
    {
        private dbCustomer customerDb; // Instance of dbCustomer to interact with the database

        public frmLogin()
        {
            InitializeComponent();
            customerDb = new dbCustomer(); // Initialize dbCustomer
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use dbCustomer to check user data from the database
            if (customerDb.Read(username)) // Reads the customer data using the username
            {
                if (customerDb.data.PASSWORD == password) // Validate password
                {
                    MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Pass username to Dashboard (frmDashboard can be replaced with your actual main form)
                    this.Hide();
                    //new frmDashboard(customerDb.data.USERNAME).Show(); // Replace with the correct form
                }
                else
                {
                    MessageBox.Show("Invalid password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"User not found.\nError: {customerDb.LastError}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmRegister().Show(); // Navigate to Registration form
        }
    }
}
