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
            var manager = new SqliteDbContextManager<ModelDbContext>("databases", 10, 5);
            manager.Initialize();
            manager.CreateBranch(null, "test01", CancellationToken.None).Wait();
            
            //await new SedioServerHost().Run(arguments);
            return 0;
        }
    }
}
