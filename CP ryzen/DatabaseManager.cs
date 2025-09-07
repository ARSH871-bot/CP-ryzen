using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShippingManagementSystem
{
    /// <summary>
    /// Centralized database management using MySQL with XAMPP
    /// Professional database integration for the shipping management system
    /// </summary>
    public class DatabaseManager
    {
        private readonly string connectionString;
        private readonly string databaseName = "shipping_management";

        public DatabaseManager()
        {
            // XAMPP default MySQL connection (adjust if your XAMPP uses different settings)
            connectionString = $"Server=localhost;Port=3306;Database={databaseName};Uid=root;Pwd=;";

            InitializeDatabase();
        }

        /// <summary>
        /// Initialize database schema
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                // First, create database if it doesn't exist
                CreateDatabaseIfNotExists();

                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create Users table
                    string createUsersTable = @"
                        CREATE TABLE IF NOT EXISTS Users (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Username VARCHAR(50) UNIQUE NOT NULL,
                            PasswordHash VARCHAR(255) NOT NULL,
                            Email VARCHAR(100),
                            Phone VARCHAR(20),
                            Company VARCHAR(100),
                            Role VARCHAR(20) DEFAULT 'Employee',
                            CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            LastLogin DATETIME NULL,
                            IsActive BOOLEAN DEFAULT TRUE,
                            FailedLoginAttempts INT DEFAULT 0,
                            LastFailedLogin DATETIME NULL,
                            AccountLockedUntil DATETIME NULL,
                            INDEX idx_username (Username),
                            INDEX idx_email (Email),
                            INDEX idx_locked (AccountLockedUntil),
                            INDEX idx_failed_attempts (FailedLoginAttempts)
                        ) ENGINE=InnoDB;";

                    // Create Shipments table
                    string createShipmentsTable = @"
                        CREATE TABLE IF NOT EXISTS Shipments (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            Description VARCHAR(255) NOT NULL,
                            Status VARCHAR(50) NOT NULL,
                            Destination VARCHAR(255) NOT NULL,
                            Origin VARCHAR(255),
                            DateShipped DATETIME NOT NULL,
                            EstimatedArrival DATETIME NOT NULL,
                            ActualArrival DATETIME NULL,
                            AssignedUserId INT,
                            CreatedBy INT,
                            CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            ModifiedDate DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                            TrackingNumber VARCHAR(50) UNIQUE,
                            Weight DECIMAL(10,2),
                            Dimensions VARCHAR(50),
                            ShippingCost DECIMAL(10,2),
                            Notes TEXT,
                            INDEX idx_status (Status),
                            INDEX idx_tracking (TrackingNumber),
                            INDEX idx_date_shipped (DateShipped),
                            INDEX idx_assigned_user (AssignedUserId),
                            FOREIGN KEY (AssignedUserId) REFERENCES Users(Id) ON DELETE SET NULL,
                            FOREIGN KEY (CreatedBy) REFERENCES Users(Id) ON DELETE SET NULL
                        ) ENGINE=InnoDB;";

                    // Create Notifications table
                    string createNotificationsTable = @"
                        CREATE TABLE IF NOT EXISTS Notifications (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            UserId INT NOT NULL,
                            Title VARCHAR(255) NOT NULL,
                            Message TEXT NOT NULL,
                            Type VARCHAR(50) DEFAULT 'Info',
                            IsRead BOOLEAN DEFAULT FALSE,
                            CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            ReadDate DATETIME NULL,
                            RelatedShipmentId INT,
                            INDEX idx_user_read (UserId, IsRead),
                            INDEX idx_created_date (CreatedDate),
                            FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
                            FOREIGN KEY (RelatedShipmentId) REFERENCES Shipments(Id) ON DELETE SET NULL
                        ) ENGINE=InnoDB;";

                    // Create Reports table for tracking generated reports
                    string createReportsTable = @"
                        CREATE TABLE IF NOT EXISTS Reports (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            ReportType VARCHAR(50) NOT NULL,
                            GeneratedBy INT NOT NULL,
                            GeneratedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            Parameters TEXT,
                            FilePath VARCHAR(500),
                            INDEX idx_report_type (ReportType),
                            INDEX idx_generated_date (GeneratedDate),
                            FOREIGN KEY (GeneratedBy) REFERENCES Users(Id) ON DELETE CASCADE
                        ) ENGINE=InnoDB;";

                    // Create UserSessions table for session management
                    string createUserSessionsTable = @"
                        CREATE TABLE IF NOT EXISTS UserSessions (
                            Id INT AUTO_INCREMENT PRIMARY KEY,
                            UserId INT NOT NULL,
                            SessionToken VARCHAR(255) NOT NULL,
                            CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                            ExpiryDate DATETIME NOT NULL,
                            IsActive BOOLEAN DEFAULT TRUE,
                            LastActivity DATETIME DEFAULT CURRENT_TIMESTAMP,
                            IPAddress VARCHAR(45),
                            INDEX idx_session_token (SessionToken),
                            INDEX idx_user_active (UserId, IsActive),
                            FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                        ) ENGINE=InnoDB;";

                    // Execute table creation
                    ExecuteNonQuery(connection, createUsersTable);
                    ExecuteNonQuery(connection, createShipmentsTable);
                    ExecuteNonQuery(connection, createNotificationsTable);
                    ExecuteNonQuery(connection, createReportsTable);
                    ExecuteNonQuery(connection, createUserSessionsTable);

                    // Create default admin user if no users exist
                    CreateDefaultAdminUser(connection);

                    ErrorHandler.LogInfo("MySQL database initialized successfully", "DatabaseManager");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database Initialization", false);
                throw new InvalidOperationException("Failed to initialize MySQL database. Please ensure XAMPP MySQL is running.", ex);
            }
        }

        /// <summary>
        /// Create database if it doesn't exist
        /// </summary>
        private void CreateDatabaseIfNotExists()
        {
            try
            {
                string createDbConnectionString = "Server=localhost;Port=3306;Uid=root;Pwd=;";
                using (var connection = new MySqlConnection(createDbConnectionString))
                {
                    connection.Open();
                    string createDbSql = $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                    using (var command = new MySqlCommand(createDbSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not create database. Please check XAMPP MySQL configuration.", ex);
            }
        }

        /// <summary>
        /// Create default admin user for initial system access
        /// </summary>
        private void CreateDefaultAdminUser(MySqlConnection connection)
        {
            try
            {
                // Check if any users exist
                string checkSql = "SELECT COUNT(*) FROM Users";
                using (var command = new MySqlCommand(checkSql, connection))
                {
                    object result = command.ExecuteScalar();
                    if (Convert.ToInt32(result) == 0)
                    {
                        // Create default admin user with secure password
                        string hashedPassword = SecurityManager.HashPassword("admin123");
                        string insertSql = @"
                            INSERT INTO Users (Username, PasswordHash, Email, Company, Role, CreatedDate)
                            VALUES (@username, @passwordHash, @email, @company, @role, @createdDate)";

                        using (var insertCommand = new MySqlCommand(insertSql, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@username", "admin");
                            insertCommand.Parameters.AddWithValue("@passwordHash", hashedPassword);
                            insertCommand.Parameters.AddWithValue("@email", "admin@shippingmanagement.com");
                            insertCommand.Parameters.AddWithValue("@company", "Ryzen Shipping Management");
                            insertCommand.Parameters.AddWithValue("@role", "Admin");
                            insertCommand.Parameters.AddWithValue("@createdDate", DateTime.Now);

                            insertCommand.ExecuteNonQuery();
                        }

                        ErrorHandler.LogInfo("Default admin user created: admin/admin123", "DatabaseManager");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Create Default Admin User", false);
            }
        }

        /// <summary>
        /// Execute non-query SQL command
        /// </summary>
        private void ExecuteNonQuery(MySqlConnection connection, string sql)
        {
            using (var command = new MySqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get database connection
        /// </summary>
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Execute SQL command and return number of affected rows
        /// </summary>
        public int ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database ExecuteNonQuery");
                return -1;
            }
        }

        /// <summary>
        /// Execute SQL query and return DataTable
        /// </summary>
        public DataTable ExecuteQuery(string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }

                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            var dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database ExecuteQuery");
                return new DataTable();
            }
        }

        /// <summary>
        /// Execute SQL scalar query and return single value
        /// </summary>
        public object ExecuteScalar(string sql, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                            }
                        }
                        return command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database ExecuteScalar");
                return null;
            }
        }

        /// <summary>
        /// Test database connection
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get database statistics
        /// </summary>
        public Dictionary<string, object> GetDatabaseStats()
        {
            try
            {
                var stats = new Dictionary<string, object>();

                // Get table row counts
                string[] tables = { "Users", "Shipments", "Notifications", "Reports" };
                foreach (string table in tables)
                {
                    string sql = $"SELECT COUNT(*) FROM {table}";
                    object count = ExecuteScalar(sql);
                    stats[table] = count ?? 0;
                }

                // Get database size
                string sizeSql = @"
                    SELECT ROUND(SUM(data_length + index_length) / 1024 / 1024, 2) AS 'SizeMB'
                    FROM information_schema.tables 
                    WHERE table_schema = @databaseName";

                var parameters = new Dictionary<string, object> { { "@databaseName", databaseName } };
                object size = ExecuteScalar(sizeSql, parameters);
                stats["DatabaseSizeMB"] = size ?? 0;

                return stats;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Get Database Stats", false);
                return new Dictionary<string, object>();
            }
        }

        /// <summary>
        /// Backup database using mysqldump (requires mysqldump.exe in PATH)
        /// </summary>
        public bool BackupDatabase(string backupPath)
        {
            try
            {
                string backupFileName = $"shipping_backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                string fullBackupPath = System.IO.Path.Combine(backupPath, backupFileName);

                // Use mysqldump to create backup
                var processInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "mysqldump",
                    Arguments = $"--host=localhost --port=3306 --user=root --password= --databases {databaseName}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (var process = System.Diagnostics.Process.Start(processInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        System.IO.File.WriteAllText(fullBackupPath, output);
                        ErrorHandler.LogInfo($"Database backed up to: {fullBackupPath}", "DatabaseManager");
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database Backup", false);
                return false;
            }
        }

        /// <summary>
        /// Optimize database tables
        /// </summary>
        public bool OptimizeDatabase()
        {
            try
            {
                string[] tables = { "Users", "Shipments", "Notifications", "Reports", "UserSessions" };
                foreach (string table in tables)
                {
                    string sql = $"OPTIMIZE TABLE {table}";
                    ExecuteNonQuery(sql);
                }

                ErrorHandler.LogInfo("Database optimized successfully", "DatabaseManager");
                return true;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Database Optimization", false);
                return false;
            }
        }

        /// <summary>
        /// Clean up old sessions and expired data
        /// </summary>
        public void CleanupExpiredData()
        {
            try
            {
                // Remove expired sessions
                string cleanupSessionsSql = "DELETE FROM UserSessions WHERE ExpiryDate < NOW()";
                ExecuteNonQuery(cleanupSessionsSql);

                // Remove old notifications (older than 30 days)
                string cleanupNotificationsSql = "DELETE FROM Notifications WHERE CreatedDate < DATE_SUB(NOW(), INTERVAL 30 DAY) AND IsRead = TRUE";
                ExecuteNonQuery(cleanupNotificationsSql);

                ErrorHandler.LogInfo("Expired data cleaned up successfully", "DatabaseManager");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleException(ex, "Cleanup Expired Data", false);
            }
        }
    }
}