using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using NJsonSchema.Infrastructure;
using Sedio.Core.Runtime.EntityFramework.Schema;
using Sedio.Core.Runtime.EntityFramework.ValueConverters;
using Sedio.Server.Runtime.Model.Components;

namespace Sedio.Server.Runtime.Model
{
    public class ModelDbContext : DbContext
    {
        private static readonly AbstractEntitySchema[] schemata = new[]
        {
            new Service.Schema(),
        };
        
        public ModelDbContext()
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var schema in schemata)
            {
                schema.Apply(modelBuilder);
            }  
        }
    }
}