using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
using quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Models;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.CreateCastMember;

[Collection(nameof(CastMemberApiBaseFixture))]
public class CreateCastMemberApiTest
{
    private readonly CastMemberApiBaseFixture _fixture;

    public CreateCastMemberApiTest(CastMemberApiBaseFixture fixture)
    {
        _fixture = fixture;
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
}
