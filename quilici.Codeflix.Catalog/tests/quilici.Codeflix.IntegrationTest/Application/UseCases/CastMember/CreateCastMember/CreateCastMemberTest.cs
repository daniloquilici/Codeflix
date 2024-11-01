using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using System.Net;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.CreateCastMember;

[Collection(nameof(CreateCastMemberTestFixture))]
public class CreateCastMemberTest
{
    private readonly CreateCastMemberTestFixture _fixture;

    public CreateCastMemberTest(CreateCastMemberTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Create))]
    [Trait("Integration/Application", "CreateCastMember - Use Cases")]
    public async Task Create()
    {
        var actDbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(actDbContext);
        var castMemberRepository = new CastMemberRepository(actDbContext);

        var input = new UseCase.CreateCastMemberInput(_fixture.GetValidName(), _fixture.GetRandomCastMemberType());
        var useCase = new UseCase.CreateCastMember(unitOfWork, castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);
        output.Id.Should().NotBeEmpty();
        output.CreatedAt.Should().NotBe(default);

        var assertDbContext = _fixture.CreateDbContext(true);
        var castMembers = await assertDbContext.CastMembers.AsNoTracking().ToListAsync();
        castMembers.Should().HaveCount(1);
        var castMemberFromDb = castMembers[0];
        castMemberFromDb.Name.Should().Be(input.Name);
        castMemberFromDb.Type.Should().Be(input.Type);
        castMemberFromDb.Id.Should().Be(output.Id);
    }
}
