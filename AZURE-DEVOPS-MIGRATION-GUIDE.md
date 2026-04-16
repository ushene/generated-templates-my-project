# Azure DevOps to GitHub Actions Migration Guide

This guide helps you migrate your Azure DevOps pipelines to GitHub Actions for the **my-dotnet-app** project.

## Table of Contents

1. [Overview](#overview)
2. [Concept Mapping](#concept-mapping)
3. [Service Connections to Secrets](#service-connections-to-secrets)
4. [Variable Groups to Environments](#variable-groups-to-environments)
5. [Pipeline Patterns](#pipeline-patterns)
6. [Migration Steps](#migration-steps)
7. [Common Patterns](#common-patterns)
8. [Troubleshooting](#troubleshooting)

## Overview

This migration converts your Azure DevOps pipeline to GitHub Actions using:
- **Reusable Workflows**: Modular CI/CD components
- **GitHub Environments**: Replace Azure DevOps variable groups
- **GitHub Secrets**: Replace Azure DevOps service connections
- **Workflow Dispatch**: Manual deployment triggers

### Key Differences

| Azure DevOps | GitHub Actions |
|--------------|----------------|
| Pipeline | Workflow |
| Job | Job |
| Task | Step |
| Service Connection | Secret |
| Variable Group | Environment + Variables |
| Pipeline Library | Reusable Workflow |
| Release Pipeline | CD Workflow |

## Concept Mapping

### Service Connections â†’ GitHub Secrets

**Azure DevOps Service Connections** are replaced by **GitHub Repository Secrets**.

#### JFrog Service Connection
```yaml
# Azure DevOps
service: jfrog-artifactory

# GitHub Actions
secrets:
  JFROG_URL: ${{ secrets.JFROG_URL }}
  JFROG_USERNAME: ${{ secrets.JFROG_USERNAME }}
  JFROG_PASSWORD: ${{ secrets.JFROG_PASSWORD }}
```

#### Azure Service Connection
```yaml
# Azure DevOps
azureSubscription: 'Azure-Production'

# GitHub Actions
secrets:
  AZURE_SUBSCRIPTION_ID: ${{ secrets.AZURE_SUBSCRIPTION_ID }}
  AZURE_TENANT_ID: ${{ secrets.AZURE_TENANT_ID }}
  AZURE_CLIENT_ID: ${{ secrets.AZURE_CLIENT_ID }}
  AZURE_CLIENT_SECRET: ${{ secrets.AZURE_CLIENT_SECRET }}
```

### Variable Groups â†’ GitHub Environments

**Azure DevOps Variable Groups** are replaced by **GitHub Environment Variables**.

#### Setup GitHub Environments

1. Go to Settings > Environments
2. Create environments: \development\, \staging\, \production\
3. Add environment-specific variables

```yaml
# Azure DevOps Variable Group
variables:
  - group: prod-config
  - name: ResourceGroup
    value: 'rg-prod'

# GitHub Actions Environment
environment:
  name: production
  # Configure in Settings > Environments > production > Variables
  # Add: RESOURCE_GROUP = rg-prod
```

### Build/Release Patterns â†’ Workflow Patterns

#### Azure DevOps Multi-Stage Pipeline
```yaml
# azure-pipelines.yml
stages:
- stage: Build
  jobs:
  - job: BuildJob
    steps:
    - task: DotNetCoreCLI@2
      
- stage: Deploy
  dependsOn: Build
  jobs:
  - deployment: DeployJob
    environment: production
```

#### GitHub Actions Equivalent
```yaml
# .github/workflows/build.yml
jobs:
  build:
    uses: ./.github/workflows/reusable-ci-dotnet.yml
    
  deploy:
    needs: build
    uses: ./.github/workflows/reusable-cd-azure.yml
    with:
      environment: production
```

## Migration Steps

### Step 1: Export Azure DevOps Configuration

1. **Service Connections**
   - Navigate to Project Settings > Service Connections
   - Document all connections (JFrog, Azure, etc.)
   - Note the connection names and types

2. **Variable Groups**
   - Navigate to Pipelines > Library
   - Export all variable groups
   - Document environment-specific values

3.  **Pipeline YAML**
   - Export your existing azure-pipelines.yml
   - Note any custom tasks or scripts

### Step 2: Create GitHub Secrets

Navigate to your GitHub repository > Settings > Secrets and variables > Actions

#### Required Secrets

**JFrog Artifactory:**
- \JFROG_URL\
- \JFROG_USERNAME\
- \JFROG_PASSWORD\

**Security Scanning:**
- \SONAR_TOKEN\ (optional)
- \SNYK_TOKEN\ (optional)

**Azure Deployment:**
- `AZURE_SUBSCRIPTION_ID`
- `AZURE_TENANT_ID`
- `AZURE_CLIENT_ID`
- `AZURE_CLIENT_SECRET`

### Step 3: Create GitHub Environments

1. Go to Settings > Environments
2. Create three environments:
   - **development**
   - **staging**
   - **production**

3. For each environment, add variables:

#### Development Environment
- `DEV_RESOURCE_GROUP`: Azure resource group
- `DEV_RESOURCE_NAME`: App Service name

*(Repeat for staging and production with appropriate prefixes)*

### Step 4: Setup Workflow Files

Create the following structure in your repository:

```
.github/
â””â”€â”€ workflows/
    â”œâ”€â”€ build.yml                    # Main orchestrator
    â”œâ”€â”€ reusable-ci-net.yml      # CI reusable workflow
    â””â”€â”€ reusable-cd-azure.yml     # CD reusable workflow
```

### Step 5: Configure Branch Protection

1. Go to Settings > Branches
2. Add protection rule for \main\ branch:
   - Require pull request reviews
   - Require status checks (build workflow)
   - Require conversation resolution
   - Include administrators

### Step 6: Test the Migration

1. **Test CI Workflow**
   ```bash
   # Create a feature branch
   git checkout -b feature/test-github-actions
   
   # Make a small change
   echo "# Testing GitHub Actions" >> README.md
   git add README.md
   git commit -m "test: GitHub Actions migration"
   git push origin feature/test-github-actions
   
   # Create PR and verify workflow runs
   ```

2. **Test CD Workflow**
   - Trigger manual deployment to development
   - Verify artifacts download from JFrog
   - Confirm deployment succeeds
   - Check health endpoints

## Common Patterns

### Pattern 1: Conditional Deployment

**Azure DevOps:**
```yaml
- stage: Deploy
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
```

**GitHub Actions:**
```yaml
deploy:
  if: github.ref == 'refs/heads/main' && github.event_name == 'push'
```

### Pattern 2: Manual Approvals

**Azure DevOps:**
```yaml
environment:
  name: production
  # Approvals configured in UI
```

**GitHub Actions:**
```yaml
environment:
  name: production
  # Configure protection rules in Settings > Environments
```

### Pattern 3: Artifact Publishing

**Azure DevOps:**
```yaml
- task: PublishBuildArtifacts@1
  inputs:
    PathtoPublish: '$(Build.ArtifactStagingDirectory)'
    ArtifactName: 'drop'
```

**GitHub Actions:**
```yaml
- uses: actions/upload-artifact@v4
  with:
    name: build-artifacts
    path: artifacts/
```

### Pattern 4: Environment Variables

**Azure DevOps:**
```yaml
variables:
  buildConfiguration: 'Release'
  
steps:
- script: dotnet build --configuration $(buildConfiguration)
```

**GitHub Actions:**
```yaml
env:
  BUILD_CONFIGURATION: 'Release'
  
steps:
- run: dotnet build --configuration ${{ env.BUILD_CONFIGURATION }}
```

## Troubleshooting

### Issue: Secrets Not Available

**Problem:** Workflow can't access secrets

**Solution:**
- Verify secrets are created at repository level
- Check secret names match exactly (case-sensitive)
- Ensure workflow has proper permissions

### Issue: Reusable Workflow Not Found

**Problem:** Cannot find reusable workflow

**Solution:**
- Ensure reusable workflows are in \.github/workflows/\
- Use correct path: \./.github/workflows/filename.yml\
- Verify file names match references

### Issue: Environment Variables Not Available

**Problem:** Environment variables are empty

**Solution:**
- Create environments in Settings > Environments
- Add variables to specific environments
- Reference with \ars.VARIABLE_NAME\

### Issue: Deployment Fails with Permission Error

**Problem:** Cannot deploy to target environment

**Solution:**
- Verify service principal has contributor role
- Check subscription ID is correct
- Ensure resource group permissions

## Next Steps

1. âœ… Complete secret migration
2. âœ… Setup GitHub environments
3. âœ… Test CI workflows
4. âœ… Test CD workflows
5. âœ… Configure branch protection
6. âœ… Setup environment protection rules
7. âœ… Train team on GitHub Actions
8. âœ… Decommission Azure DevOps pipelines

## Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Migrating from Azure Pipelines](https://docs.github.com/en/actions/migrating-to-github-actions/migrating-from-azure-pipelines-to-github-actions)
- [Reusable Workflows](https://docs.github.com/en/actions/using-workflows/reusing-workflows)
- [GitHub Environments](https://docs.github.com/en/actions/deployment/targeting-different-environments/using-environments-for-deployment)

## Support

For migration assistance:
1. Review this guide thoroughly
2. Check GitHub Actions documentation
3. Review workflow runs in Actions tab
4. Contact DevOps team for infrastructure questions

---

**Migration Checklist:**
- [ ] Export Azure DevOps configuration
- [ ] Create GitHub secrets
- [ ] Setup GitHub environments
- [ ] Create workflow files
- [ ] Configure branch protection
- [ ] Test CI pipeline
- [ ] Test CD pipeline
- [ ] Train team
- [ ] Go live with GitHub Actions
- [ ] Archive Azure DevOps pipelines
