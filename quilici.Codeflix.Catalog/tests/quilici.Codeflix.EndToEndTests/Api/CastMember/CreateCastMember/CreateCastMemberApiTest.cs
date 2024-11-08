using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Models;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.CreateCastMember;

[Collection(nameof(CastMemberApiBaseFixture))]
public class CreateCastMemberApiTest : IDisposable
{
    private readonly CastMemberApiBaseFixture _fixture;

    public CreateCastMemberApiTest(CastMemberApiBaseFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }

    [Fact(DisplayName = nameof(Create))]
    [Trait("EndtoEnd/Api", "CastMember/Create")]
    public async Task Create()
    {
        var example = _fixture.GetExampleCastMember();

        var (response, output) = await _fixture.ApiClient.Post<TestApiResponse<CastMemberModelOutput>>("/castmember", new CreateCastMemberInput(example.Name, example.Type));

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status201Created);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Data!.Id.Should().NotBeEmpty();
        output.Data.Name.Should().Be(example.Name);
        output.Data.Type.Should().Be(example.Type);

        var castMemberFromDb = await _fixture.Persistence.GetById(output.Data.Id);
        castMemberFromDb.Should().NotBeNull();
        castMemberFromDb!.Name.Should().Be(example.Name);
        castMemberFromDb.Type.Should().Be(example.Type);
    }

    [Theory(DisplayName = nameof(ThrowWhenNameIsEmpty))]
    [Trait("EndtoEnd/Api", "CastMember/Create")]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ThrowWhenNameIsEmpty(string name)
    {
        var example = _fixture.GetExampleCastMember();

        var (response, output) = await _fixture.ApiClient.Post<ProblemDetails>("/castmember", new CreateCastMemberInput(name!, example.Type));

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Detail.Should().Be("Name should not be empty or null");

    }
}
