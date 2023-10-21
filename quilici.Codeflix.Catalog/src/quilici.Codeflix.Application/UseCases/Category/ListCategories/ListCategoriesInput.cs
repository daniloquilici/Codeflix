using MediatR;
using quilici.Codeflix.Application.Common;
using quilici.Codeflix.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesInput : PaginatedListInput, IRequest<ListCategoriesOutput>
    {
        public ListCategoriesInput(int page, int perPage, string search, string sort, SearchOrder dir)
            : base(page, perPage, search, sort, dir)
        {
        }
    }
}
