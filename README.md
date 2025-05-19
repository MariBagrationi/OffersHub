# 🛍️ OffersHub

OffersHub is a modern ASP.NET Core MVC + Web API platform that allows companies to post product offers and clients to browse, purchase, and manage their orders. The application follows **Clean Architecture** principles and includes authentication, role-based access, image uploads, and a worker service for background tasks.

---

## 📐 Architecture

OffersHub uses a **Clean Architecture** structure with the following layers:

- `Core`: Contains domain entities, enums, and interfaces.
- `Application`: Holds services, DTOs, validation logic, and business rules.
- `Infrastructure`: Handles data access using Entity Framework Core and integrates with external services.
- `Presentation`: ASP.NET Core MVC and API controllers.
- `Worker`: Background service for long-running or scheduled tasks.

---

## 🔐 Features

### ✅ General
- Clean Architecture with separation of concerns
- Dependency Injection and SOLID principles
- JWT-based authentication 
- Role-based access: **Admin**, **Company**, **Client**, **Guest**
- Swagger UI for API documentation
- Responsive design with Bootstrap 5

### 🧑‍💼 Roles & Dashboards
- **Admin**:
  - View all users, companies, categories
  - Manage platform settings
- **Company**:
  - Create and manage offers
  - View statistics and transactions
- **Client**:
  - Browse offers
  - Add to cart, buy, cancel orders
  - Manage profile & balance

### 📦 Offers
- Company-created product offers
- Clients can browse, search, and purchase offers
- Offers belong to categories and have discounts and expiry

### 📁 Image Upload
- Uploaded via `IFormFile` and validated in the service layer
- Stored in a designated directory with unique names

### 🛠️ Worker Service
- Handles background jobs (e.g., cleaning expired offers)

---

## 🧪 Tech Stack

- **Framework**: ASP.NET Core 8, Entity Framework Core
- **UI**: Razor Pages (MVC), Bootstrap
- **Database**: SQL Server
- **Auth**: JWT + ASP.NET Identity
- **Tools**: Swagger, Mapster, FluentValidation

---

## 🚀 Getting Started

### 1. Clone the repo
```bash
git clone https://github.com/yourusername/OffersHub.git
cd OffersHub
