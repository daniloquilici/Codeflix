using MediatR;
using quilici.Codeflix.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
    {
    }
}
