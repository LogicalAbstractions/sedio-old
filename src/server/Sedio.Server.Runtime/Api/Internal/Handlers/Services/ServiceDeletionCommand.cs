using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Services
{
    public sealed class ServiceDeletionCommand : AbstractDeletionCommand<string,Service>
    {
        public ServiceDeletionCommand(string id)
            : base(id)
        {}

        protected override async Task<Expression<Func<Service, bool>>> OnGetFilterExpression(IExecutionContext context, string id)
        {
            return service => service.ServiceId == id;
        }
    }
}