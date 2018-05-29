using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sedio.Contracts.Components;

namespace Sedio.Core.Runtime.EntityFramework.ValueConverters
{
    public sealed class ServiceIdValueConverter : ValueConverter<ServiceId,string>
    {
        public ServiceIdValueConverter()
            : base(id => id.ToString(),value => new ServiceId(value))
        {
        }
    }
}