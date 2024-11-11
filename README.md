# Employee Management System (EMS)

## Overview

The Employee Management System (EMS) is a robust web application built using ASP.NET Core and .NET 8, designed to streamline the management of employees, leave requests, and attendance tracking. The application integrates secure JWT-based authentication, role-based authorization, and leverages design patterns such as the Repository, Unit of Work, Factory, and Strategy patterns to promote scalable and maintainable code.

## Features

- **Employee Management**: Create, read, update, and delete (CRUD) operations for employee profiles.
- **Leave Request Management**: Employees can submit leave requests, and administrators can review and update their status.
- **Attendance Tracking**: Track employee attendance on an hourly or daily basis using a Strategy pattern.
- **Role-Based Authorization**: Role-based permissions to restrict specific actions to admin users.
- **JWT Authentication**: JSON Web Token (JWT) for secure user authentication and authorization.

## Architecture & Design Patterns

### Repository Pattern
Used for data access logic encapsulation. Each model, such as `Employee`, `LeaveRequest`, and `Attendance`, has its own repository interface and implementation, ensuring clean separation of concerns and testability.

### Unit of Work Pattern
The Unit of Work pattern is implemented to manage and coordinate transactions across multiple repositories, ensuring data integrity. The Unit of Work class aggregates multiple repository operations into a single transaction, minimizing database calls and providing a rollback mechanism in case of errors. This is particularly useful for complex operations that span multiple entities, such as processing leave requests and updating attendance records.

### Factory Pattern
Utilized to create instances for different leave types. A factory class is responsible for returning the correct type of leave request, making it easier to add or modify leave types without changing core logic.

### Strategy Pattern
Applied in the attendance module to handle different attendance calculation strategies, such as hourly and daily maintenance. This allows the system to dynamically choose the correct attendance tracking strategy based on the employee type or department requirements.

## Project Structure

- **Controllers**: Define API endpoints for interacting with Employee, LeaveRequest, and Attendance resources.
- **Models**: Data models representing core entities, including `Employee`, `LeaveRequest`, and `Attendance`.
- **Data**: Contains `DbContext` and database migrations.
- **DTOs**: Data Transfer Objects used for API requests and responses.
- **Repositories**: Implements the Repository pattern with interfaces and classes for data access logic.
- **UnitOfWork**: A class that manages transactions across multiple repositories, using a single instance of the `DbContext`.
- **Services**: Business logic layer, including Factory and Strategy pattern implementations.
- **Middleware**: Configured JWT authentication and authorization for secure access.

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Postman or similar API testing tool

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/VinothaManimuthu/MyDev
