# 🥊 Sports Stats API

A RESTful API for UFC fighter statistics, comparisons, and division rankings built with **ASP.NET Core 8**, **Entity Framework Core**, and **SQLite**.

![.NET](https://img.shields.io/badge/.NET-8.0-purple?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-blue?logo=dotnet)
![EF Core](https://img.shields.io/badge/Entity_Framework-Core_8-green?logo=nuget)
![SQLite](https://img.shields.io/badge/Database-SQLite-lightblue?logo=sqlite)
![Swagger](https://img.shields.io/badge/Docs-Swagger_UI-brightgreen?logo=swagger)
![License](https://img.shields.io/badge/License-MIT-yellow)

---

## 📖 About

This project is a portfolio-ready REST API that provides comprehensive UFC fighter statistics. It demonstrates backend development skills including:

- ✅ **RESTful API Design** with proper HTTP methods and status codes
- ✅ **Clean Architecture** (Controllers → Services → Data)
- ✅ **Entity Framework Core** with SQLite and seed data
- ✅ **API Documentation** with Swagger / OpenAPI
- ✅ **Pagination, Filtering & Sorting** on endpoints
- ✅ **DTOs** for clean data transfer
- ✅ **Dependency Injection** and interface-based services
- ✅ **CORS** enabled for frontend integration

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

```bash
# Clone the repository
git clone https://github.com/raaulx/SportsStatsApi.git
cd SportsStatsApi

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The API will start and Swagger UI will be available at: **http://localhost:5051**

---

## 📡 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/fighters` | Get all fighters (paginated, filterable) |
| `GET` | `/api/fighters/{name}` | Get fighter by name |
| `GET` | `/api/fighters/id/{id}` | Get fighter by ID |
| `GET` | `/api/fighters/compare?fighter1=X&fighter2=Y` | Compare two fighters |
| `GET` | `/api/fighters/rankings/{division}` | Get division rankings |
| `GET` | `/api/fighters/divisions` | List all divisions |
| `GET` | `/api/fighters/search?q=query` | Search fighters |

### Example Requests

#### Get all fighters (with pagination)
```
GET /api/fighters?page=1&pageSize=5&sortBy=wins&descending=true
```

#### Get fighter by name
```
GET /api/fighters/Jon Jones
```

#### Compare two fighters
```
GET /api/fighters/compare?fighter1=Jon Jones&fighter2=Tom Aspinall
```

#### Get Heavyweight rankings
```
GET /api/fighters/rankings/Heavyweight
```

#### Search fighters
```
GET /api/fighters/search?q=brazil
```

### Query Parameters (GET /api/fighters)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | int | 1 | Page number |
| `pageSize` | int | 10 | Items per page (max 50) |
| `division` | string | null | Filter by division |
| `country` | string | null | Filter by country |
| `isActive` | bool | null | Filter by active status |
| `sortBy` | string | null | Sort field: name, wins, losses, kopercentage, height, reach, ranking |
| `descending` | bool | true | Sort direction |

---

## 🏗️ Project Structure

```
SportsStatsApi/
├── Controllers/
│   └── FightersController.cs    # API endpoints
├── Models/
│   └── Fighter.cs               # Database entity
├── DTOs/
│   └── FighterDto.cs            # Data transfer objects
├── Services/
│   ├── IFighterService.cs       # Service interface
│   └── FighterService.cs        # Business logic
├── Data/
│   └── AppDbContext.cs          # EF Core context + seed data
├── Program.cs                    # App configuration
├── appsettings.json             # Configuration
├── SportsStatsApi.csproj        # Project file
└── README.md                    # This file
```

---

## 🗃️ Database

The API uses **SQLite** with **Entity Framework Core**. The database is automatically created and seeded with **25 real UFC fighters** across 6 divisions on first run.

### Divisions included:
- 🏋️ Heavyweight
- 🥊 Middleweight
- 🤼 Welterweight
- ⚡ Lightweight
- 🦅 Featherweight
- 🎯 Bantamweight

### Fighter Model

| Field | Type | Description |
|-------|------|-------------|
| Id | int | Primary key |
| Name | string | Fighter name (unique) |
| Nickname | string? | Fighter nickname |
| Wins | int | Total wins |
| Losses | int | Total losses |
| Draws | int | Total draws |
| Division | string | Weight division |
| Height | decimal | Height (cm) |
| Reach | decimal | Reach (cm) |
| KOPercentage | decimal | KO win percentage |
| SubmissionPercentage | decimal | Submission win percentage |
| Country | string? | Country of origin |
| Age | int? | Current age |
| IsActive | bool | Active status |
| Ranking | int? | Division ranking |

---

## 🛠️ Technologies

| Technology | Purpose |
|-----------|---------|
| ASP.NET Core 8 | Web API framework |
| Entity Framework Core 8 | ORM / Data access |
| SQLite | Database |
| Swagger / Swashbuckle | API documentation |
| C# 12 | Programming language |

---

## 📸 Screenshots

### Swagger UI
> Run the project and navigate to `http://localhost:5051` to see the interactive API documentation.

---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Arturo** - *Backend Developer*

- GitHub: [@raaulx](https://github.com/raaulx)
- LinkedIn: [Raul](www.linkedin.com/in/raul-lopez-marañon-07719b323)

---

> ⭐ If you found this project useful, please consider giving it a star on GitHub!
