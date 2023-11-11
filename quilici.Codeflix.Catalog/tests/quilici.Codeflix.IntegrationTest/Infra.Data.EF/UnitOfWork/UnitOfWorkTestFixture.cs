<<<<<<< HEAD
﻿using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.IntegrationTest.Base;
=======
﻿using quilici.Codeflix.IntegrationTest.Base;
>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
using Xunit;

namespace quilici.Codeflix.IntegrationTest.Infra.Data.EF.UnitOfWork
{
    [CollectionDefinition(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTestFixtureCollection : ICollectionFixture<UnitOfWorkTestFixture>
    {
    }

    public class UnitOfWorkTestFixture : BaseFixture
    {
<<<<<<< HEAD
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

        public List<Category> GetExampleCategoriesList(int length = 10) => Enumerable.Range(0, length)
            .Select(_ => GetExampleCategory()).ToList();
=======
>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
    }
}
