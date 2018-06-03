using System;
using System.Collections.Immutable;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Sedio.Core.Runtime.EntityFramework.Management.Sqlite
{
    public sealed class SqliteDbContextFactory<T> : IDbContextFactory<T>
        where T : DbContext
    {
        private readonly string path;

        public SqliteDbContextFactory(string path)
        {
            this.path = path;
        }

        public DbContextOptions<T> CreateOptions()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = path ?? "memory"
            };

            var connectionString = connectionStringBuilder.ConnectionString;
            
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<T>();
            dbContextOptionsBuilder.UseSqlite(connectionString, builder =>
            {
                
            });

            return dbContextOptionsBuilder.Options;
        }
       
        public T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), CreateOptions()) as T;
        }
    }
}