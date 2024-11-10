using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmNotifications : Form
    {
        public frmNotifications()
        {
            InitializeComponent();
        }

        private void frmNotifications_Load(object sender, EventArgs e)
        {
            InitializeNotifications();
            ConfigureListView();
        }

        // Initialize sample notifications
        private void InitializeNotifications()
        {
            AddNotification("Package shipped.", "High", "2023-11-10 10:00 AM");
            AddNotification("Delivery expected tomorrow.", "Medium", "2023-11-11 5:00 PM");
            AddNotification("Shipment delayed due to weather conditions.", "High", "2023-11-10 12:30 PM");
            AddNotification("Your package has been delivered.", "Low", "2023-11-09 4:15 PM");
        }

        // Configure ListView for notifications
        private void ConfigureListView()
        {
            lvNotifications.Columns.Add("Message", 300);
            lvNotifications.Columns.Add("Priority", 100);
            lvNotifications.Columns.Add("Timestamp", 140);
        }

        // Add a notification to the ListView
        private void AddNotification(string message, string priority, string timestamp)
        {
            ListViewItem item = new ListViewItem(message);
            item.SubItems.Add(priority);
            item.SubItems.Add(timestamp);

            if (priority == "High")
                item.ForeColor = Color.Red;
            else if (priority == "Medium")
                item.ForeColor = Color.Orange;
            else
                item.ForeColor = Color.Green;

            lvNotifications.Items.Add(item);
        }

        // Clear all notifications
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all notifications?", "Clear Notifications", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                lvNotifications.Items.Clear();
            }
        }

        // Mark selected notification as read
        private void btnMarkAsRead_Click(object sender, EventArgs e)
        {
            if (lvNotifications.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lvNotifications.SelectedItems)
                {
                    item.BackColor = Color.LightGray;
                    item.Font = new Font(lvNotifications.Font, FontStyle.Regular);
                }
            }
            else
            {
                MessageBox.Show("Please select a notification to mark as read.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Mark selected notification as unread
        private void btnMarkAsUnread_Click(object sender, EventArgs e)
        {
            if (lvNotifications.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lvNotifications.SelectedItems)
                {
                    item.BackColor = Color.White;
                    item.Font = new Font(lvNotifications.Font, FontStyle.Bold);
                }
            }
            else
            {
                MessageBox.Show("Please select a notification to mark as unread.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Search notifications
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearchBar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(query) || query == "search...")
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (ListViewItem item in lvNotifications.Items)
            {
                item.BackColor = Color.White;
                if (item.SubItems[0].Text.ToLower().Contains(query))
                {
                    item.BackColor = Color.Yellow;
                }
            }
        }

        // Export notifications to a file
        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Export Notifications";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        foreach (ListViewItem item in lvNotifications.Items)
                        {
                            string line = $"{item.Text}, Priority: {item.SubItems[1].Text}, Timestamp: {item.SubItems[2].Text}";
                            writer.WriteLine(line);
                        }
                    }

                    MessageBox.Show("Notifications exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Filter notifications by priority
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = cmbFilter.SelectedItem.ToString();

            foreach (ListViewItem item in lvNotifications.Items)
            {
                item.BackColor = Color.White;

                if (filter == "All" || item.SubItems[1].Text == filter)
                {
                    item.ForeColor = item.SubItems[1].Text == "High" ? Color.Red :
                                     item.SubItems[1].Text == "Medium" ? Color.Orange : Color.Green;
                }
                else
                {
                    item.ForeColor = Color.LightGray;
                }
            }
        }

        // Toggle popup notifications
        private void chkEnablePopups_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show($"Popups are now {(chkEnablePopups.Checked ? "enabled" : "disabled")}.", "Notification Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Toggle sound notifications
        private void chkEnableSounds_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show($"Sounds are now {(chkEnableSounds.Checked ? "enabled" : "disabled")}.", "Notification Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Change theme
        private void cmbThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string theme = cmbThemes.SelectedItem.ToString();

            if (theme == "Light")
            {
                ApplyTheme(Color.White, Color.Black);
            }
            else if (theme == "Dark")
            {
                ApplyTheme(Color.Black, Color.White);
            }
            else if (theme == "Blue")
            {
                ApplyTheme(Color.LightBlue, Color.Black);
            }
        }

        private void ApplyTheme(Color backgroundColor, Color textColor)
        {
            BackColor = backgroundColor;
            ForeColor = textColor;

            foreach (Control control in Controls)
            {
                control.BackColor = backgroundColor;
                control.ForeColor = textColor;
            }

            lvNotifications.BackColor = backgroundColor;
            lvNotifications.ForeColor = textColor;
        }

        // Dark mode toggle
        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            ApplyTheme(BackColor == Color.Black ? Color.White : Color.Black, ForeColor == Color.Black ? Color.White : Color.Black);
        }

        // Placeholder for search bar
        private void txtSearchBar_Enter(object sender, EventArgs e)
        {
            if (txtSearchBar.Text == "Search...")
            {
                txtSearchBar.Text = string.Empty;
                txtSearchBar.ForeColor = Color.Black;
            }
        }

        private void txtSearchBar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchBar.Text))
            {
                txtSearchBar.Text = "Search...";
                txtSearchBar.ForeColor = Color.Gray;
            }
        }

        // Close form
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
