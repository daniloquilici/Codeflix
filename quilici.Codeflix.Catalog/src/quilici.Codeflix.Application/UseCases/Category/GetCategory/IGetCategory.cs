using MediatR;
using quilici.Codeflix.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
    {
    }
}
