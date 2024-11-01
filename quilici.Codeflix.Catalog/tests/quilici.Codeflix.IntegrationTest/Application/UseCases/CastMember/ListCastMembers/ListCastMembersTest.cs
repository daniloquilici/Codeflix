using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.Common;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.ListCastMembers;

[Collection(nameof(CastMemberUseCasesBaseFixture))]
public class ListCastMembersTest
{
    private readonly CastMemberUseCasesBaseFixture _fixture;

    public ListCastMembersTest(CastMemberUseCasesBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("Intagration/Application", "ListCastMembers - Use Cases")]
    public async Task List()
    {
        var examples = _fixture.GetExampleCastMembersList(10);
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(examples);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.ListCastMembersInput(1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(examples.Count);
        output.Items.Should().HaveCount(examples.Count);

        output.Items.ToList().ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            exampleItem.Should().BeEquivalentTo(outputItem);
        });
    }

    [Fact(DisplayName = nameof(Empty))]
    [Trait("Intagration/Application", "ListCastMembers - Use Cases")]
    public async Task Empty()
    {
        var actDbContext = _fixture.CreateDbContext();
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.ListCastMembersInput(1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }

    [Theory(DisplayName = nameof(Pagination))]
    [Trait("Intagration/Application", "ListCastMembers - Use Cases")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(7, 3, 5, 0)]
    public async Task Pagination(int quantityToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        var examples = _fixture.GetExampleCastMembersList(quantityToGenerate);
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(examples);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.ListCastMembersInput(page, perPage, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(quantityToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        output.Items.ToList().ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            exampleItem.Should().BeEquivalentTo(outputItem);
        });
    }

    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Intagration/Application", "ListCastMembers - Use Cases")]
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
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(examples);
        await arrangeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.ListCastMembersInput(page, perPage, search, "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(expectedQuantityItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);

        output.Items.ToList().ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            exampleItem.Should().BeEquivalentTo(outputItem);
        });
    }

    [Theory(DisplayName = nameof(SearchOrdened))]
    [Trait("Intagration/Application", "ListCastMembers - Use Cases")]
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
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddRangeAsync(examples);
        await arrangeDbContext.SaveChangesAsync();

        var searchOrder = order.ToLower() == "asc" ? SearchOrder.Asc : SearchOrder.Desc;
        var actDbContext = _fixture.CreateDbContext(true);
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.ListCastMembersInput(1, 20, "", orderBy, searchOrder);
        var useCase = new UseCase.ListCastMembers(castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);
       
        output.Should().NotBeNull();
        output.Page.Should().Be(input.Page);
        output.PerPage.Should().Be(input.PerPage);
        output.Total.Should().Be(examples.Count);
        output.Items.Should().HaveCount(examples.Count);

        var orderedList = _fixture.CloneListOrdered(examples, orderBy, searchOrder);
        for (int i = 0; i < orderedList.Count; i++)
        {
            output.Items[i].Should().BeEquivalentTo(orderedList[i]);
        }       
    }
}
