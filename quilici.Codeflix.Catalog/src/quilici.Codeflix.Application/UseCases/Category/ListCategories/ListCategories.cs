using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Category.ListCategories
{
    public class ListCategories : IListCategories
    {
        private readonly ICategoryRepository _categoryRepository;

        public ListCategories(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ListCategoriesOutput> Handle(ListCategoriesInput request, CancellationToken cancellationToken)
        {
            var searchOutput = await _categoryRepository.Search(new SearchInput(request.Page, request.PerPage, request.Search, request.Sort, request.Dir), cancellationToken);

            //var output = new ListCategoriesOutput(searchOutput.CurrentPage, searchOutput.PerPage, searchOutput.Total, searchOutput.Items.Select(x => CategoryModelOutput.FromCategory(x)).ToList());
            //Mesmo comando que a linha acima
            return new ListCategoriesOutput(searchOutput.CurrentPage, searchOutput.PerPage, searchOutput.Total, searchOutput.Items.Select(CategoryModelOutput.FromCategory).ToList());
        }
    }
}
