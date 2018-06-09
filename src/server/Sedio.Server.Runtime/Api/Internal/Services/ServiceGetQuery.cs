using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sedio.Contracts;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Queries;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Api.Internal.Services
{
    public sealed class ServiceGetQuery : AbstractGetQuery<string,Service,ServiceOutputDto>
    {
        public ServiceGetQuery(string id) 
            : base(id)
        {}

        protected override async Task<Expression<Func<Service, bool>>> OnGetFilterExpression(IExecutionContext context, string id)
        {
            return service => service.ServiceId == id;
        }

        protected override ServiceOutputDto OnMapToOutput(IExecutionContext context, string id, Service entity)
        {
            return entity.ToOutput();
        }
    }
}