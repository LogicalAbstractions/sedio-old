using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;

namespace Sedio.Core.Runtime.Http.Binding
{
    public sealed class ModelBindingProvider : IModelBinderProvider
    {
        private readonly Dictionary<Type,Type> modelBinderTypes = new Dictionary<Type, Type>()
        {
            {typeof(IPAddress),typeof(IpAddressModelBinder) },
            {typeof(PagingParameters),typeof(PagingParametersModelBinder) },
            {typeof(SemanticVersion),typeof(SemanticVersionModelBinder) },
            {typeof(VersionRange),typeof(VersionRangeModelBinder) }
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (modelBinderTypes.TryGetValue(context.Metadata.ModelType, out var binderType))
            {
                return new BinderTypeModelBinder(binderType);
            }

            return null;
        }
    }
}