using quilici.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTestFixtureCollection : ICollectionFixture<UpdateCategoryApiTestFixture> { }

    public class UpdateCategoryApiTestFixture : CategoryBaseFixture
    {
        public UpdateCategoryInput GetExampleInput(Guid? id = null)
            => new(id ?? Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());
    }
}
