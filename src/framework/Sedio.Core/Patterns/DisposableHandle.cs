using System;

namespace Sedio.Core.Patterns
{
    public abstract class DisposableHandle : IDisposable
    {
        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnDispose();
    }

    public sealed class DisposableHandle<T> : DisposableHandle
        where T : class
    {
        private readonly Action<T> disposalAction;

        public static implicit operator T(DisposableHandle<T> handle)
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            return handle.Value;
        }
        
        public DisposableHandle(T value, Action<T> disposalAction)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            this.disposalAction = disposalAction ?? throw new ArgumentNullException(nameof(disposalAction));
        }

        public T Value { get; }
        
        protected override void OnDispose()
        {
            disposalAction.Invoke(Value);
        }
    }
}