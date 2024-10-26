using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
public interface ICreateCastMember : IRequestHandler<CreateCastMemberInput, CastMemberModelOutput>
{
}
