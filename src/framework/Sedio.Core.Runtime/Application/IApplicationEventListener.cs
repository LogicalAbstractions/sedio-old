using System;

namespace Sedio.Core.Runtime.Application
{
    public interface IApplicationEventListener
    {
        void OnStart();

        void OnStop();
    }
}