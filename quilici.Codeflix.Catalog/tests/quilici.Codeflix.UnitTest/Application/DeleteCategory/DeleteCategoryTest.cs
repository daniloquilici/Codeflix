using Moq;
using Xunit;
using UseCases = quilici.Codeflix.Application.UseCases.Category.DeleteCategory;
using FluentAssertions;
using quilici.Codeflix.Application.Exceptions;

namespace quilici.Codeflix.UnitTest.Application.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Application", "DeleteCategory - Uses Cases")]
        public async Task DeleteCategory()
        {
            //Arrange
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var categoryExample = _fixture.GetValidCategory();

            repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);

            var input = new UseCases.DeleteCategoryInput(categoryExample.Id);
            var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            //action
            await useCase.Handle(input, CancellationToken.None);

            //Asserts
            repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(x => x.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Application", "DeleteCategory - Uses Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            //Arrange
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
            var exampleGuid = Guid.NewGuid();

            repositoryMock.Setup(x => x.Get(exampleGuid, It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found."));

            var input = new UseCases.DeleteCategoryInput(exampleGuid);
            var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            //action
            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>();

            //Asserts
            repositoryMock.Verify(x => x.Get(exampleGuid, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
