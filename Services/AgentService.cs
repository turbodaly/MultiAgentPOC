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

    public async Task<EvaluationResult> EvaluateComplianceAsync(ProjectData projectData)
    {
        var prompt = $"You are a compliance officer. Ensure that the project '{projectData.ProjectName}' meets all regulatory requirements. " +
                     $"Details: {projectData.RegulationDescription}. " +
                     $"Comments: {projectData.RegulationComments}. " +
                     $"Inspections done by: {(projectData.RegulationIsCompliant ? "All inspections passed" : "Inspections not passed")}. " +
                     $"Safety Check: {projectData.SafetyCheckDescription} - {projectData.SafetyCheckComments}. " +
                     $"Assessment: {projectData.AssessmentDescription} - {projectData.AssessmentComments}.";
        try
        {
            var response = await _openAIService.CallOpenAIAsync(prompt);
            var messageContent = ExtractMessageContent(response);
            _logger.LogInformation("ComplianceAgent response: {Response}", messageContent);
            return new EvaluationResult { Agent = "ComplianceAgent", IsMet = !messageContent.Contains("not compliant"), Message = messageContent, Conversation = prompt };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling OpenAI service for compliance evaluation.");
            throw;
        }
    }

    public async Task<EvaluationResult> EvaluateBudgetAsync(ProjectData projectData)
    {
        var prompt = $"You are a budget analyst. Check if the project '{projectData.ProjectName}' is within the budget. " +
                     $"Total budget: {projectData.BudgetTotal}. Spent: {projectData.BudgetSpent}. " +
                     $"Comments: {projectData.BudgetComments}. " +
                     $"Resource: {projectData.ResourceName} - {projectData.ResourceComments}. " +
                     $"Milestone: {projectData.MilestoneDescription} - {projectData.MilestoneComments}.";
        try
        {
            var response = await _openAIService.CallOpenAIAsync(prompt);
            var messageContent = ExtractMessageContent(response);
            _logger.LogInformation("BudgetAgent response: {Response}", messageContent);
            return new EvaluationResult { Agent = "BudgetAgent", IsMet = !messageContent.Contains("over budget"), Message = messageContent, Conversation = prompt };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error calling OpenAI service for budget evaluation.");
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
