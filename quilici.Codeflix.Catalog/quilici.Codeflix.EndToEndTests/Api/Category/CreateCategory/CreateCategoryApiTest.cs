using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Application.UseCases.Category.Common;
using quilici.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    [Collection(nameof(CreateCategoryApiTestFixture))]
    public class CreateCategoryApiTest
    {
        private readonly CreateCategoryApiTestFixture _fixture;

        public CreateCategoryApiTest(CreateCategoryApiTestFixture fixture)
            => _fixture = fixture;

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("EndToEnd/API", "Category - Endpoints")]
        public async Task CreateCategory()
        {
            var input = _fixture.GetExampleInput();

            var (response, output) = await _fixture.ApiClient.Post<CategoryModelOutput>("/categories", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.Created);
            output.Should().NotBeNull();
            output!.Name.Should().Be(input.Name);
            output.Description.Should().Be(input.Description);
            output.IsActive.Should().Be(input.IsActive);
            output.Id.Should().NotBeEmpty();
            output.CreatedAt.Should().NotBeSameDateAs(default);
            var dbCategory = await _fixture.Persistence.GetById(output.Id);
            dbCategory.Should().NotBeNull();
            dbCategory!.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be(input.IsActive);
            dbCategory.Id.Should().NotBeEmpty();
            dbCategory.CreatedAt.Should().NotBeSameDateAs(default);
        }

        [Theory(DisplayName = nameof(ThrowWhenCantIntantiateAggregate))]
        [Trait("EndToEnd/API", "Category - Endpoints")]
        [MemberData(nameof(CreateCategoryApiTestDataGenerator.GetInvalidInputs), MemberType = typeof(CreateCategoryApiTestDataGenerator))]
        public async Task ThrowWhenCantIntantiateAggregate(CreateCategoryInput input, string expectedDetail)
        {
            var (response, output) = await _fixture.ApiClient.Post<ProblemDetails>("/categories", input);

            response.Should().NotBeNull();
            response!.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output!.Title.Should().Be("One or more validation errors ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be(StatusCodes.Status422UnprocessableEntity);
            output.Detail.Should().Be(expectedDetail);
        }
    }
}
