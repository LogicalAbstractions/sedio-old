using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sedio.Contracts.Converters;

namespace Sedio.Server.Http.Json
{
    public class JsonConfiguration : IConfigureOptions<MvcJsonOptions>
    {
        public void Configure(MvcJsonOptions options)
        {
            options.SerializerSettings.UseContractTypes();
        }
    }
}