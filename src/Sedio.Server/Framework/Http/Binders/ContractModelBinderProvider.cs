﻿using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using NuGet.Versioning;
using Sedio.Contracts.Components;
using Sedio.Core.Collections.Paging;

namespace Sedio.Server.Framework.Http.Binders
{
    public sealed class ContractModelBinderProvider : IModelBinderProvider
    {
        private readonly Dictionary<Type,Type> modelBinderTypes = new Dictionary<Type, Type>()
        {
            {typeof(IPAddress),typeof(IpAddressModelBinder) },
            {typeof(PagingParameters),typeof(PagingParametersModelBinder) },
            {typeof(SemanticVersion),typeof(SemanticVersionModelBinder) },
            {typeof(ServiceId),typeof(ServiceIdModelBinder) },
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