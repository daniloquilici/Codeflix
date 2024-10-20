using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace quilici.Codeflix.Catalog.Api.Configurations
{
    public static class UseCasesConfiguration
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCategory).Assembly));
            services.AddRepositories();
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
