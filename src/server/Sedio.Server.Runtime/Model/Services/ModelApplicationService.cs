using System;
using Sedio.Core.Runtime.Application;
using Sedio.Core.Runtime.EntityFramework.Management;

namespace Sedio.Server.Runtime.Model.Services
{
    public sealed class ModelApplicationService : IApplicationService
    {
        private readonly IDbContextManager<ModelDbContext> contextManager;

        public ModelApplicationService(IDbContextManager<ModelDbContext> contextManager)
        {
            this.contextManager = contextManager ?? throw new ArgumentNullException(nameof(contextManager));
        }

        public void OnStart()
        {
            contextManager.Initialize();
        }

        public void OnStop()
        {
           
        }
    }
}