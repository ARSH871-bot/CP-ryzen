using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ShippingManagementSystem
{
    public partial class frmNotifications : Form
    {
        private List<NotificationItem> notifications;
        private bool popupsEnabled = true;
        private bool soundsEnabled = true;

        public frmNotifications()
        {
            InitializeComponent();
            notifications = new List<NotificationItem>();
        }

        private void frmNotifications_Load(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                InitializeNotifications();
                ConfigureListView();
                SetupControls();
                ErrorHandler.LogInfo("Notifications form loaded successfully", "frmNotifications_Load");
            }, "Loading Notifications Form");
        }

        private void SetupControls()
        {
            ErrorHandler.SafeExecute(() =>
            {
                // Initialize checkboxes
                if (chkEnablePopups != null)
                    chkEnablePopups.Checked = popupsEnabled;
                if (chkEnableSounds != null)
                    chkEnableSounds.Checked = soundsEnabled;

                // Initialize filter combo
                if (cmbFilter != null && cmbFilter.Items.Count == 0)
                {
                    cmbFilter.Items.AddRange(new string[] { "All", "High", "Medium", "Low", "Unread", "Read" });
                    cmbFilter.SelectedIndex = 0;
                }

                // Initialize themes combo
                if (cmbThemes != null && cmbThemes.Items.Count == 0)
                {
                    cmbThemes.Items.AddRange(new string[] { "Light", "Dark", "Blue" });
                    cmbThemes.SelectedIndex = 0;
                }

                // Setup search placeholder
                if (txtSearchBar != null)
                {
                    txtSearchBar.Text = "Search...";
                    txtSearchBar.ForeColor = Color.Gray;
                }
            }, "Controls Setup");
        }

        private void InitializeNotifications()
        {
            ErrorHandler.SafeExecute(() =>
            {
                notifications.Clear();
                AddNotification("Package shipped to Auckland", "High", DateTime.Now.AddHours(-2), false);
                AddNotification("Delivery completed - Customer satisfied", "Medium", DateTime.Now.AddHours(-4), true);
                AddNotification("Weather delay - Shipment postponed", "High", DateTime.Now.AddHours(-6), false);
                AddNotification("Your package has been delivered", "Low", DateTime.Now.AddDays(-1), true);
                AddNotification("New shipment request received", "Medium", DateTime.Now.AddMinutes(-30), false);
                AddNotification("System maintenance scheduled", "Low", DateTime.Now.AddHours(-12), true);
                AddNotification("Payment confirmation received", "Medium", DateTime.Now.AddHours(-8), false);
                AddNotification("Route optimization completed", "Low", DateTime.Now.AddDays(-2), true);

                RefreshNotificationsList();
                ErrorHandler.LogInfo($"Initialized {notifications.Count} notifications", "InitializeNotifications");
            }, "Notifications Initialization");
        }

        private void ConfigureListView()
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvNotifications != null)
                {
                    lvNotifications.Columns.Clear();
                    lvNotifications.Columns.Add("Message", 400);
                    lvNotifications.Columns.Add("Priority", 80);
                    lvNotifications.Columns.Add("Time", 120);
                    lvNotifications.Columns.Add("Status", 60);
                    lvNotifications.View = View.Details;
                    lvNotifications.FullRowSelect = true;
                    lvNotifications.GridLines = true;
                }
            }, "ListView Configuration");
        }

        private void AddNotification(string message, string priority, DateTime timestamp, bool isRead)
        {
            ErrorHandler.SafeExecute(() =>
            {
                var notification = new NotificationItem
                {
                    Message = message,
                    Priority = priority,
                    Timestamp = timestamp,
                    IsRead = isRead
                };

                notifications.Insert(0, notification); // Add to top
                RefreshNotificationsList();
            }, "Add Notification");
        }

        private void RefreshNotificationsList()
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvNotifications == null) return;

                string filter = cmbFilter?.SelectedItem?.ToString() ?? "All";
                var filteredNotifications = GetFilteredNotifications(filter);

                lvNotifications.Items.Clear();

                foreach (var notification in filteredNotifications)
                {
                    ListViewItem item = new ListViewItem(notification.Message);
                    item.SubItems.Add(notification.Priority);
                    item.SubItems.Add(GetTimeDisplayText(notification.Timestamp));
                    item.SubItems.Add(notification.IsRead ? "Read" : "Unread");
                    item.Tag = notification;

                    // Apply styling based on priority and read status
                    ApplyNotificationStyling(item, notification);

                    lvNotifications.Items.Add(item);
                }

                UpdateNotificationCounts();
            }, "Notifications List Refresh");
        }

        private List<NotificationItem> GetFilteredNotifications(string filter)
        {
            return ErrorHandler.SafeExecute(() =>
            {
                switch (filter.ToLower())
                {
                    case "high":
                    case "medium":
                    case "low":
                        return notifications.Where(n => n.Priority.ToLower() == filter.ToLower()).ToList();
                    case "unread":
                        return notifications.Where(n => !n.IsRead).ToList();
                    case "read":
                        return notifications.Where(n => n.IsRead).ToList();
                    default:
                        return notifications.ToList();
                }
            }, new List<NotificationItem>(), "Notification Filtering");
        }

        private void ApplyNotificationStyling(ListViewItem item, NotificationItem notification)
        {
            ErrorHandler.SafeExecute(() =>
            {
                // Priority color coding
                switch (notification.Priority.ToLower())
                {
                    case "high":
                        item.ForeColor = Color.Red;
                        break;
                    case "medium":
                        item.ForeColor = Color.Orange;
                        break;
                    case "low":
                        item.ForeColor = Color.Green;
                        break;
                }

                // Read/Unread styling
                if (notification.IsRead)
                {
                    item.BackColor = Color.LightGray;
                    item.Font = new Font(lvNotifications.Font, FontStyle.Regular);
                }
                else
                {
                    item.BackColor = Color.White;
                    item.Font = new Font(lvNotifications.Font, FontStyle.Bold);
                }
            }, "Notification Styling");
        }

        private void UpdateNotificationCounts()
        {
            ErrorHandler.SafeExecute(() =>
            {
                int totalCount = notifications.Count;
                int unreadCount = notifications.Count(n => !n.IsRead);

                if (lblNotifications != null)
                {
                    lblNotifications.Text = $"Notifications ({unreadCount} unread of {totalCount})";
                }
            }, "Notification Count Update");
        }

        private string GetTimeDisplayText(DateTime timestamp)
        {
            return ErrorHandler.SafeExecute(() =>
            {
                var timeSpan = DateTime.Now - timestamp;

                if (timeSpan.TotalMinutes < 60)
                    return $"{(int)timeSpan.TotalMinutes}m ago";
                else if (timeSpan.TotalHours < 24)
                    return $"{(int)timeSpan.TotalHours}h ago";
                else if (timeSpan.TotalDays < 7)
                    return $"{(int)timeSpan.TotalDays}d ago";
                else
                    return timestamp.ToString("MMM dd");
            }, "N/A", "Time Display Calculation");
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (notifications.Count == 0)
                {
                    ErrorHandler.ShowInfo("No notifications to clear.", "No Notifications");
                    return;
                }

                var result = ErrorHandler.ShowConfirmation(
                    $"Are you sure you want to clear all {notifications.Count} notifications?\n\nThis action cannot be undone.",
                    "Clear All Notifications");

                if (result == DialogResult.Yes)
                {
                    int clearedCount = notifications.Count;
                    notifications.Clear();
                    RefreshNotificationsList();
                    ErrorHandler.ShowInfo($"Successfully cleared {clearedCount} notifications.", "Notifications Cleared");
                    ErrorHandler.LogInfo($"Cleared {clearedCount} notifications", "btnClearAll_Click");
                }
            }, "Clear All Notifications");
        }

        private void btnMarkAsRead_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvNotifications.SelectedItems.Count == 0)
                {
                    ErrorHandler.ShowWarning("Please select one or more notifications to mark as read.", "No Selection");
                    return;
                }

                int markedCount = 0;
                foreach (ListViewItem item in lvNotifications.SelectedItems)
                {
                    if (item.Tag is NotificationItem notification && !notification.IsRead)
                    {
                        notification.IsRead = true;
                        markedCount++;
                    }
                }

                if (markedCount > 0)
                {
                    RefreshNotificationsList();
                    ErrorHandler.ShowInfo($"Marked {markedCount} notification(s) as read.", "Status Updated");
                    ErrorHandler.LogInfo($"Marked {markedCount} notifications as read", "btnMarkAsRead_Click");
                }
                else
                {
                    ErrorHandler.ShowInfo("Selected notifications are already marked as read.", "No Changes");
                }
            }, "Mark As Read");
        }

        private void btnMarkAsUnread_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvNotifications.SelectedItems.Count == 0)
                {
                    ErrorHandler.ShowWarning("Please select one or more notifications to mark as unread.", "No Selection");
                    return;
                }

                int markedCount = 0;
                foreach (ListViewItem item in lvNotifications.SelectedItems)
                {
                    if (item.Tag is NotificationItem notification && notification.IsRead)
                    {
                        notification.IsRead = false;
                        markedCount++;
                    }
                }

                if (markedCount > 0)
                {
                    RefreshNotificationsList();
                    ErrorHandler.ShowInfo($"Marked {markedCount} notification(s) as unread.", "Status Updated");
                    ErrorHandler.LogInfo($"Marked {markedCount} notifications as unread", "btnMarkAsUnread_Click");
                }
                else
                {
                    ErrorHandler.ShowInfo("Selected notifications are already marked as unread.", "No Changes");
                }
            }, "Mark As Unread");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string query = txtSearchBar?.Text?.Trim()?.ToLower();

                if (string.IsNullOrEmpty(query) || query == "search...")
                {
                    ErrorHandler.ShowWarning("Please enter a search term.", "Search Input Required");
                    txtSearchBar?.Focus();
                    return;
                }

                SearchNotifications(query);
            }, "Notification Search");
        }

        private void SearchNotifications(string query)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvNotifications == null) return;

                int matchCount = 0;
                foreach (ListViewItem item in lvNotifications.Items)
                {
                    bool isMatch = item.Text.ToLower().Contains(query) ||
                                  item.SubItems.Cast<ListViewItem.ListViewSubItem>()
                                      .Any(subItem => subItem.Text.ToLower().Contains(query));

                    if (isMatch)
                    {
                        item.BackColor = Color.Yellow;
                        matchCount++;
                    }
                    else
                    {
                        // Restore original background color
                        if (item.Tag is NotificationItem notification)
                        {
                            ApplyNotificationStyling(item, notification);
                        }
                    }
                }

                ErrorHandler.ShowInfo($"Search completed.\nFound {matchCount} matching notifications.", "Search Results");
                ErrorHandler.LogInfo($"Search performed for '{query}', found {matchCount} matches", "SearchNotifications");
            }, "Notification Search Process");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (notifications.Count == 0)
                {
                    ErrorHandler.ShowWarning("No notifications to export.", "No Data Available");
                    return;
                }

                ExportNotifications();
            }, "Export Notifications");
        }

        private void ExportNotifications()
        {
            ErrorHandler.SafeExecute(() =>
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv";
                    saveFileDialog.Title = "Export Notifications";
                    saveFileDialog.FileName = $"Notifications_Export_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        bool isCSV = saveFileDialog.FileName.ToLower().EndsWith(".csv");

                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            if (isCSV)
                            {
                                writer.WriteLine("Message,Priority,Timestamp,Status");
                                foreach (var notification in notifications)
                                {
                                    writer.WriteLine($"\"{notification.Message}\",{notification.Priority},{notification.Timestamp:yyyy-MM-dd HH:mm:ss},{(notification.IsRead ? "Read" : "Unread")}");
                                }
                            }
                            else
                            {
                                writer.WriteLine("RYZEN SHIPPING MANAGEMENT SYSTEM");
                                writer.WriteLine("NOTIFICATIONS EXPORT");
                                writer.WriteLine($"Generated: {DateTime.Now}");
                                writer.WriteLine($"Total Notifications: {notifications.Count}");
                                writer.WriteLine(new string('=', 60));
                                writer.WriteLine();

                                foreach (var notification in notifications)
                                {
                                    writer.WriteLine($"Message: {notification.Message}");
                                    writer.WriteLine($"Priority: {notification.Priority}");
                                    writer.WriteLine($"Time: {notification.Timestamp}");
                                    writer.WriteLine($"Status: {(notification.IsRead ? "Read" : "Unread")}");
                                    writer.WriteLine(new string('-', 40));
                                }
                            }
                        }

                        ErrorHandler.ShowInfo($"Notifications exported successfully to:\n{saveFileDialog.FileName}", "Export Complete");
                        ErrorHandler.LogInfo($"Exported {notifications.Count} notifications to: {saveFileDialog.FileName}", "ExportNotifications");
                    }
                }
            }, "Export Process");
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                RefreshNotificationsList();
                string filter = cmbFilter?.SelectedItem?.ToString() ?? "All";
                ErrorHandler.LogInfo($"Filter changed to: {filter}", "cmbFilter_SelectedIndexChanged");
            }, "Filter Change");
        }

        private void chkEnablePopups_CheckedChanged(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                popupsEnabled = chkEnablePopups?.Checked ?? false;
                ErrorHandler.ShowInfo($"Popup notifications are now {(popupsEnabled ? "enabled" : "disabled")}.", "Notification Settings");
                ErrorHandler.LogInfo($"Popup notifications {(popupsEnabled ? "enabled" : "disabled")}", "chkEnablePopups_CheckedChanged");
            }, "Popup Settings Change");
        }

        private void chkEnableSounds_CheckedChanged(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                soundsEnabled = chkEnableSounds?.Checked ?? false;
                ErrorHandler.ShowInfo($"Sound notifications are now {(soundsEnabled ? "enabled" : "disabled")}.", "Notification Settings");
                ErrorHandler.LogInfo($"Sound notifications {(soundsEnabled ? "enabled" : "disabled")}", "chkEnableSounds_CheckedChanged");
            }, "Sound Settings Change");
        }

        private void cmbThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (cmbThemes?.SelectedItem != null)
                {
                    string theme = cmbThemes.SelectedItem.ToString();
                    ApplyTheme(theme);
                    ErrorHandler.LogInfo($"Theme changed to: {theme}", "cmbThemes_SelectedIndexChanged");
                }
            }, "Theme Change");
        }

        private void ApplyTheme(string theme)
        {
            ErrorHandler.SafeExecute(() =>
            {
                Color backgroundColor, textColor;

                switch (theme.ToLower())
                {
                    case "dark":
                        backgroundColor = Color.FromArgb(45, 45, 48);
                        textColor = Color.White;
                        break;
                    case "blue":
                        backgroundColor = Color.LightBlue;
                        textColor = Color.Black;
                        break;
                    default: // Light
                        backgroundColor = Color.White;
                        textColor = Color.Black;
                        break;
                }

                BackColor = backgroundColor;
                ForeColor = textColor;

                foreach (Control control in Controls)
                {
                    ApplyThemeToControl(control, backgroundColor, textColor);
                }

                // Refresh notifications to reapply styling
                RefreshNotificationsList();
            }, "Theme Application");
        }

        private void ApplyThemeToControl(Control control, Color backgroundColor, Color textColor)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (control is TextBox || control is ComboBox)
                {
                    control.BackColor = backgroundColor == Color.White ? Color.White : Color.FromArgb(62, 62, 66);
                }
                else if (control is ListView)
                {
                    control.BackColor = backgroundColor;
                }
                else
                {
                    control.BackColor = backgroundColor;
                }
                control.ForeColor = textColor;

                foreach (Control child in control.Controls)
                {
                    ApplyThemeToControl(child, backgroundColor, textColor);
                }
            }, "Individual Control Theme");
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string currentTheme = BackColor == Color.White ? "Dark" : "Light";
                ApplyTheme(currentTheme);

                if (btnDarkMode != null)
                {
                    btnDarkMode.Text = currentTheme == "Dark" ? "Light Mode" : "Dark Mode";
                }
            }, "Dark Mode Toggle");
        }

        private void txtSearchBar_Enter(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (txtSearchBar?.Text == "Search...")
                {
                    txtSearchBar.Text = string.Empty;
                    txtSearchBar.ForeColor = Color.Black;
                }
            }, "Search Text Focus");
        }

        private void txtSearchBar_Leave(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(txtSearchBar?.Text))
                {
                    txtSearchBar.Text = "Search...";
                    txtSearchBar.ForeColor = Color.Gray;
                }
            }, "Search Text Blur");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                this.Close();
            }, "Form Close");
        }

        // Method to add new notification (called from other parts of the application)
        public void AddNewNotification(string message, string priority = "Medium")
        {
            ErrorHandler.SafeExecute(() =>
            {
                AddNotification(message, priority, DateTime.Now, false);

                if (popupsEnabled)
                {
                    ShowNotificationPopup(message, priority);
                }

                ErrorHandler.LogInfo($"New notification added: {message} ({priority})", "AddNewNotification");
            }, "Add New Notification");
        }

        private void ShowNotificationPopup(string message, string priority)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string title = $"{priority} Priority Notification";
                MessageBox.Show(message, title, MessageBoxButtons.OK,
                    priority == "High" ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            }, "Notification Popup");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                ErrorHandler.LogInfo("Notifications form closed", "OnFormClosing");
            }, "Form Cleanup");

            base.OnFormClosing(e);
        }

        // Method to refresh data (called from dashboard)
        public void RefreshData()
        {
            ErrorHandler.SafeExecute(() =>
            {
                RefreshNotificationsList();
            }, "External Data Refresh");
        }
    }

    // Data model for notifications
    public class NotificationItem
    {
        public string Message { get; set; }
        public string Priority { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}