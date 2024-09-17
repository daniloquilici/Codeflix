using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenresOutput : PaginatedListOutput<GenreModelOuput>
    {
        public ListGenresOutput(int page, int perPage, int total, IReadOnlyList<GenreModelOuput> items) : base(page, perPage, total, items)
        {
        }
    }
}
