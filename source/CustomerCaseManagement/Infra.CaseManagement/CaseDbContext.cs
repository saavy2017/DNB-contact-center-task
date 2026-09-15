using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Domain.CaseManagement.Entity;

namespace Infra.CaseManagement
{
    public class CaseDbContext : DbContext
    {
        public CaseDbContext(DbContextOptions<CaseDbContext> options) : base(options)
        {            

        }

        public DbSet<SupportCase> SupportCase { get; set; }
    }
}
