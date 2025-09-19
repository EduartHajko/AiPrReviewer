using AiPrReviewer.Data;
using AiPrReviewer.Helpers;
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

        public void Add(string comment, string filePath, int prNumber, string commitSha, string committer, string prTitle, string codeSnippet)
        {
            var entry = new AuditLogEntry
            {
                Comment = comment,
                FilePath = filePath,
                PrNumber = prNumber,
                CommitSha = commitSha,
                Committer = committer,
                PrTitle = prTitle,
                CodeSnippet = codeSnippet,
                CreatedAt = DateTime.UtcNow
            };

            _context.AuditLogs.Add(entry);
            _context.SaveChanges();
        }

        public PagedResult<AuditLogEntry> GetPaged(int page, int pageSize = 10)
        {
            var query = _context.AuditLogs.OrderByDescending(x => x.CreatedAt);

            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<AuditLogEntry>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public IEnumerable<AuditLogEntry> GetAll()
        {
            return _context.AuditLogs.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}

