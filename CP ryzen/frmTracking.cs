using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmTracking : Form
    {
        private Timer clockTimer;
        private bool isDarkMode = false;

        public frmTracking()
        {
            InitializeComponent();
            InitializeClock();
        }

        private void frmTracking_Load(object sender, EventArgs e)
        {
            rtbTrackingDetails.Text = "Welcome to the Shipment Tracking System.\nPlease enter a tracking number above to retrieve details.";
        }

        private void InitializeClock()
        {
            clockTimer = new Timer();
            clockTimer.Interval = 1000; // Update every second
            clockTimer.Tick += (s, e) =>
            {
                lblClock.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy hh:mm:ss tt");
            };
            clockTimer.Start();
        }

        private void txtTrackingNumber_GotFocus(object sender, EventArgs e)
        {
            if (txtTrackingNumber.Text == "Enter Tracking Number")
            {
                txtTrackingNumber.Text = string.Empty;
                txtTrackingNumber.ForeColor = Color.Black;
            }
        }

        private void txtTrackingNumber_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTrackingNumber.Text))
            {
                txtTrackingNumber.Text = "Enter Tracking Number";
                txtTrackingNumber.ForeColor = Color.Gray;
            }
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            string trackingNumber = txtTrackingNumber.Text.Trim();

            if (string.IsNullOrEmpty(trackingNumber) || trackingNumber == "Enter Tracking Number")
            {
                MessageBox.Show("Please enter a valid tracking number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            rtbTrackingDetails.Text = RetrieveTrackingDetails(trackingNumber);

            if (!cmbHistory.Items.Contains(trackingNumber))
            {
                cmbHistory.Items.Add(trackingNumber);
            }
        }

        private string RetrieveTrackingDetails(string trackingNumber)
        {
            if (trackingNumber == "1234567890")
            {
                pbShipmentProgress.Value = 80;
                return "Tracking Number: 1234567890\nStatus: In Transit\nLast Location: Auckland, NZ\nEstimated Delivery: 2 days.";
            }
            else
            {
                pbShipmentProgress.Value = 0;
                return $"Tracking Number: {trackingNumber}\nStatus: Not Found\nPlease verify the tracking number.";
            }
        }

        private void cmbHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTrackingNumber.Text = cmbHistory.SelectedItem.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Save Tracking Details";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, rtbTrackingDetails.Text);
                    MessageBox.Show("Tracking details saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Printing details...", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter a tracking number and click Track to view shipment details.\nUse the Save button to save details or Print to print them.",
                            "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;

            if (isDarkMode)
            {
                BackColor = Color.Black;
                ForeColor = Color.White;
                txtTrackingNumber.BackColor = Color.Gray;
                txtTrackingNumber.ForeColor = Color.White;
                rtbTrackingDetails.BackColor = Color.Gray;
                rtbTrackingDetails.ForeColor = Color.White;
            }
            else
            {
                BackColor = SystemColors.Control;
                ForeColor = SystemColors.ControlText;
                txtTrackingNumber.BackColor = Color.White;
                txtTrackingNumber.ForeColor = Color.Black;
                rtbTrackingDetails.BackColor = Color.White;
                rtbTrackingDetails.ForeColor = Color.Black;
            }
        }
    }
}
