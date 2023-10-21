using MediatR;

namespace quilici.Codeflix.Application.UseCases.Category.ListCategories
{
    public interface IListCategories : IRequestHandler<ListCategoriesInput, ListCategoriesOutput>
    {
    }
}
