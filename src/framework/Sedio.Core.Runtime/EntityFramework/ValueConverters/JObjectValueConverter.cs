using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;

namespace Sedio.Core.Runtime.EntityFramework.ValueConverters
{
    public sealed class JObjectValueConverter : ValueConverter<JObject,string>
    {
        public JObjectValueConverter() 
            : base(token => token.ToString(),value => JObject.Parse(value))
        {
        }
    }
}