using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using DomianEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
        {
            var category = new DomianEntity.Category(input.Name, input.Description, input.IsActive);

            await _categoryRepository.Insert(category, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return new CategoryModelOutput(category.Id, category.Name, category.Description, category.IsActive, category.CreatedAt);
        }
    }
}
