using Moq;
using quilici.Codeflix.Application.Interfaces;
using quilici.Codeflix.Domain.Repository;
using quilici.Codeflix.UnitTest.Common;
using DomianEntity = quilici.Codeflix.Domain.Entity;

namespace quilici.Codeflix.UnitTest.Application.Category.Common
{
    public abstract class CategoryUsesCaseBaseFixture : BaseFixture
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

        public DomianEntity.Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());
    }
}
