using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Providers.HealthAggregation
{
    public interface IHealthAggregationProvider
    {
        Task<ServiceStatus> Aggregate(IReadOnlyList<ServiceStatus> inputStatus, CancellationToken cancellationToken);
    }
}