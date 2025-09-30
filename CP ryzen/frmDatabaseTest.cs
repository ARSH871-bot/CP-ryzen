using System;
using System.Windows.Forms;
using ShippingManagementSystem.Repositories;

namespace ShippingManagementSystem
{
    public partial class frmDatabaseTest : Form
    {
        public frmDatabaseTest()
        {
            InitializeComponent();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            txtResults.Clear();
            txtResults.AppendText("Testing database connection...\n");

            if (DatabaseConnection.TestConnection())
            {
                txtResults.AppendText("✓ Connection successful!\n");
                txtResults.AppendText("Database: ryzen_shipping\n");
                btnMigrateData.Enabled = true;
                btnTestRepositories.Enabled = true;
            }
            else
            {
                txtResults.AppendText("✗ Connection failed!\n");
                txtResults.AppendText("Check:\n");
                txtResults.AppendText("- XAMPP MySQL is running\n");
                txtResults.AppendText("- Database 'ryzen_shipping' exists\n");
                txtResults.AppendText("- App.config settings are correct\n");
            }
        }

        private void btnMigrateData_Click(object sender, EventArgs e)
        {
            txtResults.AppendText("\n--- Starting Migration ---\n");

            var migration = new DataMigration();
            if (migration.MigrateUsersFromJson())
            {
                txtResults.AppendText("✓ User migration completed\n");
            }
            else
            {
                txtResults.AppendText("✗ Migration failed\n");
            }
        }

        private void btnTestRepositories_Click(object sender, EventArgs e)
        {
            txtResults.AppendText("\n--- Testing Repositories ---\n");

            var userRepo = new UserRepository();

            // Test GetAll
            var users = userRepo.GetAll();
            txtResults.AppendText($"Found {users.Count} users in database\n");

            foreach (var user in users)
            {
                txtResults.AppendText($"- {user.Username} ({user.Role})\n");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}