# 📋 TarefasApi

📋 **API REST para gestão de tarefas pessoais, desenvolvida em ASP.NET Core 9 utilizando Minimal APIs, Entity Framework Core e PostgreSQL. Implementa autenticação JWT, injeção de dependência e arquitetura organizada por feature. Gerencia usuários e tarefas classificadas por tipo (diárias e de longo prazo) através de endpoints RESTful protegidos por autorização baseada em token.**

---

## 🚀 Tecnologias utilizadas

- **ASP.NET Core 9** — Minimal APIs (sem Controllers)
- **C#**
- **Entity Framework Core 9** — ORM
- **PostgreSQL** — banco de dados relacional
- **Npgsql.EntityFrameworkCore.PostgreSQL** — provider do EF Core para PostgreSQL
- **JWT Bearer Authentication** — autenticação via token
- **ASP.NET Core Identity (`PasswordHasher<T>`)** — hash seguro de senha
- **Swagger / OpenAPI (Swashbuckle)** — documentação interativa da API

---

## 🏗️ Arquitetura

Projeto estruturado com **Minimal APIs organizadas por feature**, sem uso de Controllers tradicionais. Os endpoints são definidos como extension methods (`Map*Endpoints`), agrupados por domínio e registrados no `Program.cs`.

```
TarefasApi/
├── Program.cs                        # Composição da aplicação: DI, JWT, Swagger, pipeline HTTP
├── Models/                           # Entidades do domínio
│   ├── User.cs
│   └── TaskItem.cs
├── Data/
│   └── AppDbContext.cs               # Contexto do EF Core e configuração de relacionamentos
├── DTOs/                             # Contratos de entrada/saída da API
│   ├── AuthDtos.cs
│   └── TaskDtos.cs
├── Services/
│   ├── Interfaces/
│   │   └── ITokenService.cs
│   └── Implementation/
│       └── TokenService.cs           # Geração de tokens JWT
├── Extensions/
│   └── ClaimsPrincipalExtensions.cs  # Helpers para extrair dados do usuário autenticado
└── Endpoints/
    ├── AuthEndpoints.cs
    └── TaskEndpoints.cs
```

---

## 🔑 Autenticação e segurança

- Login via **JWT**, com claims de `id`, `nome`, `email` e `role`
- Senhas nunca armazenadas em texto puro — hash gerado pelo `PasswordHasher<T>` do ASP.NET Core Identity
- Todos os endpoints de tarefas exigem token válido
- Cada usuário só acessa, edita e remove as **próprias** tarefas (isolamento por `UserId`)

---

## 📌 Endpoints

### Autenticação

| Método | Rota | Descrição |
|---|---|---|
| POST | `/auth/register` | Cria um novo usuário |
| POST | `/auth/login` | Autentica e retorna o token JWT |

### Tarefas *(requer autenticação)*

| Método | Rota | Descrição |
|---|---|---|
| GET | `/tasks` | Lista todas as tarefas do usuário logado |
| GET | `/tasks?type=Daily` | Lista apenas as tarefas do dia |
| GET | `/tasks?type=LongTerm` | Lista apenas as tarefas de longo prazo |
| GET | `/tasks/{id}` | Detalha uma tarefa específica |
| POST | `/tasks` | Cria uma nova tarefa |
| PUT | `/tasks/{id}` | Atualiza título, descrição e data |
| PATCH | `/tasks/{id}/complete` | Marca a tarefa como concluída |
| DELETE | `/tasks/{id}` | Remove uma tarefa |

---

## ⚙️ Como rodar localmente

### Pré-requisitos
- [.NET SDK 9](https://dotnet.microsoft.com/download)
- PostgreSQL instalado (local ou via Docker)

### Passos

```bash
# clonar o repositório
git clone https://github.com/SEU-USUARIO/TarefasApi.git
cd TarefasApi

# restaurar dependências
dotnet restore

# criar o banco
psql -U postgres -c "CREATE DATABASE tarefasdb;"

# configurar appsettings.json (ver seção abaixo)

# aplicar as migrations
dotnet ef database update

# rodar a aplicação
dotnet run
```

Acesse a documentação interativa em `https://localhost:{porta}/swagger`.

---

## 🔐 Configuração

No `appsettings.json` (em produção, prefira variáveis de ambiente ou secrets):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tarefasdb;Username=postgres;Password=SUA_SENHA"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-com-no-minimo-32-caracteres",
    "Issuer": "TarefasApi",
    "Audience": "TarefasApiUsers",
    "ExpiresInMinutes": 120
  }
}
```

---

## 🧠 Regras de negócio

- Toda tarefa pertence a um único usuário
- Tarefas são classificadas por `Type`: `Daily` (do dia) ou `LongTerm` (longo prazo)
- Isolamento total entre usuários — ninguém acessa tarefa de outra conta
- E-mail único no sistema, sem duplicidade de cadastro

---

## 🗺️ Roadmap

- [ ] Refresh tokens
- [ ] Testes automatizados (unitários e de integração)
- [ ] Paginação nas listagens
- [ ] Notificações de tarefas próximas do vencimento
- [ ] Containerização com Docker

---

## 📄 Licença

Este projeto está sob a licença MIT.
