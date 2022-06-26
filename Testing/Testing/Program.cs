using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.BuildDb;
using Testing.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Testing;


namespace Testing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            //SQliteDbInitializer dbInit = serviceProvider.GetRequiredService<SQliteDbInitializer>();
  
            CreateHostBuilder(args).Build().Run();
      
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                });
    }
}
