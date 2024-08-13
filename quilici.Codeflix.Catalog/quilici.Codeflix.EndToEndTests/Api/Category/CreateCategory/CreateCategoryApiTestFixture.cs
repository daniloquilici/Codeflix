using quilici.Codeflix.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.EndToEndTests.Api.Category.Common;

namespace quilici.Codeflix.EndToEndTests.Api.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTestFixtureCollection : ICollectionFixture<CreateCategoryApiTestFixture> { }


    public class CreateCategoryApiTestFixture : CategoryBaseFixture
    {
        public CreateCategoryInput GetExampleInput() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());
    }
}
