using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sedio.Core.Runtime.EntityFramework.Schema
{
    public abstract class AbstractEntitySchema
    {
        public void Apply(ModelBuilder modelBuilder)
        {
            OnApply(modelBuilder);
        }

        protected abstract void OnApply(ModelBuilder modelBuilder);
    }

    public abstract class AbstractEntitySchema<TEntity> : AbstractEntitySchema
        where TEntity : class 
    {
        protected sealed override void OnApply(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<TEntity>();
            
            OnApply(entityBuilder);
        }
      
        protected abstract void OnApply(EntityTypeBuilder<TEntity> builder);
    }
}