﻿using quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Base;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;
public class GenreBaseFixture : BaseFixture
{
    public GenrePersistence Persistence { get; set; }
    public CategoryPersistence CategoryPersistence { get; set; }

    public GenreBaseFixture()
        : base()
    {
        var dbContext = CreateDbContext();
        Persistence = new GenrePersistence(dbContext);
        CategoryPersistence = new CategoryPersistence(dbContext);
    }

    public string GetValidGenreName()
        => Faker.Commerce.Categories(1)[0];

    public bool GetRandoBoolean() => new Random().NextDouble() < 0.5;

    public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null, string? name = null)
    {
        var genre = new DomainEntity.Genre(name ?? GetValidGenreName(), isActive ?? GetRandoBoolean());
        categoriesIds?.ForEach(genre.AddCategory);
        return genre;
    }

    public List<DomainEntity.Genre> GetExampleListGenre(int count = 10) => Enumerable.Range(1, count).Select(_ => GetExampleGenre()).ToList();

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

    public DomainEntity.Category GetExampleCategory() => new(GetValidCategoryName(), GetValidCategoryDescription(), GetRandoBoolean());

    public List<DomainEntity.Category> GetExampleCategoriesList(int length = 10) => Enumerable.Range(0, length)
        .Select(_ => GetExampleCategory()).ToList();
}
