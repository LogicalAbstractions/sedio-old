using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sedio.Core.Runtime.Http.Filters;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Execution
{
    public sealed class ExecutionModule : Autofac.Module
    {
        public class ExecutionOptions : IConfigureOptions<MvcOptions>
        {
            public void Configure(MvcOptions options)
            {
                options.Filters.Add<ExecutionContextFilter<ModelDbContext>>();
            }
        }
        
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<ExecutionOptions>().As<IConfigureOptions<MvcOptions>>().SingleInstance();
        }
    }
}