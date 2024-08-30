using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using quilici.Codeflix.Catalog.Infra.Data.EF;

namespace quilici.Codeflix.Catalog.EndToEndTests.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }
        public CuspomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }
        public readonly string _dbConnectionString;


        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CuspomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
            var configuration = WebAppFactory.Services.GetService(typeof(IConfiguration));
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
            _dbConnectionString = ((IConfiguration)configuration).GetConnectionString("CatalogDB");
        }

        public CodeFlixCatalogDbContext CreateDbContext()
        {
            var context = new CodeFlixCatalogDbContext(new DbContextOptionsBuilder<CodeFlixCatalogDbContext>()
                    .UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString))
                .Options);

            return context;
        }

        public void CleanPersistence()
        {
            var context = CreateDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}