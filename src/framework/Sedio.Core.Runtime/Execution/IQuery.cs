namespace Sedio.Core.Runtime.Execution
{
    public interface IQuery : IExecutable
    {
        
    }
    
    public interface IQuery<TResult> : IQuery, IExecutable<TResult>
    {
        
    }
}