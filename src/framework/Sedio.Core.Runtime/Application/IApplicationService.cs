using System;

namespace Sedio.Core.Runtime.Application
{
    public interface IApplicationService
    {
        void OnStart();

        void OnStop();
    }
}