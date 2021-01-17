using Microsoft.EntityFrameworkCore;

namespace CompanyManagementApi.Models
{
    public class CompanyContext : DbContext

    {
        public CompanyContext(DbContextOptions<CompanyContext> options) 
            : base (options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Employees)
                .WithOne()
                .OnDelete(DeleteBehavior.ClientCascade);
        }

    }
}
