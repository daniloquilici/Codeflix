using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using quilici.Codeflix.Catalog.IntegrationTest.Base;
using Xunit;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.IntegrationTest.Application.UseCases.CastMember.Common;

[CollectionDefinition(nameof(CastMemberUseCasesBaseFixture))]
public class CastMemberUseCasesBaseFixtureCollection : ICollectionFixture<CastMemberUseCasesBaseFixture> { }

public class CastMemberUseCasesBaseFixture : BaseFixture
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

    public List<DomainEntity.CastMember> CloneListOrdered(List<DomainEntity.CastMember> castMemberList, string orderBy, SearchOrder order)
    {
        var listClone = new List<DomainEntity.CastMember>(castMemberList);

        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Name).ThenByDescending(x => x.Id),
            ("id", SearchOrder.Asc) => listClone.OrderBy(x => x.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(x => x.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(x => x.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(x => x.CreatedAt),
            _ => listClone.OrderBy(x => x.Name).ThenBy(x => x.Id),
        };

        return orderedEnumerable.ToList();
    }
}
