# GetzTest - .NET Developer (2025) Coding Test

## Project Overview
This project is a **User Management API** built with **ASP.NET Core (.NET 9)** following **Clean Architecture** principles.  
It includes authentication using **JWT** and uses **in-memory persistence** (no external database required).

---

## Tech Stack
- **.NET 9.0**
- **ASP.NET Core Web API**
- **Clean Architecture**
- **EF Core InMemory Provider**
- **JWT Authentication** use RSA algorithm
- **xUnit + Moq** for unit testing

---

## 📂 Project Structure
GetzTest
│── GetzTest.Application # Application layer (services, use cases)
│── GetzTest.Domain # Domain layer (entities, interfaces)
│── GetzTest.Infrastructure # Infrastructure layer (persistence, JWT)
│── GetzTest.API # API layer (controllers, endpoints)
│── GetzTest.UnitTest # Unit tests

## Service
Authentication & Endpoints
Login
POST http://localhost:5036/api/identity/v1/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "123456"
}


Response:

{
  "accessToken": "<JWT_TOKEN>",
  "expiresIn": 3600
}

Register
POST http://localhost:5036/api/accounts/register
Content-Type: application/json

{
  "username": "newuser",
  "password": "123456",
  "email": "newuser@getz.co"
}

Get All Accounts (Requires JWT)
GET http://localhost:5036/api/identity/v1/accounts
Authorization: Bearer <JWT_TOKEN>

🌐 OpenID & JWKs Endpoints

JWKs: http://localhost:5036/.well-known/jwks.json

OpenID Config: http://localhost:5036/.well-known/openid-configuration

🧪 Running Unit Tests
cd GetzTest.UnitTest
dotnet test

📌 Design Choices

Applied Clean Architecture to ensure separation of concerns.

Used EF Core InMemory for fast testing and zero database setup.

Implemented JWT Authentication for secure API access.

⚖️ Trade-offs

Used in-memory persistence instead of a real database → fast setup but data resets on restart.

Minimal logging to keep code simple under time constraints.

JWT secret stored in appsettings.json for demo purposes (should use secrets in production).

🔮 Next Steps (if more time allowed)

Add integration tests using Testcontainers + PostgreSQL.

Improve validation with FluentValidation.

Add role-based authorization.

Add CI/CD pipeline for automated testing.

## Run project
cd .\GetzTest.Application\
dotnet run

## Postman 
go to folder docs/GetzTest.postman_collection.json
