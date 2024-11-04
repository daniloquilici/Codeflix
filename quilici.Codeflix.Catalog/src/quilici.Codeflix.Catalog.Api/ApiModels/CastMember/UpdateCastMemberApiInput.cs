using quilici.Codeflix.Catalog.Domain.Enum;

namespace quilici.Codeflix.Catalog.Api.ApiModels.CastMember;

public class UpdateCastMemberApiInput
{
    public UpdateCastMemberApiInput(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; private set; }

    public CastMemberType Type { get; private set; }
}
