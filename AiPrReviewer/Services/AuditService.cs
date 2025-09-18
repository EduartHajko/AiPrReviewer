using AiPrReviewer.Data;
using AiPrReviewer.Models;
using System;

namespace AiPrReviewer.Services
{
    public class AuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public void Add(string comment, string filePath, int prNumber, string commitSha, string committer, string prTitle)
        {
            var entry = new AuditLogEntry
            {
                Comment = comment,
                FilePath = filePath,
                PrNumber = prNumber,
                CommitSha = commitSha,
                Committer = committer,
                PrTitle = prTitle,
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(entry);
            _context.SaveChanges();
        }

        public IEnumerable<AuditLogEntry> GetAll()
        {
            return _context.AuditLogs
                           .OrderByDescending(a => a.CreatedAt)
                           .ToList();
        }
    }
}

