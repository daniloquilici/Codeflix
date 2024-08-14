using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput>
    {
    }
}
