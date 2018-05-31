using System.Reflection;

namespace Sedio.Core.Runtime.Application.Assemblies
{
    public interface IAssemblyProvider
    {
        Assembly[] Assemblies { get; }
    }
}