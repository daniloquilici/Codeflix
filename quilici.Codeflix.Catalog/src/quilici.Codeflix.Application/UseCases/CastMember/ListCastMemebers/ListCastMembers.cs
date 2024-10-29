
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

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
        var searchOutput = await _castMemberRepository.Search(new SearchInput(request.Page, request.PerPage, request.Search, request.Sort, request.Dir), cancellationToken);

        return new ListCastMembersOutput(searchOutput.CurrentPage, searchOutput.PerPage, searchOutput.Total, searchOutput.Items.Select(castMember => CastMemberModelOutput.FromCastMember(castMember)).ToList().AsReadOnly());
    }
}
