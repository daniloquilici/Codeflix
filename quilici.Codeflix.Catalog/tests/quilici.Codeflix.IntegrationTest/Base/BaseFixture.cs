using Bogus;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Domain.Entity;
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
            var context = new CodeFlixCatalogDbContext(new DbContextOptionsBuilder<CodeFlixCatalogDbContext>().UseInMemoryDatabase($"integration-tests-db")
                .Options);

            if (preserveData == false)
                context.Database.EnsureDeleted();

            return context;
        }

        public string GetValidCategoryName()
        {
            var categoryName = string.Empty;

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName[..255];

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            var categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10_000)
                categoryDescription = categoryDescription[..10_000];

            return categoryDescription;
        }

        public bool GetRandoBoolean() => new Random().NextDouble() < 0.5;

        public Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

        public List<Category> GetExampleCategoriesList(int length = 10) => Enumerable.Range(0, length)
            .Select(_ => GetExampleCategory()).ToList();
    }
}
