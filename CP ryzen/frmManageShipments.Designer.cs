using System.Windows.Forms;
using System.Drawing;

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
        private Panel pnlEditShipment;
        private TextBox txtEditDescription;
        private ComboBox cmbEditStatus;
        private TextBox txtEditDestination;
        private DateTimePicker dtpEditDateShipped;
        private DateTimePicker dtpEditEstimatedArrival;
        private Button btnSaveEdit;
        private Button btnCancelEdit;
        private Label lblEditDescription;
        private Label lblEditStatus;
        private Label lblEditDestination;
        private Label lblEditDateShipped;
        private Label lblEditEstArrival;

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
            this.lblManageShipments = new System.Windows.Forms.Label();
            this.dgvShipments = new System.Windows.Forms.DataGridView();
            this.btnAddShipment = new System.Windows.Forms.Button();
            this.btnEditShipment = new System.Windows.Forms.Button();
            this.btnDeleteShipment = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.cmbFilterByRole = new System.Windows.Forms.ComboBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.pnlEditShipment = new System.Windows.Forms.Panel();
            this.lblEditDescription = new System.Windows.Forms.Label();
            this.txtEditDescription = new System.Windows.Forms.TextBox();
            this.lblEditStatus = new System.Windows.Forms.Label();
            this.cmbEditStatus = new System.Windows.Forms.ComboBox();
            this.lblEditDestination = new System.Windows.Forms.Label();
            this.txtEditDestination = new System.Windows.Forms.TextBox();
            this.lblEditDateShipped = new System.Windows.Forms.Label();
            this.dtpEditDateShipped = new System.Windows.Forms.DateTimePicker();
            this.lblEditEstArrival = new System.Windows.Forms.Label();
            this.dtpEditEstimatedArrival = new System.Windows.Forms.DateTimePicker();
            this.btnSaveEdit = new System.Windows.Forms.Button();
            this.btnCancelEdit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).BeginInit();
            this.pnlEditShipment.SuspendLayout();
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
            this.dgvShipments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShipments.Location = new System.Drawing.Point(30, 100);
            this.dgvShipments.MultiSelect = false;
            this.dgvShipments.Name = "dgvShipments";
            this.dgvShipments.RowHeadersWidth = 51;
            this.dgvShipments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            this.btnAddShipment.UseVisualStyleBackColor = true;
            this.btnAddShipment.Click += new System.EventHandler(this.btnAddShipment_Click);
            // 
            // btnEditShipment
            // 
            this.btnEditShipment.Location = new System.Drawing.Point(160, 370);
            this.btnEditShipment.Name = "btnEditShipment";
            this.btnEditShipment.Size = new System.Drawing.Size(120, 30);
            this.btnEditShipment.TabIndex = 3;
            this.btnEditShipment.Text = "Edit Shipment";
            this.btnEditShipment.UseVisualStyleBackColor = true;
            this.btnEditShipment.Click += new System.EventHandler(this.btnEditShipment_Click);
            // 
            // btnDeleteShipment
            // 
            this.btnDeleteShipment.Location = new System.Drawing.Point(290, 370);
            this.btnDeleteShipment.Name = "btnDeleteShipment";
            this.btnDeleteShipment.Size = new System.Drawing.Size(120, 30);
            this.btnDeleteShipment.TabIndex = 4;
            this.btnDeleteShipment.Text = "Delete Shipment";
            this.btnDeleteShipment.UseVisualStyleBackColor = true;
            this.btnDeleteShipment.Click += new System.EventHandler(this.btnDeleteShipment_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Location = new System.Drawing.Point(420, 370);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(120, 30);
            this.btnViewDetails.TabIndex = 5;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // cmbFilterByRole
            // 
            this.cmbFilterByRole.FormattingEnabled = true;
            this.cmbFilterByRole.Location = new System.Drawing.Point(550, 55);
            this.cmbFilterByRole.Name = "cmbFilterByRole";
            this.cmbFilterByRole.Size = new System.Drawing.Size(150, 24);
            this.cmbFilterByRole.TabIndex = 7;
            this.cmbFilterByRole.SelectedIndexChanged += new System.EventHandler(this.cmbFilterByRole_SelectedIndexChanged);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(550, 35);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(89, 16);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "Filter by Role:";
            // 
            // pnlEditShipment
            // 
            this.pnlEditShipment.BackColor = System.Drawing.Color.LightGray;
            this.pnlEditShipment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEditShipment.Controls.Add(this.lblEditDescription);
            this.pnlEditShipment.Controls.Add(this.txtEditDescription);
            this.pnlEditShipment.Controls.Add(this.lblEditStatus);
            this.pnlEditShipment.Controls.Add(this.cmbEditStatus);
            this.pnlEditShipment.Controls.Add(this.lblEditDestination);
            this.pnlEditShipment.Controls.Add(this.txtEditDestination);
            this.pnlEditShipment.Controls.Add(this.lblEditDateShipped);
            this.pnlEditShipment.Controls.Add(this.dtpEditDateShipped);
            this.pnlEditShipment.Controls.Add(this.lblEditEstArrival);
            this.pnlEditShipment.Controls.Add(this.dtpEditEstimatedArrival);
            this.pnlEditShipment.Controls.Add(this.btnSaveEdit);
            this.pnlEditShipment.Controls.Add(this.btnCancelEdit);
            this.pnlEditShipment.Location = new System.Drawing.Point(30, 420);
            this.pnlEditShipment.Name = "pnlEditShipment";
            this.pnlEditShipment.Size = new System.Drawing.Size(700, 200);
            this.pnlEditShipment.TabIndex = 8;
            this.pnlEditShipment.Visible = false;
            // 
            // lblEditDescription
            // 
            this.lblEditDescription.Location = new System.Drawing.Point(10, 20);
            this.lblEditDescription.Name = "lblEditDescription";
            this.lblEditDescription.Size = new System.Drawing.Size(80, 20);
            this.lblEditDescription.TabIndex = 0;
            this.lblEditDescription.Text = "Description:";
            // 
            // txtEditDescription
            // 
            this.txtEditDescription.Location = new System.Drawing.Point(100, 20);
            this.txtEditDescription.Name = "txtEditDescription";
            this.txtEditDescription.Size = new System.Drawing.Size(200, 22);
            this.txtEditDescription.TabIndex = 1;
            // 
            // lblEditStatus
            // 
            this.lblEditStatus.Location = new System.Drawing.Point(10, 60);
            this.lblEditStatus.Name = "lblEditStatus";
            this.lblEditStatus.Size = new System.Drawing.Size(80, 20);
            this.lblEditStatus.TabIndex = 2;
            this.lblEditStatus.Text = "Status:";
            // 
            // cmbEditStatus
            // 
            this.cmbEditStatus.Location = new System.Drawing.Point(100, 60);
            this.cmbEditStatus.Name = "cmbEditStatus";
            this.cmbEditStatus.Size = new System.Drawing.Size(200, 24);
            this.cmbEditStatus.TabIndex = 3;
            // 
            // lblEditDestination
            // 
            this.lblEditDestination.Location = new System.Drawing.Point(10, 100);
            this.lblEditDestination.Name = "lblEditDestination";
            this.lblEditDestination.Size = new System.Drawing.Size(80, 20);
            this.lblEditDestination.TabIndex = 4;
            this.lblEditDestination.Text = "Destination:";
            // 
            // txtEditDestination
            // 
            this.txtEditDestination.Location = new System.Drawing.Point(100, 100);
            this.txtEditDestination.Name = "txtEditDestination";
            this.txtEditDestination.Size = new System.Drawing.Size(200, 22);
            this.txtEditDestination.TabIndex = 5;
            // 
            // lblEditDateShipped
            // 
            this.lblEditDateShipped.Location = new System.Drawing.Point(350, 20);
            this.lblEditDateShipped.Name = "lblEditDateShipped";
            this.lblEditDateShipped.Size = new System.Drawing.Size(100, 20);
            this.lblEditDateShipped.TabIndex = 6;
            this.lblEditDateShipped.Text = "Date Shipped:";
            // 
            // dtpEditDateShipped
            // 
            this.dtpEditDateShipped.Location = new System.Drawing.Point(460, 20);
            this.dtpEditDateShipped.Name = "dtpEditDateShipped";
            this.dtpEditDateShipped.Size = new System.Drawing.Size(200, 22);
            this.dtpEditDateShipped.TabIndex = 7;
            // 
            // lblEditEstArrival
            // 
            this.lblEditEstArrival.Location = new System.Drawing.Point(350, 60);
            this.lblEditEstArrival.Name = "lblEditEstArrival";
            this.lblEditEstArrival.Size = new System.Drawing.Size(100, 20);
            this.lblEditEstArrival.TabIndex = 8;
            this.lblEditEstArrival.Text = "Est. Arrival:";
            // 
            // dtpEditEstimatedArrival
            // 
            this.dtpEditEstimatedArrival.Location = new System.Drawing.Point(460, 60);
            this.dtpEditEstimatedArrival.Name = "dtpEditEstimatedArrival";
            this.dtpEditEstimatedArrival.Size = new System.Drawing.Size(200, 22);
            this.dtpEditEstimatedArrival.TabIndex = 9;
            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Location = new System.Drawing.Point(460, 150);
            this.btnSaveEdit.Name = "btnSaveEdit";
            this.btnSaveEdit.Size = new System.Drawing.Size(100, 30);
            this.btnSaveEdit.TabIndex = 10;
            this.btnSaveEdit.Text = "Save Changes";
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Location = new System.Drawing.Point(570, 150);
            this.btnCancelEdit.Name = "btnCancelEdit";
            this.btnCancelEdit.Size = new System.Drawing.Size(100, 30);
            this.btnCancelEdit.TabIndex = 11;
            this.btnCancelEdit.Text = "Cancel";
            this.btnCancelEdit.UseVisualStyleBackColor = true;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            // 
            // frmManageShipments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmManageShipments";
            this.Text = "Manage Shipments";
            this.Load += new System.EventHandler(this.frmManageShipments_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).EndInit();
            this.pnlEditShipment.ResumeLayout(false);
            this.pnlEditShipment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
