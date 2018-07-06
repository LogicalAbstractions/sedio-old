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
                    .FirstOrDefaultAsync(s => s.ServiceId == request.ServiceId,context.CancellationToken)
                    .ConfigureAwait(false);

                var successResult = Updated(new
                {
                    serviceId = request.ServiceId
                },model: service.ToOutput());
                
                if (service == null)
                {
                    // Create a new one:
                    service = new Service {CreatedAt = context.Services.GetRequiredService<ITimeProvider>().UtcNow};

                    dbContext.Services.Add(service);
                    successResult = Created(new
                    {
                        serviceId = request.ServiceId
                    },model: service.ToOutput());
                }

                service.CacheTime = request.Input.CacheTime;
                service.HealthAggregation = request.Input.HealthAggregation?.ToEntity<HealthAggregationConfiguration>();
                service.ServiceId = request.ServiceId;

                dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);

                return successResult;
            }
        }
        
        public ServiceCreationOrUpdateRequest(string serviceId, ServiceInputDto input) 
            : base(ExecutionRequestType.Mutation)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            Input = input ?? throw new ArgumentNullException(nameof(input));
        }
        
        public string ServiceId { get; }
        
        public ServiceInputDto Input { get; }
    }
}