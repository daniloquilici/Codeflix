using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
public interface IUpdateCastMember : IRequestHandler<UpdateCastMemberInput, CastMemberModelOutput>
{
}
