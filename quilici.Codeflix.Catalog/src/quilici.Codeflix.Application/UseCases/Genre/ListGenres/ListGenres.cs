using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres
{
    public class ListGenres : IListGenres
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ListGenres(IGenreRepository genreRepository, ICategoryRepository categoryRepository)
        {
            _genreRepository = genreRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ListGenresOutput> Handle(ListGenresInput input, CancellationToken cancellationToken)
        {
            var searhOutput = await _genreRepository.Search(input.ToSearchInput(), cancellationToken);
            
            var output = ListGenresOutput.FromSearchOutput(searhOutput);
            
            var relatedCategoriesIds = searhOutput.Items.SelectMany(item => item.Categories).Distinct().ToList();
            if (relatedCategoriesIds.Count > 0)
            {
                var categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);
                output.FillWithCategoryName(categories);
            }
            
            return output;
        }
    }
}
