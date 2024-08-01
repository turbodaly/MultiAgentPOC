using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EvaluationController : ControllerBase
{
    private readonly AgentService _agentService;
    private readonly ILogger<EvaluationController> _logger;

    public EvaluationController(AgentService agentService, ILogger<EvaluationController> logger)
    {
        _agentService = agentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> EvaluateProject([FromBody] ProjectData projectData)
    {
        _logger.LogInformation("Received evaluation request for project ID {ProjectId}", projectData.ProjectId);

        var complianceResult = await _agentService.EvaluateComplianceAsync(projectData);
        var budgetResult = await _agentService.EvaluateBudgetAsync(projectData);
        var safetyResult = await _agentService.EvaluateSafetyAsync(projectData);
        var resourceResult = await _agentService.EvaluateResourcesAsync(projectData);
        var environmentalResult = await _agentService.EvaluateEnvironmentalImpactAsync(projectData);

        var results = new List<EvaluationResult>
        {
            complianceResult,
            budgetResult,
            safetyResult,
            resourceResult,
            environmentalResult
        };

        var finalDecision = await _agentService.GetFinalDecisionAsync(results);

        _logger.LogInformation("Evaluation completed for project ID {ProjectId}. Final decision: {FinalDecision}", projectData.ProjectId, finalDecision);

        return Ok(new
        {
            projectData.ProjectId,
            projectData.ProjectName,
            finalDecision,
            conversationHistory = finalDecision
        });
    }
}
