using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmManageShipments : Form
    {
        private ShipmentManager shipmentManager;
        private List<Shipment> shipments;
        private Shipment selectedShipment;
        private string currentRole;
        private int currentUserId = 1; // TODO: Pass from login

        public frmManageShipments()
        {
            InitializeComponent();
            shipmentManager = new ShipmentManager();
            InitializeForm();
        }

        private void InitializeForm()
        {
            currentRole = "All";

            if (cmbFilterByRole != null && cmbFilterByRole.Items.Count == 0)
            {
                cmbFilterByRole.Items.AddRange(new string[] { "All", "Admin", "Manager", "Dispatcher", "Employee" });
                cmbFilterByRole.SelectedIndex = 0;
            }

            if (cmbEditStatus != null && cmbEditStatus.Items.Count == 0)
            {
                cmbEditStatus.Items.AddRange(new string[] { "Pending", "In Transit", "Delivered", "Cancelled" });
            }

            if (pnlEditShipment != null)
            {
                pnlEditShipment.Visible = false;
            }
        }

        private void frmManageShipments_Load(object sender, EventArgs e)
        {
            LoadShipmentsFromDatabase();
        }

        private void LoadShipmentsFromDatabase()
        {
            try
            {
                shipments = shipmentManager.GetAllShipments(currentRole);
                RefreshGrid();
                ErrorHandler.LogInfo($"Loaded {shipments.Count} shipments from database", "ManageShipments");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Load Shipments");
            }
        }

        private void RefreshGrid()
        {
            try
            {
                if (dgvShipments == null) return;

                dgvShipments.DataSource = null;
                dgvShipments.DataSource = shipments;
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Refresh Grid");
            }
        }

        private void ConfigureDataGridView()
        {
            try
            {
                if (dgvShipments?.Columns != null && dgvShipments.Columns.Count > 0)
                {
                    if (dgvShipments.Columns["ID"] != null)
                    {
                        dgvShipments.Columns["ID"].HeaderText = "ID";
                        dgvShipments.Columns["ID"].Width = 50;
                    }
                    if (dgvShipments.Columns["Description"] != null)
                        dgvShipments.Columns["Description"].Width = 200;
                    if (dgvShipments.Columns["Status"] != null)
                        dgvShipments.Columns["Status"].Width = 100;
                    if (dgvShipments.Columns["Destination"] != null)
                        dgvShipments.Columns["Destination"].Width = 120;
                    if (dgvShipments.Columns["DateShipped"] != null)
                        dgvShipments.Columns["DateShipped"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    if (dgvShipments.Columns["EstimatedArrival"] != null)
                        dgvShipments.Columns["EstimatedArrival"].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
            }
            catch { }
        }

        private void cmbFilterByRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRole = cmbFilterByRole?.SelectedItem?.ToString() ?? "All";
            LoadShipmentsFromDatabase();
        }

        private void btnAddShipment_Click(object sender, EventArgs e)
        {
            try
            {
                int shipmentId = shipmentManager.CreateShipment(
                    "New Shipment - Click Edit to modify",
                    "To be determined",
                    "",
                    DateTime.Now,
                    DateTime.Now.AddDays(5),
                    currentUserId,
                    "Pending"
                );

                if (shipmentId > 0)
                {
                    LoadShipmentsFromDatabase();

                    var newShipment = new Shipment
                    {
                        ID = shipmentId,
                        Description = "New Shipment - Click Edit to modify",
                        Status = "Pending",
                        Destination = "To be determined"
                    };

                    _ = EmailManager.SendShipmentCreatedEmail("admin@ryzenshipment.com", newShipment);
                    ErrorHandler.ShowInfo("Shipment added successfully!", "Success");
                }
                else
                {
                    ErrorHandler.ShowWarning(shipmentManager.LastError, "Add Failed");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Add Shipment");
            }
        }

        private void btnEditShipment_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment)
                {
                    selectedShipment = (Shipment)dgvShipments.CurrentRow.DataBoundItem;

                    txtEditDescription.Text = selectedShipment.Description;
                    cmbEditStatus.SelectedItem = selectedShipment.Status;
                    txtEditDestination.Text = selectedShipment.Destination;
                    dtpEditDateShipped.Value = selectedShipment.DateShipped;
                    dtpEditEstimatedArrival.Value = selectedShipment.EstimatedArrival;

                    pnlEditShipment.Visible = true;
                }
                else
                {
                    ErrorHandler.ShowWarning("Please select a shipment to edit.", "No Selection");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Edit Shipment");
            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedShipment == null) return;

                string oldStatus = selectedShipment.Status;

                bool success = shipmentManager.UpdateShipment(
                    selectedShipment.ID,
                    txtEditDescription.Text.Trim(),
                    cmbEditStatus.SelectedItem.ToString(),
                    txtEditDestination.Text.Trim(),
                    dtpEditDateShipped.Value,
                    dtpEditEstimatedArrival.Value,
                    null
                );

                if (success)
                {
                    LoadShipmentsFromDatabase();
                    pnlEditShipment.Visible = false;

                    if (oldStatus != cmbEditStatus.SelectedItem.ToString())
                    {
                        selectedShipment.Status = cmbEditStatus.SelectedItem.ToString();
                        _ = EmailManager.SendShipmentStatusUpdateEmail("admin@ryzenshipment.com", selectedShipment);
                    }

                    ErrorHandler.ShowInfo("Shipment updated successfully!", "Success");
                }
                else
                {
                    ErrorHandler.ShowWarning(shipmentManager.LastError, "Update Failed");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Save Shipment");
            }
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditShipment.Visible = false;
            selectedShipment = null;
        }

        private void btnDeleteShipment_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment shipment)
                {
                    var result = ErrorHandler.ShowConfirmation(
                        $"Delete shipment #{shipment.ID}?\n\n{shipment.Description}",
                        "Confirm Delete");

                    if (result == DialogResult.Yes)
                    {
                        bool success = shipmentManager.DeleteShipment(shipment.ID);

                        if (success)
                        {
                            LoadShipmentsFromDatabase();
                            ErrorHandler.ShowInfo("Shipment deleted successfully!", "Success");
                        }
                        else
                        {
                            ErrorHandler.ShowWarning(shipmentManager.LastError, "Delete Failed");
                        }
                    }
                }
                else
                {
                    ErrorHandler.ShowWarning("Please select a shipment to delete.", "No Selection");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Delete Shipment");
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShipments?.CurrentRow?.DataBoundItem is Shipment shipment)
                {
                    string details = $"Shipment Details:\n\n" +
                                   $"ID: {shipment.ID}\n" +
                                   $"Description: {shipment.Description}\n" +
                                   $"Status: {shipment.Status}\n" +
                                   $"Destination: {shipment.Destination}\n" +
                                   $"Date Shipped: {shipment.DateShipped:MM/dd/yyyy}\n" +
                                   $"Estimated Arrival: {shipment.EstimatedArrival:MM/dd/yyyy}\n" +
                                   $"Tracking: {shipment.TrackingNumber}";

                    ErrorHandler.ShowInfo(details, "Shipment Details");
                }
                else
                {
                    ErrorHandler.ShowWarning("Please select a shipment to view.", "No Selection");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "View Details");
            }
        }

        private void frmManageShipments_Load_1(object sender, EventArgs e)
        {
            frmManageShipments_Load(sender, e);
        }

        public void RefreshData()
        {
            LoadShipmentsFromDatabase();
        }

        public int GetShipmentCount()
        {
            return shipments?.Count ?? 0;
        }

        public Dictionary<string, int> GetShipmentsByStatus()
        {
            if (shipments == null) return new Dictionary<string, int>();
            return shipments.GroupBy(s => s.Status).ToDictionary(g => g.Key, g => g.Count());
        }
    }
}