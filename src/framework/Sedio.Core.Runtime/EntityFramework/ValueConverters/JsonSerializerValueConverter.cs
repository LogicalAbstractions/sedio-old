using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace Sedio.Core.Runtime.EntityFramework.ValueConverters
{
    public sealed class JsonSerializerValueConverter<T> : ValueConverter<T,string>
    {
        public JsonSerializerValueConverter(JsonSerializerSettings serializerSettings)
            : base(arg => JsonConvert.SerializeObject(arg,serializerSettings),
                value => JsonConvert.DeserializeObject<T>(value,serializerSettings))
        {}
    }
}