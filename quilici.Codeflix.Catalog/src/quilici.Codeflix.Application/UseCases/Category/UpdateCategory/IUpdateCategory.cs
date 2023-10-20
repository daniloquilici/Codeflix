using MediatR;
using quilici.Codeflix.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Application.UseCases.Category.UpdateCategory
{
    public interface IUpdateCategory : IRequestHandler<UpdateCategoryInput, CategoryModelOutput>
    {
    }
}
