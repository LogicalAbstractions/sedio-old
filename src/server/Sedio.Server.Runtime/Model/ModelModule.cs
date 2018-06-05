using System.IO;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Sedio.Core.Runtime.Application;
using Sedio.Core.Runtime.EntityFramework.Management;
using Sedio.Core.Runtime.EntityFramework.Management.Sqlite;
using Sedio.Core.Runtime.Http;

namespace Sedio.Server.Runtime.Model
{
    public sealed class ModelModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var hostingEnvironment = c.Resolve<IHostingEnvironment>();
                var databaseDirectoryPath = Path.Combine(hostingEnvironment.ContentRootPath, "databases");

                return new SqliteDbContextManager<ModelDbContext>(databaseDirectoryPath, 100, 5);
            }).As<IDbContextManager<ModelDbContext>>().SingleInstance();

            builder.RegisterType<ModelApplicationService>().As<IApplicationService>().SingleInstance();
        }
    }
}