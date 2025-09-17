using AiPrReviewer.Models;

namespace AiPrReviewer.Services
{
    public class AuditService
    {
        private readonly List<AuditLog> _logs = new();

        public void Add(string comment, string filePath, int prNumber, string sha)
        {
            _logs.Add(new AuditLog
            {
                Comment = comment,
                FilePath = filePath,
                PrNumber = prNumber,
                CommitSha = sha
            });
        }

        public IEnumerable<AuditLog> GetAll() => _logs.OrderByDescending(l => l.Timestamp);
    }

}
