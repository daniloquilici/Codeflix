using Bogus;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Infra.Data.EF;

namespace quilici.Codeflix.EndToEndTests.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }
        public ApiClient ApiClient { get; set; }

        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
        }

        public CodeFlixCatalogDbContext CreateDbContext()
        {
            var context = new CodeFlixCatalogDbContext(new DbContextOptionsBuilder<CodeFlixCatalogDbContext>().UseInMemoryDatabase("end2end-tests-db")
                .Options);

            return context;
        }
    }
}
