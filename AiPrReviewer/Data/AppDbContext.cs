using AiPrReviewer.Models;
using Microsoft.EntityFrameworkCore;

namespace AiPrReviewer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AuditLogEntry> AuditLogs { get; set; }
    }
}
