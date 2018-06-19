using Sedio.Contracts;
using Sedio.Core.Collections.Paging;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Services
{
    public sealed class ServiceListQuery : AbstractListQuery<Service,ServiceOutputDto>
    {
        public ServiceListQuery(PagingParameters pagingParameters) 
            : base(pagingParameters)
        {}

        protected override ServiceOutputDto OnMapToOuput(IExecutionContext context, Service entity)
        {
            return entity.ToOutput();
        }
    }
}