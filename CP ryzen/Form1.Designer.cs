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
        // Components for the form

        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblRegister;

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
/            this.ClientSize = new System.Drawing.Size(screenWidth / 2, screenHeight / 2);

            // 
            // pictureBoxLogo

            // 
 //           this.pictureBoxLogo.Image = Image.FromFile(@"C:\ARSHOP\CP ryzen\Images\Logo.jpg"); // Replace with actual path
            this.pictureBoxLogo.Location = new System.Drawing.Point((this.ClientSize.Width - 120) / 2, 40); // Center the logo

            //  
            this.pictureBoxLogo.Image = Image.FromFile(@"C:\ARSHOP\CP ryzen\Images\Logo.jpg");
            this.pictureBoxLogo.Location = new System.Drawing.Point((this.ClientSize.Width - 120) / 2, 40);

            this.pictureBoxLogo.Size = new System.Drawing.Size(120, 60);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            // 
            // lblTitle
            // 
            this.lblTitle.Text = "Login to Ryzen Shipping Management";
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTitle.Location = new System.Drawing.Point((this.ClientSize.Width - 280) / 2, 120);
            this.lblTitle.AutoSize = true;

            // 
            // lblUsername 
            // 
            this.lblUsername.Text = "Username";
            this.lblUsername.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 160);
            this.lblUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.lblUsername.ForeColor = Color.Gray;

            // s
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 185);
            this.txtUsername.Size = new System.Drawing.Size(240, 22);
            this.txtUsername.Font = new System.Drawing.Font("Arial", 10F);

            // 
            // lblPassword
            // 
            this.lblPassword.Text = "Password";
            this.lblPassword.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 220);
            this.lblPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.lblPassword.ForeColor = Color.Gray;

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 245);
            this.txtPassword.Size = new System.Drawing.Size(240, 22);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Font = new System.Drawing.Font("Arial", 10F);

            // 
            // btnLogin
            // 
            this.btnLogin.Text = "Login";
            this.btnLogin.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2, 290);
            this.btnLogin.Size = new System.Drawing.Size(240, 35);
            this.btnLogin.BackColor = System.Drawing.Color.DarkOrange;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // 
            // lblRegister
            // 
            this.lblRegister.Text = "Create an Account";
            this.lblRegister.ForeColor = System.Drawing.Color.Blue;
            this.lblRegister.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Underline);
            this.lblRegister.Location = new System.Drawing.Point((this.ClientSize.Width - 240) / 2 + 70, 340);
            this.lblRegister.AutoSize = true;
            this.lblRegister.Cursor = Cursors.Hand;
            this.lblRegister.Click += new System.EventHandler(this.lblRegister_Click);

            // 
            // frmLogin 
            // 
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login - Ryzen Shipping Management";
            this.ResumeLayout(false);
            this.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
        }
    }
}
