using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace BlazorServerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Backend.Monitor monitor = new Backend.Monitor();
            Thread thr = new Thread(new ThreadStart(monitor.Start));
            thr.Start();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:8123;http://*:8123");
                });
    }
}
