using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Infra.Data.EF;

namespace quilici.Codeflix.Catalog.Api.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConnections(this IServiceCollection services)
        {
            services.AddDbConnection();
            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services)
        {
            services.AddDbContext<CodeFlixCatalogDbContext>(options => options.UseInMemoryDatabase("InMemory-DSV-Database"));
            return services;
        }
    }
}
