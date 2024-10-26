using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Domain.Enum;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
public class CreateCastMemberInput : IRequest<CastMemberModelOutput>
{
    public string Name { get; private set; }

    public CastMemberType Type { get; private set; }

    public CreateCastMemberInput(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
    }
}
