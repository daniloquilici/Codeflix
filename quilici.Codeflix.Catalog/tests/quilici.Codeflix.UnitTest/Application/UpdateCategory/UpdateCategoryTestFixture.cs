using Moq;
using quilici.Codeflix.Application.Interfaces;
using quilici.Codeflix.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.Application.UseCases.Category.UpdateCategory;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.Repository;
using quilici.Codeflix.UnitTest.Common;
using Xunit;

namespace quilici.Codeflix.UnitTest.Application.UpdateCategory
{
    [CollectionDefinition(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
    {
    }

    public class UpdateCategoryTestFixture : BaseFixture
    {
        public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();

        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();

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

        public UpdateCategoryInput GetValidInput(Guid? id = null) => new (id ?? Guid.NewGuid(), GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

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
