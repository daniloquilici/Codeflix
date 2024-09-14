using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre
{
    public class UpdateGenre : IUpdateGenre
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenreRepository _genreRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdateGenre(IUnitOfWork unitOfWork, IGenreRepository genreRepository, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _genreRepository = genreRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<GenreModelOuput> Handle(UpdateGenreInput request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.Get(request.Id, cancellationToken);

            genre.Update(request.Name);

            if (request.IsActive is not null && request.IsActive != genre.IsActive)
                if ((bool)request.IsActive)
                    genre.Activate();
                else
                    genre.Deactivate();

            if (request.CategoriesIds is not null)
            {
                genre.RemoveAllCategories();
                if (request.CategoriesIds.Count > 0)
                {
                    await ValidateCategoriesIds(request, cancellationToken);
                    request.CategoriesIds?.ForEach(genre.AddCategory);
                }
            }

            await _genreRepository.Update(genre, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            return GenreModelOuput.FromGenre(genre);
        }

        private async Task ValidateCategoriesIds(UpdateGenreInput request, CancellationToken cancellationToken)
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
