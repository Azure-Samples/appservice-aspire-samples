# Aspire Samples for Azure App Service

This repository contains a curated set of **Aspire** sample applications focused on scenarios relevant to **Azure App Service**. Each sample demonstrates local orchestration with Aspire (AppHost + dashboard) and a path to deployment (Azure Developer CLI / Aspire CLI).

## Samples Overview

| Sample | Focus | Technologies |
|--------|-------|-------------|
| [AspireWithOpenAI](./samples/AspireWithOpenAI/) | Integrating Azure OpenAI with API + Blazor/Razor UI | .NET Aspire, ASP.NET Core, Azure OpenAI, Docker |
| [AspireWithPythonAndReact](./samples/AspireWithPythonAndReact/) | Orchestrating a Python API and a React (Vite) frontend | .NET Aspire, Python (FastAPI-style), Node/React, Docker |
| [AspireWithSql](./samples/AspireWithSql/) | Azure SQL provisioning, EF Core migrations, API + Web UI | .NET Aspire, ASP.NET Core, EF Core, Azure SQL |

Each sample has its own solution under `samples/<SampleName>/` and a dedicated `AppHost` project that wires resources, environment variables, references, health checks, and launches the Aspire dashboard.

> [!IMPORTANT]
> These samples illustrate individual concepts; they are **not** production-hardened. Apply additional hardening (monitoring, scaling, security controls) before real-world use.

## Quick Start (Local Development)

Prerequisites (choose those relevant to the sample):

- .NET 10 SDK or later
- Docker Desktop
- Azure subscription (for cloud resource provisioning / deployment)

Run any sample locally (example: OpenAI integration):

```powershell
cd samples/AspireWithOpenAI/AspireWithOpenAI.AppHost
dotnet run
```

The Aspire dashboard URL will be printed; open it to inspect components, logs, health checks, and environment wiring.

## Deployment (High-Level)

You can deploy a sample using either:

1. **Azure Developer CLI (azd)** – provisions & deploys (`azd up`).
2. **Aspire CLI** – `aspire deploy` to provision & deploy.

Follow the per-sample `README.md` for detailed steps.

## Security & Secrets

- **Never commit secrets** (API keys, connection strings). Use user secrets locally and **Managed Identity** / **Azure Key Vault** in Azure.
- Prefer **Managed Identity** over static keys when accessing Azure resources (OpenAI, SQL, etc.).
- Use **App Configuration** or Key Vault references for centralizing config where appropriate.
- Keep dependencies patched.
- Review the official ASP.NET Core security guidance: [ASP.NET Core Security](https://learn.microsoft.com/aspnet/core/security?view=aspnetcore-8.0)
- Enforce HTTPS (default in ASP.NET Core) and validate CORS origins (see Python + React sample environment variables).
- Apply principle of least privilege for Azure role assignments during deployment; avoid broad Owner scopes for runtime.

## License

The content in this repository is made available under the **[MIT License](./LICENSE)**.

## Contributing

Contributions are welcome! To propose a new sample or enhancement:

1. Open an issue describing intent and scope.
2. Discuss alignment (avoid duplicates, keep sample concise).
3. Submit a PR following existing project structure and style.

Please include a short README in any new sample folder explaining architecture, prerequisites, and deployment nuances.

## Code of Conduct

We follow the .NET Foundation Code of Conduct. By participating, you agree to uphold a welcoming, inclusive community. Report unacceptable behavior to the project maintainers.

## Disclaimer

The code is intended for educational use only. **Do not rely on these samples as a production baseline** without performing your own security review, performance testing, and compliance checks.

## Related Resources

- Docs: [Aspire Documentation](https://learn.microsoft.com/dotnet/aspire)
- Blog: [Aspire Blog](https://aka.ms/dotnet/aspire/blog)
- GitHub (Aspire): [dotnet/aspire](https://github.com/dotnet/aspire)
- Project site: [aspire.dev](https://aspire.dev/)
- Azure App Service: [Azure App Service Docs](https://learn.microsoft.com/azure/app-service/)

## Feedback & Support

Issues and feature requests can be filed via the repository issue tracker. For Azure service questions, use Microsoft Q&A or Azure support channels.

---
**Next step:** Pick a sample directory, read its README, run the AppHost, explore the dashboard, then iterate or deploy.
