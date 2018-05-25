using System;

namespace Sedio.Core.Runtime.Application.Dependencies
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DependentOnAttribute : Attribute
    {
        public DependentOnAttribute(Type dependencyType)
        {
            DependencyType = dependencyType ?? throw new ArgumentNullException(nameof(dependencyType));
        }
        
        public Type DependencyType { get; }
    }
}