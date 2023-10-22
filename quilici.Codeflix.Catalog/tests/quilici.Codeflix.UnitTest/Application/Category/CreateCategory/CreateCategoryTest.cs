using FluentAssertions;
using Moq;
using quilici.Codeflix.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.Domain.Entity;
using quilici.Codeflix.Domain.Exceptions;
using Xunit;
using UseCases = quilici.Codeflix.Application.UseCases.Category.CreateCategory;

namespace quilici.Codeflix.UnitTest.Application.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _createCategoryTestFixture;

        public CreateCategoryTest(CreateCategoryTestFixture createCategoryTestFixture)
        {
            _createCategoryTestFixture = createCategoryTestFixture;
        }

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Aplication", "CreateCategory - Use cases")]
        public async void CreateCategory()
        {
            //Arrange
            var repositoryMock = _createCategoryTestFixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var input = _createCategoryTestFixture.GetInput();

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);

            //Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("Aplication", "CreateCategory - Use cases")]
        [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs), parameters: 24, MemberType = typeof(CreateCategoryTestDataGenerator))]
        public async void ThrowWhenCantInstantiateCategory(CreateCategoryInput createCategoryInput, string exceptionMessage)
        {
            var useCase = new UseCases.CreateCategory(_createCategoryTestFixture.GetCategoryRepositoryMock().Object, _createCategoryTestFixture.GetUnitOfWorkMock().Object);

            Func<Task> task = async () => await useCase.Handle(createCategoryInput, CancellationToken.None);

            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(exceptionMessage);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
        [Trait("Aplication", "CreateCategory - Use cases")]
        public async void CreateCategoryWithOnlyName()
        {
            //Arrange
            var repositoryMock = _createCategoryTestFixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var input = new CreateCategoryInput(_createCategoryTestFixture.GetValidCategoryName());

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);

            //Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
        [Trait("Aplication", "CreateCategory - Use cases")]
        public async void CreateCategoryWithOnlyNameAndDescription()
        {
            //Arrange
            var repositoryMock = _createCategoryTestFixture.GetCategoryRepositoryMock();
            var unitOfWorkMock = _createCategoryTestFixture.GetUnitOfWorkMock();

            var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

            var input = new CreateCategoryInput(_createCategoryTestFixture.GetValidCategoryName(), _createCategoryTestFixture.GetValidCategoryDescription());

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            repositoryMock.Verify(repository => repository.Insert(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);

            unitOfWorkMock.Verify(uow => uow.Commit(It.IsAny<CancellationToken>()), Times.Once);

            //Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().BeTrue();
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }
    }
}
