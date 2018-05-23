using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sedio.Server.Framework.Http
{
    public static class BindingContextExtensions
    {
        public struct BindingResult<T>
        {
            public T Value;
            public ValueProviderResult ValueProviderResult;
            public string ValueName;

        }

        public static bool TryGetValue<T>(this ModelBindingContext bindingContext, string key,out BindingResult<T> result)
        {
            result = default(BindingResult<T>);

            if (TryGetStringValue(bindingContext,key, out var stringValue))
            {
                try
                {
                    result = new BindingResult<T>()
                    {
                        Value = (T) Convert.ChangeType(stringValue.Value, typeof(T)),
                        ValueName = stringValue.ValueName,
                        ValueProviderResult = stringValue.ValueProviderResult
                    };
              
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public static bool TryGetStringValue(this ModelBindingContext bindingContext,string key,out BindingResult<string> result)
        {
            result = default(BindingResult<string>);

            var valueProviderResult = bindingContext.ValueProvider.GetValue(key);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return false;
            }

            var stringValue = valueProviderResult.FirstValue;

            if (stringValue == null)
            {
                return false;
            }

            result = new BindingResult<string>()
            {
                ValueProviderResult = valueProviderResult,
                Value = stringValue,
                ValueName = key
            };

            return true;
        }
    }
}