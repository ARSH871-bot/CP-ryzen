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
        private ShipmentManager shipmentManager; // Database manager
        private User currentUser; // Track current user for database operations

        public frmManageShipments()
        {
            InitializeComponent();
            shipmentManager = new ShipmentManager(); // Initialize database manager
            InitializeShipments();
            LoadShipmentsToGrid();
        }

        // Constructor with user parameter for better integration
        public frmManageShipments(User user)
        {
            InitializeComponent();
            shipmentManager = new ShipmentManager();
            currentUser = user;
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
                ErrorHandler.HandleException(ex, "Setup Form", true);
            }
        }

        private void InitializeShipments()
        {
            try
            {
                // Load shipments from database
                shipments = shipmentManager.GetAllShipments();

                // If no shipments exist, create some sample data
                if (shipments.Count == 0)
                {
                    CreateSampleShipments();
                    shipments = shipmentManager.GetAllShipments(); // Reload after creating samples
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Initialize Shipments", true);
                shipments = new List<Shipment>(); // Fallback to empty list
            }
        }

        private void CreateSampleShipments()
        {
            try
            {
                // Get current user ID or default to admin user
                int defaultUserId = GetCurrentUserId();

                // Create sample shipments in database for demonstration
                shipmentManager.CreateSampleShipmentsIfEmpty(defaultUserId);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Create Sample Shipments", false);
            }
        }

        private int GetCurrentUserId()
        {
            if (currentUser != null)
            {
                // Try to get Id property using reflection to handle different User class definitions
                var userType = currentUser.GetType();
                var idProperty = userType.GetProperty("Id");
                if (idProperty != null)
                {
                    return (int)idProperty.GetValue(currentUser);
                }
            }

            // Fallback: try to get admin user ID from database
            try
            {
                var userManager = new UserManager();
                var adminUser = userManager.GetUser("admin");
                if (adminUser != null)
                {
                    var userType = adminUser.GetType();
                    var idProperty = userType.GetProperty("Id");
                    if (idProperty != null)
                    {
                        return (int)idProperty.GetValue(adminUser);
                    }
                }
            }
            catch
            {
                // Ignore errors
            }

            return 1; // Default to ID 1
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

                // Load shipments from database with role filter
                var filteredShipments = shipmentManager.GetAllShipments(null, currentRole == "All" ? null : currentRole);
                shipments = filteredShipments; // Update local list

                dgvShipments.DataSource = filteredShipments;

                // Configure grid appearance
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Load Shipments", true);
            }
        }

        private void ConfigureDataGridView()
        {
            try
            {
                if (dgvShipments?.Columns != null && dgvShipments.Columns.Count > 0)
                {
                    // Set column headers and widths safely - using database field names
                    SetColumnIfExists("Id", "ID", 50);
                    SetColumnIfExists("ID", "ID", 50); // Fallback for different property names
                    SetColumnIfExists("Description", "Description", 200);
                    SetColumnIfExists("Status", "Status", 100);
                    SetColumnIfExists("Destination", "Destination", 120);
                    SetColumnIfExists("DateShipped", "Date Shipped", 100, "MM/dd/yyyy");
                    SetColumnIfExists("EstimatedArrival", "Est. Arrival", 100, "MM/dd/yyyy");
                    SetColumnIfExists("TrackingNumber", "Tracking #", 120);
                    SetColumnIfExists("Role", "Role", 80);

                    // Hide database-specific columns that users don't need to see
                    HideColumnIfExists("Origin");
                    HideColumnIfExists("ActualArrival");
                    HideColumnIfExists("Weight");
                    HideColumnIfExists("Dimensions");
                    HideColumnIfExists("ShippingCost");
                    HideColumnIfExists("Notes");
                    HideColumnIfExists("CreatedDate");
                    HideColumnIfExists("ModifiedDate");
                }
            }
            catch (Exception ex)
            {
                // Don't show error for grid configuration - it's not critical
                System.Diagnostics.Debug.WriteLine($"Grid configuration error: {ex.Message}");
            }
        }

        private void SetColumnIfExists(string columnName, string headerText, int width, string format = null)
        {
            if (dgvShipments.Columns[columnName] != null)
            {
                dgvShipments.Columns[columnName].HeaderText = headerText;
                dgvShipments.Columns[columnName].Width = width;
                if (!string.IsNullOrEmpty(format))
                {
                    dgvShipments.Columns[columnName].DefaultCellStyle.Format = format;
                }
            }
        }

        private void HideColumnIfExists(string columnName)
        {
            if (dgvShipments.Columns[columnName] != null)
            {
                dgvShipments.Columns[columnName].Visible = false;
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
                ErrorHandler.HandleException(ex, "Filter Shipments", true);
            }
        }

        private void btnAddShipment_Click(object sender, EventArgs e)
        {
            try
            {
                int defaultUserId = GetCurrentUserId();

                // Create shipment in database
                bool success = shipmentManager.CreateShipment(
                    "New Shipment - Click Edit to modify",
                    "To be determined",
                    "Origin TBD",
                    DateTime.Now,
                    DateTime.Now.AddDays(5),
                    defaultUserId,
                    "Pending"
                );

                if (success)
                {
                    LoadShipmentsToGrid(); // Refresh from database
                    MessageBox.Show("Shipment added successfully! Select it and click 'Edit' to modify details.",
                        "Shipment Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(shipmentManager.LastError, "Add Shipment Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Add Shipment", true);
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
                        if (row.DataBoundItem is Shipment shipment &&
                            (shipment.ID == shipmentId || GetShipmentId(shipment) == shipmentId))
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

        private int GetShipmentId(Shipment shipment)
        {
            // Handle both ID and Id property names
            var shipmentType = shipment.GetType();
            var idProperty = shipmentType.GetProperty("Id") ?? shipmentType.GetProperty("ID");
            if (idProperty != null)
            {
                return (int)idProperty.GetValue(shipment);
            }
            return shipment.ID; // Fallback to existing property
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
                ErrorHandler.HandleException(ex, "Edit Shipment", true);
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

                    // Update shipment in database
                    int shipmentId = GetShipmentId(selectedShipment);

                    bool success = shipmentManager.UpdateShipment(
                        shipmentId,
                        txtEditDescription.Text.Trim(),
                        cmbEditStatus.SelectedItem.ToString(),
                        txtEditDestination.Text.Trim(),
                        dtpEditDateShipped.Value,
                        dtpEditEstimatedArrival.Value,
                        selectedShipment.Role
                    );

                    if (success)
                    {
                        LoadShipmentsToGrid(); // Refresh from database
                        if (pnlEditShipment != null)
                            pnlEditShipment.Visible = false;

                        MessageBox.Show("Shipment updated successfully!", "Edit Shipment",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(shipmentManager.LastError, "Update Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Save Shipment", true);
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
                    int shipmentId = GetShipmentId(shipment);

                    var result = MessageBox.Show(
                        $"Are you sure you want to delete this shipment?\n\nID: {shipmentId}\nDescription: {shipment.Description}\nDestination: {shipment.Destination}",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        bool success = shipmentManager.DeleteShipment(shipmentId);

                        if (success)
                        {
                            LoadShipmentsToGrid(); // Refresh from database
                            MessageBox.Show("Shipment deleted successfully!", "Delete Shipment",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(shipmentManager.LastError, "Delete Failed",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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
                ErrorHandler.HandleException(ex, "Delete Shipment", true);
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment)
                {
                    var shipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;
                    int shipmentId = GetShipmentId(shipment);

                    string details = $"Shipment Details:\n\n" +
                                   $"ID: {shipmentId}\n" +
                                   $"Description: {shipment.Description}\n" +
                                   $"Status: {shipment.Status}\n" +
                                   $"Destination: {shipment.Destination}\n" +
                                   $"Date Shipped: {shipment.DateShipped:MM/dd/yyyy}\n" +
                                   $"Estimated Arrival: {shipment.EstimatedArrival:MM/dd/yyyy}\n" +
                                   $"Assigned Role: {shipment.Role}";

                    // Add tracking number if available
                    var trackingProperty = shipment.GetType().GetProperty("TrackingNumber");
                    if (trackingProperty != null)
                    {
                        var trackingNumber = trackingProperty.GetValue(shipment)?.ToString();
                        if (!string.IsNullOrEmpty(trackingNumber))
                        {
                            details += $"\nTracking Number: {trackingNumber}";
                        }
                    }

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
                ErrorHandler.HandleException(ex, "View Details", true);
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
                ErrorHandler.HandleException(ex, "Initialize Form", true);
            }
        }

        // Method to refresh data (useful when called from parent dashboard)
        public void RefreshData()
        {
            try
            {
                shipments = shipmentManager.GetAllShipments();
                LoadShipmentsToGrid();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Refresh Data", false);
                LoadShipmentsToGrid(); // Use existing data
            }
        }

        // Method to get shipment count for dashboard summary
        public int GetShipmentCount()
        {
            return shipments?.Count ?? 0;
        }

        // Method to get shipments by status for dashboard summary  
        public Dictionary<string, int> GetShipmentsByStatus()
        {
            try
            {
                return shipmentManager.GetShipmentsByStatus();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Shipments by Status", false);
                return new Dictionary<string, int>();
            }
        }
    }

    // Keep the existing Shipment class as-is to avoid conflicts
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