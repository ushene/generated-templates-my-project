# Generated GitHub Actions Workflows for my-dotnet-app

This folder contains GitHub Actions workflows generated for your project.

## Folder Structure

    .github/workflows/
        build.yml - Main orchestrator workflow - USE THIS
        reusable-ci-net.yml - Reusable CI workflow
        reusable-cd-azure.yml - Reusable CD workflow
        ci-legacy-*.yml - Legacy CI template
        cd-legacy-*.yml - Legacy CD template

## Quick Start

### Option 1: Reusable Workflows - Recommended

1. Copy the generated .github folder to your repository root
2. Configure GitHub Secrets in repository settings
   Required: JFROG_URL, JFROG_USERNAME, JFROG_PASSWORD
   See CONFIGURATION-GUIDE.md for complete list
3. Create GitHub Environments: development, staging, production
4. Push code to trigger workflows

### Option 2: Legacy Templates

Use ci-legacy and cd-legacy files for traditional separate CI/CD workflows.

## Documentation

- CONFIGURATION-GUIDE.md: Setup instructions and secrets
- AZURE-DEVOPS-MIGRATION-GUIDE.md: Migration guide from Azure DevOps

## Workflow Features

- Main Build Workflow: Orchestrates CI/CD using reusable workflows
- Reusable CI: Build, test, scan, package, publish to JFrog
- Reusable CD: Download from JFrog, deploy, health check

## Azure DevOps Migration

Read AZURE-DEVOPS-MIGRATION-GUIDE.md for complete migration steps.

## Support

Review configuration guides or check GitHub Actions documentation.

---
Generated: 2026-04-15 16:38:37
Version: 2.0 - Reusable Workflows
