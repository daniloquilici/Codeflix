using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;
public interface IGetCastMember : IRequestHandler<GetCastMemberInput, CastMemberModelOutput>
{
}
