using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sedio.Core.Runtime.EntityFramework.Management.Sqlite;
using Sedio.Server.Runtime;
using Sedio.Server.Runtime.Model;

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
