using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories
{
    public interface IListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
    {
    }
}
