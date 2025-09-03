using System;
using System.Drawing; // Required for color and image usage
using System.Windows.Forms; // Required for creating Windows Form

//This is the Login Form 
// Namespace for the Shipping Management System

namespace ShippingManagementSystem
{
    // Partial class for the login form
    partial class frmLogin
    {
        //  Declaring UI components used in the Form
        // Components for the form
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.PictureBox pictureBoxLogo;// Logo image
        private System.Windows.Forms.Label lblTitle; // Form title
        private System.Windows.Forms.Label lblUsername;// Username label
        private System.Windows.Forms.TextBox txtUsername; // Username input
        private System.Windows.Forms.Label lblPassword;  // Password label
        private System.Windows.Forms.TextBox txtPassword; // Password input
        private System.Windows.Forms.Button btnLogin; // Login button
        private System.Windows.Forms.Label lblRegister; // Register link

        // Method to dispose resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose(); // Dispose of form components
            }
            base.Dispose(disposing); // Call base class Dispose method
        }

        // Method to initialize the components of the form
        private void InitializeComponent()
        {
            // Initialize all components
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblRegister = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();

            // Set form size to 50% of screen dimensions
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            this.ClientSize = new System.Drawing.Size(screenWidth / 2, screenHeight / 2);

            // 
            // pictureBoxLogo - displays the company logo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.LightGray;
            this.pictureBoxLogo.Location = new System.Drawing.Point((this.ClientSize.Width - 120) / 2, 40);
            this.pictureBoxLogo.Size = new System.Drawing.Size(120, 60);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;

            // 
            // lblTitle - displays the title of the login page
            // 
            this.lblTitle.Text = "Login to Ryzen Shipping Management";
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTitle.Location = new System.Drawing.Point((this.ClientSize.Width - 350) / 2, 120);
            this.lblTitle.Size = new System.Drawing.Size(350, 30);
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.TabIndex = 1;

            // 
            // lblUsername - label for the Username field
            // 
            this.lblUsername.Text = "Username";
            this.lblUsername.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 170);
            this.lblUsername.Size = new System.Drawing.Size(240, 20);
            this.lblUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.lblUsername.ForeColor = Color.Gray;
            this.lblUsername.TabIndex = 2;

            // 
            // txtUsername - text box to enter Username
            // 
            this.txtUsername.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 195);
            this.txtUsername.Size = new System.Drawing.Size(240, 22);
            this.txtUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUsername_KeyPress);

            // 
            // lblPassword - label for the Password field
            // 
            this.lblPassword.Text = "Password";
            this.lblPassword.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 230);
            this.lblPassword.Size = new System.Drawing.Size(240, 20);
            this.lblPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.lblPassword.ForeColor = Color.Gray;
            this.lblPassword.TabIndex = 4;

            // 
            // txtPassword - text box to enter Password
            // 
            this.txtPassword.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 255);
            this.txtPassword.Size = new System.Drawing.Size(240, 22);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);

            // 
            // btnLogin - button to submit login credentials
            // 
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 300);
            this.btnLogin.Size = new System.Drawing.Size(240, 35);
            this.btnLogin.BackColor = System.Drawing.Color.DarkOrange;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // 
            // lblRegister - label to navigate to the registration form
            // 
            this.lblRegister.Text = "Create an Account";
            this.lblRegister.ForeColor = System.Drawing.Color.Blue;
            this.lblRegister.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Underline);
            this.lblRegister.Location = new System.Drawing.Point((this.ClientSize.Width - 120) / 2, 350);
            this.lblRegister.Size = new System.Drawing.Size(120, 20);
            this.lblRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRegister.TabIndex = 7;
            this.lblRegister.Cursor = Cursors.Hand;
            this.lblRegister.Click += new System.EventHandler(this.lblRegister_Click);

            // 
            // frmLogin - form properties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login - Ryzen Shipping Management";
            this.Name = "frmLogin";

            // FIXED: Changed from frmLogin_Load_1 to frmLogin_Load
            this.Load += new System.EventHandler(this.frmLogin_Load);

            // Complete layout
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}