using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
public class DeleteCastMemberInput : IRequest
{
    public Guid Id { get; private set; }

    public DeleteCastMemberInput(Guid id)
    {
        Id = id;
    }
}
