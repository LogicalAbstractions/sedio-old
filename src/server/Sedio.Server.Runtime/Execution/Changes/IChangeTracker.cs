using System.Threading.Tasks;
using Sedio.Server.Runtime.Model;
using Service = Autofac.Core.Service;

namespace Sedio.Server.Runtime.Execution.Changes
{
    public interface IChangeTracker
    {
        Task OnServiceCreated(IExecutionContext context, Service service);

        Task OnServiceVersionCreated(IExecutionContext context, ServiceVersion serviceVersion);
    }
}