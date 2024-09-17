using MediatR;
using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenresInput : PaginatedListInput, IRequest<ListGenresOutput>
    {
        public ListGenresInput(int page, int perPage, string search, string sort, SearchOrder dir) : base(page, perPage, search, sort, dir)
        {
        }
    }
}
