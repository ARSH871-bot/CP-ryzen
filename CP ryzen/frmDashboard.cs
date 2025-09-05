using System;
using System.Windows.Forms;

//This is the Dashboard - Enhanced with better navigation and error handling
namespace ShippingManagementSystem
{
    public partial class frmDashboard : Form
    {
        private User loggedInUser;
        private Form currentChildForm = null; // Track the currently loaded form

        public frmDashboard(User user)
        {
            InitializeComponent();
            loggedInUser = user;
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            // Display welcome message and user role
            lblWelcome.Text = $"Welcome, {loggedInUser.Username}!";
            lblRole.Text = $"Role: {loggedInUser.Role}";

            // Load default welcome content
            LoadWelcomeContent();
        }

        private void LoadWelcomeContent()
        {
            // Clear the main panel and show welcome message
            mainContentPanel.Controls.Clear();

            Label welcomeLabel = new Label();
            welcomeLabel.Text = $"Welcome to Ryzen Shipping Management System\n\n" +
                               $"User: {loggedInUser.Username}\n" +
                               $"Role: {loggedInUser.Role}\n\n" +
                               "Please select an option from the sidebar to navigate through the system.\n\n" +
                               "Available Features:\n" +
                               "• Manage Shipments - Add, edit, and track shipments\n" +
                               "• Tracking - Track individual packages\n" +
                               "• Reports - Generate and export reports\n" +
                               "• Notifications - View system notifications\n" +
                               "• Settings - Manage your account settings";

            welcomeLabel.Font = new System.Drawing.Font("Arial", 11F);
            welcomeLabel.ForeColor = System.Drawing.Color.DarkSlateGray;
            welcomeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            welcomeLabel.Dock = DockStyle.Fill;
            welcomeLabel.Padding = new Padding(20);

            mainContentPanel.Controls.Add(welcomeLabel);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Clean up any open child forms
                CleanupCurrentForm();

                MessageBox.Show("You have been logged out successfully.", "Logout",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                var loginForm = new frmLogin();
                loginForm.FormClosed += (s, args) => this.Close(); // Close dashboard when login closes
                loginForm.Show();
            }
        }

        private void btnManageShipments_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayInMainContentPanel(new frmManageShipments(), "Manage Shipments");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Manage Shipments", ex.Message);
            }
        }

        private void btnTracking_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayInMainContentPanel(new frmTracking(), "Shipment Tracking");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Tracking", ex.Message);
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayInMainContentPanel(new frmReports(), "Reports");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Reports", ex.Message);
            }
        }

        private void btnNotifications_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayInMainContentPanel(new frmNotifications(), "Notifications");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Notifications", ex.Message);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayInMainContentPanel(new frmAccountSettings(), "Account Settings");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Account Settings", ex.Message);
            }
        }

        // Enhanced method to display forms in the main content panel
        private void DisplayInMainContentPanel(Form form, string sectionName = "")
        {
            // Clean up previous form first
            CleanupCurrentForm();

            // Clear the main content panel
            mainContentPanel.Controls.Clear();

            // Configure the new form
            currentChildForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            // Add the form to the panel and show it
            mainContentPanel.Controls.Add(form);
            form.Show();

            // Update window title if section name is provided
            if (!string.IsNullOrEmpty(sectionName))
            {
                this.Text = $"Dashboard - {sectionName} - Ryzen Shipping Management";
            }

            // Bring form to front and ensure it's focused
            form.BringToFront();
            form.Focus();
        }

        // Clean up the current child form
        private void CleanupCurrentForm()
        {
            if (currentChildForm != null)
            {
                try
                {
                    currentChildForm.Close();
                    currentChildForm.Dispose();
                }
                catch (Exception)
                {
                    // Ignore disposal errors
                }
                finally
                {
                    currentChildForm = null;
                }
            }
        }

        // Show error message when forms fail to load
        private void ShowErrorMessage(string moduleName, string errorMessage)
        {
            MessageBox.Show($"Error loading {moduleName}:\n\n{errorMessage}\n\nPlease try again or contact support if the problem persists.",
                "Module Loading Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Load welcome content as fallback
            LoadWelcomeContent();
        }

        // Handle form closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CleanupCurrentForm();
            base.OnFormClosing(e);
        }

        private void headerPanel_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom header painting logic here
            // Could add gradients, logos, or other visual enhancements
        }

        // Utility method to refresh current content
        public void RefreshCurrentContent()
        {
            if (currentChildForm != null)
            {
                currentChildForm.Refresh();
            }
            else
            {
                LoadWelcomeContent();
            }
        }

        // Method to get current user (useful for child forms that might need user context)
        public User GetCurrentUser()
        {
            return loggedInUser;
        }

        // Method to navigate programmatically (useful for shortcuts or external calls)
        public void NavigateToSection(string sectionName)
        {
            switch (sectionName.ToLower())
            {
                case "shipments":
                case "manage shipments":
                    btnManageShipments_Click(this, EventArgs.Empty);
                    break;
                case "tracking":
                    btnTracking_Click(this, EventArgs.Empty);
                    break;
                case "reports":
                    btnReports_Click(this, EventArgs.Empty);
                    break;
                case "notifications":
                    btnNotifications_Click(this, EventArgs.Empty);
                    break;
                case "settings":
                case "account settings":
                    btnSettings_Click(this, EventArgs.Empty);
                    break;
                default:
                    LoadWelcomeContent();
                    break;
            }
        }
    }
}