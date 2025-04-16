using Invoicing.Core.Domain;
using Invoicing.Features.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Features.Billing.Infrastructure.DataAccess
{
    public class BillingDbContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BillingDbContext).Assembly);
        }

        public BillingDbContext(DbContextOptions options) : base(options) { }
    }
}
