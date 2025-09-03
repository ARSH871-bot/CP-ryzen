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

            // Enhanced validation with specific error messages
            if (!ValidateInput(username, password, confirmPassword, email, role))
            {
                return; // Validation failed, error message already shown
            }

            // Check if the user already exists
            if (customerDb.Read(username))
            {
                MessageBox.Show("A user with this username already exists. Please choose a different username.",
                    "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsername.Focus();
                return;
            }

            try
            {
                // Set user data with secure password handling
                customerDb.data.USERNAME = username;
                customerDb.SetPassword(password); // Use secure password hashing
                customerDb.data.COMPANY = companyName;
                customerDb.data.EMAIL = email;
                customerDb.data.PHONE = phone;

                // Save to database
                if (customerDb.Update(username))
                {
                    MessageBox.Show("Registration successful! You can now log in with your credentials.",
                        "Registration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    var loginForm = new frmLogin();
                    loginForm.FormClosed += (s, args) => this.Close();
                    loginForm.Show();
                }
                else
                {
                    MessageBox.Show($"Failed to save user data.\nError: {customerDb.LastError}",
                        "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Enhanced input validation method
        private bool ValidateInput(string username, string password, string confirmPassword, string email, string role)
        {
            // Check for empty required fields
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill in all required fields (Username, Password, Confirm Password, and Role).",
                    "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate username
            if (!dbCustomer.IsValidUsername(username))
            {
                MessageBox.Show("Username must be 3-20 characters long and contain only letters, numbers, and underscores.",
                    "Invalid Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            // Validate password strength
            if (!dbCustomer.IsValidPassword(password))
            {
                MessageBox.Show("Password must be at least 6 characters long.",
                    "Weak Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            // Check password confirmation
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please re-enter your password.",
                    "Password Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtConfirmPassword.Focus();
                txtConfirmPassword.SelectAll();
                return false;
            }

            // Validate email if provided
            if (!string.IsNullOrWhiteSpace(email) && !dbCustomer.IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.",
                    "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Additional password strength recommendations
            if (password.Length < 8)
            {
                var result = MessageBox.Show(
                    "Your password is less than 8 characters. For better security, consider using a longer password.\n\nDo you want to continue with this password?",
                    "Password Recommendation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    txtPassword.Focus();
                    return false;
                }
            }

            return true; // All validation passed
        }

        private void lblLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new frmLogin();
            loginForm.FormClosed += (s, args) => this.Show(); // Show registration form again if login is closed
            loginForm.Show();
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

            // Return focus to username field
            txtUsername.Focus();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            // Set focus to username field when form loads
            txtUsername.Focus();

            // Ensure role dropdown has default options if not set
            if (cmbRole.Items.Count == 0)
            {
                cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "Dispatcher", "Employee" });
            }
        }

        // Optional: Add password strength indicator
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            // You could add a password strength indicator here
            // For now, we'll just ensure the form validates properly
        }

        private void pictureBoxLogo_Click(object sender, EventArgs e)
        {
            // Optional: Handle logo click events if required
            // Could show application info or version details
        }

        // Override form closing to ensure proper cleanup
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            customerDb = null; // Clean up database connection
            base.OnFormClosed(e);
        }
    }
}