using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;
public interface IListCastMembers : IRequestHandler<ListCastMembersInput, ListCastMembersOutput>
{
}
