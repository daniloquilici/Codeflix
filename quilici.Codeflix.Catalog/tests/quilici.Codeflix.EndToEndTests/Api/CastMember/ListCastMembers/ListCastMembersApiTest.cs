using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Models;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.ListCastMembers;

[Collection(nameof(CastMemberApiBaseFixture))]
public class ListCastMembersApiTest
{
    private readonly CastMemberApiBaseFixture _fixture;

    public ListCastMembersApiTest(CastMemberApiBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("EndToEnd/API", "CastMember/List")]
    public async Task List()
    {
        var examples = _fixture.GetExampleCastMembersList(5);
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CastMemberModelOutput>>("castmembers");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(1);
        output.Meta.Total.Should().Be(examples.Count);
        output.Data.Should().NotBeNull();
        output!.Data.Should().HaveCount(examples.Count);
        output.Data!.ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Type.Should().Be(exampleItem!.Type);
        });
    }
}
