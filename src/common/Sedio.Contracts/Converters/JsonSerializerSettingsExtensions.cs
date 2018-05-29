using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sedio.Core.Collections.Paging;
using Sedio.Core.Converters;

namespace Sedio.Contracts.Converters
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings UseContractTypes(this JsonSerializerSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            
            settings.Converters.Add(new PagingCursorJsonConverter());
            settings.Converters.Add(new IpAddressJsonConverter());
            settings.Converters.Add(new SemanticVersionJsonConverter());
            settings.Converters.Add(new ServiceIdJsonConverter());
            settings.Converters.Add(new VersionRangeJsonConverter());
            settings.Converters.Add(new StringEnumConverter(true));
            settings.Converters.Add(new TimespanJsonConverter());

            return settings;
        }
    }
}