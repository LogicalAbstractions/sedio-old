using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sedio.Server.Framework.Http.Binders
{
    public abstract class AbstractStringModelBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueName = string.IsNullOrEmpty(bindingContext.ModelName)
                ? bindingContext.ModelMetadata.Name
                : bindingContext.ModelName;

            if (string.IsNullOrEmpty(valueName))
            {
                return Task.CompletedTask;
            }

            if (bindingContext.TryGetStringValue(valueName,out var value))
            {
                try
                {
                    bindingContext.ModelState.SetModelValue(value.ValueName,value.ValueProviderResult);
                    bindingContext.Result = ModelBindingResult.Success(OnConvert(bindingContext, value.Value));
                }
                catch (Exception ex)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    bindingContext.ModelState.TryAddModelException(value.ValueName, ex);
                }

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        protected abstract T OnConvert(ModelBindingContext context,string value);
    }
}