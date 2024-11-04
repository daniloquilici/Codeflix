using MediatR;
using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMemebers;
public class ListCastMembersInput : PaginatedListInput, IRequest<ListCastMembersOutput>
{
    public ListCastMembersInput(int page, int perPage, string search, string sort, SearchOrder dir)
        : base(page, perPage, search, sort, dir)
    {
    }

    public ListCastMembersInput()
    : base(1, 15, "", "", SearchOrder.Asc)
    {
    }
}
