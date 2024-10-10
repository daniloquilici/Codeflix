using Moq;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.UnitTest.Common;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.UnitTest.Application.Genre.Commom
{
    public class GenreUseCaseBaseFixture : BaseFixture
    {
        public string GetValidGenreName()
            => Faker.Commerce.Categories(1)[0];

        public Mock<IGenreRepository> GetGenreRepositoryMock()
            => new Mock<IGenreRepository>();

        public Mock<IUnitOfWork> GetUnitOfWorkMock()
            => new Mock<IUnitOfWork>();

        public Mock<ICategoryRepository> GetCategoryRepositoryMock()
            => new Mock<ICategoryRepository>();

        public DomainEntity.Genre GetExampleGenre(bool? isActive = null, List<Guid>? categoriesIds = null)
        {
            var genre = new DomainEntity.Genre(GetValidGenreName(), isActive ?? GetRandoBoolean());
            categoriesIds?.ForEach(genre.AddCategory);
            return genre;
        }

        public List<DomainEntity.Genre> GetExampleGenresList(int count = 10)
        {
            return Enumerable.Range(1, count).Select(_ => 
            {
                var genre = new DomainEntity.Genre(GetValidGenreName(), GetRandoBoolean());
                GetRandomIdsList().ForEach(genre.AddCategory);
                return genre;
            }).ToList();
        }

        public List<Guid> GetRandomIdsList(int? count = null)
            => Enumerable.Range(1, count ?? (new Random()).Next(1, 10)).Select(_ => Guid.NewGuid()).ToList();
    }
}
