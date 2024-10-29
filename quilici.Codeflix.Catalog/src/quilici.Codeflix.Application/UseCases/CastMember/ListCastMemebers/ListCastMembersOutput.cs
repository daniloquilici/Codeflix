using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;
public class ListCastMembersOutput : PaginatedListOutput<CastMemberModelOutput>
{
    public ListCastMembersOutput(int page, int perPage, int total, IReadOnlyList<CastMemberModelOutput> items)
        : base(page, perPage, total, items)
    {
    }
}
