using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenres : IListGenres
    {
        private readonly IGenreRepository _genreRepository;

        public ListGenres(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<ListGenresOutput> Handle(ListGenresInput input, CancellationToken cancellationToken)
        {
            var searhOutput = await _genreRepository.Search(input.ToSearchInput(), cancellationToken);
            return ListGenresOutput.FromSearchOutput(searhOutput);
        }
    }
}
