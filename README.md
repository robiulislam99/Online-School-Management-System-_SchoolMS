# 🏫 School Management System
> A full-stack web application built with ASP.NET Core 8 MVC and MySQL

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue?style=for-the-badge&logo=dotnet)
![MySQL](https://img.shields.io/badge/MySQL-8.0-orange?style=for-the-badge&logo=mysql)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-purple?style=for-the-badge&logo=bootstrap)
![AI Powered](https://img.shields.io/badge/AI-Groq%20LLaMA-green?style=for-the-badge&logo=ai)

---

## 📋 Table of Contents
- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Screenshots](#screenshots)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [API Integration](#api-integration)
- [Contributing](#contributing)
- [License](#license)

---

## 📖 About

A comprehensive **School Management System** designed to streamline school administration. It includes modules for managing students, teachers, classes, subjects, attendance, marks, fees, and expenses — all in one place. Features a built-in **AI Chat Assistant** powered by Groq LLaMA for instant help.

---

## ✨ Features

### 👨‍💼 Admin Module
- 📊 **Dashboard** — Overview of students, teachers, classes, subjects and expenses
- 🏫 **Classes** — Full CRUD with student and subject counts
- 📚 **Subjects** — Manage subjects per class
- 💰 **Class Fees** — Define fee types (Monthly, Annual, Admission, Exam, Transport)
- 👩‍🏫 **Teachers** — Add teachers with auto-generated login accounts
- 🎓 **Students** — Search and filter students by class or name
- 📝 **Marks** — Add exam marks with auto grade calculation
- ✅ **Teacher Attendance** — Daily attendance with toggle switches
- 📅 **Student Attendance** — Class-wise attendance with bulk actions
- 💸 **Expenses** — Track school expenses by category

### 👩‍🏫 Teacher Module
- 📊 **Dashboard** — Assigned subjects and today's attendance count
- 📚 **My Subjects** — View all assigned subjects
- ✅ **Take Attendance** — Mark student attendance
- 📋 **View Attendance** — Filter by class, student, and date
- 📝 **View Marks** — Read-only access to student marks

### 🤖 AI Chat Assistant
- Powered by **Groq LLaMA** (free, no credit card required)
- Helps with school administration questions
- Floating widget available on all pages

### 🔐 Authentication & Security
- Role-based authentication (**Admin** / **Teacher**)
- ASP.NET Identity with secure password hashing
- Account lockout after failed attempts
- Session management with cookie authentication

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Framework** | ASP.NET Core 8.0 MVC |
| **Database** | MySQL 8.x |
| **ORM** | Entity Framework Core 8.0.2 |
| **MySQL Driver** | Pomelo.EntityFrameworkCore.MySql 8.0.2 |
| **Authentication** | ASP.NET Core Identity |
| **Frontend** | Bootstrap 5.3, Font Awesome 6.5 |
| **AI Integration** | Groq API (LLaMA 3) |
| **Language** | C# 12 |

---

## 🖥️ Screenshots

> Dashboard, Login, Students, Attendance pages

| Login | Dashboard |
|-------|-----------|
| ![Login](screenshots/login.png) | ![Dashboard](screenshots/dashboard.png) |

| Students | Attendance |
|----------|-----------|
| ![Students](screenshots/students.png) | ![Attendance](screenshots/attendance.png) |

---

## ✅ Prerequisites

Before you begin, make sure you have installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.x](https://dev.mysql.com/downloads/installer/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) with **ASP.NET workload**
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (optional but recommended)
- Free [Groq API Key](https://console.groq.com) for AI Chat

---

## 🚀 Installation

### Step 1: Clone the Repository
```bash
git clone https://github.com/yourusername/SchoolMS.git
cd SchoolMS
```

### Step 2: Create MySQL Database
Open **MySQL Workbench** and run:
```sql
CREATE DATABASE SchoolMS;
CREATE USER 'schooluser'@'localhost' IDENTIFIED BY 'yourpassword';
GRANT ALL PRIVILEGES ON SchoolMS.* TO 'schooluser'@'localhost';
FLUSH PRIVILEGES;
```

### Step 3: Configure appsettings.json
Open `appsettings.json` and update:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=SchoolMS;User=schooluser;Password=yourpassword;"
  },
  "AdminSettings": {
    "Email": "admin@school.com",
    "Password": "Admin@123",
    "FullName": "System Administrator"
  },
  "AIChat": {
    "ApiUrl": "https://api.groq.com/openai/v1/chat/completions",
    "ApiKey": "YOUR_GROQ_API_KEY_HERE",
    "Model": "llama3-70b-8192"
  }
}
```

### Step 4: Install Dependencies
Open **Package Manager Console** in Visual Studio and run:
```powershell
dotnet restore
```

### Step 5: Apply Database Migrations
```powershell
Add-Migration InitialCreate
Update-Database
```

### Step 6: Run the Project
Press **F5** in Visual Studio or run:
```bash
dotnet run
```

Open your browser at `https://localhost:xxxx`

---

## 🔑 Default Login Credentials

| Role | Email | Password |
|------|-------|----------|
| **Admin** | admin@school.com | Admin@123 |
| **Teacher** | (email set when adding teacher) | Teacher@123 |

> ⚠️ **Important:** Change default passwords after first login in production!

---

## ⚙️ Configuration

### Database Connection
Update `appsettings.json` with your MySQL credentials:
```json
"DefaultConnection": "Server=localhost;Port=3306;Database=SchoolMS;User=root;Password=yourpassword;"
```

### AI Chat Setup
1. Go to [console.groq.com](https://console.groq.com)
2. Sign up for free (no credit card required)
3. Create an API key
4. Add it to `appsettings.json` under `AIChat:ApiKey`

### Available AI Models
| Model | Description |
|-------|-------------|
| `llama3-70b-8192` | Best quality, recommended |
| `llama-3.3-70b-versatile` | Latest and most capable |
| `llama-3.1-8b-instant` | Fastest response |
| `gemma2-9b-it` | Google's Gemma model |
| `mixtral-8x7b-32768` | Large context window |

---

## 📖 Usage

### Adding Data — Recommended Order
```
1. ➕ Add Classes          → /Classes/Create
2. ➕ Add Subjects         → /Subjects/Create
3. ➕ Add Class Fees       → /ClassFees/Create
4. ➕ Add Teachers         → /Teachers/Create
5. ➕ Assign Subjects      → /Teachers/AssignSubject
6. ➕ Add Students         → /Students/Create
7. ➕ Add Marks            → /Marks/Create
8. ✅ Take Attendance      → /Attendance/StudentAttendance
9. 💸 Record Expenses     → /Expenses/Create
```

### Teacher Login
When you add a teacher, a login account is automatically created:
- **Email:** the email you entered
- **Password:** `Teacher@123` (or the password you set)

---

## 📁 Project Structure
```
SchoolMS/
│
├── Controllers/
│   ├── AccountController.cs          # Login/Logout
│   ├── AIChatController.cs           # AI Chat API
│   ├── AttendanceController.cs       # Teacher & Student Attendance
│   ├── ClassesController.cs          # Classes CRUD
│   ├── DashboardController.cs        # Admin Dashboard
│   ├── HomeController.cs             # Role-based redirect
│   ├── MarksController.cs            # Student Marks
│   ├── OtherControllers.cs           # Expenses & ClassFees
│   ├── StudentsController.cs         # Students CRUD
│   ├── SubjectsController.cs         # Subjects CRUD
│   ├── TeacherDashboardController.cs # Teacher Portal
│   └── TeachersController.cs         # Teachers CRUD
│
├── Data/
│   ├── ApplicationDbContext.cs       # EF Core DbContext
│   └── DbInitializer.cs              # Migrations & Seeding
│
├── Models/
│   ├── ApplicationUser.cs            # Extended Identity User
│   ├── Class.cs                      # Class model
│   ├── OtherModels.cs                # Fees, Expenses, Marks, Attendance
│   ├── Student.cs                    # Student model
│   ├── Subject.cs                    # Subject model
│   └── Teacher.cs                    # Teacher model
│
├── ViewModels/
│   └── LoginViewModel.cs             # Login form model
│
├── Views/
│   ├── Account/                      # Login page
│   ├── Attendance/                   # Attendance views
│   ├── ClassFees/                    # Class fees views
│   ├── Classes/                      # Classes views
│   ├── Dashboard/                    # Admin dashboard
│   ├── Expenses/                     # Expenses views
│   ├── Marks/                        # Marks views
│   ├── Shared/                       # Layout & shared views
│   ├── Students/                     # Students views
│   ├── Subjects/                     # Subjects views
│   ├── TeacherDashboard/             # Teacher portal views
│   ├── Teachers/                     # Teachers views
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
│
├── wwwroot/
│   ├── css/site.css                  # Custom styles
│   └── js/site.js                    # Custom scripts
│
├── appsettings.json                  # Configuration
├── Program.cs                        # App startup
└── SchoolMS.csproj                   # Project file
```

---

## 🗄️ Database Schema
```
AspNetUsers          # Identity users (Admin, Teacher accounts)
AspNetRoles          # Roles (Admin, Teacher)
AspNetUserRoles      # User-role mapping
Classes              # School classes
Subjects             # Subjects per class
Teachers             # Teacher profiles
TeacherSubjects      # Teacher-subject assignments
Students             # Student profiles
StudentMarks         # Exam marks
TeacherAttendances   # Teacher daily attendance
StudentAttendances   # Student daily attendance
ClassFees            # Fee structure per class
Expenses             # School expenses
```

---

## 🤖 API Integration

### Groq AI Chat
The AI assistant uses the Groq API with LLaMA models:
```
POST https://api.groq.com/openai/v1/chat/completions
Authorization: Bearer {API_KEY}
Content-Type: application/json

{
    "model": "llama3-70b-8192",
    "messages": [
        { "role": "system", "content": "School assistant..." },
        { "role": "user", "content": "User message..." }
    ],
    "max_tokens": 500,
    "temperature": 0.7
}
```

---

## 🔒 Security Notes

- Never commit `appsettings.json` with real passwords to GitHub
- Add `appsettings.json` to `.gitignore` for production
- Use environment variables for sensitive data in production
- Change default admin password after first login

### Recommended .gitignore additions
```
appsettings.json
appsettings.Production.json
*.user
.vs/
bin/
obj/
```

---

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| `Cannot connect to MySQL` | Check MySQL service is running: `net start MySQL80` |
| `Access denied for user` | Verify credentials in `appsettings.json` |
| `Table doesn't exist` | Run `Update-Database` in Package Manager Console |
| `Add-Migration not recognized` | Install: `Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.2` |
| `AI Chat not working` | Check Groq API key and model name in `appsettings.json` |
| `Port already in use` | Change port in `Properties/launchSettings.json` |
| `Package version conflict` | Make sure all EF Core packages are version `8.0.2` |

---

## 🤝 Contributing

Contributions are welcome! Here's how:

1. Fork the repository
2. Create your feature branch
```bash
git checkout -b feature/AmazingFeature
```
3. Commit your changes
```bash
git commit -m "Add AmazingFeature"
```
4. Push to the branch
```bash
git push origin feature/AmazingFeature
```
5. Open a Pull Request

---

## 👨‍💻 Author

**Your Name**
- GitHub: [@yourusername](# 🏫 School Management System
> A full-stack web application built with ASP.NET Core 8 MVC and MySQL

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue?style=for-the-badge&logo=dotnet)
![MySQL](https://img.shields.io/badge/MySQL-8.0-orange?style=for-the-badge&logo=mysql)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-purple?style=for-the-badge&logo=bootstrap)
![AI Powered](https://img.shields.io/badge/AI-Groq%20LLaMA-green?style=for-the-badge&logo=ai)

---

## 📋 Table of Contents
- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Screenshots](#screenshots)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Database Schema](#database-schema)
- [API Integration](#api-integration)
- [Contributing](#contributing)
- [License](#license)

---

## 📖 About

A comprehensive **School Management System** designed to streamline school administration. It includes modules for managing students, teachers, classes, subjects, attendance, marks, fees, and expenses — all in one place. Features a built-in **AI Chat Assistant** powered by Groq LLaMA for instant help.

---

## ✨ Features

### 👨‍💼 Admin Module
- 📊 **Dashboard** — Overview of students, teachers, classes, subjects and expenses
- 🏫 **Classes** — Full CRUD with student and subject counts
- 📚 **Subjects** — Manage subjects per class
- 💰 **Class Fees** — Define fee types (Monthly, Annual, Admission, Exam, Transport)
- 👩‍🏫 **Teachers** — Add teachers with auto-generated login accounts
- 🎓 **Students** — Search and filter students by class or name
- 📝 **Marks** — Add exam marks with auto grade calculation
- ✅ **Teacher Attendance** — Daily attendance with toggle switches
- 📅 **Student Attendance** — Class-wise attendance with bulk actions
- 💸 **Expenses** — Track school expenses by category

### 👩‍🏫 Teacher Module
- 📊 **Dashboard** — Assigned subjects and today's attendance count
- 📚 **My Subjects** — View all assigned subjects
- ✅ **Take Attendance** — Mark student attendance
- 📋 **View Attendance** — Filter by class, student, and date
- 📝 **View Marks** — Read-only access to student marks

### 🤖 AI Chat Assistant
- Powered by **Groq LLaMA** (free, no credit card required)
- Helps with school administration questions
- Floating widget available on all pages

### 🔐 Authentication & Security
- Role-based authentication (**Admin** / **Teacher**)
- ASP.NET Identity with secure password hashing
- Account lockout after failed attempts
- Session management with cookie authentication

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Framework** | ASP.NET Core 8.0 MVC |
| **Database** | MySQL 8.x |
| **ORM** | Entity Framework Core 8.0.2 |
| **MySQL Driver** | Pomelo.EntityFrameworkCore.MySql 8.0.2 |
| **Authentication** | ASP.NET Core Identity |
| **Frontend** | Bootstrap 5.3, Font Awesome 6.5 |
| **AI Integration** | Groq API (LLaMA 3) |
| **Language** | C# 12 |

---

## 🖥️ Screenshots

> Dashboard, Login, Students, Attendance pages

| Login | Dashboard |
|-------|-----------|
| ![Login](https://github.com/robiulislam99/Online-School-Management-System-_SchoolMS/blob/master/ss%20of%20functionalities/sign%20in.png) | ![Dashboard](https://github.com/robiulislam99/Online-School-Management-System-_SchoolMS/blob/master/ss%20of%20functionalities/admin%20dashboard.png) |

| Students | Attendance |
|----------|-----------|
| ![Students](https://github.com/robiulislam99/Online-School-Management-System-_SchoolMS/blob/master/ss%20of%20functionalities/student%20dashboard.png) | ![Attendance](https://github.com/robiulislam99/Online-School-Management-System-_SchoolMS/blob/master/ss%20of%20functionalities/student%20attendence.png) |

---

## ✅ Prerequisites

Before you begin, make sure you have installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.x](https://dev.mysql.com/downloads/installer/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) with **ASP.NET workload**
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (optional but recommended)
- Free [Groq API Key](https://console.groq.com) for AI Chat

---

## 🚀 Installation

### Step 1: Clone the Repository
```bash
git clone https://github.com/yourusername/SchoolMS.git
cd SchoolMS
```

### Step 2: Create MySQL Database
Open **MySQL Workbench** and run:
```sql
CREATE DATABASE SchoolMS;
CREATE USER 'schooluser'@'localhost' IDENTIFIED BY 'yourpassword';
GRANT ALL PRIVILEGES ON SchoolMS.* TO 'schooluser'@'localhost';
FLUSH PRIVILEGES;
```

### Step 3: Configure appsettings.json
Open `appsettings.json` and update:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=SchoolMS;User=schooluser;Password=yourpassword;"
  },
  "AdminSettings": {
    "Email": "admin@school.com",
    "Password": "Admin@123",
    "FullName": "System Administrator"
  },
  "AIChat": {
    "ApiUrl": "https://api.groq.com/openai/v1/chat/completions",
    "ApiKey": "YOUR_GROQ_API_KEY_HERE",
    "Model": "llama3-70b-8192"
  }
}
```

### Step 4: Install Dependencies
Open **Package Manager Console** in Visual Studio and run:
```powershell
dotnet restore
```

### Step 5: Apply Database Migrations
```powershell
Add-Migration InitialCreate
Update-Database
```

### Step 6: Run the Project
Press **F5** in Visual Studio or run:
```bash
dotnet run
```

Open your browser at `https://localhost:xxxx`

---

## 🔑 Default Login Credentials

| Role | Email | Password |
|------|-------|----------|
| **Admin** | admin@school.com | Admin@123 |
| **Teacher** | (email set when adding teacher) | Teacher@123 |

> ⚠️ **Important:** Change default passwords after first login in production!

---

## ⚙️ Configuration

### Database Connection
Update `appsettings.json` with your MySQL credentials:
```json
"DefaultConnection": "Server=localhost;Port=3306;Database=SchoolMS;User=root;Password=yourpassword;"
```

### AI Chat Setup
1. Go to [console.groq.com](https://console.groq.com)
2. Sign up for free (no credit card required)
3. Create an API key
4. Add it to `appsettings.json` under `AIChat:ApiKey`

### Available AI Models
| Model | Description |
|-------|-------------|
| `llama3-70b-8192` | Best quality, recommended |
| `llama-3.3-70b-versatile` | Latest and most capable |
| `llama-3.1-8b-instant` | Fastest response |
| `gemma2-9b-it` | Google's Gemma model |
| `mixtral-8x7b-32768` | Large context window |

---

## 📖 Usage

### Adding Data — Recommended Order
```
1. ➕ Add Classes          → /Classes/Create
2. ➕ Add Subjects         → /Subjects/Create
3. ➕ Add Class Fees       → /ClassFees/Create
4. ➕ Add Teachers         → /Teachers/Create
5. ➕ Assign Subjects      → /Teachers/AssignSubject
6. ➕ Add Students         → /Students/Create
7. ➕ Add Marks            → /Marks/Create
8. ✅ Take Attendance      → /Attendance/StudentAttendance
9. 💸 Record Expenses     → /Expenses/Create
```

### Teacher Login
When you add a teacher, a login account is automatically created:
- **Email:** the email you entered
- **Password:** `Teacher@123` (or the password you set)

---

## 📁 Project Structure
```
SchoolMS/
│
├── Controllers/
│   ├── AccountController.cs          # Login/Logout
│   ├── AIChatController.cs           # AI Chat API
│   ├── AttendanceController.cs       # Teacher & Student Attendance
│   ├── ClassesController.cs          # Classes CRUD
│   ├── DashboardController.cs        # Admin Dashboard
│   ├── HomeController.cs             # Role-based redirect
│   ├── MarksController.cs            # Student Marks
│   ├── OtherControllers.cs           # Expenses & ClassFees
│   ├── StudentsController.cs         # Students CRUD
│   ├── SubjectsController.cs         # Subjects CRUD
│   ├── TeacherDashboardController.cs # Teacher Portal
│   └── TeachersController.cs         # Teachers CRUD
│
├── Data/
│   ├── ApplicationDbContext.cs       # EF Core DbContext
│   └── DbInitializer.cs              # Migrations & Seeding
│
├── Models/
│   ├── ApplicationUser.cs            # Extended Identity User
│   ├── Class.cs                      # Class model
│   ├── OtherModels.cs                # Fees, Expenses, Marks, Attendance
│   ├── Student.cs                    # Student model
│   ├── Subject.cs                    # Subject model
│   └── Teacher.cs                    # Teacher model
│
├── ViewModels/
│   └── LoginViewModel.cs             # Login form model
│
├── Views/
│   ├── Account/                      # Login page
│   ├── Attendance/                   # Attendance views
│   ├── ClassFees/                    # Class fees views
│   ├── Classes/                      # Classes views
│   ├── Dashboard/                    # Admin dashboard
│   ├── Expenses/                     # Expenses views
│   ├── Marks/                        # Marks views
│   ├── Shared/                       # Layout & shared views
│   ├── Students/                     # Students views
│   ├── Subjects/                     # Subjects views
│   ├── TeacherDashboard/             # Teacher portal views
│   ├── Teachers/                     # Teachers views
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
│
├── wwwroot/
│   ├── css/site.css                  # Custom styles
│   └── js/site.js                    # Custom scripts
│
├── appsettings.json                  # Configuration
├── Program.cs                        # App startup
└── SchoolMS.csproj                   # Project file
```

---

## 🗄️ Database Schema
```
AspNetUsers          # Identity users (Admin, Teacher accounts)
AspNetRoles          # Roles (Admin, Teacher)
AspNetUserRoles      # User-role mapping
Classes              # School classes
Subjects             # Subjects per class
Teachers             # Teacher profiles
TeacherSubjects      # Teacher-subject assignments
Students             # Student profiles
StudentMarks         # Exam marks
TeacherAttendances   # Teacher daily attendance
StudentAttendances   # Student daily attendance
ClassFees            # Fee structure per class
Expenses             # School expenses
```

---

## 🤖 API Integration

### Groq AI Chat
The AI assistant uses the Groq API with LLaMA models:
```
POST https://api.groq.com/openai/v1/chat/completions
Authorization: Bearer {API_KEY}
Content-Type: application/json

{
    "model": "llama3-70b-8192",
    "messages": [
        { "role": "system", "content": "School assistant..." },
        { "role": "user", "content": "User message..." }
    ],
    "max_tokens": 500,
    "temperature": 0.7
}
```

---

## 🔒 Security Notes

- Never commit `appsettings.json` with real passwords to GitHub
- Add `appsettings.json` to `.gitignore` for production
- Use environment variables for sensitive data in production
- Change default admin password after first login

### Recommended .gitignore additions
```
appsettings.json
appsettings.Production.json
*.user
.vs/
bin/
obj/
```

---

## 🐛 Troubleshooting

| Problem | Solution |
|---------|----------|
| `Cannot connect to MySQL` | Check MySQL service is running: `net start MySQL80` |
| `Access denied for user` | Verify credentials in `appsettings.json` |
| `Table doesn't exist` | Run `Update-Database` in Package Manager Console |
| `Add-Migration not recognized` | Install: `Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.2` |
| `AI Chat not working` | Check Groq API key and model name in `appsettings.json` |
| `Port already in use` | Change port in `Properties/launchSettings.json` |
| `Package version conflict` | Make sure all EF Core packages are version `8.0.2` |

---

## 🤝 Contributing

Contributions are welcome! Here's how:

1. Fork the repository
2. Create your feature branch
```bash
git checkout -b feature/AmazingFeature
```
3. Commit your changes
```bash
git commit -m "Add AmazingFeature"
```
4. Push to the branch
```bash
git push origin feature/AmazingFeature
```
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## 👨‍💻 Author

**Your Name**
- GitHub: [@robiulislam99](https://github.com/yourusername)
- LinkedIn: [Robiul](https://www.linkedin.com/in/md-robiul-islam-a926aa206/)

---

## ⭐ Show Your Support

If this project helped you, please give it a **⭐ star** on GitHub!

---

*Built with ❤️ using ASP.NET Core 8 MVC*)
- LinkedIn: [Your LinkedIn](https://linkedin.com/in/yourprofile)

---

## ⭐ Show Your Support

If this project helped you, please give it a **⭐ star** on GitHub!

---

*Built with ❤️ using ASP.NET Core 8 MVC*
