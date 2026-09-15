using Domain.CaseManagement.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.CaseManagement
{
    public class CaseDbContext : DbContext
    {
        public CaseDbContext(DbContextOptions<CaseDbContext> options) : base(options)
        {

        }

        public DbSet<SupportCase> SupportCase { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SupportCase>().HasIndex(s => s.ReferenceNumber).IsUnique();
        }
    }
}
