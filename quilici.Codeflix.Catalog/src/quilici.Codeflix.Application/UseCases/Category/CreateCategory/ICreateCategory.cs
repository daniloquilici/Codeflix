using MediatR;

namespace quilici.Codeflix.Application.UseCases.Category.CreateCategory
{
    public interface ICreateCategory : IRequestHandler<CreateCategoryInput, CreateCategoryOutput>
    {
        public Task<CreateCategoryOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
    }
}
