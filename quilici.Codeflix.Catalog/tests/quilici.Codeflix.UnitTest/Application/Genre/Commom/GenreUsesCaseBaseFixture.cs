using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.UnitTest.Common;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom
{
    public class GenreUsesCaseBaseFixture : BaseFixture
    {
        public string GetValidGenreName() => Faker.Commerce.Categories(1)[0];

        public Mock<IGenreRepository> GetGenreRepositoryMock() => new Mock<IGenreRepository>();

        public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();

        public Mock<ICategoryRepository> GetCategoryRepositoryMock() => new Mock<ICategoryRepository>();

        public DomainEntity.Genre GetExampleGente(bool? isActive = null) => new DomainEntity.Genre(GetValidGenreName(), isActive ?? GetRandoBoolean());
    }
}
