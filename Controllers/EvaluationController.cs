using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        var results = new List<EvaluationResult>();

        try
        {
            results.Add(await _agentService.EvaluateComplianceAsync(projectData));
            results.Add(await _agentService.EvaluateBudgetAsync(projectData));
            // Call other agents similarly...
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error occurred while evaluating project ID {ProjectId}", projectData.ProjectId);
            return StatusCode(500, "Internal server error");
        }

        var finalDecision = await _agentService.GetFinalDecisionAsync(results);
        projectData.ConversationHistory = finalDecision;
        projectData.FinalDecision = finalDecision.Contains("Approved") ? "Approved" : "Not Approved";

        _logger.LogInformation("Evaluation completed for project ID {ProjectId}. Final decision: {FinalDecision}", projectData.ProjectId, projectData.FinalDecision);

        return Ok(new {
            ProjectId = projectData.ProjectId,
            ProjectName = projectData.ProjectName,
            FinalDecision = projectData.FinalDecision,
            ConversationHistory = projectData.ConversationHistory
        });
    }
}
