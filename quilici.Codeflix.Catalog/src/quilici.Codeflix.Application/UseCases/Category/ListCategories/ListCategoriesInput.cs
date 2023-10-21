using MediatR;
using quilici.Codeflix.Application.Common;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesInput : PaginatedListInput, IRequest<ListCategoriesOutput>
    {
        public ListCategoriesInput(int page = 1, int perPage = 15, string search = "", string sort = "", SearchOrder dir = SearchOrder.Asc)
            : base(page, perPage, search, sort, dir)
        {
        }
    }
}
