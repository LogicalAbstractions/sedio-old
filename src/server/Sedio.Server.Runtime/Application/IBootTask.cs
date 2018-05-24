namespace Sedio.Server.Application
{
    public interface IBootTask
    {
        int Order { get; }

        void Boot();
    }
}