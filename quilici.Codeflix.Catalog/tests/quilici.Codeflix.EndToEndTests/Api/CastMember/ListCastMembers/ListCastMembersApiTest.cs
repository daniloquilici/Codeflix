using FluentAssertions;
using Microsoft.AspNetCore.Http;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.Common;
using quilici.Codeflix.Catalog.EndToEndTests.Extensions;
using quilici.Codeflix.Catalog.EndToEndTests.Models;
using System.Net;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.CastMember.ListCastMembers;

[Collection(nameof(CastMemberApiBaseFixture))]
public class ListCastMembersApiTest : IDisposable
{
    private readonly CastMemberApiBaseFixture _fixture;

    public ListCastMembersApiTest(CastMemberApiBaseFixture fixture)
    {
        _fixture = fixture;
    }

    public void Dispose()
    {
        _fixture.CleanPersistence();
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("EndToEnd/API", "CastMember/List")]
    public async Task List()
    {
        var examples = _fixture.GetExampleCastMembersList(5);
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CastMemberModelOutput>>("castmember");

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

    [Fact(DisplayName = nameof(ReturnsEmpty))]
    [Trait("EndToEnd/API", "CastMember/List")]
    public async Task ReturnsEmpty()
    {
        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CastMemberModelOutput>>("castmember");

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(1);
        output.Meta.Total.Should().Be(0);
        output.Data.Should().NotBeNull();
        output!.Data.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(Paginated))]
    [Trait("EndToEnd/API", "CastMember/List")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task Paginated(int quantityToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        var examples = _fixture.GetExampleCastMembersList(quantityToGenerate);
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CastMemberModelOutput>>("castmember", new ListCastMembersInput() { Page = page, PerPage = perPage });

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(page);
        output.Meta!.PerPage.Should().Be(perPage);
        output.Meta.Total.Should().Be(examples.Count);
        output.Data.Should().NotBeNull();
        output!.Data.Should().HaveCount(expectedQuantityItems);
        output.Data!.ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Type.Should().Be(exampleItem!.Type);
        });
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("EndToEnd/API", "CastMember/List")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(string search, int page, int perPage, int expectedQuantityItemsReturned, int expectedQuantityItems)
    {
        var namesToGenerate = new List<string>() { "Action", "Horror", "Horror - Robots", "Horror - Based on Real Facts", "Drama", "Sci-fi IA", "Sci-fi Space", "Sci-fi Robots", "Sci-fi Future" };

        var examples = _fixture.GetExampleCastMembersListByNames(namesToGenerate);
        await _fixture.Persistence.InsertList(examples);

        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CastMemberModelOutput>>("castmember", new ListCastMembersInput() { Page = page, PerPage = perPage, Search = search });

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(page);
        output.Meta!.PerPage.Should().Be(perPage);
        output.Meta.Total.Should().Be(expectedQuantityItems);
        output.Data.Should().NotBeNull();
        output!.Data.Should().HaveCount(expectedQuantityItemsReturned);
        output.Data!.ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(x => x.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            outputItem.Id.Should().Be(exampleItem!.Id);
            outputItem.Name.Should().Be(exampleItem!.Name);
            outputItem.Type.Should().Be(exampleItem!.Type);
        });
    }

    [Theory(DisplayName = nameof(SearchOrdened))]
    [Trait("EndToEnd/API", "CastMember/List")]
    [InlineData("name", "asc")]
    [InlineData("name", "desc")]
    [InlineData("id", "asc")]
    [InlineData("id", "desc")]
    [InlineData("CreatedAt", "asc")]
    [InlineData("CreatedAt", "desc")]
    [InlineData("", "asc")]
    public async Task SearchOrdened(string orderBy, string order)
    {
        var examples = _fixture.GetExampleCastMembersList(10);
        await _fixture.Persistence.InsertList(examples);

        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var (response, output) = await _fixture.ApiClient.Get<TestApiResponseList<CastMemberModelOutput>>("castmember", new { Sort = orderBy, Dir = order });

        response.Should().NotBeNull();
        response!.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        output!.Should().NotBeNull();
        output!.Meta.Should().NotBeNull();
        output.Meta!.CurrentPage.Should().Be(1);
        output.Meta.Total.Should().Be(examples.Count);
        output.Data.Should().NotBeNull();
        output!.Data.Should().HaveCount(examples.Count);

        var orderedList = _fixture.CloneListOrdered(examples, orderBy, searchOrder);
        for (int i = 0; i < orderedList.Count; i++)
        {
            output.Data.Should().NotBeNull();
            output.Data![i].Id.Should().Be(orderedList[i].Id);
            output.Data![i].Name.Should().Be(orderedList[i].Name);
            output.Data![i].Type.Should().Be(orderedList[i].Type);
            output.Data![i].CreatedAt.TrimMillisseconds().Should().Be(orderedList[i].CreatedAt.TrimMillisseconds());
        }
    }
}
