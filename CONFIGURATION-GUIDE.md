# Configuration Guide

This guide explains how to configure and use the generated CI/CD templates for your project.

## Project Information

- **Application Type**: Azure App Service
- **Language**: .NET
- **Deployment Type**: Azure
- **Project Name**: my-dotnet-app

## Generated Files

1. **CI Template**: Builds, tests, scans, and publishes artifacts to JFrog
2. **CD Template**: Deploys application to Azure

## Prerequisites

### Required Tools

- **Git**: Version control
- **GitHub CLI** (optional): For easier workflow management
- **.NET SDK**: Version 6.0 or later
- **Azure CLI**: For Azure deployments

### Required GitHub Secrets

Configure the following secrets in your GitHub repository (Settings > Secrets and variables > Actions):

#### JFrog Artifactory Secrets

- `JFROG_URL`: Your JFrog Artifactory URL (e.g., https://yourcompany.jfrog.io)
- `JFROG_REPOSITORY`: Target repository name in JFrog
- `JFROG_USERNAME`: JFrog username or email
- `JFROG_PASSWORD`: JFrog password or API token (recommended: use API token)

#### Security Scanning Secrets

- `SONAR_TOKEN`: SonarQube authentication token (optional, for code quality analysis)
- `SNYK_TOKEN`: Snyk authentication token (optional, for vulnerability scanning)


#### Azure Deployment Secrets

- `AZURE_SUBSCRIPTION_ID`: Azure subscription ID
- `AZURE_TENANT_ID`: Azure Active Directory tenant ID
- `AZURE_CLIENT_ID`: Service principal client ID
- `AZURE_CLIENT_SECRET`: Service principal client secret
- `AZURE_RESOURCE_GROUP`: Target resource group name
- `AZURE_APP_NAME`: Azure app service or function app name

## Setup Instructions

### Step 1: Copy Templates to Your Repository

1. Copy the generated CI template to: `.github/workflows/ci.yml`
2. Copy the generated CD template to: `.github/workflows/cd.yml`

### Step 2: Configure GitHub Secrets

Add all required secrets listed above to your GitHub repository.

### Step 3: Customize Templates

Review and customize the following sections in the templates:

#### CI Template Customization

- **Build Configuration**: Adjust the `BUILD_CONFIGURATION` environment variable
- **Version Strategy**: Modify the `APP_VERSION` calculation if needed
- **Test Commands**: Update test execution commands for your project structure
- **Package Structure**: Adjust packaging logic based on your application structure

#### CD Template Customization

- **Environment Protection**: Configure environment protection rules in GitHub
- **Deployment Paths**: Update deployment paths and configuration based on your infrastructure
- **Health Check**: Customize the health check endpoint and validation logic
- **Rollback Strategy**: Implement appropriate rollback procedures for your deployment type

### Step 4: Test the Pipelines

1. **Test CI Pipeline**:
   - Push code to a feature branch
   - Verify the CI pipeline runs successfully
   - Check that artifacts are published to JFrog

2. **Test CD Pipeline**:
   - Trigger the CD workflow manually from GitHub Actions
   - Select the target environment and version
   - Verify deployment completes successfully

## Workflow Usage

### CI Pipeline

The CI pipeline automatically runs on:
- Push to `main`, `develop`, or `feature/**` branches
- Pull requests to `main` or `develop`

You can also trigger it manually with options to skip tests.

### CD Pipeline

The CD pipeline is triggered manually using workflow_dispatch:

1. Go to Actions tab in GitHub
2. Select "CD - Deploy to Azure"
3. Click "Run workflow"
4. Select:
   - **Environment**: development, staging, or production
   - **Version**: Build number or version tag from JFrog
   - **Skip Health Check**: Optional, to skip post-deployment validation

## Best Practices

### Security

1. **Never hardcode secrets** in the YAML files
2. **Use GitHub Environments** with protection rules for production
3. **Rotate secrets regularly** and use API tokens instead of passwords
4. **Enable branch protection** rules for main branches

### Version Management

1. **Use semantic versioning** for releases
2. **Tag releases** in Git for easy rollback
3. **Keep artifacts** in JFrog for at least 30 days

### Deployment Strategy

1. **Test in development** first, then staging, then production
2. **Use blue-green deployment** or canary releases for zero-downtime deployments
3. **Have a rollback plan** ready before deploying to production
4. **Monitor applications** after deployment

## Troubleshooting

### Common Issues

#### CI Pipeline Fails at Build Step

- Verify the build configuration and SDK versions
- Check that all dependencies are accessible
- Review build logs for specific error messages

#### Artifacts Not Published to JFrog

- Verify JFrog credentials are correct
- Check that the repository exists and is accessible
- Ensure the JFrog CLI is properly configured

#### CD Pipeline Fails at Deployment

- Verify all deployment secrets are configured
- Check network connectivity to deployment target
- Review deployment logs for specific error messages


#### Azure-Specific Issues

- **Authentication Failed**: Verify service principal credentials and permissions
- **Resource Not Found**: Check resource group and app name are correct
- **Deployment Timeout**: Increase timeout settings or check Azure service health

## Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [JFrog Artifactory Documentation](https://www.jfrog.com/confluence/display/JFROG/JFrog+Artifactory)
- [Azure App Service Documentation](https://docs.microsoft.com/en-us/azure/app-service/)
- [Azure Functions Documentation](https://docs.microsoft.com/en-us/azure/azure-functions/)

## Support

For issues or questions about these templates:

1. Review the comments in the YAML files
2. Check the troubleshooting section above
3. Consult the official documentation for each tool
4. Contact your DevOps team for infrastructure-specific guidance

---

**Note**: These templates are generated based on your selections and may require adjustments
for your specific environment and requirements. Always test thoroughly in non-production
environments before deploying to production.
