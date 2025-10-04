# Ryzen Shipping Management System

[![Version](https://img.shields.io/badge/version-v1.2.0-blue.svg)](https://github.com/ARSH871-bot/CP-ryzen/releases)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-purple.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Database](https://img.shields.io/badge/Database-MySQL-orange.svg)](https://www.mysql.com/)

A comprehensive Windows Forms application for managing shipping operations with MySQL database, advanced security, error handling, and professional UI/UX features.

## 🚀 Features

### Database Integration (NEW in v1.2.0)
- **MySQL Database**
  - Full MySQL implementation via XAMPP
  - Auto-creates schema on first run
  - Centralized data management
  - Improved data integrity and relationships
  - Connection pooling and optimization

### Core Functionality
- **User Authentication & Authorization**
  - Secure password hashing with SHA256
  - Input validation and sanitization
  - Role-based access control
  - Account lockout after failed attempts
  - Session management

- **Shipment Management**
  - Complete CRUD operations with database
  - Advanced filtering and search capabilities
  - Role-based shipment visibility
  - Real-time status updates from database

- **Package Tracking**
  - Live database tracking queries
  - Progress indicators and status updates
  - Historical tracking data
  - Export tracking details

- **Reporting System**
  - Multiple report types (Daily, Weekly, Monthly, Custom)
  - Database-driven reports
  - Data export (CSV, Text formats)
  - Print functionality

- **Notifications Center**
  - Priority-based notification system
  - Read/unread status management
  - Advanced filtering and search
  - Export capabilities

### Professional Features
- **Error Handling**
  - Centralized error management system
  - User-friendly error messages
  - Comprehensive logging
  - Graceful error recovery

- **UI/UX Enhancements**
  - Dark/Light theme support
  - Responsive design patterns
  - Loading states and progress indicators
  - Professional styling and typography

- **Data Management**
  - MySQL database with XAMPP
  - Automatic backup capabilities
  - Data validation and integrity checks
  - Memory leak prevention

## 📋 System Requirements

- **Operating System**: Windows 7 SP1 or later
- **Framework**: .NET Framework 4.7.2 or later
- **Database**: XAMPP MySQL (localhost:3306)
- **Memory**: 512 MB RAM minimum, 1 GB recommended
- **Storage**: 200 MB available space
- **Display**: 1024x768 minimum resolution

## 🛠 Installation

### Prerequisites

1. **Install .NET Framework 4.7.2+**
   - Download from [Microsoft .NET](https://dotnet.microsoft.com/download/dotnet-framework)

2. **Install XAMPP**
   - Download from [Apache Friends](https://www.apachefriends.org)
   - Install MySQL component
   - Start XAMPP Control Panel
   - Start MySQL service (default port 3306)

### First Time Setup

1. **Download Release**
   ```
   Download latest release from GitHub Releases
   Extract ZIP to desired location
   ```

2. **Start XAMPP MySQL**
   ```
   Open XAMPP Control Panel
   Click "Start" next to MySQL
   Verify MySQL is running (green indicator)
   ```

3. **Run Application**
   ```
   Run CP ryzen.exe
   Database will auto-create on first launch
   ```

4. **Default Admin Login**
   - Username: `admin`
   - Password: `admin123`
   - **⚠️ Change password immediately after first login**

### Database Configuration

**Default Settings:**
- Host: `localhost`
- Port: `3306`
- Database: `shipping_management`
- User: `root`
- Password: (blank)

**To modify:** Edit connection string in `DatabaseManager.cs`

### Database Schema

The application automatically creates these tables:

| Table | Description |
|-------|-------------|
| **Users** | User accounts and authentication |
| **Shipments** | Shipment records and tracking |
| **Notifications** | System notifications |
| **Reports** | Generated report metadata |
| **UserSessions** | Active user sessions |

## 🎯 Quick Start

### First Time User

1. Launch application
2. Click "Create an Account"
3. Fill in registration details
4. Login with new credentials
5. Navigate using sidebar menu

### Basic Workflow

1. **Dashboard** → Navigate through modules
2. **Manage Shipments** → Add, edit, delete shipments
3. **Tracking** → Track packages by number
4. **Reports** → Generate business reports
5. **Notifications** → View system updates

## 📖 User Guide

### Database Access

**phpMyAdmin (Recommended):**
```
1. Open browser: http://localhost/phpmyadmin
2. Login with root (no password by default)
3. Select 'shipping_management' database
4. View/edit tables, run SQL queries
5. Export/backup database
```

### Authentication

- **Username**: 3-20 characters, alphanumeric + underscore
- **Password**: Minimum 6 characters
  - At least one uppercase letter
  - At least one lowercase letter
  - At least one number
- **Email**: Valid format for notifications

### Shipment Management

**Operations:**
- **Add**: Create new shipment with validation
- **Edit**: Modify existing shipment details
- **Delete**: Remove shipment (with confirmation)
- **Filter**: View by role or status
- **Export**: Save data for external use

**Tracking Numbers:**
- Auto-generated: `RYZ{timestamp}{random}`
- Unique across all shipments
- Used for tracking queries

### Database Tracking

Enter any tracking number from the database to see:
- Real-time shipment status
- Location information
- Estimated delivery dates
- Complete shipment history

### Reports

**Report Types:**
- Daily, Weekly, Monthly
- Quarterly, Annual
- Custom date range

**Export Formats:**
- CSV (Excel compatible)
- Text files
- Print-ready format

## 🔧 Configuration

### Database Backup

**Manual Backup via phpMyAdmin:**
```
1. Open phpMyAdmin
2. Select 'shipping_management'
3. Click 'Export' tab
4. Choose 'Quick' export
5. Click 'Go'
```

**Restore Backup:**
```
1. Open phpMyAdmin
2. Select database
3. Click 'Import' tab
4. Choose backup file
5. Click 'Go'
```

### Logging

Logs automatically created in `Logs/` directory:
- `application_log.txt` - Main log
- `email_log.txt` - Email operations
- `error_log_YYYYMMDD.txt` - Daily error logs

Log levels: INFO, WARNING, ERROR

### Themes

- **Light Theme**: Default professional appearance
- **Dark Theme**: Reduced eye strain
- **Blue Theme**: Alternative color scheme

Access: Settings → Theme Selection

## 🚨 Troubleshooting

### Application Won't Start

**Error: Database connection failed**
```
1. Ensure XAMPP MySQL is running
2. Check port 3306 is not blocked
3. Verify firewall settings
4. Check Windows Event Viewer for details
```

**Error: .NET Framework missing**
```
1. Install .NET Framework 4.7.2+
2. Restart computer
3. Run application as Administrator
```

### Login Issues

**Forgot password:**
```
1. Open phpMyAdmin
2. Navigate to: shipping_management → Users
3. Find your user record
4. Update PasswordHash with new hash
   (Use SecurityManager.HashPassword in code)
```

**Account locked:**
```
Wait 30 minutes, or:
1. Open phpMyAdmin
2. Update Users table
3. Set FailedLoginAttempts = 0
4. Set AccountLockedUntil = NULL
```

### Database Issues

**Tables missing:**
```
1. Delete shipping_management database
2. Restart application
3. Tables will auto-create
```

**Data corruption:**
```
1. Restore from backup
2. Or use phpMyAdmin to repair tables
```

### Performance Issues

**Slow queries:**
```
1. Optimize database tables (see DatabaseManager.cs)
2. Close unused connections
3. Clear old logs and notifications
```

## 📈 Version History

### v1.2.0 (Current) - Database Migration
- ✅ Full MySQL database implementation
- ✅ Removed JSON file storage
- ✅ Enhanced security and performance
- ✅ Auto-create database schema
- ✅ Improved data integrity
- ✅ Session management
- ✅ Advanced error handling

### v1.1.0 - Email Notifications
- ✅ Email notification system
- ✅ Welcome emails
- ✅ Shipment status updates
- ✅ Delivery confirmations

### v1.0.0 - Initial Release
- ✅ Basic shipping management
- ✅ User authentication
- ✅ JSON file storage
- ✅ Tracking system
- ✅ Reporting features

## 🤝 Contributing

Contributions welcome! Please follow these guidelines:

1. Fork the repository
2. Create feature branch: `git checkout -b feature/amazing-feature`
3. Commit changes: `git commit -m 'feat: add amazing feature'`
4. Push to branch: `git push origin feature/amazing-feature`
5. Open Pull Request

### Development Standards
- Follow C# coding conventions
- Include comprehensive error handling
- Add appropriate comments
- Test thoroughly before submitting
- Update documentation

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/ARSH871-bot/CP-ryzen/issues)
- **Feature Requests**: Use feature request template
- **Security**: Contact maintainers directly

## 👥 Author & Contributors

- **Arsh** - Lead Developer, UI/UX, QA
  - Email: vhoraarsh91@gmail.com, vhoraarsh87@gmail.com

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- MySql.Data for database connectivity
- BCrypt.Net-Next for password hashing (legacy)
- Microsoft .NET Framework team
- XAMPP team for MySQL solution
- All contributors and testers

## 🔜 Roadmap

### v1.3.0 (Planned)
- [ ] Automated backup/restore
- [ ] Database migration tools
- [ ] Admin panel for DB management
- [ ] REST API endpoints
- [ ] Mobile app integration
- [ ] Advanced analytics dashboard

### v1.4.0 (Future)
- [ ] Multi-language support
- [ ] Cloud database option
- [ ] Real-time notifications
- [ ] GPS tracking integration
- [ ] Customer portal
- [ ] Automated testing suite

---

**Made with ❤️ by the Ryzen Development Team**

*For business inquiries and enterprise licensing, please contact the development team.*

## 🔗 Quick Links

- [Download Latest Release](https://github.com/ARSH871-bot/CP-ryzen/releases/latest)
- [Installation Guide](#-installation)
- [User Guide](#-user-guide)
- [Troubleshooting](#-troubleshooting)
- [API Documentation](docs/API.md) *(coming soon)*
