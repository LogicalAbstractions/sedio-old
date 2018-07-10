using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sedio.Contracts;
using Sedio.Contracts.Components;
using Sedio.Core.Runtime.EntityFramework.Schema;

namespace Sedio.Server.Runtime.Model
{
    public class ServiceStatus
    {
        public class Schema : AbstractEntitySchema<ServiceStatus>
        {
            protected override void OnApply(EntityTypeBuilder<ServiceStatus> builder)
            {
                builder.HasIndex(s => s.Status);
            }
        }
        
        public long Id { get; set; }
        
        public StatusType Status { get; set; }
        
        public string Message { get; set; }
        
        public DateTimeOffset CreatedAt { get; set; }
        
        // Navigation properties
        
        public long ServiceInstanceId { get; set; }
        
        public ServiceInstance ServiceInstance { get; set; }
    }

    public static class ServiceStatusMappingExtensions
    {
        public static ServiceStatusOutputDto ToOutput(this ServiceStatus serviceStatus)
        {
            if (serviceStatus == null) throw new ArgumentNullException(nameof(serviceStatus));

            return new ServiceStatusOutputDto()
            {
                CreatedAt = serviceStatus.CreatedAt,
                Message = serviceStatus.Message,
                Status = serviceStatus.Status
            };
        }
    }

    public static class ServiceStatusQueryExtensions
    {
        public static async Task<ServiceStatus> FindMostRecentStatus(this DbSet<ServiceStatus> serviceStatus,ServiceInstance serviceInstance,
                                                                     CancellationToken cancellationToken,bool asNoTracking = false)
        {
            if (serviceStatus == null) throw new ArgumentNullException(nameof(serviceStatus));

            if (serviceInstance == null)
            {
                return null;
            }

            var queryable = asNoTracking ? serviceStatus.AsNoTracking() : serviceStatus;

            return await serviceStatus.Where(s => s.ServiceInstanceId == serviceInstance.Id)
                .OrderByDescending(s => s.CreatedAt).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}