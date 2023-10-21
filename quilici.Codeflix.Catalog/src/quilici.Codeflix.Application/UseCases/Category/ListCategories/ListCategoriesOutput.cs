using MediatR;
using quilici.Codeflix.Application.Common;
using quilici.Codeflix.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Application.UseCases.Category.ListCategories
{
    public class ListCategoriesOutput : PaginatedListOutput<CategoryModelOutput>
    {
        public ListCategoriesOutput(int page, int perPage, int total, IReadOnlyList<CategoryModelOutput> items) 
            : base(page, perPage, total, items)
        {
        }
    }
}
