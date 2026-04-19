# GitHub Actions Workflow Override Checklist

This checklist documents the workflow inputs you may need to override for reusable builds and deployments.

## Main `build.yml` inputs
Use these when triggering `build.yml` manually or when reusing it in another repository.

- `environment`
  - `none`, `development`, `staging`, `production`
- `app-name`
  - Example: `MyTestApp`
- `jfrog-repository`
  - Example: `my-app-local`
- `build-configuration`
  - Example: `Release`
- `dotnet-version`
  - Example: `8.x`
- `source-directory`
  - Example: `test-dotnet-app`
- `project-file`
  - Example: `test-dotnet-app/src/MyTestApp.Api/MyTestApp.Api.csproj`
- `test-project-file`
  - Example: `test-dotnet-app`
- `publish-directory`
  - Example: `test-dotnet-app/publish`
- `snyk-scan-directory`
  - Example: `test-dotnet-app/src/MyTestApp.Api`
- `dev-resource-group`
  - Azure resource group name
- `dev-resource-name`
  - Azure web app / function app name
- `resource-type`
  - `webapp` or `functionapp`
- `skip_tests`
  - `true` or `false`

## `reusable-ci-net.yml` inputs
These should be provided by `build.yml` or manually if you invoke the reusable CI workflow directly.

- `app-name`
- `build-configuration`
- `NET-version`
- `skip-tests`
- `skip-security-scan`
- `runner`
- `jfrog-repository`
- `source-directory`
- `project-file`
- `test-project-file`
- `publish-directory`
- `snyk-scan-directory`

## `reusable-cd-azure.yml` inputs
Use these values when invoking the deploy workflow.

- `app-name`
- `version`
  - Provided by build job output
- `environment`
  - `development`, `staging`, `production`
- `skip-health-check`
  - `true` or `false`
- `runner`
- `resource-group`
- `resource-name`
- `jfrog-repository`
- `resource-type`
  - `webapp` or `functionapp`
- `checkout-paths`
  - Default:
    ```yaml
    deployment/
    scripts/
    ```
  - Override if your repo needs different sparse-checkout paths.

## Current repo default values
These defaults are set in the current `build.yml` file.

- `app-name`: `MyTestApp`
- `jfrog-repository`: `my-app-local`
- `build-configuration`: `Release`
- `dotnet-version`: `8.x`
- `source-directory`: `test-dotnet-app`
- `project-file`: `test-dotnet-app/src/MyTestApp.Api/MyTestApp.Api.csproj`
- `test-project-file`: `test-dotnet-app`
- `publish-directory`: `test-dotnet-app/publish`
- `snyk-scan-directory`: `test-dotnet-app/src/MyTestApp.Api`
- `resource-type`: `webapp`

## When to override inputs
Override inputs when:

- your repository layout differs from the current folder structure
- your project file is in a different location
- tests are run from a different folder or file
- your publish output directory is not the current default
- Snyk should scan a different application folder
- your Azure target resource group or app name changes
- your JFrog repository name is different

## Example override usage
When manually dispatching the workflow, set values similar to:

```yaml
app-name: MyTestApp
jfrog-repository: my-app-local
build-configuration: Release
project-file: test-dotnet-app/src/MyTestApp.Api/MyTestApp.Api.csproj
publish-directory: test-dotnet-app/publish
snyk-scan-directory: test-dotnet-app/src/MyTestApp.Api
resource-type: webapp
```

If you want, I can also add a second file with a direct `workflow_dispatch` example for this repo.