using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;
public class GetCastMember : IGetCastMember
{
    private readonly ICastMemberRepository _castMemberRepository;

    public GetCastMember(ICastMemberRepository castMemberRepository)
    {
        _castMemberRepository = castMemberRepository;
    }

    public async Task<CastMemberModelOutput> Handle(GetCastMemberInput request, CancellationToken cancellationToken)
    {
        var casMember = await _castMemberRepository.Get(request.Id, cancellationToken);
        return CastMemberModelOutput.FromCastMember(casMember);
    }
}
