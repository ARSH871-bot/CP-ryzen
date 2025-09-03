using System.Windows.Forms;
using System.Drawing;

//This is the Dashboard (Samika)

namespace ShippingManagementSystem
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // DashboardComponents
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
            // Initialize all components first
            this.sidebarPanel = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnNotifications = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnTracking = new System.Windows.Forms.Button();
            this.btnManageShipments = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.mainContentPanel = new System.Windows.Forms.Panel();

            this.sidebarPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // Configure sidebar buttons using the helper method
            // 
            this.CreateSidebarButton(this.btnManageShipments, "Manage Shipments", this.btnManageShipments_Click);
            this.CreateSidebarButton(this.btnTracking, "Tracking", this.btnTracking_Click);
            this.CreateSidebarButton(this.btnReports, "Reports", this.btnReports_Click);
            this.CreateSidebarButton(this.btnNotifications, "Notifications", this.btnNotifications_Click);
            this.CreateSidebarButton(this.btnSettings, "Settings", this.btnSettings_Click);
            this.CreateSidebarButton(this.btnLogout, "Logout", this.btnLogout_Click);

            // 
            // sidebarPanel - Fixed and completed
            // 
            this.sidebarPanel.BackColor = System.Drawing.Color.Maroon;
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Location = new System.Drawing.Point(0, 100);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(250, 600);
            this.sidebarPanel.TabIndex = 1;

            // Add buttons to sidebar panel in correct order (first added appears at top due to DockStyle.Top)
            this.sidebarPanel.Controls.Add(this.btnLogout);
            this.sidebarPanel.Controls.Add(this.btnSettings);
            this.sidebarPanel.Controls.Add(this.btnNotifications);
            this.sidebarPanel.Controls.Add(this.btnReports);
            this.sidebarPanel.Controls.Add(this.btnTracking);
            this.sidebarPanel.Controls.Add(this.btnManageShipments);

            // 
            // headerPanel - Fixed background color
            // 
            this.headerPanel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.headerPanel.Controls.Add(this.lblWelcome);
            this.headerPanel.Controls.Add(this.lblRole);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1200, 100);
            this.headerPanel.TabIndex = 2;
            this.headerPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.headerPanel_Paint);

            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(270, 20);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(200, 29);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome";

            // 
            // lblRole - displays the role of the logged-in user
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblRole.ForeColor = System.Drawing.Color.White;
            this.lblRole.Location = new System.Drawing.Point(270, 50);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(50, 24);
            this.lblRole.TabIndex = 1;
            this.lblRole.Text = "Role:";

            // 
            // mainContentPanel
            // 
            this.mainContentPanel.BackColor = System.Drawing.Color.White;
            this.mainContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContentPanel.Location = new System.Drawing.Point(250, 100);
            this.mainContentPanel.Name = "mainContentPanel";
            this.mainContentPanel.Size = new System.Drawing.Size(950, 600);
            this.mainContentPanel.TabIndex = 0;

            //  
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.mainContentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "frmDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dashboard - Ryzen Shipping Management";
            this.Load += new System.EventHandler(this.frmDashboard_Load);

            this.sidebarPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private void CreateSidebarButton(Button button, string text, System.EventHandler clickHandler)
        {
            button.Text = text;
            button.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            button.ForeColor = System.Drawing.Color.White;
            button.BackColor = System.Drawing.Color.Maroon;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Dock = DockStyle.Top;
            button.Height = 50;
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Padding = new Padding(20, 0, 0, 0);
            button.Cursor = Cursors.Hand;

            // Add hover effects
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(180, 0, 0);
            button.MouseLeave += (s, e) => button.BackColor = Color.Maroon;

            button.Click += clickHandler;
        }
    }
}