# 🧑‍💻 AiPrReviewer

AiPrReviewer is an **AI-powered assistant** for reviewing, fixing, and managing **GitHub Pull Requests**.  
It integrates with **GitHub API** and **OpenAI GPT models** to automatically suggest and apply fixes based on reviewer comments.  
Audit logs are stored in a **local SQLite database** for full traceability.

---

## ✨ Features

- 🔗 **GitHub Integration**  
  - Fetch repositories, pull requests, comments, and commits.  
  - Retrieve file content directly from PR branches (`refs/pull/{prNumber}/head`).  

- 🤖 **AI Fix Suggestions**  
  - Uses OpenAI GPT to analyze reviewer comments.  
  - Suggests fixes, either as full snippets or single-line changes.  
  - Allows one-click commit back to the PR branch.  

- 📜 **Audit Logging (SQLite)**  
  - Every AI-applied fix is stored with:  
    - PR number & title  
    - Commit SHA  
    - File path  
    - Reviewer comment  
    - Owner & repo info  
    - Timestamp  

- 🖥️ **UI with ASP.NET Core MVC**  
  - Home dashboard with navigation to **Repositories**, **Pull Requests**, and **Audit Log**.  
  - Pull request detail page with comments, AI solve button, and commit action.  
  - Bootstrap-based responsive UI with clean navigation.  

---

## 🛠️ Tech Stack

- **Backend**: ASP.NET Core 8 MVC  
- **Frontend**: Razor Views + Bootstrap 5  
- **Database**: SQLite (via Entity Framework Core)  
- **APIs**:
  - GitHub API (via Octokit)  
  - OpenAI API (via OpenAI .NET client)  

## 📦 Dependencies

This project relies on the following NuGet packages:

- **Microsoft.EntityFrameworkCore (9.0.9)** – Core EF ORM features.
- **Microsoft.EntityFrameworkCore.Sqlite (9.0.9)** – SQLite provider for EF Core, used as the local database.
- **Microsoft.EntityFrameworkCore.Tools (9.0.9)** – Tools for EF Core migrations and scaffolding.
- **Octokit (14.0.0)** – Official GitHub API client for .NET.
- **OpenAI (2.4.0)** – .NET client for interacting with the OpenAI API (used for AI-assisted code review and suggestions).


## 📂 Project Structure

```
AiPrReviewer/
│
├── Controllers/
│   ├── RepositoryController.cs   # Handles repository browsing
│   ├── PRController.cs           # Handles PR details, AI fixes, commits
│
├── Services/
│   ├── GitHubService.cs          # Octokit integration for PRs & file content
│   ├── AiService.cs              # OpenAI API calls (AI fix suggestions)
│   ├── AuditService.cs           # Saves applied fixes into SQLite
│
├── Data/
│   ├── AppDbContext.cs           # EF Core DbContext for Audit Logs
│   ├── AuditLogEntry.cs          # Entity model for audit records
│
├── Views/
│   ├── Home/Index.cshtml         # Dashboard (Repositories + Audit Log)
│   ├── Repository/Index.cshtml   # Repository list
│   ├── PR/Index.cshtml           # PR list for selected repo
│   ├── PR/Details.cshtml         # PR details, comments, AI fixes
│   ├── Audit/Index.cshtml        # Audit log view
│
├── appsettings.json              # Stores GitHub & OpenAI keys, Owner, Repo
├── Program.cs                    # Service registration & middleware
├── README.md                     # Project documentation
└── .gitignore                    # Ignores .json (secrets), bin/, obj/, etc
```

---

## ⚙️ Setup & Configuration

### 1. Clone the repo

```bash
git clone https://github.com/<your-username>/AiPrReviewer.git
cd AiPrReviewer
```

### 2. Install dependencies

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- SQLite (pre-installed on most systems)  

Install EF Core tools:

```bash
dotnet tool install --global dotnet-ef
```

### 3. Configure Secrets

#### Option A – User Secrets (recommended)

```bash
dotnet user-secrets init
dotnet user-secrets set "GitHub:Token" "<your-github-pat>"
dotnet user-secrets set "OpenAI:ApiKey" "<your-openai-api-key>"
```


### 4. Database Migration

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This will create the local `audits.db` SQLite database.

---

## 🚀 Running the App

```bash
dotnet run
```

App will be available at:  
👉 `https://localhost:7175/`

- **Repositories** → Browse your repos  
- **Pull Requests** → Open details & AI-assisted fixes  
- **Audit Log** → See history of AI-applied commits  

---

## 🔐 Security

- **Secrets**: Managed via [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets).  
- **.gitignore**: `.json` files excluded from git to avoid leaking API keys.  
- **Audit Trail**: Every AI commit is persisted for transparency.

---

## 📈 Next Steps / Enhancements

- 🔎 Diff view: Highlight AI changes vs original code.  
- 📝 More granular audit: Track who applied the fix (local user).  
- 🌐 Multi-repo support with dynamic switching.  
- 📬 Email/Slack notifications for AI-applied fixes.  

---

## 👨‍💻 Author

**Eduart Hajko**  
GitHub: [EduartHajko](https://github.com/EduartHajko)  
