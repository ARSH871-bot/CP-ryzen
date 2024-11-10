using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmLogin : Form
    {
        public static List<User> Users = new List<User>(); // In-memory storage for users

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Check if user exists in memory
            var user = Users.Find(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                new frmDashboard(user).Show(); // Pass the logged-in user to Dashboard
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmRegister().Show(); // Open Register form
        }
    }
}
