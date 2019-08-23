using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PieshopFunctions.Model;
using System.IO;
using System.Linq;

[assembly: FunctionsStartup(typeof(PieshopFunctions.Startup))]
namespace PieshopFunctions
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {

            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            //apparenty responses are compressed (Gzip) by default
            //builder.Services.AddResponseCompression(options =>
            //{
            //    options.EnableForHttps = true;
            //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "imagejpeg" });
            //});

            //builder.Services.Configure<GzipCompressionProviderOptions>(
            //    options => options.Level = System.IO.Compression.CompressionLevel.Optimal);

            //setup EF Core context with SqlServer and the connection string to use
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DbConectionString"))
            );

           

        }
    }
}
