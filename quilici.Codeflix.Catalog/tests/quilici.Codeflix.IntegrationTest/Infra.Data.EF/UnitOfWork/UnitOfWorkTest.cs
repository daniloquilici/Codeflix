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
<<<<<<< HEAD
        [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
        public async Task Commit() 
        { 
            var dbContext = _fixture.CreateDbContext();
            var exampleCategoriesList = _fixture.GetExampleCategoriesList();
            await dbContext.AddRangeAsync(exampleCategoriesList);

            var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
            await unitOfWork.CommitAsync(CancellationToken.None);

            var assertDbContext = _fixture.CreateDbContext(true);
            var savedCategories = assertDbContext.Categories.AsNoTracking().ToList();
=======
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

>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
            savedCategories.Should().HaveCount(exampleCategoriesList.Count);
        }

        [Fact(DisplayName = "Rollback")]
<<<<<<< HEAD
        [Trait("Integration/Infra.Data", "UnitOfWork - Persistence")]
        public async Task Rollback()
        {
            var dbContext = _fixture.CreateDbContext();
            var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);
            
            var task = async () => await unitOfWork.RollbackAsync(CancellationToken.None);

            await task.Should().NotThrowAsync();
=======
        [Trait("Integration/Infra.Data", "Persistence")]
        public async Task Rollback()
        {
            var dbContext = _fixture.CreateDbContext();

            var unitOfwork = new UnitOfWorkInfra.UnitOfWork(dbContext);
            var task = async () => await unitOfwork.Rollbakc(CancellationToken.None);

            await task.Should().NotThrowAsync();            
>>>>>>> d6f462914faeb68acc03e5802e620ad18dc6d46c
        }
    }
}
