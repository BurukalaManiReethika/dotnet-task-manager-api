# 🚀 Task Manager API

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet" />
  <img src="https://img.shields.io/badge/ASP.NET-Core-blue?style=for-the-badge&logo=.net" />
  <img src="https://img.shields.io/badge/SQLite-Database-green?style=for-the-badge&logo=sqlite" />
  <img src="https://img.shields.io/badge/Swagger-API%20Docs-brightgreen?style=for-the-badge&logo=swagger" />
</p>

<p align="center">
  A modern RESTful Task Management API built with ASP.NET Core 8, Entity Framework Core, SQLite, and Swagger.
</p>

---

## 🌐 Live Demo

### Swagger Documentation

https://dotnet-task-manager-api-1.onrender.com/swagger/index.html

### API Endpoint

https://dotnet-task-manager-api-1.onrender.com/api/tasks

---

## 📂 GitHub Repository

https://github.com/BurukalaManiReethika/dotnet-task-manager-api

---

## ✨ Features

✅ Create Tasks

✅ Get All Tasks

✅ Sort Tasks by Creation Date

✅ Get Task By ID

✅ Update Existing Tasks

✅ Delete Tasks

✅ SQLite Database Integration

✅ Entity Framework Core

✅ Swagger UI Documentation

✅ RESTful API Design

✅ Cloud Deployment with Render

---

## 🛠 Tech Stack

| Technology | Usage |
|------------|--------|
| ASP.NET Core 8 | Backend Framework |
| C# | Programming Language |
| Entity Framework Core | ORM |
| SQLite | Database |
| Swagger | API Documentation |
| Docker | Containerization |
| Render | Cloud Deployment |
| GitHub | Version Control |

---

## 📌 API Endpoints

| Method | Endpoint | Description |
|----------|----------|----------|
| GET | `/api/tasks` | Get all tasks |
| GET | `/api/tasks?sort=newest` | Get newest tasks first |
| GET | `/api/tasks?sort=oldest` | Get oldest tasks first |
| GET | `/api/tasks/{id}` | Get task by ID |
| POST | `/api/tasks` | Create a task |
| PUT | `/api/tasks/{id}` | Update task |
| DELETE | `/api/tasks/{id}` | Delete task |

---

## 📥 Sample Request

### POST /api/tasks

```json
{
  "title": "Deploy ASP.NET Core API",
  "description": "Hosted on Render",
  "isCompleted": false
}
```

---

## 📤 Sample Response

```json
{
  "id": 1,
  "title": "Deploy ASP.NET Core API",
  "description": "Hosted on Render",
  "isCompleted": false,
  "createdAt": "2026-06-13T12:00:00Z"
}
```

---

## 🔃 Sorting Tasks

Use the optional `sort` query parameter on `GET /api/tasks`:

```text
GET /api/tasks?sort=newest
GET /api/tasks?sort=oldest
```

---

## 🚀 Getting Started

### Clone Repository

```bash
git clone https://github.com/BurukalaManiReethika/dotnet-task-manager-api.git
```

### Navigate

```bash
cd dotnet-task-manager-api
```

### Restore Packages

```bash
dotnet restore
```

### Run Project

```bash
dotnet run
```

### Open Swagger

```text
https://localhost:5001/swagger
```

---

## 📸 Project Highlights

- REST API Architecture
- CRUD Operations
- Database Persistence
- Swagger Integration
- Cloud Deployment
- Docker Support
- GitHub Portfolio Project

---

## 🎯 Project Status

🟢 Active

🟢 Production Ready

🟢 Successfully Deployed on Render

---

## 👨‍💻 Author

**Mani**

GitHub:
https://github.com/BurukalaManiReethika

---

## ⭐ Support

If you like this project:

⭐ Star the repository

🍴 Fork the project

🚀 Share it with others
