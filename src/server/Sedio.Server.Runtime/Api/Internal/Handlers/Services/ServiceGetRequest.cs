using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Runtime.Execution;
using Sedio.Core.Runtime.Execution.Handlers;
using Sedio.Core.Runtime.Execution.Requests;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Middleware;

namespace Sedio.Server.Runtime.Api.Internal.Handlers.Services
{
    public sealed class ServiceGetRequest : AbstractExecutionRequest
    {
        public sealed class Handler : AbstractExecutionHandler<ServiceGetRequest>
        {
            protected override async Task<IExecutionResponse> OnExecute(IExecutionContext context, ServiceGetRequest request)
            {
                var dbContext = context.DbContext();

                var service = await dbContext.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == request.Id, context.CancellationToken)
                    .ConfigureAwait(false);

                if (service != null)
                {
                    return Ok(service.ToOutput());
                }

                return NotFound();
            }
        }
        
        public ServiceGetRequest(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            Id = id;
        }

        public string Id { get; }
    }
}