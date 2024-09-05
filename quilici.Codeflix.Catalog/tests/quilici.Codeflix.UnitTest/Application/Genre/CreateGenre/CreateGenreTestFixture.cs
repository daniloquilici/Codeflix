using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom;
using Xunit;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.CreateGenre
{
    [CollectionDefinition(nameof(CreateGenreTestFixture))]
    public class CreateGenreTestFixtureCollection : ICollectionFixture<CreateGenreTestFixture> { }

    public class CreateGenreTestFixture : GenreUsesCaseBaseFixture
    {
        public Mock<IGenreRepository> GetGenreRepositoryMock() => new Mock<IGenreRepository>();

        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();

        public CreateGenreInput GetExampleInput() => new CreateGenreInput(GetValidGenreName(), GetRandoBoolean());


    }
}
