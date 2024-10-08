# MultiAgentPOC

## Overview
MultiAgentPOC is a proof-of-concept project that demonstrates the use of multiple agents to evaluate construction projects. This project leverages Azure OpenAI to provide insights into compliance, budgeting, and overall project health. It is built with ASP.NET Core and deployed on Azure App Services.

## Features
- **Compliance Evaluation**: Ensures the project meets all regulatory requirements.
- **Budget Evaluation**: Checks if the project is within the allocated budget.
- **Safety and Resource Assessment**: Evaluates safety checks and resource availability.
- **Impact Assessment**: Assesses the impact on local wildlife and environment.

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) installed on your development machine.
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed and logged in to your Azure account.
- [Postman](https://www.postman.com/downloads/) for API testing.

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/turbodaly/MultiAgentPOC.git
    cd MultiAgentPOC
    ```

2. Install the required .NET packages:
    ```sh
    dotnet restore
    ```

3. Set up the Azure OpenAI keys in `appsettings.json`:
    ```json
    {
        "AzureOpenAI": {
            "Endpoint": "https://your-openai-endpoint",
            "Key": "your-openai-key"
        }
    }
    ```

### Running Locally

1. Run the application:
    ```sh
    dotnet run
    ```

2. The API will be available at `http://localhost:5000` or `https://localhost:5001`.

### Deployment to Azure

1. Deploy the app to Azure:
    ```sh
    az webapp up --name MultiAgentPOC --resource-group MultiAgentPOC --plan ASP-MultiAgentPOC-8015 --location uksouth --sku FREE
    ```

2. Your application will be accessible at:
    ```
    https://multiagentpoc-cvgvh5d8eba2a4g6.uksouth-01.azurewebsites.net
    ```

### Testing the API

1. Open Postman and create a new POST request.
2. Set the URL to:
    ```
    https://multiagentpoc-cvgvh5d8eba2a4g6.uksouth-01.azurewebsites.net/api/evaluation
    ```
3. Add the following headers:
    - `Content-Type: application/json`
4. Add the following body:
    ```json
    {
        "ProjectId": 1,
        "ProjectName": "Residential Building Project",
        "ProjectComments": "Initial phase completed without any major issues. Waiting for the next funding round.",
        "BudgetTotal": 500000,
        "BudgetSpent": 450000,
        "BudgetComments": "On track with budget. Additional costs might be incurred due to unforeseen circumstances.",
        "RegulationId": 1,
        "RegulationDescription": "Building Code Compliance",
        "RegulationIsCompliant": true,
        "RegulationComments": "All codes followed correctly. Inspections done by John Doe.",
        "ResourceId": 1,
        "ResourceName": "Cranes",
        "ResourceAvailable": true,
        "ResourceComments": "All cranes operational. Maintenance scheduled for next month.",
        "SafetyCheckId": 1,
        "SafetyCheckDescription": "Site Safety Inspection",
        "SafetyCheckIsPassed": true,
        "SafetyCheckComments": "No safety issues found during the inspection by Safety Officer Mary.",
        "AssessmentId": 1,
        "AssessmentDescription": "Impact on Local Wildlife",
        "AssessmentIsPassed": true,
        "AssessmentComments": "No negative impact on wildlife. Assessment by Environmental Specialist Dr. Smith.",
        "TimelineStartDate": "2023-01-01",
        "TimelineEndDate": "2024-01-01",
        "MilestoneId": 1,
        "MilestoneDescription": "Foundation Complete",
        "MilestoneDueDate": "2023-06-01",
        "MilestoneCompleted": true,
        "MilestoneComments": "Foundation solid and up to code. Completed ahead of schedule."
    }
    ```

### cURL Request

You can also use the following curl command to test the API:

```sh
curl --location 'https://multiagentpoc-cvgvh5d8eba2a4g6.uksouth-01.azurewebsites.net/api/evaluation' \
--header 'Content-Type: application/json' \
--data '{
    "ProjectId": 1,
    "ProjectName": "Residential Building Project",
    "ProjectComments": "Initial phase completed without any major issues. Waiting for the next funding round.",
    "BudgetTotal": 500000,
    "BudgetSpent": 450000,
    "BudgetComments": "On track with budget. Additional costs might be incurred due to unforeseen circumstances.",
    "RegulationId": 1,
    "RegulationDescription": "Building Code Compliance",
    "RegulationIsCompliant": true,
    "RegulationComments": "All codes followed correctly. Inspections done by John Doe.",
    "ResourceId": 1,
    "ResourceName": "Cranes",
    "ResourceAvailable": true,
    "ResourceComments": "All cranes operational. Maintenance scheduled for next month.",
    "SafetyCheckId": 1,
    "SafetyCheckDescription": "Site Safety Inspection",
    "SafetyCheckIsPassed": true,
    "SafetyCheckComments": "No safety issues found during the inspection by Safety Officer Mary.",
    "AssessmentId": 1,
    "AssessmentDescription": "Impact on Local Wildlife",
    "AssessmentIsPassed": true,
    "AssessmentComments": "No negative impact on wildlife. Assessment by Environmental Specialist Dr. Smith.",
    "TimelineStartDate": "2023-01-01",
    "TimelineEndDate": "2024-01-01",
    "MilestoneId": 1,
    "MilestoneDescription": "Foundation Complete",
    "MilestoneDueDate": "2023-06-01",
    "MilestoneCompleted": true,
    "MilestoneComments": "Foundation solid and up to code. Completed ahead of schedule."
}'
