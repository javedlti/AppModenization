using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace ConsoleToWebCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateBuilder(string[] args)
        {
           return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(web =>
            {
                web.UseStartup<Startup>();
            });
        }
    }
}
