namespace Sedio.Core.Runtime.Execution.Requests
{
    public abstract class AbstractExecutionRequest : IExecutionRequest
    {
        protected AbstractExecutionRequest(ExecutionRequestType type = ExecutionRequestType.Query)
        {
            Type = type;
        }

        public ExecutionRequestType Type { get; }
    }
}