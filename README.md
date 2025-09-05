# Ryzen Shipping Management System

[![Version](https://img.shields.io/badge/version-v1.0.0-blue.svg)](https://github.com/ARSH871-bot/CP-ryzen/releases)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![.NET Framework](https://img.shields.io/badge/.NET%20Framework-4.7.2-purple.svg)](https://dotnet.microsoft.com/download/dotnet-framework)

A comprehensive Windows Forms application for managing shipping operations with advanced security, error handling, and professional UI/UX features.

## 🚀 Features

### Core Functionality
- **User Authentication & Authorization**
  - Secure password hashing with BCrypt
  - Input validation and sanitization
  - Role-based access control
  - Two-factor authentication support

- **Shipment Management**
  - Complete CRUD operations for shipments
  - Advanced filtering and search capabilities
  - Role-based shipment visibility
  - Real-time status updates

- **Package Tracking**
  - Multi-scenario tracking simulation
  - Progress indicators and status updates
  - Historical tracking data
  - Export tracking details

- **Reporting System**
  - Multiple report types (Daily, Weekly, Monthly, Custom)
  - Data export (CSV, Text formats)
  - Visual data representation
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
  - JSON-based data persistence
  - Automatic backup creation
  - Data validation and integrity checks
  - Memory leak prevention

## 📋 System Requirements

- **Operating System**: Windows 7 SP1 or later
- **Framework**: .NET Framework 4.7.2 or later
- **Memory**: 512 MB RAM minimum, 1 GB recommended
- **Storage**: 100 MB available space
- **Display**: 1024x768 minimum resolution

## 🛠 Installation

### Prerequisites
1. Install [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework) or later
2. Ensure Windows Defender or antivirus allows the application

### Download & Install
1. Download the latest release from [Releases](https://github.com/ARSH871-bot/CP-ryzen/releases)
2. Extract the ZIP file to your desired location
3. Run `CP ryzen.exe` to start the application

### Development Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/ARSH871-bot/CP-ryzen.git
   ```
2. Open `CP ryzen.sln` in Visual Studio 2019 or later
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Build the solution:
   ```bash
   dotnet build
   ```

## 🎯 Quick Start

### First Time Setup
1. **Launch the application**
2. **Create an account** by clicking "Create an Account"
3. **Fill in your details** with a secure password (minimum 6 characters)
4. **Login** with your new credentials

### Basic Workflow
1. **Dashboard**: Navigate through different modules via sidebar
2. **Manage Shipments**: Add, edit, delete, and view shipment details
3. **Tracking**: Enter tracking numbers to view package status
4. **Reports**: Generate and export various business reports
5. **Notifications**: Stay updated with system notifications

## 📖 User Guide

### Authentication
- **Username**: 3-20 characters, alphanumeric and underscores only
- **Password**: Minimum 6 characters, recommendations for 8+ characters
- **Email**: Valid email format required for notifications

### Shipment Management
- **Add Shipment**: Create new shipment entries with validation
- **Edit Shipment**: Modify existing shipment details
- **Filter**: View shipments by role or status
- **Export**: Save shipment data for external use

### Tracking System
- **Sample Tracking Numbers**:
  - `1234567890` - In Transit package
  - `0987654321` - Delivered package  
  - `TEST123456` - Processing package

### Reports
- **Report Types**: Daily, Weekly, Monthly, Quarterly, Annual, Custom
- **Export Formats**: CSV, Text files
- **Date Ranges**: Flexible date selection for custom reports

## 🔧 Configuration

### Logging
Logs are automatically created in the `Logs/` directory:
- `application_log.txt` - Main application log
- Logs include INFO, WARNING, and ERROR levels

### Database
- Data stored in `Database/customer/` directory
- JSON format for easy backup and portability
- Automatic directory creation on first run

### Themes
- **Light Theme**: Default professional appearance
- **Dark Theme**: Reduced eye strain for extended use
- **Blue Theme**: Alternative color scheme

## 🚨 Troubleshooting

### Common Issues

**Application won't start**
- Ensure .NET Framework 4.7.2 is installed
- Check Windows Event Viewer for detailed error information
- Run as Administrator if permission errors occur

**Login issues**
- Verify username format (3-20 characters, alphanumeric + underscore)
- Check password length (minimum 6 characters)
- Clear application data and recreate account if needed

**Performance issues**
- Close other resource-intensive applications
- Check available disk space (minimum 100 MB free)
- Restart application to clear memory usage

## 📈 Version History

### v1.0.0 (Current) - Production Release
- ✅ Complete shipping management system
- ✅ Secure authentication with BCrypt password hashing
- ✅ Comprehensive error handling and logging
- ✅ Professional UI with theme support
- ✅ Advanced tracking and reporting features
- ✅ Export functionality for all major data types

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/amazing-feature`
3. **Commit changes**: `git commit -m 'Add amazing feature'`
4. **Push to branch**: `git push origin feature/amazing-feature`
5. **Open a Pull Request**

### Development Standards
- Follow C# coding conventions
- Include comprehensive error handling
- Add appropriate comments and documentation
- Test thoroughly before submitting

## 📞 Support

- **Technical Issues**: Open an issue on [GitHub Issues](https://github.com/ARSH871-bot/CP-ryzen/issues)
- **Feature Requests**: Use the feature request template
- **Security Concerns**: Contact maintainers directly

## 👥 Author & Contributor

- **Arsh**  vhoraarsh91@gmail.com, vhoraarsh87@gmail.com - Lead Developer, UI/UX Developer, Quality Assurance

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- BCrypt.Net-Next for secure password hashing
- Microsoft .NET Framework team
- All contributors and testers

---

**Made with ❤️ by the Ryzen Development Team**

*For business inquiries and enterprise licensing, please contact the development team.*
