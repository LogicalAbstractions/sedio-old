using System.Threading;
using System.Threading.Tasks;
using Sedio.Server.Runtime;

namespace Sedio.Server
{
    public static class EntryPoint
    {
        public static async Task<int> Main(string[] arguments)
        {
            await new SedioServerHost().Run(arguments);
            return 0;
        }
    }
}
