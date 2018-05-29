using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sedio.Contracts.Converters;

namespace Sedio.Core.Runtime.Http.Json
{
    public class JsonConfiguration : IConfigureOptions<MvcJsonOptions>
    {
        public void Configure(MvcJsonOptions options)
        {
            options.SerializerSettings.UseContractTypes();
        }
    }
}