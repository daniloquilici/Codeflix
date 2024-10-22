using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public class DeleteGenre : IDeleteGenre
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenreRepository _genreRepository;

        public DeleteGenre(IUnitOfWork unitOfWork, IGenreRepository genreRepository)
        {
            _unitOfWork = unitOfWork;
            _genreRepository = genreRepository;
        }

        public async Task Handle(DeleteGenreInput request, CancellationToken cancellationToken)
        {
            var genre = await _genreRepository.Get(request.Id, cancellationToken);
            await _genreRepository.Delete(genre, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
