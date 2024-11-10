using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    partial class frmAccountSettings
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblAccountSettings;
        private Label lblRole;
        private ComboBox cmbRole;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblPassword;
        private TextBox txtPassword;
        private PictureBox pbProfilePicture;
        private Button btnUploadProfilePicture;
        private Button btnSaveChanges;
        private Button btnResetPassword;
        private Button btnViewLogs;
        private Button btnTwoFactorAuth;
        private Label lblBio;
        private TextBox txtBio;
        private RichTextBox rtbLogs;
        private Label lblOTP;
        private TextBox txtOTP;
        private Button btnGenerateOTP;

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
            this.lblAccountSettings = new Label();
            this.lblRole = new Label();
            this.cmbRole = new ComboBox();
            this.lblUsername = new Label();
            this.txtUsername = new TextBox();
            this.lblEmail = new Label();
            this.txtEmail = new TextBox();
            this.lblPassword = new Label();
            this.txtPassword = new TextBox();
            this.pbProfilePicture = new PictureBox();
            this.btnUploadProfilePicture = new Button();
            this.btnSaveChanges = new Button();
            this.btnResetPassword = new Button();
            this.btnViewLogs = new Button();
            this.btnTwoFactorAuth = new Button();
            this.lblBio = new Label();
            this.txtBio = new TextBox();
            this.rtbLogs = new RichTextBox();
            this.lblOTP = new Label();
            this.txtOTP = new TextBox();
            this.btnGenerateOTP = new Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePicture)).BeginInit();
            this.SuspendLayout();

            // lblAccountSettings
            this.lblAccountSettings.AutoSize = true;
            this.lblAccountSettings.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblAccountSettings.ForeColor = Color.DarkSlateBlue;
            this.lblAccountSettings.Location = new Point(30, 30);
            this.lblAccountSettings.Name = "lblAccountSettings";
            this.lblAccountSettings.Size = new Size(239, 32);
            this.lblAccountSettings.TabIndex = 0;
            this.lblAccountSettings.Text = "Account Settings";

            // lblRole
            this.lblRole.AutoSize = true;
            this.lblRole.Font = new Font("Arial", 12F);
            this.lblRole.Location = new Point(30, 100);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new Size(56, 23);
            this.lblRole.TabIndex = 3;
            this.lblRole.Text = "Role:";

            // cmbRole
            this.cmbRole.Font = new Font("Arial", 12F);
            this.cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "Dispatcher", "Employee" });
            this.cmbRole.Location = new Point(150, 100);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new Size(200, 31);
            this.cmbRole.TabIndex = 4;

            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new Font("Arial", 12F);
            this.lblUsername.Location = new Point(30, 190);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new Size(105, 23);
            this.lblUsername.TabIndex = 7;
            this.lblUsername.Text = "Username:";

            // txtUsername
            this.txtUsername.Font = new Font("Arial", 12F);
            this.txtUsername.Location = new Point(150, 190);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(200, 30);
            this.txtUsername.TabIndex = 8;

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new Font("Arial", 12F);
            this.lblEmail.Location = new Point(30, 230);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(64, 23);
            this.lblEmail.TabIndex = 9;
            this.lblEmail.Text = "Email:";

            // txtEmail
            this.txtEmail.Font = new Font("Arial", 12F);
            this.txtEmail.Location = new Point(150, 230);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(200, 30);
            this.txtEmail.TabIndex = 10;

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new Font("Arial", 12F);
            this.lblPassword.Location = new Point(30, 270);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new Size(104, 23);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Password:";

            // txtPassword
            this.txtPassword.Font = new Font("Arial", 12F);
            this.txtPassword.Location = new Point(150, 270);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new Size(200, 30);
            this.txtPassword.TabIndex = 12;
            this.txtPassword.UseSystemPasswordChar = true;

            // pbProfilePicture
            this.pbProfilePicture.BorderStyle = BorderStyle.FixedSingle;
            this.pbProfilePicture.Location = new Point(500, 30);
            this.pbProfilePicture.Name = "pbProfilePicture";
            this.pbProfilePicture.Size = new Size(120, 120);
            this.pbProfilePicture.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pbProfilePicture.TabIndex = 1;
            this.pbProfilePicture.TabStop = false;

            // btnUploadProfilePicture
            this.btnUploadProfilePicture.Location = new Point(500, 160);
            this.btnUploadProfilePicture.Name = "btnUploadProfilePicture";
            this.btnUploadProfilePicture.Size = new Size(120, 30);
            this.btnUploadProfilePicture.TabIndex = 2;
            this.btnUploadProfilePicture.Text = "Upload Picture";
            this.btnUploadProfilePicture.Click += new EventHandler(this.btnUploadProfilePicture_Click);

            // btnSaveChanges
            this.btnSaveChanges.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.btnSaveChanges.Location = new Point(30, 350);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new Size(150, 40);
            this.btnSaveChanges.TabIndex = 13;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.Click += new EventHandler(this.btnSaveChanges_Click);

            // btnResetPassword
            this.btnResetPassword.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.btnResetPassword.Location = new Point(200, 350);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new Size(150, 40);
            this.btnResetPassword.TabIndex = 14;
            this.btnResetPassword.Text = "Reset Password";
            this.btnResetPassword.Click += new EventHandler(this.btnResetPassword_Click);

            // btnViewLogs
            this.btnViewLogs.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.btnViewLogs.Location = new Point(30, 410);
            this.btnViewLogs.Name = "btnViewLogs";
            this.btnViewLogs.Size = new Size(150, 40);
            this.btnViewLogs.TabIndex = 15;
            this.btnViewLogs.Text = "View Logs";
            this.btnViewLogs.Click += new EventHandler(this.btnViewLogs_Click);

            // btnTwoFactorAuth
            this.btnTwoFactorAuth.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.btnTwoFactorAuth.Location = new Point(200, 410);
            this.btnTwoFactorAuth.Name = "btnTwoFactorAuth";
            this.btnTwoFactorAuth.Size = new Size(200, 40);
            this.btnTwoFactorAuth.TabIndex = 16;
            this.btnTwoFactorAuth.Text = "Enable Two-Factor Auth";
            this.btnTwoFactorAuth.Click += new EventHandler(this.btnTwoFactorAuth_Click);

            // lblBio
            this.lblBio.AutoSize = true;
            this.lblBio.Font = new Font("Arial", 12F);
            this.lblBio.Location = new Point(30, 140);
            this.lblBio.Name = "lblBio";
            this.lblBio.Size = new Size(44, 23);
            this.lblBio.TabIndex = 5;
            this.lblBio.Text = "Bio:";

            // txtBio
            this.txtBio.Location = new Point(150, 140);
            this.txtBio.Multiline = true;
            this.txtBio.Name = "txtBio";
            this.txtBio.Size = new Size(300, 30);
            this.txtBio.TabIndex = 6;

            // lblOTP
            this.lblOTP.AutoSize = true;
            this.lblOTP.Font = new Font("Arial", 12F);
            this.lblOTP.Location = new Point(30, 470);
            this.lblOTP.Name = "lblOTP";
            this.lblOTP.Size = new Size(110, 23);
            this.lblOTP.TabIndex = 18;
            this.lblOTP.Text = "Your OTP:";

            // txtOTP
            this.txtOTP.Font = new Font("Arial", 12F);
            this.txtOTP.Location = new Point(150, 470);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.ReadOnly = true;
            this.txtOTP.Size = new Size(150, 30);
            this.txtOTP.TabIndex = 19;

            // btnGenerateOTP
            this.btnGenerateOTP.Font = new Font("Arial", 12F, FontStyle.Bold);
            this.btnGenerateOTP.Location = new Point(320, 470);
            this.btnGenerateOTP.Name = "btnGenerateOTP";
            this.btnGenerateOTP.Size = new Size(150, 30);
            this.btnGenerateOTP.TabIndex = 20;
            this.btnGenerateOTP.Text = "Generate OTP";
            this.btnGenerateOTP.Click += new EventHandler(this.btnGenerateOTP_Click);

            // rtbLogs
            this.rtbLogs.Location = new Point(30, 520);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new Size(650, 100);
            this.rtbLogs.TabIndex = 17;
            this.rtbLogs.Text = "";
            this.rtbLogs.ReadOnly = true;
            this.rtbLogs.Visible = false;

            // frmAccountSettings
            this.ClientSize = new Size(700, 650);
            this.Controls.Add(this.lblAccountSettings);
            this.Controls.Add(this.pbProfilePicture);
            this.Controls.Add(this.btnUploadProfilePicture);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.lblBio);
            this.Controls.Add(this.txtBio);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnSaveChanges);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.btnViewLogs);
            this.Controls.Add(this.btnTwoFactorAuth);
            this.Controls.Add(this.lblOTP);
            this.Controls.Add(this.txtOTP);
            this.Controls.Add(this.btnGenerateOTP);
            this.Controls.Add(this.rtbLogs);
            this.Name = "frmAccountSettings";
            this.Text = "Account Settings";
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
