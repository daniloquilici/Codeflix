using MediatR;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using System.Threading;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public class CreateGenre : ICreateGenre
    {
        public readonly IGenreRepository _genreRepository;
        public readonly ICategoryRepository _categoryRepository;
        public readonly IUnitOfWork _unitOfWork;
        public CreateGenre(IGenreRepository genreRepository, IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _genreRepository = genreRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<GenreModelOutput> Handle(CreateGenreInput request, CancellationToken cancellationToken)
        {
            var genre = new DomainEntity.Genre(request.Name, request.IsActive);

            if ((request.CategoriesIds?.Count ?? 0) > 0)
            {
                await ValidateCategoriesIds(request, cancellationToken);
                request.CategoriesIds?.ForEach(genre.AddCategory);
            }

            await _genreRepository.Insert(genre, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return GenreModelOutput.FromGenre(genre);
        }

        private async Task ValidateCategoriesIds(CreateGenreInput request, CancellationToken cancellationToken) 
        {
            var idsInPersistence = await _categoryRepository.GetIdsListByIds(request.CategoriesIds!, cancellationToken);
            if (idsInPersistence.Count < request.CategoriesIds!.Count)
            {
                var notFoundIds = request.CategoriesIds.FindAll(x => !idsInPersistence.Contains(x));
                var notFoundIdsAsString = string.Join(", ", notFoundIds);
                throw new RelatedAggregateException($"Related category id(s) not found: {notFoundIdsAsString}");
            }
        }
    }
}
