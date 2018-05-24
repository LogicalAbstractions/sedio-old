namespace Sedio.Core.Runtime.Application
{
    public interface IStartupTask
    {
        int Order { get; }

        void Boot();
    }
}