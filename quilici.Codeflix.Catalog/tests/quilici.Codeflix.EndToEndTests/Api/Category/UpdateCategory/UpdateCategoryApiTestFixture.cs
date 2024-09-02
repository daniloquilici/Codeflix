using quilici.Codeflix.Catalog.Api.ApiModels.Category;
using quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture> { }

    public class UpdateCategoryApiTestFixture : CategoryBaseFixture
    {
        public UpdateCategoryApiInput GetExampleInput()
            => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());
    }
}
