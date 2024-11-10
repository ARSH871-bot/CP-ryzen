using System.Windows.Forms;

namespace ShippingManagementSystem
{
    partial class frmManageShipments
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblManageShipments;
        private DataGridView dgvShipments;
        private Button btnAddShipment;
        private Button btnEditShipment;
        private Button btnDeleteShipment;
        private Button btnViewDetails;
        private ComboBox cmbFilterByRole;
        private Label lblFilter;
        private Panel pnlEditShipment; // Panel for inline editing
        private TextBox txtEditDescription;
        private ComboBox cmbEditStatus;
        private TextBox txtEditDestination;
        private DateTimePicker dtpEditDateShipped;
        private DateTimePicker dtpEditEstimatedArrival;
        private Button btnSaveEdit;
        private Button btnCancelEdit;

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
            this.lblManageShipments = new Label();
            this.dgvShipments = new DataGridView();
            this.btnAddShipment = new Button();
            this.btnEditShipment = new Button();
            this.btnDeleteShipment = new Button();
            this.btnViewDetails = new Button();
            this.cmbFilterByRole = new ComboBox();
            this.lblFilter = new Label();
            this.pnlEditShipment = new Panel();
            this.txtEditDescription = new TextBox();
            this.cmbEditStatus = new ComboBox();
            this.txtEditDestination = new TextBox();
            this.dtpEditDateShipped = new DateTimePicker();
            this.dtpEditEstimatedArrival = new DateTimePicker();
            this.btnSaveEdit = new Button();
            this.btnCancelEdit = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).BeginInit();
            this.SuspendLayout();

            // 
            // lblManageShipments
            // 
            this.lblManageShipments.AutoSize = true;
            this.lblManageShipments.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblManageShipments.Location = new System.Drawing.Point(30, 30);
            this.lblManageShipments.Name = "lblManageShipments";
            this.lblManageShipments.Size = new System.Drawing.Size(264, 32);
            this.lblManageShipments.TabIndex = 0;
            this.lblManageShipments.Text = "Manage Shipments";

            // 
            // dgvShipments
            // 
            this.dgvShipments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShipments.Location = new System.Drawing.Point(30, 100);
            this.dgvShipments.Name = "dgvShipments";
            this.dgvShipments.RowHeadersWidth = 51;
            this.dgvShipments.Size = new System.Drawing.Size(700, 250);
            this.dgvShipments.TabIndex = 1;

            // 
            // btnAddShipment
            // 
            this.btnAddShipment.Location = new System.Drawing.Point(30, 370);
            this.btnAddShipment.Name = "btnAddShipment";
            this.btnAddShipment.Size = new System.Drawing.Size(120, 30);
            this.btnAddShipment.TabIndex = 2;
            this.btnAddShipment.Text = "Add Shipment";
            this.btnAddShipment.Click += new System.EventHandler(this.btnAddShipment_Click);

            // 
            // btnEditShipment
            // 
            this.btnEditShipment.Location = new System.Drawing.Point(160, 370);
            this.btnEditShipment.Name = "btnEditShipment";
            this.btnEditShipment.Size = new System.Drawing.Size(120, 30);
            this.btnEditShipment.TabIndex = 3;
            this.btnEditShipment.Text = "Edit Shipment";
            this.btnEditShipment.Click += new System.EventHandler(this.btnEditShipment_Click);

            // 
            // btnDeleteShipment
            // 
            this.btnDeleteShipment.Location = new System.Drawing.Point(290, 370);
            this.btnDeleteShipment.Name = "btnDeleteShipment";
            this.btnDeleteShipment.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteShipment.TabIndex = 4;
            this.btnDeleteShipment.Text = "Delete Shipment";
            this.btnDeleteShipment.Click += new System.EventHandler(this.btnDeleteShipment_Click);

            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Location = new System.Drawing.Point(420, 370);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(120, 30);
            this.btnViewDetails.TabIndex = 5;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);

            // 
            // cmbFilterByRole
            // 
            this.cmbFilterByRole.Items.AddRange(new object[] {
            "All",
            "Admin",
            "Manager",
            "Dispatcher",
            "Employee"});
            this.cmbFilterByRole.Location = new System.Drawing.Point(600, 30);
            this.cmbFilterByRole.Name = "cmbFilterByRole";
            this.cmbFilterByRole.Size = new System.Drawing.Size(150, 24);
            this.cmbFilterByRole.TabIndex = 7;
            this.cmbFilterByRole.SelectedIndexChanged += new System.EventHandler(this.cmbFilterByRole_SelectedIndexChanged);

            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(505, 30);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(89, 16);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "Filter by Role:";

            // 
            // pnlEditShipment
            // 
            this.pnlEditShipment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEditShipment.Location = new System.Drawing.Point(30, 420);
            this.pnlEditShipment.Size = new System.Drawing.Size(700, 200);
            this.pnlEditShipment.Visible = false;
            this.pnlEditShipment.Controls.Add(new Label { Text = "Description:", Location = new System.Drawing.Point(10, 20) });
            this.pnlEditShipment.Controls.Add(this.txtEditDescription);
            this.pnlEditShipment.Controls.Add(new Label { Text = "Status:", Location = new System.Drawing.Point(10, 60) });
            this.pnlEditShipment.Controls.Add(this.cmbEditStatus);
            this.pnlEditShipment.Controls.Add(new Label { Text = "Destination:", Location = new System.Drawing.Point(10, 100) });
            this.pnlEditShipment.Controls.Add(this.txtEditDestination);
            this.pnlEditShipment.Controls.Add(new Label { Text = "Date Shipped:", Location = new System.Drawing.Point(350, 20) });
            this.pnlEditShipment.Controls.Add(this.dtpEditDateShipped);
            this.pnlEditShipment.Controls.Add(new Label { Text = "Estimated Arrival:", Location = new System.Drawing.Point(350, 60) });
            this.pnlEditShipment.Controls.Add(this.dtpEditEstimatedArrival);
            this.pnlEditShipment.Controls.Add(this.btnSaveEdit);
            this.pnlEditShipment.Controls.Add(this.btnCancelEdit);

            // 
            // txtEditDescription
            // 
            this.txtEditDescription.Location = new System.Drawing.Point(120, 20);
            this.txtEditDescription.Size = new System.Drawing.Size(200, 24);

            // 
            // cmbEditStatus
            // 
            this.cmbEditStatus.Items.AddRange(new object[] { "Pending", "In Transit", "Delivered" });
            this.cmbEditStatus.Location = new System.Drawing.Point(120, 60);
            this.cmbEditStatus.Size = new System.Drawing.Size(200, 24);

            // 
            // txtEditDestination
            // 
            this.txtEditDestination.Location = new System.Drawing.Point(120, 100);
            this.txtEditDestination.Size = new System.Drawing.Size(200, 24);

            // 
            // dtpEditDateShipped
            // 
            this.dtpEditDateShipped.Location = new System.Drawing.Point(480, 20);
            this.dtpEditDateShipped.Size = new System.Drawing.Size(200, 24);

            // 
            // dtpEditEstimatedArrival
            // 
            this.dtpEditEstimatedArrival.Location = new System.Drawing.Point(480, 60);
            this.dtpEditEstimatedArrival.Size = new System.Drawing.Size(200, 24);

            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Text = "Save";
            this.btnSaveEdit.Location = new System.Drawing.Point(480, 150);
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);

            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Text = "Cancel";
            this.btnCancelEdit.Location = new System.Drawing.Point(580, 150);
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);

            // 
            // frmManageShipments
            // 
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Controls.Add(this.lblManageShipments);
            this.Controls.Add(this.dgvShipments);
            this.Controls.Add(this.btnAddShipment);
            this.Controls.Add(this.btnEditShipment);
            this.Controls.Add(this.btnDeleteShipment);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.cmbFilterByRole);
            this.Controls.Add(this.pnlEditShipment);
            this.Name = "frmManageShipments";
            this.Text = "Manage Shipments";
            this.Load += new System.EventHandler(this.frmManageShipments_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
