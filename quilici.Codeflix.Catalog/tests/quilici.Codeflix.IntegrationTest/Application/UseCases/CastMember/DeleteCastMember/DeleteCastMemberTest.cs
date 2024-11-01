using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.Common;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.DeleteCastMember;


[Collection(nameof(CastMemberUseCasesBaseFixture))]
public class DeleteCastMemberTest
{
    private readonly CastMemberUseCasesBaseFixture _fixture;

    public DeleteCastMemberTest(CastMemberUseCasesBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Application", "DeleteCastMember - Use Cases")]
    public async Task Delete()
    {
        var example = _fixture.GetExampleCastMember();
        var arrengeDbContext = _fixture.CreateDbContext();
        await arrengeDbContext.CastMembers.AddAsync(example);
        await arrengeDbContext.SaveChangesAsync();

        var actDbContext = _fixture.CreateDbContext(true);
        var unitOfWork = new UnitOfWork(actDbContext);
        var castMemberRepository = new CastMemberRepository(actDbContext);

        var input = new UseCase.DeleteCastMemberInput(example.Id);
        var useCase = new UseCase.DeleteCastMember(unitOfWork, castMemberRepository);
        await useCase.Handle(input, CancellationToken.None);

        var assertDbContext = _fixture.CreateDbContext(true);
        var list = await assertDbContext.CastMembers.AsNoTracking().ToListAsync();
        list.Should().HaveCount(0);
    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Integration/Application", "DeleteCastMember - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var randomGuid = Guid.NewGuid();

        var actDbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWork(actDbContext);
        var castMemberRepository = new CastMemberRepository(actDbContext);

        var input = new UseCase.DeleteCastMemberInput(randomGuid);
        var useCase = new UseCase.DeleteCastMember(unitOfWork, castMemberRepository);
        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"CastMember '{randomGuid}' not found.");
    }
}
