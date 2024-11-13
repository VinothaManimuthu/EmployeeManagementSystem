Project Name :Employee managment system.


Employee System - Code First Migration

Description :

This project implements an Employee Management System using Entity Framework Core with a Code First approach. It allows administrators to manage employee details, attendance, and leave requests, along with user login and registration functionalities.


The key functionalities include:

Code First Approach: The database schema is generated based on the defined C# classes (models).

Migration: Enables schema changes through Entity Framework Core migrations.

Models: Defines entities such as Employee, Attendance, LeaveRequest, Role, and user authentication-related models.


Installation :

1..NET 5.0 or later

2.PostgreSQL installed and configured.


Represents an employee in the organization.

EmployeeId: Primary key.

FirstName, LastName: Basic details of the employee.

Email, PhoneNumber: Contact information.

DateOfJoining: Date the employee joined.

RoleId: Foreign key to the Role table.

LeaveRequests: Navigation property for related LeaveRequest records.

Attendances: Navigation property for related Attendance records.


AttendanceId: Primary key.

EmployeeId: Foreign key to the Employee table.

CheckInTime, CheckOutTime: Track employee’s daily attendance.

WorkHours: Calculated work hours for each attendance entry.


LeaveRequestId: Primary key.

LeaveType: Type of leave (e.g., sick, vacation).

EmployeeId: Foreign key to the Employee table.

StartDate, EndDate: Duration of the leave.

Reason: Reason for requesting leave.

Status: Current status of the leave request (e.g., pending, approved, rejected).


Inherits from IdentityRole and represents user roles in the system (e.g., Admin, HR, Employee).


Libraries Used :

1.Entity Framework (EF) Core

2.MSSQL for PostgreSQL database connectivity 3.Mapping 4.FluentValidation 5.microsoft.AspNetCore.Identity EntityFrameworkCore 6. microsoft.AspNetCore.Authentication.JwtBearer 7. microsoft.AspNetCore.Tools 8.System.Identitymodel.Tokens.jwt 9.AutoMapper

3.NLog for logging

4.XUnit for unit testing

Use below commands to install package:

Add-Migration

Update-Database


Code First with ApplicationDbContext :

dentity for role-based access control. The ApplicationDbContext class manages the database operations, migrations, and model relationships.

Code First Approach: The database schema is generated based on model definitions.

ASP.NET Identity: Manages roles and user authentication.

Relationships: Configures one-to-many relationships and cascades using Fluent API in ApplicationDbContext.

Migrations: Changes to the model structure are managed via migrations.

Defines roles like Admin, HR, or Employee, and supports ASP.NET Identity.

ApplicationDbContext Configuration

The ApplicationDbContext class, which inherits from IdentityDbContext, is configured with the following key aspects:


Entities:

DbSet<Employee>: Employee information.

DbSet<Role>: Role-based access.

DbSet<LeaveRequest>: Leave requests by employees.

DbSet<Attendance>: Employee attendance records.

Relationships:


Employee to Role: Configured as a one-to-many relationship.

Employee to LeaveRequest: Configured as a one-to-many relationship.

Employee to Attendance: Configured as a one-to-many relationship.


Dependency Injection and Service Configuration

The following services and repositories are registered in Program.cs:


DbContext: Configured with PostgreSQL.

Repositories: Generic and specific repositories are injected using Scoped lifetime.

Services: Includes employee, leave request, attendance services, etc.

Validators: Automatically applies FluentValidation for model validation.

AutoMapper: Maps between models and DTOs.


JWT Authentication

The JWT configuration uses a secret key from the configuration file. Ensure the key is at least 32 bytes for security. JWT is used for authenticating users and authorizing API endpoints.

Swagger is configured to document and test API endpoints with JWT support. Access Swagger at /swagger.


The Employee_System.Dto_s namespace contains Data Transfer Objects (DTOs), which are used for data transfer between the client and server in a simplified format. They allow for efficient communication and separation between the API and the underlying database entities.


The AttendanceDTO class represents attendance data in a structured form. It is primarily used for data retrieval, providing essential details about an employee's attendance record.


Properties:

The AttendanceDTO class represents attendance data in a structured form. It is primarily used for data retrieval, providing essential details about an employee's attendance record.

AttendanceId (int): Unique identifier for the attendance record.

EmployeeId (int): Identifier for the associated employee.

