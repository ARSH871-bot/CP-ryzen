Ryzen Shipping Management System
Table of Contents
Project Overview
Features
File Structure
Technologies Used
Setup and Installation
Usage
Screenshots
Future Enhancements
Project Overview
The Ryzen Shipping Management System is a Windows Forms-based application designed to streamline the management of shipping operations. This system provides user-friendly interfaces for logging in, managing shipments, viewing logs, and enabling security features like two-factor authentication. It is developed using C# in Visual Studio and works without a database, relying on in-memory data storage.

Features
Login System

Secure user authentication.
In-memory storage for user credentials.
Role-based greeting messages (e.g., Admin, Manager).
Account Settings

Manage user profile details, including username, email, and bio.
Upload profile pictures.
Enable two-factor authentication for enhanced security.
View activity logs dynamically without using a database.
Shipment Management

Add, edit, and delete shipment details.
Filter shipments based on roles.
View shipment details dynamically.
Two-Factor Authentication

Implemented using a code generator and email simulator.
Does not require database storage for codes.
Error Handling

User-friendly messages for login errors and invalid inputs.
File Structure
arduino
Copy code
RyzenShippingManagement/
├── frmLogin.cs                // Login form logic
├── frmLogin.Designer.cs       // Login form UI design
├── frmAccountSettings.cs      // Account settings form logic
├── frmAccountSettings.Designer.cs // Account settings form UI design
├── frmManageShipments.cs      // Shipment management form logic
├── frmManageShipments.Designer.cs // Shipment management form UI design
├── Resources/
│   └── Logo.jpg               // Application logo
└── README.md                  // Project documentation
Technologies Used
Programming Language: C#
Framework: .NET Framework (Windows Forms)
Development Environment: Visual Studio
In-Memory Data Handling: Static Lists
UI Design: Windows Forms Designer
Setup and Installation
Pre-requisites

Windows Operating System.
Visual Studio with .NET Framework installed.
Clone the Repository

bash
Copy code
git clone <repository_url>
Open the Project

Open Visual Studio.
Select File > Open > Project/Solution.
Navigate to the project directory and open the .sln file.
Run the Application

Set frmLogin as the startup form.
Press F5 or click on the Start button to run the application.
Usage
1. Login
Enter the username and password.
Click on the "Login" button.
Default credentials:
Admin: admin / admin123
Manager: manager / manager123
2. Account Settings
Update user details like username, email, password, and bio.
Upload a profile picture.
Enable two-factor authentication for added security.
View activity logs dynamically.
3. Manage Shipments
Add, edit, or delete shipment records.
Filter shipments by role (e.g., Dispatcher, Manager).
View shipment details with dynamic updates.
Screenshots
Login Form

Account Settings

Manage Shipments

Future Enhancements
Database Integration

Replace in-memory storage with a relational database for persistent data.
Email Notifications

Enable real-time email alerts for two-factor authentication codes.
Enhanced Logs

Store logs in a file or database for retrieval and analysis.
Multi-User Support

Expand the system to support multiple concurrent users.
Shipment Tracking

Add real-time shipment tracking using APIs.
License
This project is open-source and available for personal and educational use.

Author
RYZEN

Email:
vhoraarsh87@gmail.com(Arsh)
samika.ashen@gamil.com (Samika)
harpreetsinghnz2003@gmail.com(Harpreet)

