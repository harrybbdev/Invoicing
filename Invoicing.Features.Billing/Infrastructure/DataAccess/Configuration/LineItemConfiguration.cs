using Invoicing.Features.Billing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Features.Billing.Infrastructure.DataAccess.Configuration
{
    public class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
    {
        public void Configure(EntityTypeBuilder<LineItem> builder)
        {
            builder.ComplexProperty(b => b.Quantity);
            builder.ComplexProperty(b => b.Price);
            builder.ComplexProperty(b => b.Tax);
        }
    }
}
