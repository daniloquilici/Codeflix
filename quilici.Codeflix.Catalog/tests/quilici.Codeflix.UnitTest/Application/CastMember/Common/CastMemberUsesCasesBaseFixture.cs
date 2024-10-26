using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.UnitTest.Common;

namespace quilici.Codeflix.Catalog.UnitTest.Application.CastMember.Common;
public class CastMemberUsesCasesBaseFixture : BaseFixture
{
    public string GetValidName()
        => Faker.Name.FullName();

    public CastMemberType GetRandomCastMemberType()
        => (CastMemberType)(new Random().Next(1, 2));
}
