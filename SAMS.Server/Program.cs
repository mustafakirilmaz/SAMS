using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace SAMS.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                var path = Directory.GetCurrentDirectory();
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "error_log.txt");
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                using (StreamWriter writer = System.IO.File.AppendText(filePath))
                {
                    writer.WriteLine(ex.ToString());
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.StackTrace);
                    writer.WriteLine(ex.InnerException);
                }
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
