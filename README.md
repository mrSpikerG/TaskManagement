# Task Management System

This project is a Task Management System built using **ASP.NET Core** with a **microservice architecture**, **Entity Framework Core** for database interaction, and **MS SQL Server** for the database.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [API Documentation](#api-documentation)
  - [User Endpoints](#user-endpoints)
  - [Task Endpoints](#task-endpoints)
- [Architecture and Design](#architecture-and-design)
  - [Microservice Architecture](#microservice-architecture)
  - [Repositories](#repositories)
  - [Entity Framework](#entity-framework)
- [Logging](#logging)
- [Testing](#testing)

---

## Prerequisites

Before you can run the project locally, ensure that you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Postman](https://www.postman.com/downloads/) (for API testing)

---

## Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-repo/task-management.git
   cd task-management
   ```

2. **Configure the database:**

    Update the connection string in `appsettings.json` to point to your local SQL Server.
   ```json
   "ConnectionStrings": {
      "TaskDb": "Server=localhost,3080;Database=TaskManagementDb;User Id=your_user;Password=your_password;"
   }
   ```
3. **Run database migrations:**
    In the project root, run the following commands to apply the migrations and set up the database schema:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

---

# Architecture and Design

## Microservice Architecture

This project follows a **microservice architecture** where each service (such as `UserService` and `TaskService`) is self-contained and communicates through a service layer. Each service has its own data model and is responsible for specific functionality.

## Repositories

We use the **Repository pattern** for interacting with the database. Each repository (e.g., `UserRepository`, `TaskRepository`) is responsible for handling CRUD operations on its associated entity and abstracts the underlying database logic.

## Entity Framework

The project uses **Entity Framework Core** as an ORM to interact with the **MS SQL Server** database. The migrations and database schema are managed through Entity Framework, and it is configured in the `Startup.cs` via dependency injection.



---

# API Documentation

## User Endpoints

- **Register a new user:**
  - **POST** `/users/register`
  - **Request Body:**
    ```json
    {
      "username": "john_doe",
      "email": "john@example.com",
      "password": "securepassword"
    }
    ```
  - **Response:**
    - `200 OK`: User successfully registered.
    - `400 BadRequest`: Validation error or other issue.

- **Login a user:**
  - **POST** `/users/login`
  - **Request Body:**
    ```json
    {
      "username": "john_doe",
      "password": "securepassword"
    }
    ```
  - **Response:**
    - `200 OK`: Returns a JWT token for authentication.
    - `401 Unauthorized`: Invalid credentials.

## Task Endpoints

- **Create a new task:**
  - **POST** `/tasks/`
  - **Authorization:** Bearer Token (JWT)
  - **Request Body:**
    ```json
    {
      "title": "New Task",
      "description": "Description of the task",
      "dueDate": "2024-09-10"
    }
    ```
  - **Response:**
    - `200 OK`: Task created successfully.
    - `400 BadRequest`: Error during task creation.

- **Get all tasks for the logged-in user:**
  - **GET** `/tasks/`
  - **Authorization:** Bearer Token (JWT)
  - **Query Parameters (optional):**
    - **filterDto** (for filtering tasks)
    - **paginationDto** (for pagination)
  - **Response:**
    - `200 OK`: Returns the list of tasks.
    - `400 BadRequest`: User undefined or error fetching tasks.

- **Get a specific task by ID:**
  - **GET** `/tasks/{id}`
  - **Authorization:** Bearer Token (JWT)
  - **Response:**
    - `200 OK`: Returns the task details.
    - `404 NotFound`: Task not found.

- **Update a task:**
  - **PUT** `/tasks/{id}`
  - **Authorization:** Bearer Token (JWT)
  - **Request Body:**
    ```json
    {
      "title": "Updated Task",
      "description": "Updated description",
      "dueDate": "2024-09-15"
    }
    ```
  - **Response:**
    - `200 OK`: Task updated successfully.
    - `404 NotFound`: Task not found.

- **Delete a task:**
  - **DELETE** `/tasks/{id}`
  - **Authorization:** Bearer Token (JWT)
  - **Response:**
    - `200 OK`: Task deleted successfully.
    - `404 NotFound`: Task not found.
