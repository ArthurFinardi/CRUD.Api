# Customer Management System

## Project Overview
This is a Customer Management System built using .NET 8, following Domain-Driven Design (DDD) principles and implementing CQRS and Event Sourcing patterns.

## Project Structure
The solution is organized into the following projects:

- **CustomerManagement.Api**: Web API project that serves as the entry point for the application
- **CustomerManagement.Domain**: Contains the domain entities, value objects, and domain logic
- **CustomerManagement.Application**: Contains application services, commands, queries, and DTOs
- **CustomerManagement.Infrastructure**: Contains implementations of repositories, external services, and database context
- **CustomerManagement.Tests**: Contains unit tests for the application

## Technologies Used
- .NET 8.0
- PostgreSQL (Database)
- Entity Framework Core
- FluentValidation
- MediatR (for CQRS implementation)
- Docker & Docker Compose
- xUnit (for testing)

## Features
- Customer registration with validation
- Support for both individual (PF) and corporate (PJ) customers
- Address management
- Unique CPF/CNPJ and email validation
- Age validation for individual customers
- IE (Inscrição Estadual) validation for corporate customers

## Requirements
- .NET 8.0 SDK
- Docker and Docker Compose
- PostgreSQL (if running locally)

## Getting Started
1. Clone the repository
2. Navigate to the project directory
3. Run `docker-compose up` to start the database
4. Run the application using `dotnet run`

## Development Status
This project is currently in development. The implementation will be done in stages:

1. Initial project setup and domain modeling
2. Implementation of basic CRUD operations
3. Implementation of CQRS and Event Sourcing
4. Adding validation rules
5. Implementing unit tests
6. Docker configuration
7. Documentation and final adjustments

## License
This project is for evaluation purposes only. 