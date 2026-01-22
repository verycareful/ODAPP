# ODapp - On-Duty Application Management System

A cross-platform .NET MAUI application for managing On-Duty (OD) requests and tracking student attendance. Built with modern technologies including .NET 9, MAUI, and Supabase for backend services.

![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0-blue)
![Supabase](https://img.shields.io/badge/Supabase-Backend-green)
![Platform](https://img.shields.io/badge/Platform-Windows%20|%20Android%20|%20iOS%20|%20macOS-purple)

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Usage](#usage)
- [API Reference](#api-reference)
- [Database Schema](#database-schema)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

ODapp is a comprehensive On-Duty management system designed for educational institutions. It allows students to submit OD requests for events, activities, or other authorized absences, while providing administrators with tools to manage and track these requests efficiently.

### Key Benefits

- **Cross-Platform**: Runs on Windows, Android, iOS, and macOS from a single codebase
- **Cloud-Based**: Leverages Supabase for secure, scalable backend services
- **Real-Time**: Instant updates and synchronization across devices
- **Secure**: Authentication and authorization built-in

## ✨ Features

### Core Features

| Feature | Description |
|---------|-------------|
| **User Authentication** | Secure login/logout with Supabase Auth |
| **OD Request Submission** | Students can submit OD details with date range and reason |
| **Namelist Management** | View and manage student namelists |
| **OD Details Tracking** | Track and view all OD requests |
| **Session Persistence** | User sessions are maintained across app restarts |

### User Workflow

1. **Login**: Users authenticate with their email and password
2. **View Namelist**: Browse the list of registered students
3. **Submit OD Request**: Fill in OD details including:
   - Register Number
   - Date of OD
   - From/To period range
   - Reason for OD
4. **View OD Details**: Track submitted OD requests

## 🏗️ Architecture

### Technology Stack

```
┌─────────────────────────────────────────────────────────────┐
│                    ODapp (.NET MAUI)                        │
├─────────────────────────────────────────────────────────────┤
│  UI Layer (XAML)                                            │
│  ├── MainPage.xaml        - Home/Dashboard                  │
│  ├── LoginPage.xaml       - User Authentication             │
│  ├── NamelistPage.xaml    - Student List View               │
│  ├── ODDetailsPage.xaml   - OD Records View                 │
│  └── ODDetailsFormPage.xaml - OD Submission Form            │
├─────────────────────────────────────────────────────────────┤
│  Service Layer (C#)                                         │
│  ├── SupabaseService.cs   - Supabase API Integration        │
│  └── SupabaseAuth.cs      - Authentication Services         │
├─────────────────────────────────────────────────────────────┤
│  Infrastructure                                             │
│  ├── .NET 9.0                                               │
│  ├── .NET MAUI                                              │
│  └── Supabase SDK                                           │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    Supabase Backend                         │
├─────────────────────────────────────────────────────────────┤
│  ├── PostgreSQL Database                                    │
│  ├── Authentication (GoTrue)                                │
│  └── REST API (PostgREST)                                   │
└─────────────────────────────────────────────────────────────┘
```

### Design Patterns

- **Dependency Injection**: Services are injected via constructor
- **MVVM-Inspired**: Data binding between XAML views and code-behind
- **Interface Segregation**: `ISupabaseService` defines service contracts

## 📦 Prerequisites

### Development Requirements

- **IDE**: Visual Studio 2022 (17.8+) or VS Code with C# DevKit
- **.NET SDK**: 9.0 or later
- **.NET MAUI Workload**: Install via Visual Studio Installer or CLI
- **Windows SDK**: 10.0.19041.0 or later (for Windows target)

### Platform-Specific Requirements

| Platform | Requirements |
|----------|-------------|
| Windows | Windows 10 version 1809 (17763) or later |
| Android | Android 5.0 (API 21) or later, Android SDK |
| iOS | macOS with Xcode 15+, iOS 11.0+ |
| macOS | macOS 11.0 or later |

## 🚀 Installation

### 1. Clone the Repository

```bash
git clone https://github.com/verycareful/ODAPP.git
cd ODAPP
```

### 2. Install .NET MAUI Workload

```bash
dotnet workload install maui
```

### 3. Restore NuGet Packages

```bash
dotnet restore
```

### 4. Build the Application

```bash
# For Windows
dotnet build -f net9.0-windows10.0.19041.0

# For Android
dotnet build -f net9.0-android

# For iOS (macOS only)
dotnet build -f net9.0-ios

# For macOS
dotnet build -f net9.0-maccatalyst
```

### 5. Run the Application

```bash
# Windows
dotnet run -f net9.0-windows10.0.19041.0

# Android (emulator or device)
dotnet run -f net9.0-android

# iOS Simulator (macOS only)
dotnet run -f net9.0-ios
```

## ⚙️ Configuration

### Supabase Configuration

The application connects to a Supabase backend. The connection details are configured in `SupabaseService.cs`:

```csharp
private const string SupabaseUrl = "https://your-project.supabase.co";
private const string SupabaseKey = "your-anon-key";
```

### Environment Configuration

Create an `appsettings.json` file for environment-specific settings:

```json
{
  "Supabase": {
    "Url": "https://your-project.supabase.co",
    "Key": "your-anon-key"
  },
  "Logging": {
    "LogLevel": "Information"
  }
}
```

### Secure Credential Storage

For production, use secure storage methods:

```csharp
// Access token storage
Preferences.Set("access_token", session.AccessToken);
Preferences.Set("refresh_token", session.RefreshToken);
```

## 📁 Project Structure

```
ODapp/
├── 📄 App.xaml                    # Application resources and styles
├── 📄 App.xaml.cs                 # Application entry point
├── 📄 AppShell.xaml               # Shell navigation structure
├── 📄 AppShell.xaml.cs            # Shell code-behind
├── 📄 MauiProgram.cs              # DI container and service registration
│
├── 📄 MainPage.xaml               # Main dashboard view
├── 📄 MainPage.xaml.cs            # Main page logic
├── 📄 LoginPage.xaml              # Login form view
├── 📄 LoginPage.xaml.cs           # Login logic
├── 📄 NamelistPage.xaml           # Student list view
├── 📄 NamelistPage.xaml.cs        # Namelist logic
├── 📄 ODDetailsPage.xaml          # OD records view
├── 📄 ODDetailsPage.xaml.cs       # OD details logic
├── 📄 ODDetailsFormPage.xaml      # OD submission form
├── 📄 ODDetailsFormPage.xaml.cs   # Form submission logic
│
├── 📄 SupabaseService.cs          # Supabase API service
├── 📄 SupabaseAuth.cs             # Authentication utilities
│
├── 📄 appsettings.json            # Application configuration
├── 📄 appxmanifest.xml            # Windows app manifest
├── 📄 ODapp.csproj                # Project file
├── 📄 ODapp.sln                   # Solution file
│
├── 📁 Platforms/                  # Platform-specific code
│   ├── 📁 Android/
│   ├── 📁 iOS/
│   ├── 📁 MacCatalyst/
│   ├── 📁 Tizen/
│   └── 📁 Windows/
│
├── 📁 Properties/
│   └── 📄 launchSettings.json     # Debug launch settings
│
└── 📁 Resources/
    ├── 📁 AppIcon/                # Application icons
    ├── 📁 Fonts/                  # Custom fonts
    ├── 📁 Images/                 # Image assets
    ├── 📁 Raw/                    # Raw assets
    ├── 📁 Splash/                 # Splash screen assets
    └── 📁 Styles/                 # XAML style resources
```

## 📖 Usage

### User Authentication

```csharp
// Login
var session = await _supabaseService.Login(email, password);

// Check login status
bool isLoggedIn = _supabaseService.IsLoggedIn();

// Get current user
string email = _supabaseService.GetCurrentUserEmail();

// Logout
await _supabaseService.Logout();
```

### Fetching Data

```csharp
// Get namelist
var namelist = await _supabaseService.GetNamelist();

// Get OD details
var odDetails = await _supabaseService.GetODDetails();
```

### Submitting OD Request

```csharp
var odDetail = new ODDetails
{
    RegisterNumber = "2021001",
    Date = DateTime.Today,
    From = 1,      // Starting period
    To = 4,        // Ending period
    Reason = "College event participation"
};

bool success = await _supabaseService.SubmitODDetail(odDetail);
```

## 🔌 API Reference

### ISupabaseService Interface

| Method | Returns | Description |
|--------|---------|-------------|
| `Login(email, password)` | `Task<Session>` | Authenticate user |
| `Logout()` | `Task<bool>` | Sign out user |
| `GetNamelist()` | `Task<ObservableCollection<Namelist>>` | Fetch all students |
| `GetODDetails()` | `Task<ObservableCollection<ODDetails>>` | Fetch all OD records |
| `SubmitODDetail(odDetail)` | `Task<bool>` | Submit new OD request |
| `IsLoggedIn()` | `bool` | Check authentication status |
| `GetCurrentUserEmail()` | `string` | Get logged-in user's email |

### Data Models

#### Namelist

```csharp
public class Namelist
{
    public string RegisterNumber { get; set; }
    public string Name { get; set; }
    // Additional properties...
}
```

#### ODDetails

```csharp
public class ODDetails
{
    public string RegisterNumber { get; set; }
    public DateTime Date { get; set; }
    public short From { get; set; }
    public short To { get; set; }
    public string Reason { get; set; }
}
```

## 🗄️ Database Schema

### Supabase Tables

#### `Namelist` Table

| Column | Type | Description |
|--------|------|-------------|
| `RegisterNumber` | TEXT | Primary key, student ID |
| `Name` | TEXT | Student name |

#### `ODDetails` Table

| Column | Type | Description |
|--------|------|-------------|
| `id` | UUID | Primary key, auto-generated |
| `RegisterNumber` | TEXT | Foreign key to Namelist |
| `Date` | DATE | Date of OD |
| `From` | SMALLINT | Starting period (1-8) |
| `To` | SMALLINT | Ending period (1-8) |
| `Reason` | TEXT | Reason for OD |
| `created_at` | TIMESTAMP | Auto-generated timestamp |

## 🤝 Contributing

We welcome contributions! Please follow these steps:

### Getting Started

1. **Fork** the repository
2. **Clone** your fork:
   ```bash
   git clone https://github.com/your-username/ODAPP.git
   ```
3. **Create** a feature branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
4. **Make** your changes
5. **Commit** with clear messages:
   ```bash
   git commit -m "feat: add new feature description"
   ```
6. **Push** to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```
7. **Open** a Pull Request

### Commit Message Convention

Follow conventional commits:
- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `style:` Code style changes
- `refactor:` Code refactoring
- `test:` Adding tests
- `chore:` Maintenance tasks

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods focused and concise

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 📞 Support

If you encounter any issues or have questions:

1. Check the [Issues](https://github.com/verycareful/ODAPP/issues) page
2. Create a new issue with detailed information
3. Include steps to reproduce any bugs

---

**Made with ❤️ using .NET MAUI**
