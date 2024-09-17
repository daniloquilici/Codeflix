using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenresOutput : PaginatedListOutput<GenreModelOuput>
    {
        public ListGenresOutput(int page, int perPage, int total, IReadOnlyList<GenreModelOuput> items) : base(page, perPage, total, items)
        {
        }

        public static ListGenresOutput FromSearchOutput(SearchOutput<DomainEntity.Genre> searhOutput)
            => new(searhOutput.CurrentPage, searhOutput.PerPage, searhOutput.Total, searhOutput.Items.Select(GenreModelOuput.FromGenre).ToList());        
    }
}
