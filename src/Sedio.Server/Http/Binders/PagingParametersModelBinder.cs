using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sedio.Core.Collections.Paging;
using Sedio.Server.Framework.Http;

namespace Sedio.Server.Http.Binders
{
    public class PagingParametersModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var limit = PagingParameters.DefaultLimit;
            var cursor = PagingCursor.Start;

            if (bindingContext.TryGetValue<int>("limit",out var limitResult))
            {
                limit = limitResult.Value;
            }

            if (bindingContext.TryGetStringValue("cursor", out var cursorResult))
            {
                cursor = new PagingCursor(cursorResult.Value);
            }

            var pagingParameters = new PagingParameters(cursor,limit).Coerce();

            bindingContext.Result = ModelBindingResult.Success(pagingParameters);

            return Task.CompletedTask;
        }
    }
}