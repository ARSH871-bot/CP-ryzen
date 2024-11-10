using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmAccountSettings : Form
    {
        private Random random = new Random(); // For generating OTPs

        public frmAccountSettings()
        {
            InitializeComponent();
        }

        private void frmAccountSettings_Load(object sender, EventArgs e)
        {
            // Set default role
            cmbRole.SelectedIndex = 0; // Default to Admin
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Account settings have been updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Password has been reset successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUploadProfilePicture_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbProfilePicture.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            // Toggle visibility of the logs section
            rtbLogs.Visible = !rtbLogs.Visible;

            if (rtbLogs.Visible)
            {
                // Example logs (these would normally come from a database or log file)
                rtbLogs.Text = "Log Data:\n";
                rtbLogs.AppendText("-------------------------------------------------------------\n");
                rtbLogs.AppendText($"[INFO] {DateTime.Now}: User logged in successfully.\n");
                rtbLogs.AppendText($"[INFO] {DateTime.Now.AddMinutes(-5)}: User updated email address.\n");
                rtbLogs.AppendText($"[WARNING] {DateTime.Now.AddMinutes(-10)}: Failed login attempt.\n");
                rtbLogs.AppendText($"[INFO] {DateTime.Now.AddMinutes(-20)}: Password changed successfully.\n");
            }
            else
            {
                // Hide logs
                rtbLogs.Text = "";
            }
        }

        private void pbProfilePicture_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Click the 'Upload Picture' button to update your profile picture.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTwoFactorAuth_Click(object sender, EventArgs e)
        {
            // Toggle visibility of OTP fields
            lblOTP.Visible = !lblOTP.Visible;
            txtOTP.Visible = !txtOTP.Visible;
            btnGenerateOTP.Visible = !btnGenerateOTP.Visible;

            if (lblOTP.Visible)
            {
                MessageBox.Show("Two-Factor Authentication is now enabled. Use the 'Generate OTP' button to generate a one-time password.",
                    "Two-Factor Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Two-Factor Authentication is now disabled.",
                    "Two-Factor Authentication", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnGenerateOTP_Click(object sender, EventArgs e)
        {
            // Generate a random 6-digit OTP
            int otp = random.Next(100000, 999999);
            txtOTP.Text = otp.ToString();

            MessageBox.Show($"Your one-time password (OTP) is: {otp}\nPlease use this OTP within 5 minutes.",
                "OTP Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
