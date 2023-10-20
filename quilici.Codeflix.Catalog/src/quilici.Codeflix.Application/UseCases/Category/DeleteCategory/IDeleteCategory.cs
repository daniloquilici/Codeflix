using MediatR;

namespace quilici.Codeflix.Application.UseCases.Category.DeleteCategory
{
    public interface IDeleteCategory : IRequestHandler<DeleteCategoryInput>
    {
    }
}
