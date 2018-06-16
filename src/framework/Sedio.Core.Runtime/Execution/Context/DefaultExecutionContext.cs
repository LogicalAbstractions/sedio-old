using System;
using System.Collections.Generic;
using System.Threading;

namespace Sedio.Core.Runtime.Execution
{
    public sealed class DefaultExecutionContext : IExecutionContext
    {
        internal DefaultExecutionContext(string branchId, CancellationToken cancellationToken, IServiceProvider services)
        {
            BranchId = branchId;
            CancellationToken = cancellationToken;
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Items = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

        public string BranchId { get; }

        public CancellationToken CancellationToken { get; }

        public IServiceProvider Services { get; }

        public IDictionary<string, object> Items { get; }

        public IExecutionRequest Request { get; set; }

        public IExecutionResponse Response { get; set; }
    }
}