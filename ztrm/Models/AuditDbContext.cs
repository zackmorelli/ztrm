using Microsoft.EntityFrameworkCore;

using ztrm.Models.Audit;

namespace ztrm.Models
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext(DbContextOptions<AuditDbContext> options) : base(options) { }

        public DbSet<AuditTrail> AuditTrails { get; set; }

    }
}
