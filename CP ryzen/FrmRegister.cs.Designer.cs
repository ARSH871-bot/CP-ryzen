namespace ShippingManagementSystem
{
    partial class frmRegister
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblLogin;

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
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.labelConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.labelPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.labelRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblLogin = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBoxLogo
            // 
            /*this.pictureBoxLogo.Image = Properties.Resources.logo; // Use the resource name here
            this.pictureBoxLogo.Location = new System.Drawing.Point(165, 20); // Centered horizontally
            this.pictureBoxLogo.Size = new System.Drawing.Size(200, 100);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;*/

            // 
            // labelTitle
            // 
            this.labelTitle.Text = "Register - Ryzen Shipping Management";
            this.labelTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.Navy;
            this.labelTitle.Location = new System.Drawing.Point(85, 130);
            this.labelTitle.Size = new System.Drawing.Size(360, 30);
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Username
            this.labelUsername.Text = "Username";
            this.labelUsername.Location = new System.Drawing.Point(50, 180);
            this.labelUsername.Font = new System.Drawing.Font("Arial", 10F);
            this.txtUsername.Location = new System.Drawing.Point(50, 205);
            this.txtUsername.Size = new System.Drawing.Size(400, 25);

            // Password
            this.labelPassword.Text = "Password";
            this.labelPassword.Location = new System.Drawing.Point(50, 250);
            this.labelPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.txtPassword.Location = new System.Drawing.Point(50, 275);
            this.txtPassword.Size = new System.Drawing.Size(400, 25);
            this.txtPassword.PasswordChar = '*';

            // Confirm Password
            this.labelConfirmPassword.Text = "Confirm Password";
            this.labelConfirmPassword.Location = new System.Drawing.Point(50, 320);
            this.labelConfirmPassword.Font = new System.Drawing.Font("Arial", 10F);
            this.txtConfirmPassword.Location = new System.Drawing.Point(50, 345);
            this.txtConfirmPassword.Size = new System.Drawing.Size(400, 25);
            this.txtConfirmPassword.PasswordChar = '*';

            // Company Name
            this.labelCompanyName.Text = "Company Name";
            this.labelCompanyName.Location = new System.Drawing.Point(50, 390);
            this.labelCompanyName.Font = new System.Drawing.Font("Arial", 10F);
            this.txtCompanyName.Location = new System.Drawing.Point(50, 415);
            this.txtCompanyName.Size = new System.Drawing.Size(400, 25);

            // Email
            this.labelEmail.Text = "Email";
            this.labelEmail.Location = new System.Drawing.Point(50, 460);
            this.labelEmail.Font = new System.Drawing.Font("Arial", 10F);
            this.txtEmail.Location = new System.Drawing.Point(50, 485);
            this.txtEmail.Size = new System.Drawing.Size(400, 25);

            // Phone
            this.labelPhone.Text = "Phone";
            this.labelPhone.Location = new System.Drawing.Point(50, 530);
            this.labelPhone.Font = new System.Drawing.Font("Arial", 10F);
            this.txtPhone.Location = new System.Drawing.Point(50, 555);
            this.txtPhone.Size = new System.Drawing.Size(400, 25);

            // Role
            this.labelRole.Text = "Role";
            this.labelRole.Location = new System.Drawing.Point(50, 600);
            this.labelRole.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbRole.Location = new System.Drawing.Point(50, 625);
            this.cmbRole.Size = new System.Drawing.Size(400, 25);
            this.cmbRole.Items.AddRange(new object[] { "Admin", "Manager", "Dispatcher", "Employee" });

            // Register Button
            this.btnRegister.Text = "Register";
            this.btnRegister.Location = new System.Drawing.Point(50, 670);
            this.btnRegister.Size = new System.Drawing.Size(190, 40);
            this.btnRegister.BackColor = System.Drawing.Color.Navy;
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            // Clear Button
            this.btnClear.Text = "Clear";
            this.btnClear.Location = new System.Drawing.Point(260, 670);
            this.btnClear.Size = new System.Drawing.Size(190, 40);
            this.btnClear.BackColor = System.Drawing.Color.DarkSlateGray;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // Login Link
            this.lblLogin.Text = "Already have an account? Login";
            this.lblLogin.Location = new System.Drawing.Point(135, 730);
            this.lblLogin.ForeColor = System.Drawing.Color.Blue;
            this.lblLogin.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Underline);
            this.lblLogin.Size = new System.Drawing.Size(250, 20);
            this.lblLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLogin.Click += new System.EventHandler(this.lblLogin_Click);

            // frmRegister
            this.ClientSize = new System.Drawing.Size(500, 800); // Increase width and height
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.labelConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.labelCompanyName);
            this.Controls.Add(this.txtCompanyName);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.labelPhone);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.labelRole);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Register - Ryzen Shipping Management";
            this.ResumeLayout(false);
            this.PerformLayout();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
        }
    }
}
