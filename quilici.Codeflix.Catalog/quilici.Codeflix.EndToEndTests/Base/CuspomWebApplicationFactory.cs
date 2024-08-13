using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using quilici.Codeflix.Infra.Data.EF;

namespace quilici.Codeflix.EndToEndTests.Base
{
    public class CuspomWebApplicationFactory<TSartup> : WebApplicationFactory<TSartup>
        where TSartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbOptions = services.FirstOrDefault(x => x.ServiceType == typeof(DbContextOptions<CodeFlixCatalogDbContext>));
                if (dbOptions is not null)
                    services.Remove(dbOptions);
                services.AddDbContext<CodeFlixCatalogDbContext>(options =>
                {
                    options.UseInMemoryDatabase("end2end-tests-db");
                });
            });

            base.ConfigureWebHost(builder);
        }
    }
}
