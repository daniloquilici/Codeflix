using MediatR;
using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesOutput : PaginatedListOutput<CategoryModelOutput>
    {
        public ListCategoriesOutput(int page, int perPage, int total, IReadOnlyList<CategoryModelOutput> items)
            : base(page, perPage, total, items)
        {
        }
    }
}
