using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using UnitOfWorkInfra = quilici.Codeflix.Infra.Data.EF;

namespace quilici.Codeflix.IntegrationTest.Infra.Data.EF.UnitOfWork
{
    [Collection(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTest
    {
        private readonly UnitOfWorkTestFixture _fixture;

        public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Commit")]
        [Trait("Integration/Infra.Data", "Persistence")]
        public async Task Commit() 
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList();
            dbContext.AddRange(exampleCategoriesList);

            var unitOfwork = new UnitOfWorkInfra.UnitOfWork(dbContext);
            await unitOfwork.Commit(CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();

            savedCategories.Should().HaveCount(exampleCategoriesList.Count);
        }

        [Fact(DisplayName = "Rollback")]
        [Trait("Integration/Infra.Data", "Persistence")]
        public async Task Rollback()
        {
            var dbContext = _fixture.CreateDbContext();

            var unitOfwork = new UnitOfWorkInfra.UnitOfWork(dbContext);
            var task = async () => await unitOfwork.Rollbakc(CancellationToken.None);

            await task.Should().NotThrowAsync();            
        }
    }
}
