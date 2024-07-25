using System;

public class ProjectData
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectComments { get; set; } = string.Empty;
    public decimal BudgetTotal { get; set; }
    public decimal BudgetSpent { get; set; }
    public string BudgetComments { get; set; } = string.Empty;
    public int RegulationId { get; set; }
    public string RegulationDescription { get; set; } = string.Empty;
    public bool RegulationIsCompliant { get; set; }
    public string RegulationComments { get; set; } = string.Empty;
    public int ResourceId { get; set; }
    public string ResourceName { get; set; } = string.Empty;
    public bool ResourceAvailable { get; set; }
    public string ResourceComments { get; set; } = string.Empty;
    public int SafetyCheckId { get; set; }
    public string SafetyCheckDescription { get; set; } = string.Empty;
    public bool SafetyCheckIsPassed { get; set; }
    public string SafetyCheckComments { get; set; } = string.Empty;
    public int AssessmentId { get; set; }
    public string AssessmentDescription { get; set; } = string.Empty;
    public bool AssessmentIsPassed { get; set; }
    public string AssessmentComments { get; set; } = string.Empty;
    public DateTime TimelineStartDate { get; set; }
    public DateTime TimelineEndDate { get; set; }
    public int MilestoneId { get; set; }
    public string MilestoneDescription { get; set; } = string.Empty;
    public DateTime MilestoneDueDate { get; set; }
    public bool MilestoneCompleted { get; set; }
    public string MilestoneComments { get; set; } = string.Empty;
    public string ConversationHistory { get; set; } = string.Empty;
    public string FinalDecision { get; set; } = string.Empty;
}