CheckInTime (DateTime): Time when the employee checked in.

CheckOutTime (DateTime): Time when the employee checked out.

WorkHours (double): Total hours worked by the employee during the attendance period.

The CreateAttendanceDTO class is used for creating new attendance records. It focuses on the input fields required to register an employee's attendance

EmployeeId (int): Identifier for the associated employee.

CheckInTime (DateTime): Time when the employee checked in.

CheckOutTime (DateTime): Time when the employee checked out.

These DTOs facilitate structured data transfer for attendance-related operations, streamlining interactions between the client and server and ensuring only necessary data is exposed.


The services are built using dependency injection and interface-based design for flexibility, testability, and separation of concerns. Below are details of the main services and their functionalities.

Each service handles specific data-related functionalities for entities in the Employee Management System. They use Unit of Work, Repository patterns, and a Cache layer to optimize performance and handle data transactions. Each service has an associated interface that defines its public methods, promoting modularity and easy testing.


Services Overview

Each service handles specific data-related functionalities for entities in the Employee Management System. They use Unit of Work, Repository patterns, and a Cache layer to optimize performance and handle data transactions. Each service has an associated interface that defines its public methods, promoting modularity and easy testing.


1. AttendanceService (IAttendanceService)

The AttendanceService manages attendance records and supports caching to minimize database calls.


Key Methods:


GetAttendanceByIdAsync: Retrieves an attendance record by ID. First checks the cache before querying the database.

GetAllAttendancesAsync: Returns all attendance records.

AddAttendanceAsync: Creates a new attendance record and caches it.

UpdateAttendanceAsync: Updates an attendance record and refreshes the cache.

DeleteAttendanceAsync: Deletes an attendance record and removes it from the cache.

Dependencies:


IUnitOfWork: For managing transactions.

IAttendanceTrackingStrategy: Used to calculate work hours based on the tracking strategy.

LoggerService: For error logging.

AttendanceCacheService: Singleton cache service to store attendance data.

2. EmployeeService (IEmployeeService)

The EmployeeService manages employee data and uses caching to improve retrieval efficiency.


Key Methods:


GetAllEmployeesAsync: Retrieves all employees without caching, as it could be memory-intensive.

GetEmployeeByIdAsync: Fetches an employee by ID, with caching to avoid repeated database queries.

AddEmployeeAsync: Adds a new employee and caches the record.

UpdateEmployeeAsync: Updates an employee's information and refreshes the cache.

DeleteEmployeeAsync: Deletes an employee and removes it from the cache.

Dependencies:


IUnitOfWork: For database transactions.

LoggerService: Logs any errors or exceptions.

EmployeeCacheService: Caches individual employee data.

3. LeaveRequestService (ILeaveRequestService)

The LeaveRequestService handles leave requests with similar caching and logging as the other services. It also uses the Factory Pattern to create leave requests based on their type.


Key Methods:


GetAllLeaveRequestsAsync: Fetches all leave requests, without caching due to potential size.

GetLeaveRequestByIdAsync: Retrieves a leave request by ID, with caching.

AddLeaveRequestAsync: Creates a leave request using the factory pattern and caches the result.

UpdateLeaveRequestAsync: Updates leave request data and refreshes the cache.

DeleteLeaveRequestAsync: Deletes a leave request and removes it from the cache.

Dependencies:


IUnitOfWork: Manages transactions for data consistency.

LeaveRequestFactory: Creates leave requests based on the type of leave.

LoggerService: Logs errors and operational messages.

LeaveRequestCacheService: Singleton cache service for leave requests.

Key Components and Patterns

Unit of Work

The Unit of Work (UoW) pattern ensures that each service's database operations (like Add, Update, Delete) are encapsulated in a single transaction, enabling rollback on errors and promoting consistent data handling.


Caching

The Cache Services (AttendanceCacheService, EmployeeCacheService, LeaveRequestCacheService) are singleton instances that improve performance by storing frequently accessed data, reducing redundant database calls.


Factory Pattern

The Factory Pattern is used in the LeaveRequestService to create leave requests based on specific types, enhancing flexibility and reducing conditional logic within the service.


LoggerService

The LoggerService provides centralized logging for all services, capturing errors, exceptions, and operational messages to help diagnose issues and improve observability.


Usage

These services can be injected into controllers or other classes where data operations are needed. The interface-based design also allows for easy mocking in unit tests.