using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quilici.Codeflix.Catalog.Api.ApiModels.CastMember;
using quilici.Codeflix.Catalog.Api.ApiModels.Response;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.UpdateCastMember;

[Collection(nameof(CastMemberApiBaseFixture))]
public class UpdateCastMemberApiTest
{
    private readonly CastMemberApiBaseFixture _fixture;

    public UpdateCastMemberApiTest(CastMemberApiBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("EndToEnd/API", "CastMembers/Update")]
    public async Task Update()
    {
        var examples = _fixture.GetExampleCastMembersList(5);
        var example = examples[2];
        var newName = _fixture.GetValidName();
        var newType = _fixture.GetRandomCastMemberType();
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Put<ApiResponse<CastMemberModelOutput>>($"castmember/{example.Id}", new UpdateCastMemberApiInput(newName, newType));

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output.Should().NotBeNull();
        output!.Data.Should().NotBeNull();
        output.Data.Id.Should().Be(example.Id);
        output.Data.Name.Should().Be(newName);
        output.Data.Type.Should().Be(newType);

        var castMemberFromDb = await _fixture.Persistence.GetById(example.Id);
        castMemberFromDb.Should().NotBeNull();
        castMemberFromDb!.Name.Should().Be(newName);
        castMemberFromDb.Type.Should().Be(newType);
    }

    [Fact(DisplayName = nameof(NotFound))]
    [Trait("EndToEnd/API", "CastMembers/Update")]
    public async Task NotFound()
    {
        var examples = _fixture.GetExampleCastMembersList(5);
        var randomGuid = Guid.NewGuid();
        var newName = _fixture.GetValidName();
        var newType = _fixture.GetRandomCastMemberType();
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"castmember/{randomGuid}", new UpdateCastMemberApiInput(newName, newType));

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
        output.Should().NotBeNull();
        output!.Title.Should().Be("Not Found");
        output.Detail.Should().Be($"CastMember '{randomGuid}' not found.");
    }

    [Fact(DisplayName = nameof(ThereAreValidationErros))]
    [Trait("EndToEnd/API", "CastMembers/Update")]
    public async Task ThereAreValidationErros()
    {
        var examples = _fixture.GetExampleCastMembersList(5);
        var example = examples[2];
        var newName = "";
        var newType = _fixture.GetRandomCastMemberType();
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>($"castmember/{example.Id}", new UpdateCastMemberApiInput(newName, newType));

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status422UnprocessableEntity);
        output.Should().NotBeNull();
        output!.Title.Should().Be("One or more validation errors ocurred");
        output.Detail.Should().Be("Name should not be empty or null");
    }
}
