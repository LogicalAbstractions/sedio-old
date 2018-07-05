using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Contracts;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Core.Timing;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Components;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Services
{
    public sealed class ServiceCreationOrUpdateRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceCreationOrUpdateRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceCreationOrUpdateRequest request)
            {
                var dbContext = context.DbContext();

                var service = await dbContext.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == request.Id,context.CancellationToken)
                    .ConfigureAwait(false);

                var successResult = Updated();
                
                if (service == null)
                {
                    // Create a new one:
                    service = new Service {CreatedAt = context.Services.GetRequiredService<ITimeProvider>().UtcNow};

                    dbContext.Services.Add(service);
                    successResult = Created(request.Id, "Get");
                }

                service.CacheTime = request.Input.CacheTime;
                service.HealthAggregation = request.Input.HealthAggregation?.ToEntity<HealthAggregationConfiguration>();
                service.ServiceId = request.Id;

                dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

                return successResult;
            }
        }
        
        public ServiceCreationOrUpdateRequest(string id, ServiceInputDto input) 
            : base(ExecutionRequestType.Mutation)
        {
            Id = id;
            Input = input;
        }
        
        public string Id { get; }
        
        public ServiceInputDto Input { get; }
    }
}