﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sedio.Contracts;
using Sedio.Core.Timing;
using Sedio.Server.Runtime.Execution;
using Sedio.Server.Runtime.Execution.Commands;
using Sedio.Server.Runtime.Model;
using Sedio.Server.Runtime.Model.Components;

namespace Sedio.Server.Runtime.Api.Internal.Services
{
    public sealed class ServiceCreationCommand : AbstractCreationCommand<string,ServiceInputDto,Service>
    {
        public ServiceCreationCommand(string id, ServiceInputDto input) : base(id, input)
        {
        }

#pragma warning disable 1998
        protected override async Task<Expression<Func<Service, bool>>> OnGetFilterExpression(IExecutionContext context,
#pragma warning restore 1998
            string id)
        {
            return service => service.ServiceId == id;
        }

        protected override Task OnMapToEntity(IExecutionContext context, string id, ServiceInputDto source, Service target, bool isUpdate)
        {
            var timeProvider = context.Services.GetRequiredService<ITimeProvider>();

            target.CreatedAt = timeProvider.UtcNow;
            target.CacheTime = source.CacheTime;
            target.ServiceId = id;

            target.HealthAggregation = source.HealthAggregation.ToEntity<HealthAggregationConfiguration>();
           
            return Task.CompletedTask;
        }
    }
}