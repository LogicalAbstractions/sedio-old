namespace Sedio.Server.Runtime.Execution.Commands
{
    public abstract class AbstractCreationCommand<TId,TInput,TEntity> : AbstractCommand<bool>
        where TEntity : class,new()
    {
        public TId CreationId { get; }
        
        public TInput Input { get; }
    }
}