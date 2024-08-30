﻿using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.EndToEndTests.Api.Category.Common;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.ListCategories
{
    [CollectionDefinition(nameof(ListCategotiesApiTestFixture))]
    public class ListCategotiesApiTestFixtureCollection : ICollectionFixture<ListCategotiesApiTestFixture> { }

    public class ListCategotiesApiTestFixture : CategoryBaseFixture
    {
        public List<Domain.Entity.Category> GetExampleCategoriesListWithName(List<string> names) => names.Select(x =>
        {
            var category = GetExampleCategory();
            category.Update(x);
            return category;
        }).ToList();

        public List<Domain.Entity.Category> CloneCategoryListOrdered(IList<Domain.Entity.Category> categoryList, string orderBy, SearchOrder order)
        {
            var listClone = new List<Domain.Entity.Category>(categoryList);

            var orderedEnumerable = (orderBy.ToLower(), order) switch
            {
                ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name),
                ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name),
                ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
                ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
                ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
                ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
                _ => listClone.OrderBy(x => x.Name),
            };

            return orderedEnumerable.ThenBy(x => x.CreatedAt).ToList();
        }
    }
}
