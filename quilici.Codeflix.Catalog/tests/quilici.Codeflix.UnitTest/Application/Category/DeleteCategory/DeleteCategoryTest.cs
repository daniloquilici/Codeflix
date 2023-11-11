using FluentAssertions;
using Moq;
using quilici.Codeflix.Application.Exceptions;
using Xunit;
using UseCases = quilici.Codeflix.Application.UseCases.Category.DeleteCategory;

namespace quilici.Codeflix.UnitTest.Application.Category.DeleteCategory
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
            var categoryExample = _fixture.GetExampleCategory();

            repositoryMock.Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>())).ReturnsAsync(categoryExample);

            var input = new UseCases.DeleteCategoryInput(categoryExample.Id);
            var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

            //action
            await useCase.Handle(input, CancellationToken.None);

            //Asserts
            repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.Verify(x => x.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
            unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
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
