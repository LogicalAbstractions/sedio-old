using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Clauses;
using Sedio.Core.Runtime.EntityFramework.ValueConverters;

namespace Sedio.Server.Runtime.Model.Components
{
    public abstract class AbstractProviderConfiguration
    {
        public string ProviderId { get; set; }
        
        public JObject Parameters { get; set; }
    }

    public static class AbstractProviderConfigurationExtensions
    {
        public static void OwnsOneProviderConfiguration<TEntity, TProviderConfiguration>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity,TProviderConfiguration>> navigationExpression,
            Action<ReferenceOwnershipBuilder<TEntity,TProviderConfiguration>> additionalConfiguration = null)
            where TEntity : class
            where TProviderConfiguration : AbstractProviderConfiguration
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (navigationExpression == null) throw new ArgumentNullException(nameof(navigationExpression));

            builder.OwnsOne(navigationExpression, innerBuilder =>
            {
                innerBuilder.Property(t => t.ProviderId).IsRequired().HasMaxLength(48);
                innerBuilder.Property(t => t.Parameters).HasConversion<JObjectValueConverter>();
                additionalConfiguration?.Invoke(innerBuilder);
            });
        }
    }
}