using System;
using System.Windows.Forms;
//This is the DashBoard
// I have adjust the size of Dashboard
namespace ShippingManagementSystem
{
    public partial class frmDashboard : Form
    {
        private User loggedInUser;

        public frmDashboard(User user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = $"Welcome, {loggedInUser.Username}!";
            lblRole.Text = $"Role: {loggedInUser.Role}";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have been logged out.", "Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            mainContentPanel.Controls.Clear();
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            mainContentPanel.Controls.Add(form);
            form.Show();
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
