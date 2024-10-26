using quilici.Codeflix.Catalog.Domain.Enum;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
public class CastMemberModelOutput
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public CastMemberType Type { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public CastMemberModelOutput(Guid id, string name, CastMemberType type, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;
    }

    public static CastMemberModelOutput FromCastMember(DomainEntity.CastMember castMember)
        => new CastMemberModelOutput(castMember.Id, castMember.Name, castMember.Type, castMember.CreatedAt);
}
