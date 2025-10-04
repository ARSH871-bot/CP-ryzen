using System;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmLogin : Form
    {
        private UserManager userManager;

        public frmLogin()
        {
            InitializeComponent();
            userManager = new UserManager();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            ErrorHandler.LogInfo("Login form loaded", "frmLogin");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ErrorHandler.ShowWarning("Please enter both username and password.", "Login Failed");
                    return;
                }

                // Authenticate using database
                User user = userManager.AuthenticateUser(username, password);

                if (user != null)
                {
                    ErrorHandler.ShowInfo("Login Successful!", "Success");
                    ErrorHandler.LogInfo($"User logged in: {username}", "Login");

                    this.Hide();
                    var dashboard = new frmDashboard(user);
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();
                }
                else
                {
                    ErrorHandler.ShowWarning(userManager.LastError, "Login Failed");
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Login Process");
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
    }
}