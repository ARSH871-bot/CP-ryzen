using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ShippingManagementSystem
{
    public partial class frmReports : Form
    {
        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            ConfigureListView();
            InitializeReportTypes();
        }

        // Configure ListView for displaying report data
        private void ConfigureListView()
        {
            lvReportData.Columns.Add("Field 1", 150);
            lvReportData.Columns.Add("Field 2", 150);
            lvReportData.Columns.Add("Field 3", 150);
            lvReportData.View = View.Details;
            lvReportData.FullRowSelect = true;
        }

        // Initialize report types dropdown
        private void InitializeReportTypes()
        {
            cmbReportType.Items.Clear();
            cmbReportType.Items.AddRange(new string[]
            {
                "Daily Report",
                "Monthly Report",
                "Yearly Report",
                "Custom Report"
            });
            cmbReportType.SelectedIndex = 0;
        }

        // Generate report based on selected type and date range
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbReportType.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a report type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string reportType = cmbReportType.SelectedItem.ToString();
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            if (startDate > endDate)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerateDummyData(reportType, startDate, endDate);
        }

        // Generate dummy data for the report
        private void GenerateDummyData(string reportType, DateTime startDate, DateTime endDate)
        {
            lvReportData.Items.Clear();
            pbLoading.Value = 0;

            for (int i = 1; i <= 10; i++)
            {
                lvReportData.Items.Add(new ListViewItem(new string[]
                {
                    $"{reportType} - Item {i}",
                    startDate.AddDays(i).ToShortDateString(),
                    endDate.AddDays(-i).ToShortDateString()
                }));
                pbLoading.Value = (i * 10);
                Application.DoEvents();
            }

            MessageBox.Show($"{reportType} generated successfully!", "Report Generated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Export report to PDF
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            ExportReport("PDF");
        }

        // Export report to Excel
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportReport("Excel");
        }

        // Export report to a file
        private void ExportReport(string format)
        {
            if (lvReportData.Items.Count == 0)
            {
                MessageBox.Show("No report data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = format == "PDF" ? "PDF Files (*.pdf)|*.pdf" : "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = $"Export Report as {format}";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    // Simulated export logic
                    File.WriteAllText(filePath, "Sample Report Export Data");
                    MessageBox.Show($"Report exported to {filePath} successfully!", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Print report
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (lvReportData.Items.Count == 0)
            {
                MessageBox.Show("No report data to print.", "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Sending report to printer...", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Refresh the report data
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lvReportData.Items.Clear();
            MessageBox.Show("Report data has been refreshed.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Search in report data
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(query) || query == "search...")
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (ListViewItem item in lvReportData.Items)
            {
                item.BackColor = Color.White;
                if (item.Text.ToLower().Contains(query))
                {
                    item.BackColor = Color.Yellow;
                }
            }
        }

        // Placeholder behavior for search box
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search...")
            {
                txtSearch.Text = string.Empty;
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        // Toggle dark mode
        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            ToggleDarkMode();
        }

        // Apply a theme
        private void cmbThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string theme = cmbThemes.SelectedItem.ToString();

            switch (theme)
            {
                case "Light":
                    ApplyTheme(Color.White, Color.Black);
                    break;
                case "Dark":
                    ApplyTheme(Color.Black, Color.White);
                    break;
                case "Blue":
                    ApplyTheme(Color.LightBlue, Color.Black);
                    break;
                default:
                    ApplyTheme(Color.White, Color.Black);
                    break;
            }
        }

        // Toggle dark mode
        private void ToggleDarkMode()
        {
            if (BackColor == Color.Black)
            {
                ApplyTheme(Color.White, Color.Black);
            }
            else
            {
                ApplyTheme(Color.Black, Color.White);
            }
        }

        // Apply theme colors
        private void ApplyTheme(Color backgroundColor, Color textColor)
        {
            BackColor = backgroundColor;
            ForeColor = textColor;

            foreach (Control control in Controls)
            {
                control.BackColor = backgroundColor;
                control.ForeColor = textColor;
            }

            lvReportData.BackColor = backgroundColor;
            lvReportData.ForeColor = textColor;
        }
    }
}
