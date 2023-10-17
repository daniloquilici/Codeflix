using MediatR;
using quilici.Codeflix.Application.UseCases.Category.Common;

namespace quilici.Codeflix.Application.UseCases.Category.GetCategory
{
    public class GetCategoryInput : IRequest<CategoryModelOutput>
    {
        public Guid Id { get; set; }

        public GetCategoryInput(Guid id) => Id = id;
    }
}
