using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmTracking : Form
    {
        private Timer clockTimer;
        private bool isDarkMode = false;
        private ShipmentManager shipmentManager;

        public frmTracking()
        {
            InitializeComponent();
            shipmentManager = new ShipmentManager();
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
                                         "Enter any tracking number from your shipments to track it.";

                ErrorHandler.LogInfo("Tracking form loaded successfully", "frmTracking_Load");
            }, "Loading Tracking Form");
        }

        private void InitializeClock()
        {
            ErrorHandler.SafeExecute(() =>
            {
                clockTimer = new Timer();
                clockTimer.Interval = 1000;
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
                ErrorHandler.LogInfo("Clock timer initialized", "InitializeClock");
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

                if (string.IsNullOrEmpty(trackingNumber) || trackingNumber == "Enter Tracking Number")
                {
                    ErrorHandler.ShowWarning("Please enter a valid tracking number.", "Input Required");
                    txtTrackingNumber.Focus();
                    return;
                }

                if (trackingNumber.Length < 5)
                {
                    ErrorHandler.ShowWarning("Tracking number must be at least 5 characters long.", "Invalid Format");
                    txtTrackingNumber.Focus();
                    return;
                }

                ShowLoadingState(true);

                try
                {
                    System.Threading.Thread.Sleep(500);

                    // Get shipment from database
                    Shipment shipment = shipmentManager.GetShipmentByTrackingNumber(trackingNumber);

                    string trackingResult;
                    if (shipment != null)
                    {
                        trackingResult = FormatShipmentDetails(shipment);
                        pbShipmentProgress.Value = CalculateProgress(shipment.Status);
                    }
                    else
                    {
                        trackingResult = $"TRACKING NOT FOUND\n" +
                                       $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                       $"Tracking Number: {trackingNumber}\n" +
                                       $"Status: NOT FOUND\n\n" +
                                       $"This tracking number could not be found in our system.\n\n" +
                                       $"Please verify the tracking number and try again.";
                        pbShipmentProgress.Value = 0;
                    }

                    rtbTrackingDetails.Text = trackingResult;
                    AddToHistory(trackingNumber);
                    ErrorHandler.LogInfo($"Tracked package: {trackingNumber}", "btnTrack_Click");
                }
                finally
                {
                    ShowLoadingState(false);
                }
            }, "Package Tracking");
        }

        private string FormatShipmentDetails(Shipment shipment)
        {
            return $"📦 TRACKING DETAILS\n" +
                   $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                   $"Tracking Number: {shipment.TrackingNumber}\n" +
                   $"Status: {shipment.Status.ToUpper()}\n" +
                   $"Description: {shipment.Description}\n" +
                   $"Destination: {shipment.Destination}\n" +
                   $"Date Shipped: {shipment.DateShipped:MMM dd, yyyy}\n" +
                   $"Estimated Arrival: {shipment.EstimatedArrival:MMM dd, yyyy}\n" +
                   (shipment.ActualArrival.HasValue ? $"Delivered: {shipment.ActualArrival:MMM dd, yyyy}\n" : "") +
                   $"\n📍 SHIPMENT HISTORY:\n" +
                   $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                   $"{shipment.DateShipped:MMM dd} - Package picked up\n" +
                   $"{shipment.DateShipped.AddDays(1):MMM dd} - In transit\n" +
                   (shipment.Status == "Delivered" ? $"{DateTime.Now:MMM dd} - Delivered\n" : $"{DateTime.Now:MMM dd} - {shipment.Status}\n");
        }

        private int CalculateProgress(string status)
        {
            switch (status?.ToLower())
            {
                case "pending": return 25;
                case "processing": return 25;
                case "in transit": return 75;
                case "delivered": return 100;
                default: return 50;
            }
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

        private void AddToHistory(string trackingNumber)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (cmbHistory != null && !cmbHistory.Items.Contains(trackingNumber))
                {
                    cmbHistory.Items.Insert(0, trackingNumber);
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
                        System.IO.File.WriteAllText(saveFileDialog.FileName, rtbTrackingDetails.Text);
                        ErrorHandler.ShowInfo("Tracking details saved successfully!", "Save Complete");
                        ErrorHandler.LogInfo($"Saved to: {saveFileDialog.FileName}", "btnSave_Click");
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
                    ErrorHandler.ShowWarning("No tracking details to print.", "No Data");
                    return;
                }

                ErrorHandler.ShowInfo("Print dialog would open here.", "Print Simulation");
            }, "Print Tracking Details");
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string helpText = "📋 TRACKING SYSTEM HELP\n" +
                                 "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                                 "🔍 HOW TO TRACK:\n" +
                                 "1. Enter your tracking number\n" +
                                 "2. Click the 'Track' button\n" +
                                 "3. View detailed tracking information\n\n" +
                                 "💾 ADDITIONAL FEATURES:\n" +
                                 "• Save tracking details to file\n" +
                                 "• Print tracking information\n" +
                                 "• Search history for quick access\n" +
                                 "• Dark mode toggle";

                MessageBox.Show(helpText, "Tracking System Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }, "Help Display");
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                isDarkMode = !isDarkMode;
                ApplyTheme(isDarkMode);
            }, "Theme Toggle");
        }

        private void ApplyTheme(bool darkMode)
        {
            ErrorHandler.SafeExecute(() =>
            {
                Color backgroundColor = darkMode ? Color.FromArgb(45, 45, 48) : Color.White;
                Color foregroundColor = darkMode ? Color.White : Color.Black;
                Color controlBackColor = darkMode ? Color.FromArgb(62, 62, 66) : Color.White;

                this.BackColor = backgroundColor;
                this.ForeColor = foregroundColor;

                foreach (Control control in this.Controls)
                {
                    ApplyThemeToControl(control, backgroundColor, foregroundColor, controlBackColor);
                }

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

                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child, bgColor, fgColor, controlBgColor);
                }
            }, "Individual Control Theme");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                clockTimer?.Stop();
                clockTimer?.Dispose();
                ErrorHandler.LogInfo("Tracking form closed", "OnFormClosing");
            }, "Form Cleanup");

            base.OnFormClosing(e);
        }

        public void RefreshData()
        {
            ErrorHandler.SafeExecute(() =>
            {
                frmTracking_Load(this, EventArgs.Empty);
            }, "Data Refresh");
        }
    }
}