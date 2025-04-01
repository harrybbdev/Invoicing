using Microsoft.EntityFrameworkCore;

namespace Invoicing.Core.Infrastructure
{
    public interface IDbContextScoped<T> where T : DbContext
    {
    }
}
