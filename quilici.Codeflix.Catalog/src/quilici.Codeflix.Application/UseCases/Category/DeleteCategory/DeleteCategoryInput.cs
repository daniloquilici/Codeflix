using MediatR;

namespace quilici.Codeflix.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategoryInput : IRequest
    {
        public Guid Id { get; set; }

        public DeleteCategoryInput(Guid id) => Id = id;
    }
}
