using Bogus;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Infra.Data.EF;

namespace quilici.Codeflix.Catalog.EndToEndTests.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }
        public CuspomWebApplicationFactory<Program> WebAppFactory { get; set; }
        public HttpClient HttpClient { get; set; }

        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
            WebAppFactory = new CuspomWebApplicationFactory<Program>();
            HttpClient = WebAppFactory.CreateClient();
            ApiClient = new ApiClient(HttpClient);
        }

        public CodeFlixCatalogDbContext CreateDbContext()
        {
            var context = new CodeFlixCatalogDbContext(new DbContextOptionsBuilder<CodeFlixCatalogDbContext>().UseInMemoryDatabase("end2end-tests-db")
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