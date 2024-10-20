﻿using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreTestFixture))]
    public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }

    public class CreateGenreTestFixture : GenreUseCaseBaseFixture
    {
        public CreateGenreInput GetExampleInput() => new CreateGenreInput(GetValidGenreName(), GetRandoBoolean());

        public CreateGenreInput GetExampleInput(string? name) => new CreateGenreInput(name!, GetRandoBoolean());

        public CreateGenreInput GetExampleInputWithCategories()
        {
            var numberOfCategoriesIds = new Random().Next(1, 10);
            var categoriesIds = Enumerable.Range(1, numberOfCategoriesIds).Select(_ => Guid.NewGuid()).ToList();
            return new CreateGenreInput(GetValidGenreName(), GetRandoBoolean(), categoriesIds);
        }
    }
}
