using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Runtime.EntityFramework.Schema;

namespace Sedio.Server.Runtime.Model
{
    public class ModelDbContext : DbContext
    {
        private static readonly AbstractEntitySchema[] schemata = new AbstractEntitySchema[]
        {
            new Service.Schema(),
            new ServiceVersion.Schema(), 
            new ServiceInstance.Schema(), 
            new ServiceStatus.Schema(),
            new ServiceDependency.Schema(),
            new ServiceEndpoint.Schema(), 
        };

        public ModelDbContext()
            : this("d:\\temp\\sedio.sqlite3")
        {
            
        }

        public ModelDbContext(string path)
            : base(CreateOptions(path))
        {
            
        }
        
        public DbSet<Service> Services { get; set; }
        
        public DbSet<ServiceVersion> ServiceVersions { get; set; }
        
        public DbSet<ServiceInstance> ServiceInstances { get; set; }
        
        public DbSet<ServiceStatus> ServiceStatus { get; set; }
        
        public DbSet<ServiceEndpoint> ServiceEndpoints { get; set; }
        
        public DbSet<ServiceDependency> ServiceDependencies { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var schema in schemata)
            {
                schema.Apply(modelBuilder);
            }  
        }

        private static DbContextOptions<ModelDbContext> CreateOptions(string path)
        {
            var connectionString = new SqliteConnectionStringBuilder()
            {
                DataSource = path,
            }.ConnectionString;
            
            return new DbContextOptionsBuilder<ModelDbContext>().UseSqlite(connectionString,
                builder =>
                {
                    
                }).Options;
        }
    }
}