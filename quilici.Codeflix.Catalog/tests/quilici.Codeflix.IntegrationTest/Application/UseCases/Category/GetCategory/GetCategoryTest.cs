﻿using FluentAssertions;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture) => _fixture = fixture;

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Integratrion/Applicatrion", "GetCategory - Use cases")]
        public async Task GetCategory()
        {
            //Arrange
            var dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            dbContext.Categories.Add(exampleCategory);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);

            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repository);

            //Action
            var output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            output.Should().NotBeNull();
            output.Name.Should().Be(exampleCategory.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
            output.Id.Should().Be(exampleCategory.Id);
            output.CreatedAt.Should().Be(exampleCategory.CreatedAt);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Integration/Applicatrion", "GetCategory - Use cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            //Arrange
            var dbContext = _fixture.CreateDbContext();
            var exampleCategory = _fixture.GetExampleCategory();
            dbContext.Categories.Add(exampleCategory);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var input = new UseCase.GetCategoryInput(Guid.NewGuid());
            var useCase = new UseCase.GetCategory(repository);

            //Action
            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Category '{input.Id}' not found.");
        }
    }
}
