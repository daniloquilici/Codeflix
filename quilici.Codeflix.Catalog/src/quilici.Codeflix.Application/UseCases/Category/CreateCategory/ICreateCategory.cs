using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CategoryModelOutput>
    {
    }
}
