using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.GetCategory
{
    public interface IGetCategory : IRequestHandler<GetCategoryInput, CategoryModelOutput>
    {
    }
}
