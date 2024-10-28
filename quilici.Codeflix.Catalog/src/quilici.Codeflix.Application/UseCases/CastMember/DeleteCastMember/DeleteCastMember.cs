using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
public class DeleteCastMember : IDeleteCastMember
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICastMemberRepository _castMemberRepository;

    public DeleteCastMember(IUnitOfWork unitOfWork, ICastMemberRepository castMemberRepository)
    {
        _unitOfWork = unitOfWork;
        _castMemberRepository = castMemberRepository;
    }

    public async Task Handle(DeleteCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = await _castMemberRepository.Get(request.Id, cancellationToken);
        await _castMemberRepository.Delete(castMember, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
