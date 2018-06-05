using System.Threading.Tasks;
using NJsonSchema;
using NSwag;
using NSwag.SwaggerGeneration.Processors;
using NSwag.SwaggerGeneration.Processors.Contexts;

namespace Sedio.Core.Runtime.Http.Swagger
{
    public sealed class BranchIdHeaderOperationProcessor : IOperationProcessor
    {
        public Task<bool> ProcessAsync(OperationProcessorContext context)
        {
            if (!context.OperationDescription.Operation.Tags.Contains("Branches"))
            {
                context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter()
                {
                    Kind = SwaggerParameterKind.Header,
                    IsRequired = false,
                    Type = JsonObjectType.String,
                    Name = "X-Branch",
                    Description = "Allows to execute/simulate any action on a different branch of the service database. The branch must have been created beforehand"
                });
            }

            return Task.FromResult(true);
        }
    }
}