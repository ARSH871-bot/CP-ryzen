namespace ShippingManagementSystem
{
    partial class frmDatabaseTest
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnMigrateData;
        private System.Windows.Forms.Button btnTestRepositories;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.Label lblTitle;

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
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnMigrateData = new System.Windows.Forms.Button();
            this.btnTestRepositories = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(30, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Database Migration Tool";

            // btnTestConnection
            this.btnTestConnection.Location = new System.Drawing.Point(30, 80);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(150, 35);
            this.btnTestConnection.TabIndex = 1;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);

            // btnMigrateData
            this.btnMigrateData.Enabled = false;
            this.btnMigrateData.Location = new System.Drawing.Point(190, 80);
            this.btnMigrateData.Name = "btnMigrateData";
            this.btnMigrateData.Size = new System.Drawing.Size(150, 35);
            this.btnMigrateData.TabIndex = 2;
            this.btnMigrateData.Text = "Migrate JSON Data";
            this.btnMigrateData.UseVisualStyleBackColor = true;
            this.btnMigrateData.Click += new System.EventHandler(this.btnMigrateData_Click);

            // btnTestRepositories
            this.btnTestRepositories.Enabled = false;
            this.btnTestRepositories.Location = new System.Drawing.Point(350, 80);
            this.btnTestRepositories.Name = "btnTestRepositories";
            this.btnTestRepositories.Size = new System.Drawing.Size(150, 35);
            this.btnTestRepositories.TabIndex = 3;
            this.btnTestRepositories.Text = "Test Repositories";
            this.btnTestRepositories.UseVisualStyleBackColor = true;
            this.btnTestRepositories.Click += new System.EventHandler(this.btnTestRepositories_Click);

            // txtResults
            this.txtResults.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtResults.Location = new System.Drawing.Point(30, 130);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResults.Size = new System.Drawing.Size(640, 350);
            this.txtResults.TabIndex = 4;

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(570, 500);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // frmDatabaseTest
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 560);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnMigrateData);
            this.Controls.Add(this.btnTestRepositories);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmDatabaseTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Migration Tool";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}