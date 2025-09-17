namespace AiPrReviewer.Models
{
    public class AuditLog
    {
        public string Comment { get; set; }
        public string FilePath { get; set; }
        public int PrNumber { get; set; }
        public string CommitSha { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
