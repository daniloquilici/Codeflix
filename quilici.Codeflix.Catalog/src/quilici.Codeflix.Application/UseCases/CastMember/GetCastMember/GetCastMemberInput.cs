using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;
public class GetCastMemberInput : IRequest<CastMemberModelOutput>
{
    public Guid Id { get; private set; }

    public GetCastMemberInput(Guid id)
    {
        Id = id;
    }
}
