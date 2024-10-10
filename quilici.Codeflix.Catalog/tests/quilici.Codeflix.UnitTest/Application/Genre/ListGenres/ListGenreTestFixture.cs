using quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.ListGenres
{
    [CollectionDefinition(nameof(ListGenreTestFixture))]
    public class ListGenreTestFixtureCollection : ICollectionFixture<ListGenreTestFixture> { }


    public class ListGenreTestFixture : GenreUseCaseBaseFixture
    {
        public ListGenresInput GetExampleInput()
        {
            var random = new Random();
            return new ListGenresInput(
                page: random.Next(1, 10),
                perPage: random.Next(15, 100),
                search: Faker.Commerce.ProductName(),
                sort: Faker.Commerce.ProductName(),
                dir: random.Next(0, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
                );
        }
    }
}
