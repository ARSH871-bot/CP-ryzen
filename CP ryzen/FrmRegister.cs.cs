using System;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmRegister : Form
    {
        private dbCustomer customerDb; // Instance of dbCustomer for database operations

        public frmRegister()
        {
            InitializeComponent();
            customerDb = new dbCustomer(); // Initialize dbCustomer
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string companyName = txtCompanyName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill in all required fields.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the user already exists
            if (customerDb.Read(username))
            {
                MessageBox.Show("A user with this username already exists.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set user data
            customerDb.data.USERNAME = username;
            customerDb.data.PASSWORD = password;
            customerDb.data.COMPANY = companyName;
            customerDb.data.EMAIL = email;
            customerDb.data.PHONE = phone;

            // Save to database
            if (customerDb.Update(username))
            {
                MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                new frmLogin().Show(); // Redirect to Login form
            }
            else
            {
                MessageBox.Show($"Failed to save user data.\nError: {customerDb.LastError}", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmLogin().Show(); // Navigate back to Login form
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtCompanyName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            cmbRole.SelectedIndex = -1;
            txtUsername.Focus();
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            // Optional: Handle logo click events if required
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            // Optional: Handle form load events if required
        }
    }
}
