using FluentAssertions;
using Moq;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.ListCastMember;

[Collection(nameof(ListCastMemberTestFixture))]
public class ListCastMemberTest
{
    private readonly ListCastMemberTestFixture _fixture;

    public ListCastMemberTest(ListCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(ListCastMember))]
    [Trait("Application", "ListCastMember - Use Cases")]
    public async Task ListCastMember()
    {
        var castMembersListExample = _fixture.GetExampleCastMembersList(3);
        var repositorySearchOutput = new SearchOutput<DomainEntity.CastMember>(1, 10, castMembersListExample.Count, (IReadOnlyList<DomainEntity.CastMember>)castMembersListExample);
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();
        castMemberRepositoryMock.Setup(x => x.Search(It.IsAny<SearchInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(repositorySearchOutput);

        var input = new UseCase.ListCastMembersInput(1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(castMemberRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositorySearchOutput.CurrentPage);
        output.PerPage.Should().Be(repositorySearchOutput.PerPage);
        output.Total.Should().Be(repositorySearchOutput.Total);
        output.Items.ToList().ForEach(outputItem =>
        {
            var exemple = castMembersListExample.Find(x => x.Id == outputItem.Id);
            exemple.Should().NotBeNull();
            exemple!.Name.Should().Be(outputItem.Name);
            exemple.Type.Should().Be(outputItem.Type);
        });

        castMemberRepositoryMock.Verify(x => x.Search(It.Is<SearchInput>(x => x.Page == input.Page
                                                                                && x.PerPage == input.PerPage
                                                                                && x.Search == input.Search
                                                                                && x.Order == input.Dir
                                                                                && x.OrderBy == input.Sort), It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact(DisplayName = nameof(ReturnsEmptyWhenIsEmpty))]
    [Trait("Application", "ListCastMember - Use Cases")]
    public async Task ReturnsEmptyWhenIsEmpty()
    {
        var castMembersListExample = new List<DomainEntity.CastMember>();
        var repositorySearchOutput = new SearchOutput<DomainEntity.CastMember>(1, 10, castMembersListExample.Count, (IReadOnlyList<DomainEntity.CastMember>)castMembersListExample);
        var castMemberRepositoryMock = new Mock<ICastMemberRepository>();
        castMemberRepositoryMock.Setup(x => x.Search(It.IsAny<SearchInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(repositorySearchOutput);

        var input = new UseCase.ListCastMembersInput(1, 10, "", "", SearchOrder.Asc);
        var useCase = new UseCase.ListCastMembers(castMemberRepositoryMock.Object);

        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Page.Should().Be(repositorySearchOutput.CurrentPage);
        output.PerPage.Should().Be(repositorySearchOutput.PerPage);
        output.Total.Should().Be(repositorySearchOutput.Total);
        output.Items.Should().HaveCount(castMembersListExample.Count);

        castMemberRepositoryMock.Verify(x => x.Search(It.Is<SearchInput>(x => x.Page == input.Page
                                                                                && x.PerPage == input.PerPage
                                                                                && x.Search == input.Search
                                                                                && x.Order == input.Dir
                                                                                && x.OrderBy == input.Sort), It.IsAny<CancellationToken>()), Times.Once);

    }
}
