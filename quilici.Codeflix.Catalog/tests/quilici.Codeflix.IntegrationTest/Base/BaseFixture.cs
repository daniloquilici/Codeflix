using Bogus;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Infra.Data.EF;

namespace quilici.Codeflix.IntegrationTest.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }

        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
        }

        public CodeFlixCatalogDbContext CreateDbContext(bool preserveData = false)
        {
            var context = new CodeFlixCatalogDbContext(new DbContextOptionsBuilder<CodeFlixCatalogDbContext>().UseInMemoryDatabase("integration-tests-db")
                .Options);

            if (preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }
    }
}
