# ODapp - On-Duty Management System
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/maui)
[![Supabase](https://img.shields.io/badge/Supabase-PostgreSQL-3ECF8E?style=flat-square&logo=supabase&logoColor=white)](https://supabase.com/)
[![License: Polyform NC](https://img.shields.io/badge/License-Polyform%20NC%201.0.0-blue.svg)](https://polyformproject.org/licenses/noncommercial/1.0.0/)
[![Status: Active](https://img.shields.io/badge/Status-Active-brightgreen?style=flat-square)](.)
A cross-platform .NET MAUI application for managing On-Duty (OD) requests and tracking student attendance records. Built with .NET 9, MAUI, and Supabase backend services.

> **Note**: This project was not pursued further due to lack of proper institutional support. The repository is now public to share the work and allow the community to benefit from it.


## Table of Contents

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


## Overview

ODapp is an On-Duty management system designed for educational institutions. Students can submit OD requests for events, activities, or other authorized absences. The application provides a simple interface to manage and track these requests.

The application currently targets Windows (net9.0-windows10.0.19041.0) but supports cross-platform deployment to Android, iOS, and macOS through .NET MAUI.


## Features

### Core Functionality

| Feature | Description |
|---------|-------------|
| User Authentication | Email/password login with Supabase Auth, session persistence using device preferences |
| Student Namelist | View student records including Register Number, Name, Admission Year, Department, Course, and Section |
| OD Request Submission | Submit OD details with Register Number, Date, From/To period, and Reason |
| OD Details View | Browse all submitted OD records with formatted display |
| Session Management | Access tokens stored locally for persistent login across app restarts |

### Application Pages

- **MainPage**: Dashboard with navigation buttons for Namelist, OD Details, Add OD, and Login/Logout
- **LoginPage**: Email and password authentication form
- **NamelistPage**: Displays student list in a three-column grid layout (Student Info, Admission Info, Course Info)
- **ODDetailsPage**: Shows OD records with student details, date, time range, and reason
- **ODDetailsFormPage**: Form for submitting new OD requests (requires authentication)


## Architecture

### Technology Stack

```
+-------------------------------------------------------------+
|                    ODapp (.NET MAUI)                        |
+-------------------------------------------------------------+
|  UI Layer (XAML + Code-Behind)                              |
|  - MainPage.xaml/cs        Dashboard and navigation         |
|  - LoginPage.xaml/cs       User authentication              |
|  - NamelistPage.xaml/cs    Student list display             |
|  - ODDetailsPage.xaml/cs   OD records display               |
|  - ODDetailsFormPage.xaml/cs  OD submission form            |
+-------------------------------------------------------------+
|  Service Layer                                              |
|  - ISupabaseService        Service interface                |
|  - SupabaseService         Supabase API implementation      |
+-------------------------------------------------------------+
|  Dependencies                                               |
|  - Microsoft.Maui.Controls                                  |
|  - Supabase (v1.1.1)                                        |
|  - Supabase.Gotrue (v6.0.3)                                 |
|  - Newtonsoft.Json                                          |
|  - Microsoft.Extensions.Configuration                       |
+-------------------------------------------------------------+
                            |
                            v
+-------------------------------------------------------------+
|                    Supabase Backend                         |
+-------------------------------------------------------------+
|  - PostgreSQL Database (Namelist, ODDetails tables)         |
|  - GoTrue Authentication                                    |
|  - PostgREST API                                            |
+-------------------------------------------------------------+
```

### Design Patterns

- **Dependency Injection**: Services registered in MauiProgram.cs and injected via constructors
- **Interface-based Design**: ISupabaseService interface defines the service contract
- **Shell Navigation**: MAUI Shell used for page routing (MainPage, LoginPage, NamelistPage, ODDetailsPage, ODDetailsFormPage)


## Prerequisites

### Development Requirements

- Visual Studio 2022 (17.8+) or VS Code with C# DevKit
- .NET SDK 9.0 or later
- .NET MAUI workload
- Windows SDK 10.0.19041.0 or later (for Windows target)

### Platform Requirements

| Platform | Minimum Version |
|----------|-----------------|
| Windows | Windows 10 version 1809 (build 17763) |
| Android | Android 5.0 (API 21) |
| iOS | iOS 11.0, requires macOS with Xcode |
| macOS | macOS 11.0 (Mac Catalyst) |


## Installation

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
# Windows (default target)
dotnet build -f net9.0-windows10.0.19041.0

# Android
dotnet build -f net9.0-android

# iOS (macOS only)
dotnet build -f net9.0-ios

# macOS
dotnet build -f net9.0-maccatalyst
```

### 5. Run the Application

```bash
# Windows
dotnet run -f net9.0-windows10.0.19041.0
```


## Configuration

### Supabase Connection

The Supabase connection is configured in `SupabaseService.cs`:

```csharp
private const string SupabaseUrl = "https://your-project.supabase.co";
private const string SupabaseKey = "your-anon-key";
```

### Session Storage

User sessions are persisted using MAUI Preferences:

```csharp
// Stored on successful login
Preferences.Set("access_token", session.AccessToken);
Preferences.Set("refresh_token", session.RefreshToken);
Preferences.Set("user_email", email);

// Cleared on logout
Preferences.Remove("access_token");
Preferences.Remove("refresh_token");
Preferences.Remove("user_email");
```


## Project Structure

```
ODapp/
|-- App.xaml                    Application resources
|-- App.xaml.cs                 Application entry point
|-- AppShell.xaml               Shell navigation structure
|-- AppShell.xaml.cs            Shell code-behind
|-- MauiProgram.cs              DI container and service registration
|
|-- MainPage.xaml               Dashboard UI
|-- MainPage.xaml.cs            Dashboard logic, Namelist/ODDetails models
|-- LoginPage.xaml              Login form UI
|-- LoginPage.xaml.cs           Login logic
|-- NamelistPage.xaml           Student list UI
|-- NamelistPage.xaml.cs        Namelist data binding
|-- ODDetailsPage.xaml          OD records UI
|-- ODDetailsPage.xaml.cs       OD details data binding
|-- ODDetailsFormPage.xaml      OD submission form UI
|-- ODDetailsFormPage.xaml.cs   Form submission logic
|
|-- SupabaseService.cs          ISupabaseService implementation
|-- SupabaseAuth.cs             Authentication utilities
|
|-- appsettings.json            Application configuration
|-- appxmanifest.xml            Windows app manifest
|-- ODapp.csproj                Project file
|-- ODapp.sln                   Solution file
|
|-- Platforms/
|   |-- Android/                Android-specific code
|   |-- iOS/                    iOS-specific code
|   |-- MacCatalyst/            macOS-specific code
|   |-- Tizen/                  Tizen-specific code
|   +-- Windows/                Windows-specific code
|
|-- Properties/
|   +-- launchSettings.json     Debug launch settings
|
+-- Resources/
    |-- AppIcon/                Application icons
    |-- Fonts/                  OpenSans fonts
    |-- Images/                 Image assets
    |-- Raw/                    Raw assets
    |-- Splash/                 Splash screen
    +-- Styles/                 Colors.xaml, Styles.xaml
```


## Usage

### Authentication

```csharp
// Login
var session = await _supabaseService.Login(email, password);

// Check if logged in
bool isLoggedIn = _supabaseService.IsLoggedIn();

// Get current user email
string email = _supabaseService.GetCurrentUserEmail();

// Logout
await _supabaseService.Logout();
```

### Fetching Data

```csharp
// Get student namelist
ObservableCollection<Namelist> students = await _supabaseService.GetNamelist();

// Get OD details
ObservableCollection<ODDetails> odRecords = await _supabaseService.GetODDetails();
```

### Submitting OD Request

```csharp
var odDetail = new ODDetails
{
    RegisterNumber = "RA2211003010001",
    Date = DateTime.Today,
    From = 1,      // Starting period
    To = 4,        // Ending period
    Reason = "College event participation"
};

bool success = await _supabaseService.SubmitODDetail(odDetail);
```


## API Reference

### ISupabaseService Interface

| Method | Return Type | Description |
|--------|-------------|-------------|
| `Login(string email, string password)` | `Task<Session>` | Authenticate user with Supabase |
| `Logout()` | `Task<bool>` | Clear session and sign out |
| `GetNamelist()` | `Task<ObservableCollection<Namelist>>` | Fetch all student records |
| `GetODDetails()` | `Task<ObservableCollection<ODDetails>>` | Fetch all OD records |
| `SubmitODDetail(ODDetails odDetail)` | `Task<bool>` | Submit new OD request |
| `IsLoggedIn()` | `bool` | Check authentication status |
| `GetCurrentUserEmail()` | `string` | Get logged-in user's email |

### Data Models

#### Namelist

```csharp
public class Namelist
{
    [JsonProperty("REGISTER NUMBER")]
    public string RegisterNumber { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("Admission Year")]
    public int AdmissionYear { get; set; }

    [JsonProperty("Department")]
    public string Department { get; set; }

    [JsonProperty("Course")]
    public string Course { get; set; }

    [JsonProperty("Section")]
    public string Section { get; set; }
}
```

#### ODDetails

```csharp
public class ODDetails
{
    [JsonProperty("ID")]
    public long ID { get; set; }

    [JsonProperty("REGISTER NUMBER")]
    public string RegisterNumber { get; set; }

    [JsonProperty("DATE")]
    public DateTime Date { get; set; }

    [JsonProperty("FROM")]
    public short From { get; set; }

    [JsonProperty("TO")]
    public short To { get; set; }

    [JsonProperty("Reason")]
    public string Reason { get; set; }
}
```


## Database Schema

### Supabase Tables

#### Namelist Table

| Column | Type | Description |
|--------|------|-------------|
| REGISTER NUMBER | TEXT | Primary key, student registration number |
| Name | TEXT | Student full name |
| Admission Year | INTEGER | Year of admission |
| Department | TEXT | Department name |
| Course | TEXT | Course name |
| Section | TEXT | Section identifier |

#### ODDetails Table

| Column | Type | Description |
|--------|------|-------------|
| ID | BIGINT | Primary key, auto-generated |
| REGISTER NUMBER | TEXT | Student registration number |
| DATE | DATE | Date of OD |
| FROM | SMALLINT | Starting period number |
| TO | SMALLINT | Ending period number |
| Reason | TEXT | Reason for OD request |


## Contributing

### Getting Started

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/your-username/ODAPP.git
   ```
3. Create a feature branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
4. Make your changes
5. Commit with clear messages:
   ```bash
   git commit -m "feat: add new feature description"
   ```
6. Push to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```
7. Open a Pull Request

### Commit Message Convention

- `feat:` New feature
- `fix:` Bug fix
- `docs:` Documentation changes
- `style:` Code formatting
- `refactor:` Code refactoring
- `test:` Adding tests
- `chore:` Maintenance tasks


## License

Copyright © 2026 Sricharan Suresh (github.com/verycareful)

This project is licensed under the **[Polyform Noncommercial License 1.0.0](https://polyformproject.org/licenses/noncommercial/1.0.0/)**.
You may use, copy, and modify this software for non-commercial purposes only.
Commercial use of any kind is prohibited without explicit written permission from the author.

See the [LICENSE](LICENSE) file for the full license text, or visit
[https://polyformproject.org/licenses/noncommercial/1.0.0/](https://polyformproject.org/licenses/noncommercial/1.0.0/).

For commercial licensing inquiries, contact [sricharanc03@gmail.com](mailto:sricharanc03@gmail.com).
## Support

For issues or questions:

1. Check existing [Issues](https://github.com/verycareful/ODAPP/issues)
2. Create a new issue with detailed information
3. Include steps to reproduce any bugs
