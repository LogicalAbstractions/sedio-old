namespace Sedio.Server.Runtime.Application
{
    public interface IBootTask
    {
        int Order { get; }

        void Boot();
    }
}