using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sedio.Server.Framework.Http.Binding
{
    public static class ModelBindingContextExtensions
    {
        public struct ModelBindingResult<T>
        {
            public T Value;
            public ValueProviderResult ValueProviderResult;
            public string ValueName;

        }

        public static bool TryGetValue<T>(this ModelBindingContext bindingContext, string key,out ModelBindingResult<T> result)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key must be valid", nameof(key));
            }

            result = default(ModelBindingResult<T>);

            if (TryGetStringValue(bindingContext,key, out var stringValue))
            {
                try
                {
                    result = new ModelBindingResult<T>()
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

        public static bool TryGetStringValue(this ModelBindingContext bindingContext,string key,out ModelBindingResult<string> result)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("key mus be valid", nameof(key));
            }

            result = default(ModelBindingResult<string>);

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

            result = new ModelBindingResult<string>()
            {
                ValueProviderResult = valueProviderResult,
                Value = stringValue,
                ValueName = key
            };

            return true;
        }
    }
}