using System;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmLogin : Form
    {
        private UserManager userManager; // Replace dbCustomer with UserManager for enhanced security

        public frmLogin()
        {
            InitializeComponent();
            userManager = new UserManager(); // Initialize UserManager with security features
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

                // Check if account is locked before attempting authentication
                if (SecurityManager.IsAccountLocked(username))
                {
                    var remaining = SecurityManager.GetRemainingLockoutTime(username);
                    MessageBox.Show($"Account is locked due to multiple failed login attempts.\n\n" +
                                  $"Please try again in {remaining.Minutes} minutes and {remaining.Seconds} seconds.",
                        "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Authenticate user with enhanced security
                var user = userManager.AuthenticateUser(username, password);

                if (user != null)
                {
                    MessageBox.Show("Login Successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    var dashboard = new frmDashboard(user);
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();

                    ErrorHandler.LogInfo($"User logged in: {user.Username}", "Login");
                }
                else
                {
                    // Show specific error message from UserManager
                    MessageBox.Show(userManager.LastError, "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Clear password field for security
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Login Process");

                // Clear sensitive fields on error
                txtPassword.Clear();
                txtUsername.Focus();
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new frmRegister();
            registerForm.FormClosed += (s, args) => this.Show();
            registerForm.Show();
        }

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

        // Remove the duplicate/unused load method
        // private void frmLogin_Load_1(object sender, EventArgs e) - REMOVED
    }
}