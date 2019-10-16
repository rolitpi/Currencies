using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Currencies
{
    public class Program
    {
        public static void Main( String[] args )
        {
            CreateWebHostBuilder( args ).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder( String[] args )
        {
            return WebHost.CreateDefaultBuilder( args )
.UseStartup<Startup>();
        }
    }
}
