MyTestApp - Purpose & Features
Primary Purpose:
A simple .NET Web API test application created specifically to validate GitHub Actions CI/CD pipeline templates. It's a demo/reference app, not a production application.

What It Does:

API Endpoints:

GET /api/health - Returns health status of the application

Response includes: status, timestamp, version, service name
Used by CD pipeline to verify successful deployments
GET /api/health/version - Returns version and build information

Shows version number and build number from environment variables
GET /weatherforecast - Sample weather forecast data (demo endpoint)

Returns 5 days of random weather data
Testing & CI/CD:

Includes unit tests (xUnit) for the health controller
Designed to work with the GitHub Actions workflows in the .github folder
CI pipeline: builds, tests, packages, publishes to JFrog Artifactory
CD pipeline: downloads from JFrog, deploys to Azure App Service, runs health checks
Technology Stack:

ASP.NET Core Web API (.NET 9.0)
Minimal API + Controllers
OpenAPI/Swagger support
This is essentially a "Hello World" CI/CD reference implementation - a working example to demonstrate how to build, test, and deploy a .NET app using the GitHub Actions templates in this repository.