using FluentAssertions;
using Moq;
using Xunit;
using UseCase = quilici.Codeflix.Application.UseCases.Category.GetCategory;

namespace quilici.Codeflix.UnitTest.Application.GetCategory
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
            var exampleCategory = _fixture.GetValidCategory();

            repositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(exampleCategory);

            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

            //Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().Be(exampleCategory.CreateAt);
        }
    }
}
