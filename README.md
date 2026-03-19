# Learning Management System (Canvas Clone)

A C# learning management system inspired by Canvas, built as a UWP (Universal Windows Platform) desktop application with a console app companion. It allows students, instructors, and teaching assistants to manage courses, assignments, grades, and more.

---

## Project Structure

The solution is organized into four main projects:

**Library.LearningManagement** — The core domain library containing all models and services shared across the application. Key models include `Course`, `Student`, `Instructor`, `TeachingAssistant`, `Assignment`, `Module`, and `Announcement`.

**App.LearningManagement** — A console-based interface for managing students and courses via the terminal. Useful for quick data entry and testing.

**UWP.LearningManagement** — The primary Windows desktop GUI application built with XAML/UWP. This is the main user-facing app.

**UWP.Library.LearningManagement** — A supporting library for the UWP app containing DTOs, additional models, and services like `CourseService` and `StudentService`.

---

## Features

### Authentication
- Users log in with an ID and password
- Role-based views: Students see a limited interface; Instructors and TAs can toggle into an expanded management mode

### People Management
- Add, update, and delete Students, Instructors, and Teaching Assistants
- Search people by name
- Paginated list view (5 per page) with first/previous/next/last navigation

### Course Management
- Create, update, and delete courses (code, name, description, room, credit hours, semester)
- Search courses by name, code, or description
- Add or remove students from a course roster
- View current and previous courses by semester

### Assignments
- Add assignments to courses, optionally placing them into weighted assignment groups
- Update and remove assignments
- Grade individual students on specific assignments
- Calculate weighted averages per course
- Calculate GPA across multiple courses based on credit hours and grade thresholds

### Announcements & Modules
- Full CRUD for course announcements and course modules
- Modules support a name and description

### Database View
- A dedicated page shows all courses and people pulled from the backend API
- Supports adding, editing, and deleting records directly through dialog windows

---

## Tech Stack

- **C# / .NET 7** (console app and core library)
- **UWP / XAML** (Windows desktop GUI)
- **.NET Standard 2.0** (shared UWP library)
- **Newtonsoft.Json** for serialization
- **REST API** backend at `http://localhost:5140` (required for the Database page)

---

## Getting Started

### Prerequisites
- Windows 10 or later
- Visual Studio 2022 with the **Universal Windows Platform development** workload installed
- .NET 7 SDK
- The backend API running locally on port `5140` (required for the Database Information page)

### Running the App

1. Clone the repository
2. Open the solution in Visual Studio
3. Set **UWP.LearningManagement** as the startup project for the GUI app, or **App.LearningManagement** for the console version
4. Build and run

### Default Login Credentials

| Name | ID | Password | Role |
|---|---|---|---|
| Daniel H | 1 | 123 | Student |
| Brian R | 2 | 234 | Instructor |
| Scott R | 3 | 345 | Teaching Assistant |

---

## Notes

- The toggle button to switch between Student and TA/Instructor mode is only visible after logging in as an Instructor or Teaching Assistant
- The Database Information page requires the REST API to be running; other pages use in-memory data seeded at startup
- GPA is calculated on a 4.0 scale weighted by credit hours (A=4.0, B=3.0, C=2.0, D=1.0, F=0.0)
