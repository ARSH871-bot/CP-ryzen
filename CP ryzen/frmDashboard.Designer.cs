﻿using System.Windows.Forms;

//This is the Dashboard (Samnika)

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
            // sidebar Panel
            // 
            this.sidebarPanel.BackColor = System.Drawing.Color.Maroon;

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
            this.lblWelcome.AutoSize = true; // Automatically adjusts label size based on content
            this.lblWelcome.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold); // Sets the font to Arial, 14pt, bold
            this.lblWelcome.ForeColor = System.Drawing.Color.White; // Sets the text color to white for better visibility
            this.lblWelcome.Location = new System.Drawing.Point(270, 20); // Specifies the position of the label
            this.lblWelcome.Name = "lblWelcome"; // Assigns a name to the label for reference in the code
            this.lblWelcome.Size = new System.Drawing.Size(0, 29); // Initial size, will be adjusted automatically by AutoSize property
            this.lblWelcome.TabIndex = 0; // Sets the tab index for focus navigation

            // 
            // lblRole - displays the role of the logged-in user
            // 
            this.lblRole.AutoSize = true; // Automatically adjusts label size based on content
            this.lblRole.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold); // Sets the font to Arial, 12pt, bold
            this.lblRole.ForeColor = System.Drawing.Color.White; // Sets the text color to white for better visibility
            this.lblRole.Location = new System.Drawing.Point(270, 50); // Specifies the position of the label
            this.lblRole.Name = "lblRole"; // Assigns a name to the label for reference in the code
            this.lblRole.Size = new System.Drawing.Size(0, 24); // Initial size, will be adjusted automatically by AutoSize property
            this.lblRole.TabIndex = 1; // Sets the tab index for focus navigation
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
