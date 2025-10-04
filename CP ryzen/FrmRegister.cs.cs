using System;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmRegister : Form
    {
        private UserManager userManager;

        public frmRegister()
        {
            InitializeComponent();
            userManager = new UserManager();
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

                // Validate
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(confirmPassword))
                {
                    ErrorHandler.ShowWarning("Please fill in all required fields.", "Registration Failed");
                    return;
                }

                if (password != confirmPassword)
                {
                    ErrorHandler.ShowWarning("Passwords do not match.", "Registration Failed");
                    return;
                }

                // Register using database
                bool success = userManager.RegisterUser(username, password, email, phone, companyName, role);

                if (success)
                {
                    // Send welcome email (async, fire and forget)
                    _ = EmailManager.SendWelcomeEmail(email, username, role, companyName);

                    ErrorHandler.ShowInfo("Registration Successful!\n\nA welcome email has been sent.", "Success");

                    this.Hide();
                    new frmLogin().Show();
                }
                else
                {
                    ErrorHandler.ShowWarning(userManager.LastError, "Registration Failed");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Registration");
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

        private void frmRegister_Load(object sender, EventArgs e) { }
        private void pictureBoxLogo_Click(object sender, EventArgs e) { }
        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtConfirmPassword_TextChanged(object sender, EventArgs e) { }
    }
}