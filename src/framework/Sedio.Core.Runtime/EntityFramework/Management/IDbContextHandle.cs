using System;
using Microsoft.EntityFrameworkCore;

namespace Sedio.Core.Runtime.EntityFramework.Management
{
    public interface IDbContextHandle<out T> : IDisposable
        where T : DbContext
    {
        T Context { get; }
    }
}