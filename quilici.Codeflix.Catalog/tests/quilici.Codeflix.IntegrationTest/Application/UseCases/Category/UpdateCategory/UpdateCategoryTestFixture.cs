using quilici.Codeflix.Application.UseCases.Category.UpdateCategory;
using quilici.Codeflix.IntegrationTest.Application.UseCases.Category.Common;
using Xunit;

namespace quilici.Codeflix.IntegrationTest.Application.UseCases.Category.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoruTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
    { }

    public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        public UpdateCategoryInput GetValidInput(Guid? id = null) => new(id ?? Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

        public UpdateCategoryInput GetInvalidInputShortName()
        {
            var invalidInputShortName = GetValidInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

            return invalidInputShortName;
        }

        public UpdateCategoryInput GetInvalidInputLongName()
        {
            //Name more than 255 character
            var invalidInputLongName = GetValidInput();
            invalidInputLongName.Name = Faker.Commerce.ProductName();
            while (invalidInputLongName.Name.Length < 255)
                invalidInputLongName.Name += $"{invalidInputLongName.Name} {Faker.Commerce.ProductName()}";

            return invalidInputLongName;
        }

        public UpdateCategoryInput CreateCategoryInputTooLongDescription()
        {
            var invalidInputTooLongDescription = GetValidInput();
            invalidInputTooLongDescription.Description = Faker.Commerce.ProductDescription();
            while (invalidInputTooLongDescription.Description.Length < 10_000)
                invalidInputTooLongDescription.Description += $"{invalidInputTooLongDescription.Description} {Faker.Commerce.ProductDescription()}";

            return invalidInputTooLongDescription;
        }
    }
}
