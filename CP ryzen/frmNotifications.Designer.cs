namespace ShippingManagementSystem
{
    partial class frmNotifications
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblNotifications;
        private System.Windows.Forms.ListView lvNotifications;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnMarkAsRead;
        private System.Windows.Forms.Button btnMarkAsUnread;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.CheckBox chkEnablePopups;
        private System.Windows.Forms.CheckBox chkEnableSounds;
        private System.Windows.Forms.ComboBox cmbThemes;
        private System.Windows.Forms.TextBox txtSearchBar;
        private System.Windows.Forms.Button btnDarkMode;

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
            this.lblNotifications = new System.Windows.Forms.Label();
            this.lvNotifications = new System.Windows.Forms.ListView();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnMarkAsRead = new System.Windows.Forms.Button();
            this.btnMarkAsUnread = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.chkEnablePopups = new System.Windows.Forms.CheckBox();
            this.chkEnableSounds = new System.Windows.Forms.CheckBox();
            this.cmbThemes = new System.Windows.Forms.ComboBox();
            this.txtSearchBar = new System.Windows.Forms.TextBox();
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNotifications
            // 
            this.lblNotifications.AutoSize = true;
            this.lblNotifications.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold);
            this.lblNotifications.Location = new System.Drawing.Point(220, 20);
            this.lblNotifications.Name = "lblNotifications";
            this.lblNotifications.Size = new System.Drawing.Size(180, 35);
            this.lblNotifications.TabIndex = 0;
            this.lblNotifications.Text = "Notifications";
            // 
            // lvNotifications
            // 
            this.lvNotifications.Font = new System.Drawing.Font("Arial", 12F);
            this.lvNotifications.FullRowSelect = true;
            this.lvNotifications.GridLines = true;
            this.lvNotifications.HideSelection = false;
            this.lvNotifications.Location = new System.Drawing.Point(30, 70);
            this.lvNotifications.MultiSelect = false;
            this.lvNotifications.Name = "lvNotifications";
            this.lvNotifications.Size = new System.Drawing.Size(540, 220);
            this.lvNotifications.TabIndex = 1;
            this.lvNotifications.View = System.Windows.Forms.View.Details;
            // 
            // btnClearAll
            // 
            this.btnClearAll.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnClearAll.Location = new System.Drawing.Point(30, 310);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(120, 40);
            this.btnClearAll.TabIndex = 2;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnMarkAsRead
            // 
            this.btnMarkAsRead.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnMarkAsRead.Location = new System.Drawing.Point(170, 310);
            this.btnMarkAsRead.Name = "btnMarkAsRead";
            this.btnMarkAsRead.Size = new System.Drawing.Size(150, 40);
            this.btnMarkAsRead.TabIndex = 3;
            this.btnMarkAsRead.Text = "Mark as Read";
            this.btnMarkAsRead.UseVisualStyleBackColor = true;
            this.btnMarkAsRead.Click += new System.EventHandler(this.btnMarkAsRead_Click);
            // 
            // btnMarkAsUnread
            // 
            this.btnMarkAsUnread.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnMarkAsUnread.Location = new System.Drawing.Point(340, 310);
            this.btnMarkAsUnread.Name = "btnMarkAsUnread";
            this.btnMarkAsUnread.Size = new System.Drawing.Size(150, 40);
            this.btnMarkAsUnread.TabIndex = 4;
            this.btnMarkAsUnread.Text = "Mark as Unread";
            this.btnMarkAsUnread.UseVisualStyleBackColor = true;
            this.btnMarkAsUnread.Click += new System.EventHandler(this.btnMarkAsUnread_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(30, 370);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 40);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(460, 370);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 40);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(320, 370);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 40);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cmbFilter
            // 
            this.cmbFilter.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "All",
            "High Priority",
            "Medium Priority",
            "Low Priority"});
            this.cmbFilter.Location = new System.Drawing.Point(170, 370);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(120, 27);
            this.cmbFilter.TabIndex = 8;
            this.cmbFilter.Text = "Filter";
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.cmbFilter_SelectedIndexChanged);
            // 
            // chkEnablePopups
            // 
            this.chkEnablePopups.AutoSize = true;
            this.chkEnablePopups.Font = new System.Drawing.Font("Arial", 10F);
            this.chkEnablePopups.Location = new System.Drawing.Point(30, 420);
            this.chkEnablePopups.Name = "chkEnablePopups";
            this.chkEnablePopups.Size = new System.Drawing.Size(146, 23);
            this.chkEnablePopups.TabIndex = 9;
            this.chkEnablePopups.Text = "Enable Popups";
            this.chkEnablePopups.UseVisualStyleBackColor = true;
            this.chkEnablePopups.CheckedChanged += new System.EventHandler(this.chkEnablePopups_CheckedChanged);
            // 
            // chkEnableSounds
            // 
            this.chkEnableSounds.AutoSize = true;
            this.chkEnableSounds.Font = new System.Drawing.Font("Arial", 10F);
            this.chkEnableSounds.Location = new System.Drawing.Point(200, 420);
            this.chkEnableSounds.Name = "chkEnableSounds";
            this.chkEnableSounds.Size = new System.Drawing.Size(146, 23);
            this.chkEnableSounds.TabIndex = 10;
            this.chkEnableSounds.Text = "Enable Sounds";
            this.chkEnableSounds.UseVisualStyleBackColor = true;
            this.chkEnableSounds.CheckedChanged += new System.EventHandler(this.chkEnableSounds_CheckedChanged);
            // 
            // cmbThemes
            // 
            this.cmbThemes.Font = new System.Drawing.Font("Arial", 10F);
            this.cmbThemes.FormattingEnabled = true;
            this.cmbThemes.Items.AddRange(new object[] {
            "Light",
            "Dark",
            "Blue"});
            this.cmbThemes.Location = new System.Drawing.Point(370, 420);
            this.cmbThemes.Name = "cmbThemes";
            this.cmbThemes.Size = new System.Drawing.Size(150, 27);
            this.cmbThemes.TabIndex = 11;
            this.cmbThemes.Text = "Theme";
            this.cmbThemes.SelectedIndexChanged += new System.EventHandler(this.cmbThemes_SelectedIndexChanged);
            // 
            // txtSearchBar
            // 
            this.txtSearchBar.Font = new System.Drawing.Font("Arial", 10F);
            this.txtSearchBar.Location = new System.Drawing.Point(30, 450);
            this.txtSearchBar.Name = "txtSearchBar";
            this.txtSearchBar.Size = new System.Drawing.Size(400, 27);
            this.txtSearchBar.TabIndex = 12;
            this.txtSearchBar.Text = "Search...";
            this.txtSearchBar.Enter += new System.EventHandler(this.txtSearchBar_Enter);
            this.txtSearchBar.Leave += new System.EventHandler(this.txtSearchBar_Leave);
            // 
            // btnDarkMode
            // 
            this.btnDarkMode.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnDarkMode.Location = new System.Drawing.Point(450, 450);
            this.btnDarkMode.Name = "btnDarkMode";
            this.btnDarkMode.Size = new System.Drawing.Size(120, 40);
            this.btnDarkMode.TabIndex = 13;
            this.btnDarkMode.Text = "Dark Mode";
            this.btnDarkMode.UseVisualStyleBackColor = true;
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);
            // 
            // frmNotifications
            // 
            this.ClientSize = new System.Drawing.Size(600, 500);
            this.Controls.Add(this.btnDarkMode);
            this.Controls.Add(this.txtSearchBar);
            this.Controls.Add(this.cmbThemes);
            this.Controls.Add(this.chkEnableSounds);
            this.Controls.Add(this.chkEnablePopups);
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnMarkAsUnread);
            this.Controls.Add(this.btnMarkAsRead);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.lvNotifications);
            this.Controls.Add(this.lblNotifications);
            this.Name = "frmNotifications";
            this.Text = "Notifications";
            this.Load += new System.EventHandler(this.frmNotifications_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
