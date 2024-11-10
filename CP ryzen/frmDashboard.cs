using System;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmDashboard : Form
    {
        private User loggedInUser;

        public frmDashboard(User user)
        {
            InitializeComponent();
            loggedInUser = user;
            lblWelcome.Text = $"Welcome, {loggedInUser.Username}!";
            lblRole.Text = $"Role: {loggedInUser.Role}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have been logged out.");
            this.Hide();
            new frmLogin().Show();
        }

        private void btnManageShipments_Click(object sender, EventArgs e)
        {
            DisplayInMainContentPanel(new frmManageShipments());
        }

        private void btnTracking_Click(object sender, EventArgs e)
        {
            DisplayInMainContentPanel(new frmTracking());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            DisplayInMainContentPanel(new frmReports());
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            DisplayInMainContentPanel(new frmNotifications());
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            DisplayInMainContentPanel(new frmAccountSettings());
        }

        private void DisplayInMainContentPanel(Form form)
        {
            this.mainContentPanel.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            this.mainContentPanel.Controls.Add(form);
            form.Show();
        }
    }
}
