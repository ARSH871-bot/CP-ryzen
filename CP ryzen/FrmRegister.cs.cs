using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmRegister : Form
    {
        private UserManager userManager; // Enhanced user manager with security features

        public frmRegister()
        {
            InitializeComponent();
            userManager = new UserManager(); // Initialize enhanced UserManager
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

                // Enhanced password strength validation
                if (!SecurityManager.ValidatePasswordStrength(password))
                {
                    MessageBox.Show($"Password does not meet security requirements:\n\n{SecurityManager.GetPasswordRequirements()}",
                        "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                    return;
                }

                // Use UserManager for secure registration with email integration
                if (userManager.RegisterUser(username, password, email, phone, companyName, role))
                {
                    // Send welcome email asynchronously if email provided
                    if (!string.IsNullOrEmpty(email))
                    {
                        Task.Run(async () =>
                        {
                            try
                            {
                                bool emailSent = await EmailManager.SendWelcomeEmail(email, username, role, companyName);
                                if (emailSent)
                                {
                                    ErrorHandler.LogInfo($"Welcome email sent to {email} for user {username}", "Registration");
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.HandleException(ex, "Send Welcome Email", false);
                            }
                        });
                    }

                    MessageBox.Show("Registration successful!\n\n" +
                                  "Security Features Enabled:\n" +
                                  "• Your password has been securely encrypted\n" +
                                  "• Account lockout protection activated\n" +
                                  "• Login attempts will be monitored\n\n" +
                                  (!string.IsNullOrEmpty(email) ? "• Welcome email sent to " + email + "\n\n" : "") +
                                  "You can now login with your credentials.",
                        "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Log successful registration
                    ErrorHandler.LogInfo($"New user registered: {username}", "Registration");

                    this.Hide();
                    new frmLogin().Show();
                }
                else
                {
                    // Show specific error message from UserManager
                    MessageBox.Show(userManager.LastError, "Registration Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Focus appropriate field based on error
                    if (userManager.LastError.Contains("username") || userManager.LastError.Contains("Username"))
                    {
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                    }
                    else if (userManager.LastError.Contains("email") || userManager.LastError.Contains("Email"))
                    {
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                    }
                    else if (userManager.LastError.Contains("password") || userManager.LastError.Contains("Password"))
                    {
                        txtPassword.Focus();
                        txtPassword.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "User Registration");

                // Clear sensitive fields on error
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                txtUsername.Focus();
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

            // Show password requirements hint
            this.Text = "Registration - Strong Password Required";
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            // Show password requirements when logo is clicked
            MessageBox.Show($"Password Security Requirements:\n\n{SecurityManager.GetPasswordRequirements()}\n\n" +
                          "Additional Security Features:\n" +
                          "• Automatic account lockout after failed attempts\n" +
                          "• Secure password encryption\n" +
                          "• Login monitoring and logging",
                "Security Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Add real-time password strength feedback
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // Optional: Real-time password strength feedback
            if (txtPassword.Text.Length > 0)
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
            // Optional: Real-time password match feedback
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
            // Optional: Real-time username validation feedback
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
            // Optional: Real-time email validation feedback
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