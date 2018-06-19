using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sedio.Core.Threading;

namespace Sedio.Core.Runtime.Execution.Responses
{
    public abstract class AbstractExecutionResponse<TResponse> : IExecutionResponse
        where TResponse : AbstractExecutionResponse<TResponse>
    {
        public interface ITransform
        {
            Type ContextType { get; }
            
            Type ResultType { get; }

            Task<object> Transform(object context, TResponse self);
        }
        
        public sealed class DelegateTransform<TContext,TResult> : ITransform
            where TContext : class
        {
     
            private readonly Func<ExecutionResponseTransformContext<TContext>,TResponse,Task<TResult>> transformDelegate;

            public DelegateTransform(Func<ExecutionResponseTransformContext<TContext>, TResponse,TResult> transformDelegate)
                : this(transformDelegate.WrapInAsync())
            {
                
            }
            
            public DelegateTransform(Func<ExecutionResponseTransformContext<TContext>,TResult> transformDelegate)
                : this(transformDelegate.WrapInAsync())
            {}
            
            public DelegateTransform(Func<ExecutionResponseTransformContext<TContext>, Task<TResult>> transformDelegate)
                : this((context, self) => transformDelegate.Invoke(context))
            {
                
            } 
            
            public DelegateTransform(Func<ExecutionResponseTransformContext<TContext>,TResponse,Task<TResult>> transformDelegate)
            {
                this.transformDelegate = transformDelegate ?? throw new ArgumentNullException(nameof(transformDelegate));
            }

            public Type ContextType { get; } = typeof(TContext);

            public Type ResultType { get; } = typeof(TResult);

            public async Task<object> Transform(object context, TResponse self)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));
                if (self == null) throw new ArgumentNullException(nameof(self));

                return await transformDelegate.Invoke((ExecutionResponseTransformContext<TContext>) context, self)
                    .ConfigureAwait(false);
            }
        }

        private readonly Dictionary<Type,ITransform> transforms = new Dictionary<Type, ITransform>();

        protected AbstractExecutionResponse(IExecutionCachingPolicy cachingPolicy)
        {
            CachingPolicy = cachingPolicy;
        }

        public IExecutionCachingPolicy CachingPolicy { get; }
        
        public async Task<TResult> TransformToOutput<TContext, TResult>(ExecutionResponseTransformContext<TContext> context)
            where TContext : class
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (transforms.TryGetValue(typeof(TContext), out var transform))
            {
                if (transform.ResultType == typeof(TResult))
                {
                    return (TResult)await transform.Transform(context, (TResponse)this).ConfigureAwait(false);
                }

                throw new NotImplementedException(
                    $"Transform does not support transforming to result type {typeof(TResult)}");
            }

            throw new NotImplementedException($"No transform for context type {typeof(TContext)} found");
        }
        
        protected void RegisterTransform<TContext, TResult>(Func<ExecutionResponseTransformContext<TContext>, TResponse, TResult> transformDelegate)
            where TContext : class
        {
            RegisterTransform(new DelegateTransform<TContext,TResult>(transformDelegate));
        }

        protected void RegisterTransform<TContext, TResult>(
            Func<ExecutionResponseTransformContext<TContext>, TResult> transformDelegate)
            where TContext : class
        {
            RegisterTransform(new DelegateTransform<TContext, TResult>(transformDelegate));
        }
        
        protected void RegisterTransform<TContext, TResult>(Func<ExecutionResponseTransformContext<TContext>, TResponse, Task<TResult>> transformDelegate)
            where TContext : class
        {
            RegisterTransform(new DelegateTransform<TContext,TResult>(transformDelegate));
        }

        protected void RegisterTransform<TContext, TResult>(
            Func<ExecutionResponseTransformContext<TContext>, Task<TResult>> transformDelegate)
            where TContext : class
        {
            RegisterTransform(new DelegateTransform<TContext, TResult>(transformDelegate));
        }

        protected void RegisterTransform(ITransform transform)
        {
            if (transform == null) throw new ArgumentNullException(nameof(transform));

            transforms[transform.ContextType] = transform;
        }
    }
}