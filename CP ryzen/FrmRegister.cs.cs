using System;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmRegister : Form
    {
        private dbCustomer customerDb;

        public frmRegister()
        {
            InitializeComponent();
            customerDb = new dbCustomer();
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

            if (customerDb.Read(username))
            {
                MessageBox.Show("A user with this username already exists.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            customerDb.data.USERNAME = username;
            customerDb.data.PASSWORD = password;
            customerDb.data.COMPANY = companyName;
            customerDb.data.EMAIL = email;
            customerDb.data.PHONE = phone;

            if (customerDb.Update(username))
            {
                _ = EmailManager.SendWelcomeEmail(email, username, role, companyName);

                MessageBox.Show("Registration Successful!\n\nA welcome email has been sent to your email address.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                new frmLogin().Show();
            }
            else
            {
                MessageBox.Show($"Failed to save user data.\nError: {customerDb.LastError}", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmLogin().Show();
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
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add real-time username validation here
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add password strength indicator here
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add email format validation here
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add password match indicator here
        }
    }
}