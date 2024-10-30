using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.IntegrationTest.Base;
using System.ComponentModel.DataAnnotations;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.IntegrationTest.Infra.Data.EF.Repositories.CastMemberRepository;

[CollectionDefinition(nameof(CastMemberRepositoryTestFixture))]
public class CastMemberRepositoryTestFixtureCollection : ICollectionFixture<CastMemberRepositoryTestFixture> { }

public class CastMemberRepositoryTestFixture : BaseFixture
{
    public string GetValidName()
    => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)(new Random().Next(1, 2));

    public DomainEntity.CastMember GetExampleCastMember()
        => new DomainEntity.CastMember(GetValidName(), GetRandomCastMemberType());

    public List<DomainEntity.CastMember> GetExampleCastMembersList(int quantity)
        => Enumerable.Range(1, quantity).Select(_ => GetExampleCastMember()).ToList();

    public List<DomainEntity.CastMember> GetExampleCastMembersListByNames(List<string> names)
    => names.Select(name => 
    {
        var example = GetExampleCastMember();
        example.Update(name, example.Type);
        return example;
    }).ToList();
}
