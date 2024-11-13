using System.Windows.Forms;

namespace ShippingManagementSystem
{
    partial class frmDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // Dashboard Components
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
            // sidebarPanel
            // 
            this.sidebarPanel.BackColor = System.Drawing.Color.Maroon;
            this.sidebarPanel.Controls.Add(this.btnSettings);
            this.sidebarPanel.Controls.Add(this.btnNotifications);
            this.sidebarPanel.Controls.Add(this.btnReports);
            this.sidebarPanel.Controls.Add(this.btnTracking);
            this.sidebarPanel.Controls.Add(this.btnManageShipments);
            this.sidebarPanel.Controls.Add(this.btnLogout);
            this.sidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebarPanel.Location = new System.Drawing.Point(0, 100);
            this.sidebarPanel.Name = "sidebarPanel";
            this.sidebarPanel.Size = new System.Drawing.Size(250, 600);
            this.sidebarPanel.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(0, 0);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 0;
            // 
            // btnNotifications
            // 
            this.btnNotifications.Location = new System.Drawing.Point(0, 0);
            this.btnNotifications.Name = "btnNotifications";
            this.btnNotifications.Size = new System.Drawing.Size(75, 23);
            this.btnNotifications.TabIndex = 1;
            // 
            // btnReports
            // 
            this.btnReports.Location = new System.Drawing.Point(0, 0);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(75, 23);
            this.btnReports.TabIndex = 2;
            // 
            // btnTracking
            // 
            this.btnTracking.Location = new System.Drawing.Point(0, 0);
            this.btnTracking.Name = "btnTracking";
            this.btnTracking.Size = new System.Drawing.Size(75, 23);
            this.btnTracking.TabIndex = 3;
            // 
            // btnManageShipments
            // 
            this.btnManageShipments.Location = new System.Drawing.Point(0, 0);
            this.btnManageShipments.Name = "btnManageShipments";
            this.btnManageShipments.Size = new System.Drawing.Size(75, 23);
            this.btnManageShipments.TabIndex = 4;
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(0, 0);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 5;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.Maroon;
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
            this.lblWelcome.Size = new System.Drawing.Size(0, 29);
            this.lblWelcome.TabIndex = 0;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblRole.ForeColor = System.Drawing.Color.White;
            this.lblRole.Location = new System.Drawing.Point(270, 50);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(0, 24);
            this.lblRole.TabIndex = 1;
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
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.mainContentPanel);
            this.Controls.Add(this.sidebarPanel);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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
            button.Dock = DockStyle.Top;
            button.Height = 50;
            button.Click += clickHandler;
        }
    }
}
