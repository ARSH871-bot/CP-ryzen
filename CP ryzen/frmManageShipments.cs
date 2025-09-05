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

        private void SetupForm()
        {
            try
            {
                // Initialize the role filter
                currentRole = "All";

                // Set up the filter combo box if it exists and is not already populated
                if (cmbFilterByRole != null && cmbFilterByRole.Items.Count == 0)
                {
                    cmbFilterByRole.Items.AddRange(new string[] { "All", "Admin", "Manager", "Dispatcher", "Employee" });
                    cmbFilterByRole.SelectedIndex = 0;
                }

                // Set up the edit status combo box if it exists and is not already populated
                if (cmbEditStatus != null && cmbEditStatus.Items.Count == 0)
                {
                    cmbEditStatus.Items.AddRange(new string[] { "Pending", "In Transit", "Delivered", "Cancelled" });
                }

                // Hide edit panel initially if it exists
                if (pnlEditShipment != null)
                {
                    pnlEditShipment.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up form: {ex.Message}", "Setup Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InitializeShipments()
        {
            shipments = new List<Shipment>
            {
                new Shipment
                {
                    ID = 1,
                    Description = "Electronics to NY",
                    Status = "In Transit",
                    Destination = "New York",
                    DateShipped = DateTime.Now.AddDays(-5),
                    EstimatedArrival = DateTime.Now.AddDays(3),
                    Role = "Dispatcher"
                },
                new Shipment
                {
                    ID = 2,
                    Description = "Furniture to CA",
                    Status = "Delivered",
                    Destination = "California",
                    DateShipped = DateTime.Now.AddDays(-10),
                    EstimatedArrival = DateTime.Now.AddDays(-2),
                    Role = "Manager"
                },
                new Shipment
                {
                    ID = 3,
                    Description = "Clothing to TX",
                    Status = "Pending",
                    Destination = "Texas",
                    DateShipped = DateTime.Now.AddDays(-1),
                    EstimatedArrival = DateTime.Now.AddDays(7),
                    Role = "Employee"
                }
            };
        }

        private void LoadShipmentsToGrid()
        {
            try
            {
                if (dgvShipments == null)
                {
                    MessageBox.Show("Data grid is not initialized. Please check the form designer.", "Control Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dgvShipments.DataSource = null;

                var filteredShipments = currentRole == "All" || string.IsNullOrEmpty(currentRole)
                    ? shipments
                    : shipments.Where(s => s.Role == currentRole).ToList();

                dgvShipments.DataSource = filteredShipments;

                // Configure grid appearance
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading shipments: {ex.Message}", "Loading Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            try
            {
                if (dgvShipments?.Columns != null && dgvShipments.Columns.Count > 0)
                {
                    // Set column headers and widths safely
                    if (dgvShipments.Columns["ID"] != null)
                    {
                        dgvShipments.Columns["ID"].HeaderText = "ID";
                        dgvShipments.Columns["ID"].Width = 50;
                    }
                    if (dgvShipments.Columns["Description"] != null)
                    {
                        dgvShipments.Columns["Description"].HeaderText = "Description";
                        dgvShipments.Columns["Description"].Width = 200;
                    }
                    if (dgvShipments.Columns["Status"] != null)
                    {
                        dgvShipments.Columns["Status"].HeaderText = "Status";
                        dgvShipments.Columns["Status"].Width = 100;
                    }
                    if (dgvShipments.Columns["Destination"] != null)
                    {
                        dgvShipments.Columns["Destination"].HeaderText = "Destination";
                        dgvShipments.Columns["Destination"].Width = 120;
                    }
                    if (dgvShipments.Columns["DateShipped"] != null)
                    {
                        dgvShipments.Columns["DateShipped"].HeaderText = "Date Shipped";
                        dgvShipments.Columns["DateShipped"].Width = 100;
                        dgvShipments.Columns["DateShipped"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    }
                    if (dgvShipments.Columns["EstimatedArrival"] != null)
                    {
                        dgvShipments.Columns["EstimatedArrival"].HeaderText = "Est. Arrival";
                        dgvShipments.Columns["EstimatedArrival"].Width = 100;
                        dgvShipments.Columns["EstimatedArrival"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    }
                    if (dgvShipments.Columns["Role"] != null)
                    {
                        dgvShipments.Columns["Role"].HeaderText = "Role";
                        dgvShipments.Columns["Role"].Width = 80;
                    }
                }
            }
            catch (Exception ex)
            {
                // Don't show error for grid configuration - it's not critical
                System.Diagnostics.Debug.WriteLine($"Grid configuration error: {ex.Message}");
            }
        }

        private void cmbFilterByRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFilterByRole?.SelectedItem != null)
                {
                    currentRole = cmbFilterByRole.SelectedItem.ToString();
                    LoadShipmentsToGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering shipments: {ex.Message}", "Filter Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddShipment_Click(object sender, EventArgs e)
        {
            try
            {
                // Generate new unique ID
                int newId = shipments.Any() ? shipments.Max(s => s.ID) + 1 : 1;

                var newShipment = new Shipment
                {
                    ID = newId,
                    Description = "New Shipment - Click Edit to modify",
                    Status = "Pending",
                    Destination = "To be determined",
                    DateShipped = DateTime.Now,
                    EstimatedArrival = DateTime.Now.AddDays(5),
                    Role = "Employee"
                };

                shipments.Add(newShipment);
                LoadShipmentsToGrid();

                // Select the newly added shipment
                SelectShipmentInGrid(newShipment.ID);

                MessageBox.Show("Shipment added successfully! Select it and click 'Edit' to modify details.",
                    "Shipment Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding shipment: {ex.Message}", "Add Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectShipmentInGrid(int shipmentId)
        {
            try
            {
                if (dgvShipments?.Rows != null)
                {
                    foreach (DataGridViewRow row in dgvShipments.Rows)
                    {
                        if (row.DataBoundItem is Shipment shipment && shipment.ID == shipmentId)
                        {
                            row.Selected = true;
                            if (row.Cells.Count > 0)
                            {
                                dgvShipments.CurrentCell = row.Cells[0];
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore selection errors - not critical
            }
        }

        private void btnEditShipment_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment)
                {
                    selectedShipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;

                    // Check if edit controls exist before using them
                    if (txtEditDescription != null)
                        txtEditDescription.Text = selectedShipment.Description;
                    if (cmbEditStatus != null)
                        cmbEditStatus.SelectedItem = selectedShipment.Status;
                    if (txtEditDestination != null)
                        txtEditDestination.Text = selectedShipment.Destination;
                    if (dtpEditDateShipped != null)
                        dtpEditDateShipped.Value = selectedShipment.DateShipped;
                    if (dtpEditEstimatedArrival != null)
                        dtpEditEstimatedArrival.Value = selectedShipment.EstimatedArrival;

                    // Show the editing panel if it exists
                    if (pnlEditShipment != null)
                    {
                        pnlEditShipment.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Edit panel is not available. Please check the form designer.", "Edit Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a shipment to edit.", "Edit Shipment",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening edit panel: {ex.Message}", "Edit Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedShipment != null)
                {
                    // Validate input with null checks
                    if (txtEditDescription == null || string.IsNullOrWhiteSpace(txtEditDescription.Text))
                    {
                        MessageBox.Show("Description cannot be empty.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEditDescription?.Focus();
                        return;
                    }

                    if (txtEditDestination == null || string.IsNullOrWhiteSpace(txtEditDestination.Text))
                    {
                        MessageBox.Show("Destination cannot be empty.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtEditDestination?.Focus();
                        return;
                    }

                    if (cmbEditStatus?.SelectedItem == null)
                    {
                        MessageBox.Show("Please select a status.", "Validation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbEditStatus?.Focus();
                        return;
                    }

                    if (dtpEditEstimatedArrival != null && dtpEditDateShipped != null &&
                        dtpEditEstimatedArrival.Value < dtpEditDateShipped.Value)
                    {
                        MessageBox.Show("Estimated arrival date cannot be before the shipped date.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dtpEditEstimatedArrival.Focus();
                        return;
                    }

                    // Update shipment
                    selectedShipment.Description = txtEditDescription.Text.Trim();
                    selectedShipment.Status = cmbEditStatus.SelectedItem.ToString();
                    selectedShipment.Destination = txtEditDestination.Text.Trim();
                    selectedShipment.DateShipped = dtpEditDateShipped.Value;
                    selectedShipment.EstimatedArrival = dtpEditEstimatedArrival.Value;

                    LoadShipmentsToGrid();
                    if (pnlEditShipment != null)
                        pnlEditShipment.Visible = false;

                    MessageBox.Show("Shipment updated successfully!", "Edit Shipment",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving shipment: {ex.Message}", "Save Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            if (pnlEditShipment != null)
                pnlEditShipment.Visible = false;
            selectedShipment = null;
        }

        private void btnDeleteShipment_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment)
                {
                    var shipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;

                    var result = MessageBox.Show(
                        $"Are you sure you want to delete this shipment?\n\nID: {shipment.ID}\nDescription: {shipment.Description}\nDestination: {shipment.Destination}",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        shipments.Remove(shipment);
                        LoadShipmentsToGrid();
                        MessageBox.Show("Shipment deleted successfully!", "Delete Shipment",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a shipment to delete.", "Delete Shipment",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting shipment: {ex.Message}", "Delete Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment)
                {
                    var shipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;

                    string details = $"Shipment Details:\n\n" +
                                   $"ID: {shipment.ID}\n" +
                                   $"Description: {shipment.Description}\n" +
                                   $"Status: {shipment.Status}\n" +
                                   $"Destination: {shipment.Destination}\n" +
                                   $"Date Shipped: {shipment.DateShipped:MM/dd/yyyy}\n" +
                                   $"Estimated Arrival: {shipment.EstimatedArrival:MM/dd/yyyy}\n" +
                                   $"Assigned Role: {shipment.Role}";

                    MessageBox.Show(details, "Shipment Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a shipment to view details.", "View Details",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing shipment details: {ex.Message}", "View Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmManageShipments_Load(object sender, EventArgs e)
        {
            try
            {
                // Set up form controls after they're initialized
                SetupForm();
                // Refresh the display
                LoadShipmentsToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing shipments form: {ex.Message}", "Initialization Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method to refresh data (useful when called from parent dashboard)
        public void RefreshData()
        {
            LoadShipmentsToGrid();
        }

        // Method to get shipment count for dashboard summary
        public int GetShipmentCount()
        {
            return shipments?.Count ?? 0;
        }

        // Method to get shipments by status for dashboard summary  
        public Dictionary<string, int> GetShipmentsByStatus()
        {
            if (shipments == null) return new Dictionary<string, int>();

            return shipments.GroupBy(s => s.Status)
                           .ToDictionary(g => g.Key, g => g.Count());
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

        // Override ToString for better display in combo boxes or lists
        public override string ToString()
        {
            return $"ID {ID}: {Description} - {Status}";
        }
    }
}