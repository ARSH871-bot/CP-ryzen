using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    partial class frmLogin
    {
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

            // pictureBoxLogo
            this.pictureBoxLogo.Location = new System.Drawing.Point(150, 20);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(100, 80);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.BackColor = System.Drawing.Color.LightGray;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(80, 110);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 26);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Ryzen Shipping Login";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.lblUsername.Location = new System.Drawing.Point(50, 160);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(73, 16);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username:";

            // txtUsername
            this.txtUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.txtUsername.Location = new System.Drawing.Point(130, 157);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 23);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUsername_KeyPress);

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.lblPassword.Location = new System.Drawing.Point(50, 200);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(72, 16);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password:";

            // txtPassword
            this.txtPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.txtPassword.Location = new System.Drawing.Point(130, 197);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 23);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);

            // btnLogin
            this.btnLogin.BackColor = System.Drawing.Color.DarkBlue;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(130, 240);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(90, 30);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);

            // lblRegister
            this.lblRegister.AutoSize = true;
            this.lblRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRegister.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Underline);
            this.lblRegister.ForeColor = System.Drawing.Color.Blue;
            this.lblRegister.Location = new System.Drawing.Point(230, 247);
            this.lblRegister.Name = "lblRegister";
            this.lblRegister.Size = new System.Drawing.Size(100, 15);
            this.lblRegister.TabIndex = 7;
            this.lblRegister.Text = "Create an Account";
            this.lblRegister.Click += new System.EventHandler(this.lblRegister_Click);

            // frmLogin
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.lblRegister);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pictureBoxLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ryzen Shipping Management - Login";
            this.Load += new System.EventHandler(this.frmLogin_Load); // FIXED: Changed from frmLogin_Load_1

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}