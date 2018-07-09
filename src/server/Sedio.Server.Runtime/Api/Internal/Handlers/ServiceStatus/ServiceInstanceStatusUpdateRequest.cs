using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Versioning;
using Sedio.Contracts;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Core.Timing;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.ServiceStatus
{
    public sealed class ServiceInstanceStatusUpdateRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceInstanceStatusUpdateRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceInstanceStatusUpdateRequest request)
            {
                var dbContext = context.DbContext();
                var timeProvider = context.Services.GetRequiredService<ITimeProvider>();
                
                var service = await dbContext.Services.FindService(request.ServiceId, context.CancellationToken)
                    .ConfigureAwait(false);

                var serviceVersion = await dbContext.ServiceVersions
                    .FindServiceVersion(service, request.ServiceVersion, context.CancellationToken)
                    .ConfigureAwait(false);

                var serviceInstance = await dbContext.ServiceInstances
                    .FindServiceInstance(serviceVersion, request.ServiceInstanceAddress, context.CancellationToken)
                    .ConfigureAwait(false);

                if (serviceInstance != null)
                {
                    var newStatus = new Model.ServiceStatus()
                    {
                        CreatedAt = timeProvider.UtcNow,
                        Message = request.Input.Message,
                        ServiceInstance = serviceInstance,
                        ServiceInstanceId = serviceInstance.Id,
                        Status = request.Input.Status
                    };

                    if (request.MaxHistorySize != null)
                    {
                        await ClampHistory(dbContext, serviceInstance.Id, request.MaxHistorySize.Value,
                            context.CancellationToken).ConfigureAwait(false);
                    }

                    dbContext.ServiceStatus.Add(newStatus);

                    await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
                }

                return NotFound();
            }
        }

        private static async Task ClampHistory(ModelDbContext dbContext,long serviceInstanceId, int maxHistorySize,CancellationToken cancellationToken)
        {
            var statusEntryCount = await dbContext.ServiceStatus.Where(s => s.ServiceInstanceId == serviceInstanceId)
                .LongCountAsync(cancellationToken).ConfigureAwait(false);

            var entriesToDeleteCount = statusEntryCount - maxHistorySize;

            if (entriesToDeleteCount > 0)
            {
                var entriesToDelete = await dbContext.ServiceStatus.Where(s => s.ServiceInstanceId == serviceInstanceId)
                    .OrderBy(s => s.CreatedAt).Take((int) entriesToDeleteCount).ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (entriesToDelete.Any())
                {
                    dbContext.RemoveRange(entriesToDelete);
                }
            }
        }
        
        public ServiceInstanceStatusUpdateRequest(string serviceId, SemanticVersion serviceVersion, IPAddress serviceInstanceAddress, ServiceStatusInputDto input, int? maxHistorySize)
            : base(ExecutionRequestType.Mutation)
        {
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceId));
            
            ServiceId = serviceId;
            ServiceVersion = serviceVersion ?? throw new ArgumentNullException(nameof(serviceVersion));
            ServiceInstanceAddress = serviceInstanceAddress ?? throw new ArgumentNullException(nameof(serviceInstanceAddress));
            Input = input ?? throw new ArgumentNullException(nameof(input));
            MaxHistorySize = maxHistorySize;
        }
        
        public string ServiceId { get; }
        
        public SemanticVersion ServiceVersion { get; }
        
        public IPAddress ServiceInstanceAddress { get; }
        
        public ServiceStatusInputDto Input { get; }
        
        public int? MaxHistorySize { get; }
    }
}