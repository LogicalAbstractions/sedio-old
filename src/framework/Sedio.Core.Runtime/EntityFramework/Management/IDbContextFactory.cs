using Microsoft.EntityFrameworkCore;

namespace Sedio.Core.Runtime.EntityFramework.Management
{
    public interface IDbContextFactory<T>
        where T : DbContext
    {
        T CreateInstance();

        DbContextOptions<T> CreateOptions();
    }
}