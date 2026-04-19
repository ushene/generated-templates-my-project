# Generated GitHub Actions Workflows for .NET Apps

This repository contains **parameterized, reusable** GitHub Actions workflows for building and deploying .NET applications to Azure.

## Folder Structure

```
.github/workflows/
    build.yml              - Main orchestrator workflow (entry point) - USE THIS
    reusable-ci-net.yml    - Reusable CI workflow (build, test, scan, package)
    reusable-cd-azure.yml  - Reusable CD workflow (deploy to Azure, health check)
```

## How It Works

The CI/CD pipeline is split into **three files**:

| File | Role | Run Directly? |
|------|------|---------------|
| `build.yml` | **Orchestrator** — triggers on push, calls the other two workflows | Yes |
| `reusable-ci-net.yml` | **CI** — restore, build, test, Snyk scan, package, publish to JFrog | No (called by build.yml) |
| `reusable-cd-azure.yml` | **CD** — download from JFrog, deploy to Azure, health check | No (called by build.yml) |

## Quick Start — Reusing for Your Own App

### Step 1: Update `build.yml` Configuration

All app-specific values are in the `env:` block at the top of `build.yml`. Change these to match your application:

```yaml
env:
  APP_NAME: 'MyTestApp'                                                   # Your app name
  JFROG_REPOSITORY: 'my-app-local'                                        # JFrog repo name
  BUILD_CONFIGURATION: 'Release'                                          # Release or Debug
  DOTNET_VERSION: '8.x'                                                   # .NET SDK version
  SOLUTION_PATH: 'test-dotnet-app'                                        # Folder with .sln file
  PROJECT_PATH: 'test-dotnet-app/src/MyTestApp.Api/MyTestApp.Api.csproj'  # Path to .csproj
  HEALTH_CHECK_PATH: '/api/health'                                        # Health endpoint
  READINESS_CHECK_PATH: '/api/ready'                                      # Readiness endpoint
  DOTNET_RUNTIME_VERSION: 'DOTNETCORE|8.0'                                # Azure runtime
```

**That's it.** No changes needed in the reusable workflow files — they read everything from inputs.

### Step 2: Configure GitHub Secrets

Go to your repo → **Settings → Secrets and variables → Actions** and add:

| Secret | Description |
|--------|-------------|
| `JFROG_URL` | JFrog Artifactory base URL |
| `JFROG_USERNAME` | JFrog username |
| `JFROG_PASSWORD` | JFrog password or API token |
| `SNYK_TOKEN` | Snyk API token (for security scanning) |
| `SONAR_TOKEN` | SonarQube token (optional, currently disabled) |
| `AZURE_SUBSCRIPTION_ID` | Azure subscription ID |
| `AZURE_TENANT_ID` | Azure AD tenant ID |
| `AZURE_CLIENT_ID` | Azure service principal client ID |
| `AZURE_CLIENT_SECRET` | Azure service principal client secret |

### Step 3: Configure GitHub Variables

Go to **Settings → Secrets and variables → Actions → Variables** and add per-environment:

| Variable | Description |
|----------|-------------|
| `DEV_RESOURCE_GROUP` | Azure resource group for dev |
| `DEV_RESOURCE_NAME` | Azure App Service name for dev |
| `STAGING_RESOURCE_GROUP` | Azure resource group for staging |
| `STAGING_RESOURCE_NAME` | Azure App Service name for staging |
| `PROD_RESOURCE_GROUP` | Azure resource group for production |
| `PROD_RESOURCE_NAME` | Azure App Service name for production |

### Step 4: Create GitHub Environments

Go to **Settings → Environments** and create: `development`, `staging`, `production`

### Step 5: Push Code

Push to `main`, `develop`, or any `feature/**` branch to trigger the pipeline.

## Workflow Pipeline Flow

```
Push / Manual Trigger
        │
        ▼
┌─────────────────┐
│  config job      │  ← Reads env: block, passes values to other jobs
└────────┬────────┘
         ▼
┌─────────────────┐
│  build job       │  ← Calls reusable-ci-net.yml
│  (CI workflow)   │     Restore → Build → Test → Snyk Scan → Package → JFrog
└────────┬────────┘
         ▼
┌─────────────────┐
│  deploy-dev      │  ← Calls reusable-cd-azure.yml (only on develop branch)
│  (CD workflow)   │     JFrog Download → Azure Login → Deploy → Health Check
└─────────────────┘
```

## Branches

| Branch | Purpose |
|--------|---------|
| `main` | Production-ready, hardcoded values for MyTestApp |
| `parameterized-backup` | Fully parameterized version — use this as a template for new apps |
| `develop` | Integration branch, triggers dev deployment |

## Deployment Environments

| Environment | Trigger |
|-------------|---------|
| Development | Auto on push to `develop`, or manual with `development` selected |
| Staging | Uncomment in build.yml — auto on push to `main`, or manual |
| Production | Uncomment in build.yml — manual trigger only |

## Documentation

- [CONFIGURATION-GUIDE.md](CONFIGURATION-GUIDE.md) — Setup instructions and secrets reference
- [AZURE-DEVOPS-MIGRATION-GUIDE.md](AZURE-DEVOPS-MIGRATION-GUIDE.md) — Migration guide from Azure DevOps

## Support

Review configuration guides or check [GitHub Actions documentation](https://docs.github.com/en/actions).

---
Generated: 2026-04-15 16:38:37
Updated: 2026-04-19 — Parameterized workflows with detailed comments
Version: 3.0 - Parameterized Reusable Workflows
