using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using quilici.Codeflix.Catalog.Application.Exceptions;
using Xunit;
using Repository = quilici.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace quilici.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CastMemberRepository;

[Collection(nameof(CastMemberRepositoryTestFixture))]
public class CastMemberRepositoryTest
{
    private readonly CastMemberRepositoryTestFixture _fixture;

    public CastMemberRepositoryTest(CastMemberRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Insert()
    {
        var castMemberExample = _fixture.GetExampleCastMember();

        var context = _fixture.CreateDbContext();
        var repository = new Repository.CastMemberRepository(context);

        await repository.Insert(castMemberExample, CancellationToken.None);
        context.SaveChanges();

        var assertionContext = _fixture.CreateDbContext(true);
        var castMemberFromDb = await assertionContext.CastMembers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == castMemberExample.Id);
        castMemberFromDb.Should().NotBeNull();
        castMemberFromDb!.Name.Should().Be(castMemberExample.Name);
        castMemberFromDb.Type.Should().Be(castMemberExample.Type);
    }

    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Get()
    {
        var castMemberExampleList = _fixture.GetExampleCastMembersList(5);
        var castMemberExample = castMemberExampleList[3];

        var arrangeContext = _fixture.CreateDbContext();
        await arrangeContext.AddRangeAsync(castMemberExampleList);
        await arrangeContext.SaveChangesAsync();
        var repository = new Repository.CastMemberRepository(_fixture.CreateDbContext(true));

        var itemFromRepository = await repository.Get(castMemberExample.Id, CancellationToken.None);

        itemFromRepository.Should().NotBeNull();
        itemFromRepository!.Name.Should().Be(castMemberExample.Name);
        itemFromRepository.Type.Should().Be(castMemberExample.Type);
    }

    [Fact(DisplayName = nameof(ThrowsWhenNotFound))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task ThrowsWhenNotFound()
    {
        var randomGuid = Guid.NewGuid();

        var repository = new Repository.CastMemberRepository(_fixture.CreateDbContext(true));

        var action = async () =>  await repository.Get(randomGuid, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"CastMember '{randomGuid}' not found.");
    }

    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Delete()
    {
        //Arrange
        var castMemberExampleList = _fixture.GetExampleCastMembersList(5);
        var castMemberExample = castMemberExampleList[3];

        var arrangeContext = _fixture.CreateDbContext();
        await arrangeContext.AddRangeAsync(castMemberExampleList);
        await arrangeContext.SaveChangesAsync();

        //Act
        var actDbContext = _fixture.CreateDbContext(true);
        var repository = new Repository.CastMemberRepository(actDbContext);       
        await repository.Delete(castMemberExample, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //assert
        var assertionContext = _fixture.CreateDbContext(true);

        var castMemberFromDb = await assertionContext.CastMembers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == castMemberExample.Id);
        castMemberFromDb.Should().BeNull();

        var itemsInDataBase = assertionContext.CastMembers.AsNoTracking().ToList();
        itemsInDataBase.Should().NotBeNull();
        itemsInDataBase.Should().HaveCount(4);
        itemsInDataBase.Should().NotContain(castMemberExample);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CastMemberRepository - Repositories")]
    public async Task Update()
    {
        //Arrange
        var castMemberExampleList = _fixture.GetExampleCastMembersList(5);        
        var arrangeContext = _fixture.CreateDbContext();
        await arrangeContext.AddRangeAsync(castMemberExampleList);
        await arrangeContext.SaveChangesAsync();

        var castMemberExample = castMemberExampleList[3];
        castMemberExample.Update(_fixture.GetValidName(), _fixture.GetRandomCastMemberType());

        //Act
        var actDbContext = _fixture.CreateDbContext(true);
        var repository = new Repository.CastMemberRepository(actDbContext);
        await repository.Update(castMemberExample, CancellationToken.None);
        await actDbContext.SaveChangesAsync();

        //assert
        var assertionContext = _fixture.CreateDbContext(true);
        var castMemberFromDb = await assertionContext.CastMembers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == castMemberExample.Id);
        castMemberFromDb.Should().NotBeNull();
        castMemberFromDb!.Name.Should().Be(castMemberExample.Name);
        castMemberFromDb.Type.Should().Be(castMemberExample.Type);
    }
}
