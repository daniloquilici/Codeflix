using quilici.Codeflix.Catalog.EndToEndTests.Base;
using System.Net.Sockets;
using DominEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common
{
    public class CategoryBaseFixture : BaseFixture
    {
        public CategoryPersistence Persistence;
        public CategoryBaseFixture()
            : base()
        {
            Persistence = new CategoryPersistence(CreateDbContext());
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

        public string GetInvalidNameTooShort()
        {
            return Faker.Commerce.ProductName().Substring(0, 2);
        }

        public string GetInvalidNameTooLong()
        {
            //Name more than 255 character
            var nameLong = Faker.Commerce.ProductName();
            while (nameLong.Length < 255)
                nameLong += $"{nameLong} {Faker.Commerce.ProductName()}";

            return nameLong;
        }

        public string GetInvalidDescriptionTooLong()
        {
            var descriptionTooLong = Faker.Commerce.ProductDescription();
            while (descriptionTooLong.Length < 10_000)
                descriptionTooLong += $"{descriptionTooLong} {Faker.Commerce.ProductDescription()}";

            return descriptionTooLong;
        }

        public DominEntity.Category GetExampleCategory() 
            => new (GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

        public IList<DominEntity.Category> GetExampleCategoriesList(int listLenght = 15)
            => Enumerable.Range(1, listLenght).Select(_ => new DominEntity.Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean())).ToList();
    }
}
