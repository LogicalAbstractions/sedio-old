namespace Sedio.Core.Runtime.Execution
{
    public interface ICommand : IExecutable
    {
        
    }
    
    public interface ICommand<TResult> : ICommand, IExecutable<TResult>
    {
        
    }
}