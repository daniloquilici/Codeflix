using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

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
            var searchInput = new SearchInput(input.Page, input.PerPage, input.Search, input.Sort, input.Dir);
            var searhOutput = await _genreRepository.Search(searchInput, cancellationToken);
            return new ListGenresOutput(searhOutput.CurrentPage, searhOutput.PerPage, searhOutput.Total, searhOutput.Items.Select(GenreModelOuput.FromGenre).ToList());
        }
    }
}
