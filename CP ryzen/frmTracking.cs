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
            ErrorHandler.SafeExecute(() =>
            {
                rtbTrackingDetails.Text = "Welcome to the Shipment Tracking System.\n\n" +
                                         "How to use:\n" +
                                         "1. Enter a tracking number in the field above\n" +
                                         "2. Click 'Track' to retrieve shipment details\n" +
                                         "3. Use the search history dropdown to quickly access previous searches\n\n" +
                                         "Sample tracking numbers to try:\n" +
                                         "• 1234567890 (In Transit)\n" +
                                         "• 0987654321 (Delivered)\n" +
                                         "• TEST123456 (Processing)";

                ErrorHandler.LogInfo("Tracking form loaded successfully", "frmTracking_Load");
            }, "Loading Tracking Form");
        }

        private void InitializeClock()
        {
            ErrorHandler.SafeExecute(() =>
            {
                clockTimer = new Timer();
                clockTimer.Interval = 1000; // Update every second
                clockTimer.Tick += (s, e) =>
                {
                    ErrorHandler.SafeExecute(() =>
                    {
                        if (lblClock != null)
                        {
                            lblClock.Text = $"Current Time: {DateTime.Now:dddd, MMMM dd, yyyy hh:mm:ss tt}";
                        }
                    }, "Clock Update");
                };
                clockTimer.Start();
                ErrorHandler.LogInfo("Clock timer initialized and started", "InitializeClock");
            }, "Clock Initialization");
        }

        private void txtTrackingNumber_GotFocus(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (txtTrackingNumber.Text == "Enter Tracking Number")
                {
                    txtTrackingNumber.Text = string.Empty;
                    txtTrackingNumber.ForeColor = Color.Black;
                }
            }, "Tracking Number Focus");
        }

        private void txtTrackingNumber_LostFocus(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(txtTrackingNumber.Text))
                {
                    txtTrackingNumber.Text = "Enter Tracking Number";
                    txtTrackingNumber.ForeColor = Color.Gray;
                }
            }, "Tracking Number Blur");
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string trackingNumber = txtTrackingNumber.Text.Trim();

                // Validate input
                if (string.IsNullOrEmpty(trackingNumber) || trackingNumber == "Enter Tracking Number")
                {
                    ErrorHandler.ShowWarning("Please enter a valid tracking number.", "Input Required");
                    txtTrackingNumber.Focus();
                    return;
                }

                // Validate tracking number format
                if (trackingNumber.Length < 5)
                {
                    ErrorHandler.ShowWarning("Tracking number must be at least 5 characters long.", "Invalid Format");
                    txtTrackingNumber.Focus();
                    return;
                }

                // Show loading state
                ShowLoadingState(true);

                try
                {
                    // Simulate API call delay
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500); // Simulate network delay

                    string trackingResult = RetrieveTrackingDetails(trackingNumber);
                    rtbTrackingDetails.Text = trackingResult;

                    // Add to history if not already present
                    AddToHistory(trackingNumber);

                    ErrorHandler.LogInfo($"Successfully tracked package: {trackingNumber}", "btnTrack_Click");
                }
                finally
                {
                    ShowLoadingState(false);
                }
            }, "Package Tracking");
        }

        private void ShowLoadingState(bool isLoading)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (btnTrack != null)
                {
                    btnTrack.Text = isLoading ? "Tracking..." : "Track";
                    btnTrack.Enabled = !isLoading;
                }

                if (rtbTrackingDetails != null && isLoading)
                {
                    rtbTrackingDetails.Text = "Retrieving tracking information...\nPlease wait...";
                }
            }, "Loading State Update");
        }

        private string RetrieveTrackingDetails(string trackingNumber)
        {
            return ErrorHandler.SafeExecute(() =>
            {
                // Enhanced tracking simulation with multiple scenarios
                switch (trackingNumber.ToUpper())
                {
                    case "1234567890":
                        pbShipmentProgress.Value = 75;
                        return $"📦 TRACKING DETAILS\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"Tracking Number: {trackingNumber}\n" +
                               $"Status: IN TRANSIT\n" +
                               $"Current Location: Auckland Distribution Center, NZ\n" +
                               $"Estimated Delivery: {DateTime.Now.AddDays(2):MMM dd, yyyy}\n" +
                               $"Service Type: Express Shipping\n\n" +
                               $"📍 TRACKING HISTORY:\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"{DateTime.Now.AddDays(-3):MMM dd} - Package picked up from sender\n" +
                               $"{DateTime.Now.AddDays(-2):MMM dd} - Arrived at sorting facility\n" +
                               $"{DateTime.Now.AddDays(-1):MMM dd} - In transit to destination\n" +
                               $"{DateTime.Now:MMM dd} - Out for delivery\n\n" +
                               $"📞 For questions, call: 0800-SHIPPING";

                    case "0987654321":
                        pbShipmentProgress.Value = 100;
                        return $"✅ DELIVERY CONFIRMED\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"Tracking Number: {trackingNumber}\n" +
                               $"Status: DELIVERED\n" +
                               $"Delivered To: Front door\n" +
                               $"Delivery Date: {DateTime.Now.AddDays(-1):MMM dd, yyyy} at 2:30 PM\n" +
                               $"Signed By: Recipient\n" +
                               $"Service Type: Standard Shipping\n\n" +
                               $"Thank you for choosing our shipping service!";

                    case "TEST123456":
                        pbShipmentProgress.Value = 25;
                        return $"🔄 PROCESSING\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"Tracking Number: {trackingNumber}\n" +
                               $"Status: PROCESSING\n" +
                               $"Location: Origin Facility\n" +
                               $"Estimated Ship Date: {DateTime.Now.AddDays(1):MMM dd, yyyy}\n" +
                               $"Estimated Delivery: {DateTime.Now.AddDays(5):MMM dd, yyyy}\n" +
                               $"Service Type: Standard Ground\n\n" +
                               $"Your package is being prepared for shipment.";

                    default:
                        pbShipmentProgress.Value = 0;
                        ErrorHandler.LogWarning($"Tracking number not found: {trackingNumber}", "RetrieveTrackingDetails");
                        return $"❌ TRACKING NOT FOUND\n" +
                               $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                               $"Tracking Number: {trackingNumber}\n" +
                               $"Status: NOT FOUND\n\n" +
                               $"This tracking number could not be found in our system.\n\n" +
                               $"Please verify the tracking number and try again.\n" +
                               $"If you continue to have issues, please contact customer support.\n\n" +
                               $"📞 Customer Support: 0800-HELP-NOW\n" +
                               $"🌐 Web: www.ryzenshipment.co.nz";
                }
            }, "Unknown", "Tracking Details Retrieval");
        }

        private void AddToHistory(string trackingNumber)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (cmbHistory != null && !cmbHistory.Items.Contains(trackingNumber))
                {
                    cmbHistory.Items.Insert(0, trackingNumber); // Add to top

                    // Limit history to 10 items
                    while (cmbHistory.Items.Count > 10)
                    {
                        cmbHistory.Items.RemoveAt(cmbHistory.Items.Count - 1);
                    }
                }
            }, "History Management");
        }

        private void cmbHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (cmbHistory.SelectedItem != null)
                {
                    txtTrackingNumber.Text = cmbHistory.SelectedItem.ToString();
                    txtTrackingNumber.ForeColor = Color.Black;
                }
            }, "History Selection");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(rtbTrackingDetails.Text) ||
                    rtbTrackingDetails.Text.Contains("Welcome to the Shipment Tracking System"))
                {
                    ErrorHandler.ShowWarning("No tracking details to save. Please track a package first.", "No Data");
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.Title = "Save Tracking Details";
                    saveFileDialog.FileName = $"Tracking_Details_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, rtbTrackingDetails.Text);
                        ErrorHandler.ShowInfo("Tracking details saved successfully!", "Save Complete");
                        ErrorHandler.LogInfo($"Tracking details saved to: {saveFileDialog.FileName}", "btnSave_Click");
                    }
                }
            }, "Save Tracking Details");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(rtbTrackingDetails.Text) ||
                    rtbTrackingDetails.Text.Contains("Welcome to the Shipment Tracking System"))
                {
                    ErrorHandler.ShowWarning("No tracking details to print. Please track a package first.", "No Data");
                    return;
                }

                ErrorHandler.ShowInfo("Print functionality would open the system print dialog here.\n\nIn a full implementation, this would integrate with the system printer.", "Print Simulation");
                ErrorHandler.LogInfo("Print tracking details requested", "btnPrint_Click");
            }, "Print Tracking Details");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string helpText = "📋 TRACKING SYSTEM HELP\n" +
                                 "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                 "🔍 HOW TO TRACK:\n" +
                                 "1. Enter your tracking number in the text field\n" +
                                 "2. Click the 'Track' button\n" +
                                 "3. View detailed tracking information\n\n" +
                                 "📝 SAMPLE TRACKING NUMBERS:\n" +
                                 "• 1234567890 - In Transit package\n" +
                                 "• 0987654321 - Delivered package\n" +
                                 "• TEST123456 - Processing package\n\n" +
                                 "💾 ADDITIONAL FEATURES:\n" +
                                 "• Save tracking details to file\n" +
                                 "• Print tracking information\n" +
                                 "• Search history for quick access\n" +
                                 "• Dark mode toggle\n\n" +
                                 "📞 SUPPORT:\n" +
                                 "Phone: 0800-SHIPPING\n" +
                                 "Email: support@ryzenshipment.co.nz";

                MessageBox.Show(helpText, "Tracking System Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ErrorHandler.LogInfo("Help information displayed", "btnHelp_Click");
            }, "Help Display");
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                isDarkMode = !isDarkMode;
                ApplyTheme(isDarkMode);
                ErrorHandler.LogInfo($"Theme changed to: {(isDarkMode ? "Dark" : "Light")}", "btnDarkMode_Click");
            }, "Theme Toggle");
        }

        private void ApplyTheme(bool darkMode)
        {
            ErrorHandler.SafeExecute(() =>
            {
                Color backgroundColor = darkMode ? Color.FromArgb(45, 45, 48) : Color.White;
                Color foregroundColor = darkMode ? Color.White : Color.Black;
                Color controlBackColor = darkMode ? Color.FromArgb(62, 62, 66) : Color.White;

                // Apply to form
                this.BackColor = backgroundColor;
                this.ForeColor = foregroundColor;

                // Apply to controls
                foreach (Control control in this.Controls)
                {
                    ApplyThemeToControl(control, backgroundColor, foregroundColor, controlBackColor);
                }

                // Update button text
                if (btnDarkMode != null)
                {
                    btnDarkMode.Text = darkMode ? "Light Mode" : "Dark Mode";
                }
            }, "Theme Application");
        }

        private void ApplyThemeToControl(Control control, Color bgColor, Color fgColor, Color controlBgColor)
        {
            ErrorHandler.SafeExecute(() =>
            {
                control.BackColor = control is TextBox || control is RichTextBox || control is ComboBox ? controlBgColor : bgColor;
                control.ForeColor = fgColor;

                // Recursively apply to child controls
                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child, bgColor, fgColor, controlBgColor);
                }
            }, "Individual Control Theme");
        }

        // Enhanced form closing with cleanup
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                clockTimer?.Stop();
                clockTimer?.Dispose();
                ErrorHandler.LogInfo("Tracking form closed and resources cleaned up", "OnFormClosing");
            }, "Form Cleanup");

            base.OnFormClosing(e);
        }

        // Method to refresh data (called from dashboard)
        public void RefreshData()
        {
            ErrorHandler.SafeExecute(() =>
            {
                frmTracking_Load(this, EventArgs.Empty);
            }, "Data Refresh");
        }
    }
}