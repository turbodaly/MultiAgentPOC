using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

public class AgentService
{
    private readonly OpenAIService _openAIService;
    private readonly ILogger<AgentService> _logger;

    public AgentService(OpenAIService openAIService, ILogger<AgentService> logger)
    {
        _openAIService = openAIService;
        _logger = logger;
    }

    // Compliance Evaluation
    public async Task<EvaluationResult> EvaluateComplianceAsync(ProjectData projectData)
    {
        var prompt = $"You are a compliance officer. Ensure that the project '{projectData.ProjectName}' meets all regulatory requirements. " +
                     $"Details: {projectData.RegulationDescription}. " +
                     $"Comments: {projectData.RegulationComments}. " +
                     $"Inspections done by: {(projectData.RegulationIsCompliant ? "All inspections passed" : "Inspections not passed")}. " +
                     $"Safety Check: {projectData.SafetyCheckDescription} - {projectData.SafetyCheckComments}. " +
                     $"Assessment: {projectData.AssessmentDescription} - {projectData.AssessmentComments}.";
        return await EvaluateAgentAsync("ComplianceAgent", prompt);
    }

    // Budget Evaluation
    public async Task<EvaluationResult> EvaluateBudgetAsync(ProjectData projectData)
    {
        var prompt = $"You are a budget analyst. Check if the project '{projectData.ProjectName}' is within the budget. " +
                     $"Total budget: {projectData.BudgetTotal}. Spent: {projectData.BudgetSpent}. " +
                     $"Comments: {projectData.BudgetComments}. " +
                     $"Resource: {projectData.ResourceName} - {projectData.ResourceComments}. " +
                     $"Milestone: {projectData.MilestoneDescription} - {projectData.MilestoneComments}.";
        return await EvaluateAgentAsync("BudgetAgent", prompt);
    }

    // Safety Evaluation
    public async Task<EvaluationResult> EvaluateSafetyAsync(ProjectData projectData)
    {
        var prompt = $"You are a safety officer. Evaluate the safety of the project '{projectData.ProjectName}'. " +
                     $"Safety Check: {projectData.SafetyCheckDescription}. " +
                     $"Comments: {projectData.SafetyCheckComments}.";
        return await EvaluateAgentAsync("SafetyAgent", prompt);
    }

    // Resource Evaluation
    public async Task<EvaluationResult> EvaluateResourcesAsync(ProjectData projectData)
    {
        var prompt = $"You are a resource manager. Assess the resource availability for the project '{projectData.ProjectName}'. " +
                     $"Resource: {projectData.ResourceName}. " +
                     $"Comments: {projectData.ResourceComments}. " +
                     $"Availability: {(projectData.ResourceAvailable ? "Available" : "Not Available")}.";
        return await EvaluateAgentAsync("ResourceAgent", prompt);
    }

    // Environmental Impact Evaluation
    public async Task<EvaluationResult> EvaluateEnvironmentalImpactAsync(ProjectData projectData)
    {
        var prompt = $"You are an environmental specialist. Assess the environmental impact of the project '{projectData.ProjectName}'. " +
                     $"Assessment: {projectData.AssessmentDescription}. " +
                     $"Comments: {projectData.AssessmentComments}. " +
                     $"Passed: {(projectData.AssessmentIsPassed ? "Yes" : "No")}.";
        return await EvaluateAgentAsync("EnvironmentalAgent", prompt);
    }

    private async Task<EvaluationResult> EvaluateAgentAsync(string agent, string prompt)
    {
        try
        {
            var response = await _openAIService.CallOpenAIAsync(prompt);
            var messageContent = ExtractMessageContent(response);
            _logger.LogInformation($"{agent} response: {{Response}}", messageContent);
            return new EvaluationResult { Agent = agent, IsMet = !messageContent.Contains("not compliant"), Message = messageContent, Conversation = prompt };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, $"Error calling OpenAI service for {agent.ToLower()} evaluation.");
            throw;
        }
    }

    private string ExtractMessageContent(string jsonResponse)
    {
        var jObject = JObject.Parse(jsonResponse);
        var messageContent = jObject["choices"]?.First?["message"]?["content"]?.ToString();
        return messageContent ?? string.Empty;
    }

    public async Task<string> GetFinalDecisionAsync(List<EvaluationResult> results)
    {
        var consensusMet = results.All(r => r.IsMet);
        var finalDecision = consensusMet ? "Approved" : "Not Approved";
        var conversationHistory = string.Join("\n", results.Select(r => $"Agent: {r.Agent}, Message: {r.Message}, Conversation: {r.Conversation}"));
        return $"Final Decision: {finalDecision}\n\nConversation History:\n{conversationHistory}";
    }
}
