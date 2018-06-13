using System;

namespace Sedio.Core.Patterns
{
    public sealed class DisposableAction : IDisposable
    {
        private readonly Action disposalAction;

        public DisposableAction(Action disposalAction)
        {
            this.disposalAction = disposalAction ?? throw new ArgumentNullException(nameof(disposalAction));
        }

        public void Dispose()
        {
            disposalAction.Invoke();
        }
    }
}