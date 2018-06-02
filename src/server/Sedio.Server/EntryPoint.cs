using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sedio.Server.Runtime;
using Sedio.Server.Runtime.Model;

namespace Sedio.Server
{
    public static class EntryPoint
    {
        public static async Task<int> Main(string[] arguments)
        {
            var context = new ModelDbContext();
            context.Database.Migrate();
            
            //await new SedioServerHost().Run(arguments);
            return 0;
        }
    }
}
