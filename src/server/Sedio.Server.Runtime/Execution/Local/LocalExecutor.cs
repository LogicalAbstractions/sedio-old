using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Sedio.Server.Runtime.Execution.Local
{
    public sealed class LocalExecutor : IExecutor
    {
        private readonly IExecutionContextProvider executionContextProvider;
        private readonly ILogger logger;

        public LocalExecutor(IExecutionContextProvider executionContextProvider, ILogger logger)
        {
            this.executionContextProvider = executionContextProvider ?? throw new ArgumentNullException(nameof(executionContextProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<object> Execute(IExecutable executable, string branchId, CancellationToken cancellationToken)
        {
            if (executable == null) throw new ArgumentNullException(nameof(executable));

            using (var executionContext =
                await executionContextProvider.GetContext(branchId, cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    return await executable.Execute(executionContext).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    logger.Error(ex,"Error executing {Executable}",executable.GetType().Name);
                    throw;
                }
            }
        }
    }
}