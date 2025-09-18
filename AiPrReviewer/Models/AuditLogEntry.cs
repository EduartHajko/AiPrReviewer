namespace AiPrReviewer.Models
{
    public class AuditLogEntry
    {
        public int Id { get; set; } 
        public string Comment { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int PrNumber { get; set; }
        public string CommitSha { get; set; } = string.Empty;
        public string Committer { get; set; } = string.Empty;
        public string PrTitle { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
