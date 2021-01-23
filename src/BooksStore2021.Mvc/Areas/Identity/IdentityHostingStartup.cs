using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BooksStore2021.Mvc.Areas.Identity.IdentityHostingStartup))]
namespace BooksStore2021.Mvc.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}