using quilici.Codeflix.Catalog.Domain.Enum;
using quilici.Codeflix.Catalog.Domain.SeedWork;
using quilici.Codeflix.Catalog.Domain.Validation;

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
        Type = type;
        CreatedAt = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
    }
}
