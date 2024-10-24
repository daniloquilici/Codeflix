﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using Xunit;
using ApplicationUseCase = quilici.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
            => _fixture = fixture;

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategory(Domain.Entity.Category exampleCategory, UpdateCategoryInput input)
        {
            //Arrange
            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackinginfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackinginfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive!);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be((bool)input.IsActive!);
        }

        [Theory(DisplayName = nameof(UpdateCategoryWithoutIsActive))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryWithoutIsActive(Domain.Entity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            //Arrange
            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackinginfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackinginfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
        }

        [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 5, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryOnlyName(Domain.Entity.Category exampleCategory, UpdateCategoryInput exampleInput)
        {
            //Arrange
            var input = new UpdateCategoryInput(exampleInput.Id, exampleInput.Name);

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            var trackinginfo = await dbContext.AddAsync(exampleCategory);
            dbContext.SaveChanges();
            trackinginfo.State = EntityState.Detached;
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            //Assert
            var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(output.Id);

            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be(exampleCategory.IsActive);
            dbCategory.CreatedAt.Should().Be(output.CreatedAt);

            output.Should().NotBeNull();
            output.Name.Should().Be(input.Name);
            output.Description.Should().Be(exampleCategory.Description);
            output.IsActive.Should().Be(exampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(UpdateCategoryThrowsWhenNotFoundCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        public async Task UpdateCategoryThrowsWhenNotFoundCategory()
        {
            //Arrange
            var input = _fixture.GetValidInput();

            var dbContext = _fixture.CreateDbContext();
            await dbContext.AddRangeAsync(_fixture.GetExampleCategoriesList());
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);

            //Assert
            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<NotFoundException>().WithMessage($"Category '{input.Id}' not found.");
        }

        [Theory(DisplayName = nameof(UpdateCategoryThrowsWhenCantInstantiateCategory))]
        [Trait("Integration/Application", "UpdateCategory - Use Cases")]
        [MemberData(nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs), parameters: 6, MemberType = typeof(UpdateCategoryTestDataGenerator))]
        public async Task UpdateCategoryThrowsWhenCantInstantiateCategory(UpdateCategoryInput input, string expectedExceptionMessage)
        {
            var dbContext = _fixture.CreateDbContext();
            var exampleCategories = _fixture.GetExampleCategoriesList();
            await dbContext.AddRangeAsync(exampleCategories);
            dbContext.SaveChanges();
            var repository = new CategoryRepository(dbContext);
            var unitOfWork = new UnitOfWork(dbContext);
            var useCase = new ApplicationUseCase.UpdateCategory(repository, unitOfWork);
            input.Id = exampleCategories[0].Id;

            //Assert
            var task = async () => await useCase.Handle(input, CancellationToken.None);
            await task.Should().ThrowAsync<EntityValidationException>().WithMessage(expectedExceptionMessage);
        }
    }
}
