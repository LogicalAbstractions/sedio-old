using System.Reflection;

namespace Sedio.Core.Runtime.Application.Assemblies
{
    public sealed class StaticAssemblyProvider : IAssemblyProvider
    {
        public StaticAssemblyProvider(params Assembly[] assemblies)
        {
            Assemblies = assemblies;
        }

        public Assembly[] Assemblies { get; }
    }
}