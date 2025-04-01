using Microsoft.EntityFrameworkCore;

namespace Invoicing.Core.Infrastructure.Outbox.DataAccess
{
    public class OutboxDbContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OutboxDbContext).Assembly);
        }

        public OutboxDbContext(DbContextOptions options) : base(options) { }
    }
}
