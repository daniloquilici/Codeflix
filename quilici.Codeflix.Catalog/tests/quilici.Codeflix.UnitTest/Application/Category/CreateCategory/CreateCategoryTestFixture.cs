using quilici.Codeflix.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.UnitTest.Application.Category.Common;
using Xunit;

namespace quilici.Codeflix.UnitTest.Application.Category.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
    {
    }

    public class CreateCategoryTestFixture : CategoryUsesCaseBaseFixture
    {
        public CreateCategoryInput GetInput() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

        public CreateCategoryInput GetInvalidInputShortName()
        {
            var invalidInputShortName = GetInput();
            invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);

            return invalidInputShortName;
        }

        public CreateCategoryInput GetInvalidInputLongName()
        {
            //Name more than 255 character
            var invalidInputLongName = GetInput();
            invalidInputLongName.Name = Faker.Commerce.ProductName();
            while (invalidInputLongName.Name.Length < 255)
                invalidInputLongName.Name += $"{invalidInputLongName.Name} {Faker.Commerce.ProductName()}";

            return invalidInputLongName;
        }

        public CreateCategoryInput CreateCategoryInputInputDescriptionNull()
        {
            var invalidInputDescriptionNull = GetInput();
            invalidInputDescriptionNull.Description = null!;
            return invalidInputDescriptionNull;
        }

        public CreateCategoryInput CreateCategoryInputTooLongDescription()
        {
            var invalidInputTooLongDescription = GetInput();
            invalidInputTooLongDescription.Description = Faker.Commerce.ProductDescription();
            while (invalidInputTooLongDescription.Description.Length < 10_000)
                invalidInputTooLongDescription.Description += $"{invalidInputTooLongDescription.Description} {Faker.Commerce.ProductDescription()}";

            return invalidInputTooLongDescription;
        }
    }
}
