using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag.AspNetCore;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Contracts.Converters;
using Sedio.Core.Collections.Paging;
using Sedio.Server;
using Sedio.Server.Framework.Http.Binders;
using Sedio.Server.Framework.Swagger;

namespace Sedio
{
    public static class EntryPoint
    {
        public static async Task<int> Main(string[] arguments)
        {
            using (var host = new ServerHost(arguments))
            {
                await host.Run(CancellationToken.None);
                return 0;
            }
        }
    }
}
