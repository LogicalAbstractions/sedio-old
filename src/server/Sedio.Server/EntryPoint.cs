using System.Threading;
using System.Threading.Tasks;
using Sedio.Server.Runtime;

namespace Sedio.Server
{
    public static class EntryPoint
    {
        public static async Task<int> Main(string[] arguments)
        {
            using (var host = new SedioHost(arguments))
            {
                await host.Run(CancellationToken.None);
                return 0;
            }
        }
    }
}
