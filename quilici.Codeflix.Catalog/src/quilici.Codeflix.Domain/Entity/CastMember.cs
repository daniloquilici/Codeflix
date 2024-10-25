using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.Domain.SeedWork;

namespace quilici.Codeflix.Catalog.Domain.Entity;
public class CastMember : AggregateRoot
{
    public string Name { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public CastMemberType Type { get; private set; }

    public CastMember(string name, CastMemberType type)
        : base()
    {
        Name = name;
        CreatedAt = DateTime.Now;
        Type = type;
    }
}
