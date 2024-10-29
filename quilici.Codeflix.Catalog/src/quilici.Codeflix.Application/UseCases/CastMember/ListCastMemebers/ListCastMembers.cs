using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;
public class ListCastMembers : IListCastMembers
{
    private readonly ICastMemberRepository _castMemberRepository;

    public ListCastMembers(ICastMemberRepository castMemberRepository)
    {
        _castMemberRepository = castMemberRepository;
    }

    public async Task<ListCastMembersOutput> Handle(ListCastMembersInput request, CancellationToken cancellationToken)
    {
        var searchOutput = await _castMemberRepository.Search(request.ToSearchInput(), cancellationToken);
        return ListCastMembersOutput.FromSearchOutput(searchOutput);
    }
}
