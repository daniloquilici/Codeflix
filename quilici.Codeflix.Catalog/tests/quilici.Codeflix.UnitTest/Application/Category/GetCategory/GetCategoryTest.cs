using FluentAssertions;
using Moq;
using quilici.Codeflix.Application.Exceptions;
using Xunit;
using UseCase = quilici.Codeflix.Application.UseCases.Category.GetCategory;

namespace quilici.Codeflix.UnitTest.Application.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Applicatrion", "GetCategory - Use cases")]
        public async Task GetCategory()
        {
            //Arrange
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var exampleCategory = _fixture.GetExampleCategory();

            repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);

            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);

            //Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().Be(exampleCategory.CreateAt);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Applicatrion", "GetCategory - Use cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            //Arrange
            var repositoryMock = _fixture.GetCategoryRepositoryMock();
            var exampleGuid = Guid.NewGuid();

            repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found"));

            var input = new UseCase.GetCategoryInput(exampleGuid);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            //Action
            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>();

            repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        }
    }
}
