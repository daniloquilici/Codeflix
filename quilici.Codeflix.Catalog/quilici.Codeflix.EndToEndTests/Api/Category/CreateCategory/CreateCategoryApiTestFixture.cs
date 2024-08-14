using quilici.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture> { }


    public class CreateCategoryApiTestFixture : CategoryBaseFixture
    {
        public CreateCategoryInput GetExampleInput() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());
    }
}
