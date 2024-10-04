using quilici.Codeflix.Catalog.IntegrationTest.Base;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Genre.Common;
public class GenreUseCasesBaseFixture : BaseFixture
{
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
}
