using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Edu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://192.168.0.102:5000,127.0.0.1:5000")
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}