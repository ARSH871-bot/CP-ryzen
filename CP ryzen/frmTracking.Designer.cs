namespace ShippingManagementSystem
{
    partial class frmTracking
    {
        // Components and controls for the form
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTracking; // Label for the form's title
        private System.Windows.Forms.TextBox txtTrackingNumber; // Input box for entering the tracking number
        private System.Windows.Forms.Button btnTrack; // Button to trigger tracking functionality
        private System.Windows.Forms.RichTextBox rtbTrackingDetails; // Area to display tracking details
        private System.Windows.Forms.PictureBox pbTrackingIcon; // Picture box for the tracking icon
        private System.Windows.Forms.ComboBox cmbHistory; // Dropdown for tracking history
        private System.Windows.Forms.Button btnSave; // Button to save tracking details
        private System.Windows.Forms.Button btnPrint; // Button to print tracking details
        private System.Windows.Forms.Button btnHelp; // Button to access help information
        private System.Windows.Forms.Button btnDarkMode; // Button to toggle dark mode
        private System.Windows.Forms.Label lblClock; // Label for the clock display
        private System.Windows.Forms.ProgressBar pbShipmentProgress; // Progress bar for shipment status

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
            this.lblTracking = new System.Windows.Forms.Label();
            this.txtTrackingNumber = new System.Windows.Forms.TextBox();
            this.btnTrack = new System.Windows.Forms.Button();
            this.rtbTrackingDetails = new System.Windows.Forms.RichTextBox();
            this.pbTrackingIcon = new System.Windows.Forms.PictureBox();
            this.cmbHistory = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.lblClock = new System.Windows.Forms.Label();
            this.pbShipmentProgress = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pbTrackingIcon)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTracking
            // 
            this.lblTracking.AutoSize = true;
            this.lblTracking.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTracking.Location = new System.Drawing.Point(200, 20);
            this.lblTracking.Name = "lblTracking";
            this.lblTracking.Size = new System.Drawing.Size(196, 32);
            this.lblTracking.TabIndex = 0;
            this.lblTracking.Text = "Track Shipment";
            this.lblTracking.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // txtTrackingNumber
            // 
            this.txtTrackingNumber.Font = new System.Drawing.Font("Arial", 12F);
            this.txtTrackingNumber.Location = new System.Drawing.Point(150, 80);
            this.txtTrackingNumber.Name = "txtTrackingNumber";
            this.txtTrackingNumber.Size = new System.Drawing.Size(300, 30);
            this.txtTrackingNumber.TabIndex = 1;
            this.txtTrackingNumber.Text = "Enter Tracking Number";
            this.txtTrackingNumber.ForeColor = System.Drawing.Color.Gray;
            this.txtTrackingNumber.GotFocus += new System.EventHandler(this.txtTrackingNumber_GotFocus);
            this.txtTrackingNumber.LostFocus += new System.EventHandler(this.txtTrackingNumber_LostFocus);

            // 
            // btnTrack
            // 
            this.btnTrack.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btnTrack.Location = new System.Drawing.Point(240, 130);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(120, 40);
            this.btnTrack.TabIndex = 2;
            this.btnTrack.Text = "Track";
            this.btnTrack.UseVisualStyleBackColor = true;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);

            // 
            // rtbTrackingDetails
            // 
            this.rtbTrackingDetails.Font = new System.Drawing.Font("Arial", 12F);
            this.rtbTrackingDetails.Location = new System.Drawing.Point(50, 200);
            this.rtbTrackingDetails.Name = "rtbTrackingDetails";
            this.rtbTrackingDetails.ReadOnly = true;
            this.rtbTrackingDetails.Size = new System.Drawing.Size(500, 150);
            this.rtbTrackingDetails.TabIndex = 3;
            this.rtbTrackingDetails.Text = "Tracking details will appear here...";

            // 
            // pbTrackingIcon - placeholder for tracking icon
            // 
            this.pbTrackingIcon.BackColor = System.Drawing.Color.LightGray;
            this.pbTrackingIcon.Location = new System.Drawing.Point(20, 20);
            this.pbTrackingIcon.Name = "pbTrackingIcon";
            this.pbTrackingIcon.Size = new System.Drawing.Size(60, 60);
            this.pbTrackingIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTrackingIcon.TabIndex = 4;
            this.pbTrackingIcon.TabStop = false;

            // 
            // pbShipmentProgress
            // 
            this.pbShipmentProgress.Location = new System.Drawing.Point(50, 370);
            this.pbShipmentProgress.Name = "pbShipmentProgress";
            this.pbShipmentProgress.Size = new System.Drawing.Size(500, 20);
            this.pbShipmentProgress.TabIndex = 11;
            this.pbShipmentProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

            // 
            // cmbHistory
            // 
            this.cmbHistory.Font = new System.Drawing.Font("Arial", 12F);
            this.cmbHistory.FormattingEnabled = true;
            this.cmbHistory.Location = new System.Drawing.Point(150, 400);
            this.cmbHistory.Name = "cmbHistory";
            this.cmbHistory.Size = new System.Drawing.Size(300, 31);
            this.cmbHistory.TabIndex = 5;
            this.cmbHistory.Text = "Search History";
            this.cmbHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHistory.SelectedIndexChanged += new System.EventHandler(this.cmbHistory_SelectedIndexChanged);

            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(50, 450);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(170, 450);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 30);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            // 
            // btnHelp
            // 
            this.btnHelp.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnHelp.Location = new System.Drawing.Point(290, 450);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(100, 30);
            this.btnHelp.TabIndex = 8;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);

            // 
            // btnDarkMode
            // 
            this.btnDarkMode.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnDarkMode.Location = new System.Drawing.Point(410, 450);
            this.btnDarkMode.Name = "btnDarkMode";
            this.btnDarkMode.Size = new System.Drawing.Size(120, 30);
            this.btnDarkMode.TabIndex = 9;
            this.btnDarkMode.Text = "Dark Mode";
            this.btnDarkMode.UseVisualStyleBackColor = true;
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);

            // 
            // lblClock
            // 
            this.lblClock.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblClock.Location = new System.Drawing.Point(10, 500);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(400, 20);
            this.lblClock.TabIndex = 10;
            this.lblClock.Text = "Current Time: ";

            // 
            // frmTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(600, 550);
            this.Controls.Add(this.pbShipmentProgress);
            this.Controls.Add(this.lblClock);
            this.Controls.Add(this.btnDarkMode);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cmbHistory);
            this.Controls.Add(this.pbTrackingIcon);
            this.Controls.Add(this.rtbTrackingDetails);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.txtTrackingNumber);
            this.Controls.Add(this.lblTracking);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.Name = "frmTracking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shipment Tracking";
            this.Load += new System.EventHandler(this.frmTracking_Load);

            ((System.ComponentModel.ISupportInitialize)(this.pbTrackingIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}