using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
public class UpdateCastMember : IUpdateCastMember
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICastMemberRepository _castMemberRepository;

    public UpdateCastMember(IUnitOfWork unitOfWork, ICastMemberRepository castMemberRepository)
    {
        _unitOfWork = unitOfWork;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<CastMemberModelOutput> Handle(UpdateCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = await _castMemberRepository.Get(request.Id, cancellationToken);
        castMember.Update(request.Name, request.Type);
        await _castMemberRepository.Update(castMember, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return CastMemberModelOutput.FromCastMember(castMember);
    }
}
