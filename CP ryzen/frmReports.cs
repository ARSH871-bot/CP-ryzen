using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmReports : Form
    {
        private List<ReportData> sampleData;
        private bool isGeneratingReport = false;

        public frmReports()
        {
            InitializeComponent();
            InitializeSampleData();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                ConfigureListView();
                InitializeReportTypes();
                SetDefaultDateRange();
                ErrorHandler.LogInfo("Reports form loaded successfully", "frmReports_Load");
            }, "Loading Reports Form");
        }

        private void InitializeSampleData()
        {
            ErrorHandler.SafeExecute(() =>
            {
                sampleData = new List<ReportData>
                {
                    new ReportData { Date = DateTime.Now.AddDays(-30), ShipmentCount = 45, Revenue = 12500.00m, Status = "Delivered" },
                    new ReportData { Date = DateTime.Now.AddDays(-29), ShipmentCount = 38, Revenue = 9800.00m, Status = "In Transit" },
                    new ReportData { Date = DateTime.Now.AddDays(-28), ShipmentCount = 52, Revenue = 15200.00m, Status = "Delivered" },
                    new ReportData { Date = DateTime.Now.AddDays(-27), ShipmentCount = 41, Revenue = 11300.00m, Status = "Processing" },
                    new ReportData { Date = DateTime.Now.AddDays(-26), ShipmentCount = 47, Revenue = 13100.00m, Status = "Delivered" },
                    new ReportData { Date = DateTime.Now.AddDays(-25), ShipmentCount = 35, Revenue = 8900.00m, Status = "In Transit" },
                    new ReportData { Date = DateTime.Now.AddDays(-24), ShipmentCount = 49, Revenue = 14200.00m, Status = "Delivered" },
                    new ReportData { Date = DateTime.Now.AddDays(-23), ShipmentCount = 43, Revenue = 12000.00m, Status = "Processing" },
                    new ReportData { Date = DateTime.Now.AddDays(-22), ShipmentCount = 56, Revenue = 16500.00m, Status = "Delivered" },
                    new ReportData { Date = DateTime.Now.AddDays(-21), ShipmentCount = 39, Revenue = 10400.00m, Status = "In Transit" }
                };
            }, "Sample Data Initialization");
        }

        private void ConfigureListView()
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData != null)
                {
                    lvReportData.Columns.Clear();
                    lvReportData.Columns.Add("Date", 100);
                    lvReportData.Columns.Add("Shipments", 80);
                    lvReportData.Columns.Add("Revenue", 100);
                    lvReportData.Columns.Add("Status", 100);
                    lvReportData.View = View.Details;
                    lvReportData.FullRowSelect = true;
                    lvReportData.GridLines = true;
                }
            }, "ListView Configuration");
        }

        private void InitializeReportTypes()
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (cmbReportType != null)
                {
                    cmbReportType.Items.Clear();
                    cmbReportType.Items.AddRange(new string[]
                    {
                        "Daily Summary Report",
                        "Weekly Performance Report",
                        "Monthly Revenue Report",
                        "Quarterly Analysis Report",
                        "Annual Overview Report",
                        "Custom Date Range Report",
                        "Shipment Status Report",
                        "Revenue Trend Analysis"
                    });
                    cmbReportType.SelectedIndex = 0;
                }
            }, "Report Types Initialization");
        }

        private void SetDefaultDateRange()
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (dtpStartDate != null && dtpEndDate != null)
                {
                    dtpStartDate.Value = DateTime.Now.AddDays(-30);
                    dtpEndDate.Value = DateTime.Now;
                }
            }, "Default Date Range Setting");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (isGeneratingReport)
                {
                    ErrorHandler.ShowWarning("A report is already being generated. Please wait for it to complete.", "Generation In Progress");
                    return;
                }

                if (cmbReportType.SelectedIndex < 0)
                {
                    ErrorHandler.ShowWarning("Please select a report type.", "Selection Required");
                    cmbReportType.Focus();
                    return;
                }

                string reportType = cmbReportType.SelectedItem.ToString();
                DateTime startDate = dtpStartDate.Value.Date;
                DateTime endDate = dtpEndDate.Value.Date;

                if (startDate > endDate)
                {
                    ErrorHandler.ShowWarning("Start date cannot be later than end date.", "Invalid Date Range");
                    dtpStartDate.Focus();
                    return;
                }

                if (endDate > DateTime.Now.Date)
                {
                    ErrorHandler.ShowWarning("End date cannot be in the future.", "Invalid Date Range");
                    dtpEndDate.Focus();
                    return;
                }

                GenerateReport(reportType, startDate, endDate);
            }, "Report Generation");
        }

        private void GenerateReport(string reportType, DateTime startDate, DateTime endDate)
        {
            ErrorHandler.SafeExecute(() =>
            {
                isGeneratingReport = true;
                UpdateUIForGeneration(true);

                try
                {
                    // Simulate report generation time
                    for (int i = 0; i <= 100; i += 10)
                    {
                        if (pbLoading != null)
                        {
                            pbLoading.Value = i;
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }

                    var filteredData = GetFilteredData(startDate, endDate);
                    PopulateReportData(filteredData, reportType);

                    ErrorHandler.ShowInfo($"{reportType} generated successfully!\n\nDate Range: {startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}\nRecords: {filteredData.Count}",
                        "Report Generated");

                    ErrorHandler.LogInfo($"Generated {reportType} for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}", "GenerateReport");
                }
                finally
                {
                    isGeneratingReport = false;
                    UpdateUIForGeneration(false);
                }
            }, "Report Generation Process");
        }

        private void UpdateUIForGeneration(bool isGenerating)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (btnGenerate != null)
                {
                    btnGenerate.Text = isGenerating ? "Generating..." : "Generate Report";
                    btnGenerate.Enabled = !isGenerating;
                }

                if (pbLoading != null)
                {
                    pbLoading.Visible = isGenerating;
                    if (!isGenerating) pbLoading.Value = 0;
                }
            }, "UI Generation State Update");
        }

        private List<ReportData> GetFilteredData(DateTime startDate, DateTime endDate)
        {
            return ErrorHandler.SafeExecute(() =>
            {
                return sampleData.Where(d => d.Date.Date >= startDate && d.Date.Date <= endDate).ToList();
            }, new List<ReportData>(), "Data Filtering");
        }

        private void PopulateReportData(List<ReportData> data, string reportType)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData == null) return;

                lvReportData.Items.Clear();

                foreach (var item in data)
                {
                    ListViewItem lvItem = new ListViewItem(item.Date.ToString("MMM dd, yyyy"));
                    lvItem.SubItems.Add(item.ShipmentCount.ToString());
                    lvItem.SubItems.Add(item.Revenue.ToString("C"));
                    lvItem.SubItems.Add(item.Status);

                    // Color code based on revenue
                    if (item.Revenue > 15000)
                        lvItem.BackColor = Color.LightGreen;
                    else if (item.Revenue < 10000)
                        lvItem.BackColor = Color.LightCoral;

                    lvReportData.Items.Add(lvItem);
                }

                // Add summary row if data exists
                if (data.Any())
                {
                    AddSummaryRow(data);
                }
            }, "Report Data Population");
        }

        private void AddSummaryRow(List<ReportData> data)
        {
            ErrorHandler.SafeExecute(() =>
            {
                var summaryItem = new ListViewItem("TOTAL SUMMARY");
                summaryItem.SubItems.Add(data.Sum(d => d.ShipmentCount).ToString());
                summaryItem.SubItems.Add(data.Sum(d => d.Revenue).ToString("C"));
                summaryItem.SubItems.Add($"{data.Count} days");
                summaryItem.Font = new Font(lvReportData.Font, FontStyle.Bold);
                summaryItem.BackColor = Color.LightYellow;

                lvReportData.Items.Add(summaryItem);
            }, "Summary Row Addition");
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData?.Items.Count == 0)
                {
                    ErrorHandler.ShowWarning("No report data to export. Please generate a report first.", "No Data Available");
                    return;
                }

                ExportReport("PDF", "*.pdf", "PDF Files (*.pdf)|*.pdf");
            }, "PDF Export");
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData?.Items.Count == 0)
                {
                    ErrorHandler.ShowWarning("No report data to export. Please generate a report first.", "No Data Available");
                    return;
                }

                ExportReport("Excel", "*.csv", "CSV Files (*.csv)|*.csv");
            }, "Excel Export");
        }

        private void ExportReport(string format, string extension, string filter)
        {
            ErrorHandler.SafeExecute(() =>
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = filter;
                    saveFileDialog.Title = $"Export Report as {format}";
                    saveFileDialog.FileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}{extension.Replace("*", "")}";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        if (format == "Excel")
                        {
                            ExportToCSV(filePath);
                        }
                        else
                        {
                            ExportToPDF(filePath);
                        }

                        ErrorHandler.ShowInfo($"Report exported successfully to:\n{filePath}", "Export Complete");
                        ErrorHandler.LogInfo($"Report exported as {format} to: {filePath}", "ExportReport");
                    }
                }
            }, $"{format} Export Process");
        }

        private void ExportToCSV(string filePath)
        {
            ErrorHandler.SafeExecute(() =>
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write header
                    writer.WriteLine("Date,Shipments,Revenue,Status");

                    // Write data
                    foreach (ListViewItem item in lvReportData.Items)
                    {
                        var line = string.Join(",",
                            item.Text,
                            item.SubItems[1].Text,
                            item.SubItems[2].Text.Replace(",", ""),
                            item.SubItems[3].Text);
                        writer.WriteLine(line);
                    }
                }
            }, "CSV Export");
        }

        private void ExportToPDF(string filePath)
        {
            ErrorHandler.SafeExecute(() =>
            {
                // Simulated PDF export - in real implementation, use a PDF library
                using (StreamWriter writer = new StreamWriter(filePath.Replace(".pdf", ".txt")))
                {
                    writer.WriteLine("RYZEN SHIPPING MANAGEMENT SYSTEM");
                    writer.WriteLine("REPORT EXPORT");
                    writer.WriteLine("Generated: " + DateTime.Now.ToString());
                    writer.WriteLine(new string('=', 50));
                    writer.WriteLine();

                    writer.WriteLine($"{"Date",-15} {"Shipments",-10} {"Revenue",-12} {"Status",-15}");
                    writer.WriteLine(new string('-', 50));

                    foreach (ListViewItem item in lvReportData.Items)
                    {
                        writer.WriteLine($"{item.Text,-15} {item.SubItems[1].Text,-10} {item.SubItems[2].Text,-12} {item.SubItems[3].Text,-15}");
                    }
                }

                ErrorHandler.ShowInfo($"Note: PDF export created as text file.\nIn a full implementation, this would create an actual PDF using a PDF library.", "PDF Export Note");
            }, "PDF Export");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData?.Items.Count == 0)
                {
                    ErrorHandler.ShowWarning("No report data to print. Please generate a report first.", "No Data Available");
                    return;
                }

                ErrorHandler.ShowInfo("Print functionality would open the system print dialog here.\n\nIn a full implementation, this would format and print the report data.", "Print Simulation");
                ErrorHandler.LogInfo("Print report requested", "btnPrint_Click");
            }, "Report Printing");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData != null)
                {
                    lvReportData.Items.Clear();
                }

                InitializeSampleData();
                ErrorHandler.ShowInfo("Report data has been refreshed with updated sample data.", "Data Refreshed");
                ErrorHandler.LogInfo("Report data refreshed", "btnRefresh_Click");
            }, "Data Refresh");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                string query = txtSearch?.Text?.Trim()?.ToLower();

                if (string.IsNullOrEmpty(query) || query == "search...")
                {
                    ErrorHandler.ShowWarning("Please enter a search term.", "Search Input Required");
                    txtSearch?.Focus();
                    return;
                }

                SearchReportData(query);
            }, "Report Search");
        }

        private void SearchReportData(string query)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (lvReportData == null) return;

                int matchCount = 0;
                foreach (ListViewItem item in lvReportData.Items)
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
                        item.BackColor = Color.White;
                    }
                }

                ErrorHandler.ShowInfo($"Search completed.\nFound {matchCount} matching records.", "Search Results");
                ErrorHandler.LogInfo($"Search performed for '{query}', found {matchCount} matches", "SearchReportData");
            }, "Report Data Search");
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (txtSearch?.Text == "Search...")
                {
                    txtSearch.Text = string.Empty;
                    txtSearch.ForeColor = Color.Black;
                }
            }, "Search Text Focus");
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(txtSearch?.Text))
                {
                    txtSearch.Text = "Search...";
                    txtSearch.ForeColor = Color.Gray;
                }
            }, "Search Text Blur");
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                ToggleDarkMode();
            }, "Dark Mode Toggle");
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

        private void ToggleDarkMode()
        {
            ApplyTheme(BackColor == Color.Black ? "Light" : "Dark");
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

                if (lvReportData != null)
                {
                    lvReportData.BackColor = backgroundColor;
                    lvReportData.ForeColor = textColor;
                }
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ErrorHandler.SafeExecute(() =>
            {
                ErrorHandler.LogInfo("Reports form closed", "OnFormClosing");
            }, "Form Cleanup");

            base.OnFormClosing(e);
        }

        // Method to refresh data (called from dashboard)
        public void RefreshData()
        {
            ErrorHandler.SafeExecute(() =>
            {
                btnRefresh_Click(this, EventArgs.Empty);
            }, "External Data Refresh");
        }
    }

    // Data model for reports
    public class ReportData
    {
        public DateTime Date { get; set; }
        public int ShipmentCount { get; set; }
        public decimal Revenue { get; set; }
        public string Status { get; set; }
    }
}