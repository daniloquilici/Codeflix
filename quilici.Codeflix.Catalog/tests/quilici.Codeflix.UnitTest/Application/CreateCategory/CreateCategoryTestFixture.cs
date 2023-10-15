using Moq;
using quilici.Codeflix.Application.Interfaces;
using quilici.Codeflix.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.Domain.Repository;
using quilici.Codeflix.UnitTest.Common;
using System.Diagnostics.SymbolStore;
using Xunit;

namespace quilici.Codeflix.UnitTest.Application.CreateCategory
{
    [CollectionDefinition(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
    {
    }

    public class CreateCategoryTestFixture : BaseFixture
    {
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

        public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();

        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();
    }
}
