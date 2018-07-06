using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Services
{
    public sealed class ServiceDeletionRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceDeletionRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceDeletionRequest request)
            {
                var dbContext = context.DbContext();

                var service = await dbContext.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId, context.CancellationToken)
                    .ConfigureAwait(false);

                if (service != null)
                {
                    dbContext.Services.Remove(service);
                    
                    await dbContext
                        .SaveChangesAsync(context.CancellationToken)
                        .ConfigureAwait(false);
                   
                    return Deleted();
                }

                return NotFound();
            }
        }
        
        public ServiceDeletionRequest(string serviceId)
            : base(ExecutionRequestType.Mutation)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
        }

        public string ServiceId { get; }
    }
}