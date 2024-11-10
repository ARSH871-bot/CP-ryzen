using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmManageShipments : Form
    {
        private List<Shipment> shipments;
        private Shipment selectedShipment;
        private string currentRole;

        public frmManageShipments()
        {
            InitializeComponent();
            InitializeShipments();
            LoadShipmentsToGrid();
        }

        private void InitializeShipments()
        {
            shipments = new List<Shipment>
            {
                new Shipment { ID = 1, Description = "Electronics to NY", Status = "In Transit", Destination = "New York", DateShipped = DateTime.Now.AddDays(-5), EstimatedArrival = DateTime.Now.AddDays(3), Role = "Dispatcher" },
                new Shipment { ID = 2, Description = "Furniture to CA", Status = "Delivered", Destination = "California", DateShipped = DateTime.Now.AddDays(-10), EstimatedArrival = DateTime.Now.AddDays(-2), Role = "Manager" },
                new Shipment { ID = 3, Description = "Clothing to TX", Status = "Pending", Destination = "Texas", DateShipped = DateTime.Now.AddDays(-1), EstimatedArrival = DateTime.Now.AddDays(7), Role = "Employee" }
            };
        }

        private void LoadShipmentsToGrid()
        {
            dgvShipments.DataSource = null;

            var filteredShipments = currentRole == "All" || string.IsNullOrEmpty(currentRole)
                ? shipments
                : shipments.Where(s => s.Role == currentRole).ToList();

            dgvShipments.DataSource = filteredShipments;
        }

        private void cmbFilterByRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRole = cmbFilterByRole.SelectedItem.ToString();
            LoadShipmentsToGrid();
        }

        private void btnAddShipment_Click(object sender, EventArgs e)
        {
            var newShipment = new Shipment
            {
                ID = shipments.Count + 1,
                Description = "New Shipment",
                Status = "Pending",
                Destination = "Unknown",
                DateShipped = DateTime.Now,
                EstimatedArrival = DateTime.Now.AddDays(5),
                Role = "Employee"
            };
            shipments.Add(newShipment);
            LoadShipmentsToGrid();
            MessageBox.Show("Shipment added successfully!");
        }

        private void btnEditShipment_Click(object sender, EventArgs e)
        {
            if (dgvShipments.CurrentRow != null)
            {
                selectedShipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;

                // Populate editing fields
                txtEditDescription.Text = selectedShipment.Description;
                cmbEditStatus.SelectedItem = selectedShipment.Status;
                txtEditDestination.Text = selectedShipment.Destination;
                dtpEditDateShipped.Value = selectedShipment.DateShipped;
                dtpEditEstimatedArrival.Value = selectedShipment.EstimatedArrival;

                // Show the editing panel
                pnlEditShipment.Visible = true;
            }
            else
            {
                MessageBox.Show("Please select a shipment to edit.", "Edit Shipment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            if (selectedShipment != null)
            {
                selectedShipment.Description = txtEditDescription.Text;
                selectedShipment.Status = cmbEditStatus.SelectedItem.ToString();
                selectedShipment.Destination = txtEditDestination.Text;
                selectedShipment.DateShipped = dtpEditDateShipped.Value;
                selectedShipment.EstimatedArrival = dtpEditEstimatedArrival.Value;

                LoadShipmentsToGrid();
                pnlEditShipment.Visible = false;

                MessageBox.Show("Shipment updated successfully!", "Edit Shipment", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditShipment.Visible = false;
        }

        private void btnDeleteShipment_Click(object sender, EventArgs e)
        {
            if (dgvShipments.CurrentRow != null)
            {
                var shipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;
                shipments.Remove(shipment);
                LoadShipmentsToGrid();
                MessageBox.Show("Shipment deleted successfully!");
            }
            else
            {
                MessageBox.Show("Please select a shipment to delete.", "Delete Shipment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvShipments.CurrentRow != null)
            {
                var shipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;
                MessageBox.Show($"Shipment Details:\nID: {shipment.ID}\nDescription: {shipment.Description}\nStatus: {shipment.Status}\nDestination: {shipment.Destination}\nDate Shipped: {shipment.DateShipped}\nEstimated Arrival: {shipment.EstimatedArrival}");
            }
            else
            {
                MessageBox.Show("Please select a shipment to view details.", "View Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmManageShipments_Load(object sender, EventArgs e)
        {
            currentRole = "All";
        }
    }

    public class Shipment
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Destination { get; set; }
        public DateTime DateShipped { get; set; }
        public DateTime EstimatedArrival { get; set; }
        public string Role { get; set; }
    }
}
