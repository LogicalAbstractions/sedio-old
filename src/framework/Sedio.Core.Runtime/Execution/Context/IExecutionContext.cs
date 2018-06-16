using System;
using System.Collections.Generic;
using System.Threading;

namespace Sedio.Core.Runtime.Execution
{
    public interface IExecutionContext
    {
        string BranchId { get; }
        
        CancellationToken CancellationToken { get; }
        
        IServiceProvider Services { get; }
        
        IDictionary<string,object> Items { get; }
        
        IExecutionRequest Request { get; set; }
        
        IExecutionResponse Response { get; set; }
    }
}