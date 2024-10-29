using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;
public class ListCastMembersOutput : PaginatedListOutput<CastMemberModelOutput>
{
    public ListCastMembersOutput(int page, int perPage, int total, IReadOnlyList<CastMemberModelOutput> items)
        : base(page, perPage, total, items)
    {
    }

    public static ListCastMembersOutput FromSearchOutput(SearchOutput<DomainEntity.CastMember> searchOutput)
        => new(
                searchOutput.CurrentPage,
                searchOutput.PerPage,
                searchOutput.Total,
                searchOutput.Items
                        .Select(castMember
                            => CastMemberModelOutput.FromCastMember(castMember))
                        .ToList()
                        .AsReadOnly());
}
