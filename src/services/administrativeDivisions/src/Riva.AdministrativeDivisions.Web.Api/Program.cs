using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Riva.BuildingBlocks.WebApi.HostBuilderExtensions;

namespace Riva.AdministrativeDivisions.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .AddAppConfiguration<Startup>()
                .AddLogging(typeof(Program), typeof(Startup))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
