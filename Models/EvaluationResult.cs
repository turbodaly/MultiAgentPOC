public class EvaluationResult
{
    public string Agent { get; set; } = string.Empty; // Initialized to avoid nullable warning
    public bool IsMet { get; set; }
    public string Message { get; set; } = string.Empty; // Initialized to avoid nullable warning
    public string Conversation { get; set; } = string.Empty; // Initialized to avoid nullable warning
}
