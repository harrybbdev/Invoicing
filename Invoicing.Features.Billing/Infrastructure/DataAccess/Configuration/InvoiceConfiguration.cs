using Invoicing.Features.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Features.Billing.Infrastructure.DataAccess.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.OwnsMany(i => i.LineItems, li =>
            {
                li.OwnsOne(x => x.UnitPrice, up =>
                {
                    up.Property(x => x.Value).IsRequired();
                });

                li.OwnsOne(x => x.UnitQuantity, up =>
                {
                    up.Property(x => x.Value).IsRequired();
                });

                li.OwnsOne(x => x.Tax, up =>
                {
                    up.Property(x => x.TaxPercentage).IsRequired();
                });
            });

            builder.Property(i => i.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(i => i.CancellationReason)
                .HasMaxLength(200);

            builder.HasIndex(i => i.UniqueId);
        }
    }
}
