using FluentAssertions;
using quilici.Codeflix.Catalog.Domain.SeedWork;
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
    public async Task Pagination(int quantityGenerate, int page, int perPage, int expectedQuantityItems)
    {
        var examples = _fixture.GetExampleCastMembersList(10);
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
        output.Total.Should().Be(quantityGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        output.Items.ToList().ForEach(outputItem =>
        {
            var exampleItem = examples.FirstOrDefault(example => example.Id == outputItem.Id);
            exampleItem.Should().NotBeNull();
            exampleItem.Should().BeEquivalentTo(outputItem);
        });
    }
}
