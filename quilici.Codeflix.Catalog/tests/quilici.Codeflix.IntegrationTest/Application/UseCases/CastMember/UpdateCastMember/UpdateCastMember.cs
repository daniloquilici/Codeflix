using FluentAssertions;
using quilici.Codeflix.Catalog.Application.Exceptions;
using quilici.Codeflix.Catalog.Infra.Data.EF;
using quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;
using quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.Common;
using Xunit;
using UseCase = quilici.Codeflix.Catalog.Application.UseCases.CastMember.UpdateCastMember;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.UpdateCastMember;

[Collection(nameof(CastMemberUseCasesBaseFixture))]
public class UpdateCastMember
{
    private readonly CastMemberUseCasesBaseFixture _fixture;

    public UpdateCastMember(CastMemberUseCasesBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Application", "UpdateCastMember - Use Cases")]
    public async Task Update() 
    {
        var example = _fixture.GetExampleCastMember();
        var arrangeDbContext = _fixture.CreateDbContext();
        await arrangeDbContext.AddAsync(example);
        await arrangeDbContext.SaveChangesAsync();
        
        var actDbContext = _fixture.CreateDbContext(true);        
        var unitOfWork = new UnitOfWork(actDbContext);
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.UpdateCastMemberInput(example.Id, _fixture.GetValidName(), _fixture.GetRandomCastMemberType());
        var useCase = new UseCase.UpdateCastMember(unitOfWork, castMemberRepository);
        var output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Id.Should().Be(example.Id);
        output.Name.Should().Be(input.Name);
        output.Type.Should().Be(input.Type);

        var assertDbContext = _fixture.CreateDbContext(true);
        var itemFromDb = await assertDbContext.CastMembers.FindAsync(example.Id);
        itemFromDb.Should().NotBeNull();
        itemFromDb!.Name.Should().Be(input.Name);
        itemFromDb.Type.Should().Be(input.Type);

    }

    [Fact(DisplayName = nameof(ThrowWhenNotFound))]
    [Trait("Integration/Application", "UpdateCastMember - Use Cases")]
    public async Task ThrowWhenNotFound()
    {
        var randomGuid = Guid.NewGuid();

        var actDbContext = _fixture.CreateDbContext(true);
        var unitOfWork = new UnitOfWork(actDbContext);
        var castMemberRepository = new CastMemberRepository(actDbContext);
        var input = new UseCase.UpdateCastMemberInput(randomGuid, _fixture.GetValidName(), _fixture.GetRandomCastMemberType());
        var useCase = new UseCase.UpdateCastMember(unitOfWork, castMemberRepository);
        
        var action = async () => await useCase.Handle(input, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"CastMember '{randomGuid}' not found.");
    }
}
