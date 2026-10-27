# TarefasApi

REST API for personal task management, built with C# and ASP.NET Core 9 using Minimal APIs, Entity Framework Core, and PostgreSQL.

The application supports user authentication with JWT, user-specific task management, and task classification by type. It follows a feature-oriented endpoint organization and uses token-based authorization to protect resources.

## Technologies

- **C#**
- **ASP.NET Core 9** — Minimal APIs
- **Entity Framework Core 9** — Object-relational mapping
- **PostgreSQL** — Relational database
- **Npgsql** — PostgreSQL provider for Entity Framework Core
- **JWT Bearer Authentication** — Token-based authentication
- **ASP.NET Core Identity PasswordHasher** — Password hashing
- **Scalar / OpenAPI** — Interactive API documentation

## Features

### Authentication and User Management

- User registration
- User login with JWT authentication
- Secure password hashing
- Unique email validation

### Task Management

- Create, retrieve, update, and delete tasks
- Mark tasks as completed
- Filter tasks by type
- Associate tasks with their respective owners
- Restrict access to tasks based on the authenticated user

### Task Types

- `Daily` — Daily tasks
- `LongTerm` — Long-term tasks

## Architecture

The application uses Minimal APIs organized by feature, without traditional MVC controllers. Endpoint mappings are defined through extension methods and registered in `Program.cs`.

```text
TarefasApi/
├── Program.cs
├── Models/
│   ├── User.cs
│   └── TaskItem.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── AuthDtos.cs
│   └── TaskDtos.cs
├── Services/
│   ├── Interfaces/
│   │   └── ITokenService.cs
│   └── Implementation/
│       └── TokenService.cs
├── Extensions/
│   └── ClaimsPrincipalExtensions.cs
└── Endpoints/
    ├── AuthEndpoints.cs
    └── TaskEndpoints.cs
```

### Component Responsibilities

- **Models:** Represent users and tasks.
- **Data:** Configures the database context and entity relationships.
- **DTOs:** Define request and response data structures.
- **Services:** Encapsulate supporting application services, including JWT generation.
- **Extensions:** Provide reusable extension methods.
- **Endpoints:** Define HTTP routes and handle API requests.
- **Program.cs:** Configures dependency injection, authentication, Swagger, and the HTTP pipeline.

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/auth/register` | Register a new user |
| `POST` | `/auth/login` | Authenticate a user and return a JWT |

### Tasks

All task endpoints require a valid access token.

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/tasks` | Retrieve the authenticated user's tasks |
| `GET` | `/tasks?type=Daily` | Filter tasks by daily type |
| `GET` | `/tasks?type=LongTerm` | Filter tasks by long-term type |
| `GET` | `/tasks/{id}` | Retrieve a specific task |
| `POST` | `/tasks` | Create a task |
| `PUT` | `/tasks/{id}` | Update task information |
| `PATCH` | `/tasks/{id}/complete` | Mark a task as completed |
| `DELETE` | `/tasks/{id}` | Delete a task |

## Security

- JWT-based authentication
- Password hashing using ASP.NET Core Identity's `PasswordHasher<T>`
- Protected task endpoints
- User-specific task access enforced through `UserId`
- Unique email addresses for user registration

Task ownership must be enforced by the application when retrieving, updating, or deleting resources to prevent unauthorized cross-user access.

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

### 1. Clone the repository

Replace `YOUR_USERNAME` with the appropriate GitHub username if the repository URL differs.

```bash
git clone https://github.com/erickthmdev/TarefasApi.git
cd TarefasApi
```

### 2. Configure the database

Create a PostgreSQL database:

```sql
CREATE DATABASE tarefasdb;
```

Alternatively, create it using the PostgreSQL command-line client:

```bash
psql -U postgres -c "CREATE DATABASE tarefasdb;"
```

### 3. Configure application settings

Configure the database connection string and JWT settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tarefasdb;Username=postgres;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_WITH_AT_LEAST_32_CHARACTERS",
    "Issuer": "TarefasApi",
    "Audience": "TarefasApiUsers",
    "ExpiresInMinutes": 120
  }
}
```

Use values compatible with the application's configuration classes. Never commit real database passwords or JWT signing keys to version control. For production deployments, use environment variables or a dedicated secrets manager.

### 4. Restore dependencies

```bash
dotnet restore
```

### 5. Apply database migrations

If the EF Core migrations are included in the repository, run:

```bash
dotnet ef database update
```

Ensure that the Entity Framework Core CLI is installed and compatible with the project's package versions.

### 6. Run the application

```bash
dotnet run
```

The application will display its listening addresses in the terminal. If Swagger is enabled, open `/swagger` on the appropriate application URL to explore the available endpoints.

## Business Rules

- Each task belongs to a single user.
- Tasks are classified as `Daily` or `LongTerm`.
- Users must only be able to access and manage their own tasks.
- Email addresses must be unique across registered users.
- Protected endpoints require valid authentication.

## Roadmap

- [ ] Refresh token support
- [ ] Unit and integration tests
- [ ] Pagination for task listings
- [ ] Notifications for upcoming task deadlines
- [ ] Docker containerization

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

## Author

**Erick** — Back-end Developer

- GitHub: [@erickthmdev](https://github.com/erickthmdev)
