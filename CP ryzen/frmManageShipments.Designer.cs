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
            this.lblEditDescription = new Label();
            this.lblEditStatus = new Label();
            this.lblEditDestination = new Label();
            this.lblEditDateShipped = new Label();
            this.lblEditEstArrival = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).BeginInit();
            this.pnlEditShipment.SuspendLayout();
            this.SuspendLayout();

            // 
            // lblManageShipments
            // 
            this.lblManageShipments.AutoSize = true;
            this.lblManageShipments.Font = new Font("Arial", 16F, FontStyle.Bold);
            this.lblManageShipments.Location = new Point(30, 30);
            this.lblManageShipments.Name = "lblManageShipments";
            this.lblManageShipments.Size = new Size(264, 32);
            this.lblManageShipments.TabIndex = 0;
            this.lblManageShipments.Text = "Manage Shipments";

            // 
            // dgvShipments
            // 
            this.dgvShipments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShipments.Location = new Point(30, 100);
            this.dgvShipments.Name = "dgvShipments";
            this.dgvShipments.RowHeadersWidth = 51;
            this.dgvShipments.Size = new Size(700, 250);
            this.dgvShipments.TabIndex = 1;
            this.dgvShipments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvShipments.MultiSelect = false;

            // 
            // btnAddShipment
            // 
            this.btnAddShipment.Location = new Point(30, 370);
            this.btnAddShipment.Name = "btnAddShipment";
            this.btnAddShipment.Size = new Size(120, 30);
            this.btnAddShipment.TabIndex = 2;
            this.btnAddShipment.Text = "Add Shipment";
            this.btnAddShipment.UseVisualStyleBackColor = true;
            this.btnAddShipment.Click += new System.EventHandler(this.btnAddShipment_Click);

            // 
            // btnEditShipment
            // 
            this.btnEditShipment.Location = new Point(160, 370);
            this.btnEditShipment.Name = "btnEditShipment";
            this.btnEditShipment.Size = new Size(120, 30);
            this.btnEditShipment.TabIndex = 3;
            this.btnEditShipment.Text = "Edit Shipment";
            this.btnEditShipment.UseVisualStyleBackColor = true;
            this.btnEditShipment.Click += new System.EventHandler(this.btnEditShipment_Click);

            // 
            // btnDeleteShipment
            // 
            this.btnDeleteShipment.Location = new Point(290, 370);
            this.btnDeleteShipment.Name = "btnDeleteShipment";
            this.btnDeleteShipment.Size = new Size(120, 30);
            this.btnDeleteShipment.TabIndex = 4;
            this.btnDeleteShipment.Text = "Delete Shipment";
            this.btnDeleteShipment.UseVisualStyleBackColor = true;
            this.btnDeleteShipment.Click += new System.EventHandler(this.btnDeleteShipment_Click);

            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Location = new Point(420, 370);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new Size(120, 30);
            this.btnViewDetails.TabIndex = 5;
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);

            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new Point(550, 35);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new Size(89, 16);
            this.lblFilter.TabIndex = 6;
            this.lblFilter.Text = "Filter by Role:";

            // 
            // cmbFilterByRole
            // 
            this.cmbFilterByRole.FormattingEnabled = true;
            this.cmbFilterByRole.Location = new Point(550, 55);
            this.cmbFilterByRole.Name = "cmbFilterByRole";
            this.cmbFilterByRole.Size = new Size(150, 24);
            this.cmbFilterByRole.TabIndex = 7;
            this.cmbFilterByRole.SelectedIndexChanged += new System.EventHandler(this.cmbFilterByRole_SelectedIndexChanged);

            // 
            // pnlEditShipment
            // 
            this.pnlEditShipment.BorderStyle = BorderStyle.FixedSingle;
            this.pnlEditShipment.Location = new Point(30, 420);
            this.pnlEditShipment.Size = new Size(700, 200);
            this.pnlEditShipment.Visible = false;
            this.pnlEditShipment.BackColor = Color.LightGray;

            // 
            // lblEditDescription
            // 
            this.lblEditDescription.Text = "Description:";
            this.lblEditDescription.Location = new Point(10, 20);
            this.lblEditDescription.Size = new Size(80, 20);
            this.pnlEditShipment.Controls.Add(this.lblEditDescription);

            // 
            // txtEditDescription
            // 
            this.txtEditDescription.Location = new Point(100, 20);
            this.txtEditDescription.Size = new Size(200, 22);
            this.pnlEditShipment.Controls.Add(this.txtEditDescription);

            // 
            // lblEditStatus
            // 
            this.lblEditStatus.Text = "Status:";
            this.lblEditStatus.Location = new Point(10, 60);
            this.lblEditStatus.Size = new Size(80, 20);
            this.pnlEditShipment.Controls.Add(this.lblEditStatus);

            // 
            // cmbEditStatus
            // 
            this.cmbEditStatus.Location = new Point(100, 60);
            this.cmbEditStatus.Size = new Size(200, 24);
            this.pnlEditShipment.Controls.Add(this.cmbEditStatus);

            // 
            // lblEditDestination
            // 
            this.lblEditDestination.Text = "Destination:";
            this.lblEditDestination.Location = new Point(10, 100);
            this.lblEditDestination.Size = new Size(80, 20);
            this.pnlEditShipment.Controls.Add(this.lblEditDestination);

            // 
            // txtEditDestination
            // 
            this.txtEditDestination.Location = new Point(100, 100);
            this.txtEditDestination.Size = new Size(200, 22);
            this.pnlEditShipment.Controls.Add(this.txtEditDestination);

            // 
            // lblEditDateShipped
            // 
            this.lblEditDateShipped.Text = "Date Shipped:";
            this.lblEditDateShipped.Location = new Point(350, 20);
            this.lblEditDateShipped.Size = new Size(100, 20);
            this.pnlEditShipment.Controls.Add(this.lblEditDateShipped);

            // 
            // dtpEditDateShipped
            // 
            this.dtpEditDateShipped.Location = new Point(460, 20);
            this.dtpEditDateShipped.Size = new Size(200, 22);
            this.pnlEditShipment.Controls.Add(this.dtpEditDateShipped);

            // 
            // lblEditEstArrival
            // 
            this.lblEditEstArrival.Text = "Est. Arrival:";
            this.lblEditEstArrival.Location = new Point(350, 60);
            this.lblEditEstArrival.Size = new Size(100, 20);
            this.pnlEditShipment.Controls.Add(this.lblEditEstArrival);

            // 
            // dtpEditEstimatedArrival
            // 
            this.dtpEditEstimatedArrival.Location = new Point(460, 60);
            this.dtpEditEstimatedArrival.Size = new Size(200, 22);
            this.pnlEditShipment.Controls.Add(this.dtpEditEstimatedArrival);

            // 
            // btnSaveEdit
            // 
            this.btnSaveEdit.Text = "Save Changes";
            this.btnSaveEdit.Location = new Point(460, 150);
            this.btnSaveEdit.Size = new Size(100, 30);
            this.btnSaveEdit.UseVisualStyleBackColor = true;
            this.btnSaveEdit.Click += new System.EventHandler(this.btnSaveEdit_Click);
            this.pnlEditShipment.Controls.Add(this.btnSaveEdit);

            // 
            // btnCancelEdit
            // 
            this.btnCancelEdit.Text = "Cancel";
            this.btnCancelEdit.Location = new Point(570, 150);
            this.btnCancelEdit.Size = new Size(100, 30);
            this.btnCancelEdit.UseVisualStyleBackColor = true;
            this.btnCancelEdit.Click += new System.EventHandler(this.btnCancelEdit_Click);
            this.pnlEditShipment.Controls.Add(this.btnCancelEdit);

            // 
            // frmManageShipments
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 650);
            this.Controls.Add(this.lblManageShipments);
            this.Controls.Add(this.dgvShipments);
            this.Controls.Add(this.btnAddShipment);
            this.Controls.Add(this.btnEditShipment);
            this.Controls.Add(this.btnDeleteShipment);
            this.Controls.Add(this.btnViewDetails);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.cmbFilterByRole);
            this.Controls.Add(this.pnlEditShipment);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "frmManageShipments";
            this.Text = "Manage Shipments";
            this.Load += new System.EventHandler(this.frmManageShipments_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvShipments)).EndInit();
            this.pnlEditShipment.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}