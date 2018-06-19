using System;
using System.Threading.Tasks;

namespace Sedio.Core.Runtime.Execution.Responses
{
    public sealed class FluentExecutionResponse : AbstractExecutionResponse<FluentExecutionResponse>
    {
        public sealed class Builder
        {
            private readonly FluentExecutionResponse owner;

            internal Builder(FluentExecutionResponse owner)
            {
                this.owner = owner ?? throw new ArgumentNullException(nameof(owner));
            }

            public Builder TransformOnTo<TContext, TResult>(Func<ExecutionResponseTransformContext<TContext>, FluentExecutionResponse, TResult> transformDelegate)
                where TContext : class
            {
                owner.RegisterTransform(new DelegateTransform<TContext,TResult>(transformDelegate));
                return this;
            }

            public Builder TransformOnTo<TContext, TResult>(
                Func<ExecutionResponseTransformContext<TContext>, TResult> transformDelegate)
                where TContext : class
            {
                owner.RegisterTransform(new DelegateTransform<TContext, TResult>(transformDelegate));
                return this;
            }

            public Builder TransformOnTo<TContext, TResult>(Func<ExecutionResponseTransformContext<TContext>, FluentExecutionResponse, Task<TResult>> transformDelegate)
                where TContext : class
            {
                owner.RegisterTransform(new DelegateTransform<TContext,TResult>(transformDelegate));
                return this;
            }

            public Builder TransformOnTo<TContext, TResult>(
                Func<ExecutionResponseTransformContext<TContext>, Task<TResult>> transformDelegate)
                where TContext : class
            {
                owner.RegisterTransform(new DelegateTransform<TContext, TResult>(transformDelegate));
                return this;
            }
        }

        public FluentExecutionResponse(IExecutionCachingPolicy cachingPolicy,Action<Builder> buildAction)
            : base(cachingPolicy)
        {
            if (buildAction == null) throw new ArgumentNullException(nameof(buildAction));
            
            buildAction.Invoke(new Builder(this));
        }
    }
}