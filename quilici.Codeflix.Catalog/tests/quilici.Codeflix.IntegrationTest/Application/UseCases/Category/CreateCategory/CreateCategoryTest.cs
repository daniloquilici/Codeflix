using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Application.UseCases.Category.CreateCategory;
using quilici.Codeflix.Domain.Exceptions;
using quilici.Codeflix.Infra.Data.EF;
using quilici.Codeflix.Infra.Data.EF.Repositories;
using Xunit;
using ApplicationUseCases = quilici.Codeflix.Application.UseCases.Category.CreateCategory;

namespace quilici.Codeflix.IntegrationTest.Application.UseCases.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))]
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture) => this._fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use cases")]
        public async void CreateCategory()
        {
            //Arrange
            var dbcontext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbcontext);
            var unitOfWork = new UnitOfWork(dbcontext);

            var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

            var input = _fixture.GetInput();

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryOnlyWithName))]
        [Trait("Integration/Application", "CreateCategory - Use cases")]
        public async void CreateCategoryOnlyWithName()
        {
            //Arrange
            var dbcontext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbcontext);
            var unitOfWork = new UnitOfWork(dbcontext);

            var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

            var input = new CreateCategoryInput(_fixture.GetInput().Name);

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be("");
            dbCategory.IsActive.Should().Be(true);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be("");
            output.IsActive.Should().Be(true);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Fact(DisplayName = nameof(CreateCategoryOnlyWithNameAndDescription))]
        [Trait("Integration/Application", "CreateCategory - Use cases")]
        public async void CreateCategoryOnlyWithNameAndDescription()
        {
            //Arrange
            var dbcontext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbcontext);
            var unitOfWork = new UnitOfWork(dbcontext);

            var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

            var exampleInput = _fixture.GetInput();
            var input = new CreateCategoryInput(exampleInput.Name, exampleInput.Description);

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert

            var dbCategory = await (_fixture.CreateDbContext(true)).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(true);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(true);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "CreateCategory - Use cases")]
        [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs), parameters: 4, MemberType = typeof(CreateCategoryTestDataGenerator))]
        public async void ThrowWhenCantInstantiateCategory(
            CreateCategoryInput input,
            string expectedEsceptionMessage)
        {
            //Arrange
            var dbcontext = _fixture.CreateDbContext();
            var repository = new CategoryRepository(dbcontext);
            var unitOfWork = new UnitOfWork(dbcontext);

            var useCase = new ApplicationUseCases.CreateCategory(repository, unitOfWork);

            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedEsceptionMessage);
            var dbCategoriesList = _fixture.CreateDbContext(true).Categories.AsNoTracking().ToList();
            dbCategoriesList.Should().HaveCount(0);
        }
    }
}
