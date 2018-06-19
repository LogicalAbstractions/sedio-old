namespace Sedio.Core.Runtime.Execution
{
    public enum ExecutionRequestType
    {
        Query,
        Mutation
    }
    
    public interface IExecutionRequest
    {
        ExecutionRequestType Type { get; }
    }
}