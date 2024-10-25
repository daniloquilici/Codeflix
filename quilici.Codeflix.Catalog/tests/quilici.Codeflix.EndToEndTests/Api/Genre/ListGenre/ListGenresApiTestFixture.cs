using quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.ListGenre;

[CollectionDefinition(nameof(ListGenresApiTestFixture))]
public class ListGenresApiTestFixtureCollection : ICollectionFixture<ListGenresApiTestFixture> { }

public class ListGenresApiTestFixture : GenreBaseFixture
{
    public List<DomainEntity.Genre> GetExampleListGenreByNames(List<string> names) => names.Select(name => GetExampleGenre(name: name)).ToList();
}
