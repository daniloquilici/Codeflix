using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public class CreateGenre : ICreateGenre
    {
        public readonly IGenreRepository _genreRepository;
        public readonly IUnitOfWork _unitOfWork;
        public CreateGenre(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
        {
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<GenreModelOuput> Handle(CreateGenreInput request, CancellationToken cancellationToken)
        {
            var genre = new DomainEntity.Genre(request.Name, request.IsActive);

            if (request.CategoriesIds is not null)
                request.CategoriesIds.ForEach(genre.AddCategory);

            await _genreRepository.Insert(genre, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return GenreModelOuput.FromGenre(genre);
        }
    }
}
