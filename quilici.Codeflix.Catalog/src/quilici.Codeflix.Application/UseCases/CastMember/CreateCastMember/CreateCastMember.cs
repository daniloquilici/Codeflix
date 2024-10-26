using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using quilici.Codeflix.Catalog.Domain.Repository;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
public class CreateCastMember : ICreateCastMember
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICastMemberRepository _castMemberRepository;

    public CreateCastMember(IUnitOfWork unitOfWork, ICastMemberRepository castMemberRepository)
    {
        _unitOfWork = unitOfWork;
        _castMemberRepository = castMemberRepository;
    }

    public async Task<CastMemberModelOutput> Handle(CreateCastMemberInput request, CancellationToken cancellationToken)
    {
        var castMember = new DomainEntity.CastMember(request.Name, request.Type);
        await _castMemberRepository.Insert(castMember, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return new CastMemberModelOutput(castMember.Id, castMember.Name, castMember.Type, castMember.CreatedAt);
    }
}
