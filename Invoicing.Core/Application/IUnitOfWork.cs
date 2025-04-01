using Invoicing.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Core.Application
{
    public interface IUnitOfWork<T> : IDbContextScoped<T> where T : DbContext
    {
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
