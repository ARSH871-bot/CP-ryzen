using System.Windows.Forms;

//This is the Dashboard (Samnika)

namespace ShippingManagementSystem
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private Panel sidebarPanel;
        private Panel headerPanel;
        private Label lblWelcome;
        private Label lblRole;
        private Button btnLogout;
        private Button btnManageShipments;
        private Button btnTracking; 
        private Button btnReports;
        private Button btnNotifications;
        private Button btnSettings;
        private Panel mainContentPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.sidebarPanel = new Panel();
            this.headerPanel = new Panel();
            this.mainContentPanel = new Panel();
            this.lblWelcome = new Label();
            this.lblRole = new Label();
            this.btnLogout = new Button();
            this.btnManageShipments = new Button();
            this.btnTracking = new Button();
            this.btnReports = new Button();
            this.btnNotifications = new Button();
            this.btnSettings = new Button();

            // Sidebar Panel 
            this.sidebarPanel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.sidebarPanel.Dock = DockStyle.Left;
            this.sidebarPanel.Width = 200;
            this.sidebarPanel.Controls.Add(this.btnManageShipments);
            this.sidebarPanel.Controls.Add(this.btnTracking);
            this.sidebarPanel.Controls.Add(this.btnReports);
            this.sidebarPanel.Controls.Add(this.btnNotifications);
            this.sidebarPanel.Controls.Add(this.btnSettings); 

            // Header Panel 
            this.headerPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.Controls.Add(this.lblWelcome);
            this.headerPanel.Controls.Add(this.lblRole);
            this.headerPanel.Controls.Add(this.btnLogout);

            // Main Content Panel
            this.mainContentPanel.Dock = DockStyle.Fill;
            this.mainContentPanel.BackColor = System.Drawing.Color.LightGray;

            // Welcome Label
            this.lblWelcome.Text = "Welcome, [Username]!";
            this.lblWelcome.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 10);
            this.lblWelcome.AutoSize = true;

            // Role Label
            this.lblRole.Text = "Role: [UserRole]";
            this.lblRole.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Italic);
            this.lblRole.ForeColor = System.Drawing.Color.LightGray;
            this.lblRole.Location = new System.Drawing.Point(20, 35);
            this.lblRole.AutoSize = true;

            // Logout Button
            this.btnLogout.Text = "Logout";
            this.btnLogout.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.BackColor = System.Drawing.Color.Firebrick;
            this.btnLogout.FlatStyle = FlatStyle.Flat;
            this.btnLogout.Location = new System.Drawing.Point(this.headerPanel.Width - 100, 15);
            this.btnLogout.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // Sidebar Buttons
            CreateSidebarButton(this.btnManageShipments, "Manage Shipments", new System.EventHandler(this.btnManageShipments_Click));
            CreateSidebarButton(this.btnTracking, "Tracking", new System.EventHandler(this.btnTracking_Click));
            CreateSidebarButton(this.btnReports, "Reports", new System.EventHandler(this.btnReports_Click));
            CreateSidebarButton(this.btnNotifications, "Notifications", new System.EventHandler(this.btnNotifications_Click));
            CreateSidebarButton(this.btnSettings, "Account Settings", new System.EventHandler(this.btnSettings_Click));

            // frmDashboard
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.mainContentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);
            this.Text = "Dashboard - Shipping Management System";
            this.ResumeLayout(false);
        }

        private void CreateSidebarButton(Button button, string text, System.EventHandler clickHandler)
        {
            button.Text = text;
            button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            button.ForeColor = System.Drawing.Color.White;
            button.BackColor = System.Drawing.Color.DarkSlateGray;
            button.FlatStyle = FlatStyle.Flat;
            button.Dock = DockStyle.Top;
            button.Height = 50;
            button.Click += clickHandler;
        }
    }
}
