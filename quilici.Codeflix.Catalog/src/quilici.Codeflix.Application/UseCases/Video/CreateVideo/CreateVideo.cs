using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Exceptions;
using quilici.Codeflix.Catalog.Domain.Repository;
using quilici.Codeflix.Catalog.Domain.Validation;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;


namespace quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
public class CreateVideo : ICreateVideo
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoRepository _videoRepository;

    public CreateVideo(IUnitOfWork unitOfWork, IVideoRepository videoRepository)
    {
        _unitOfWork = unitOfWork;
        _videoRepository = videoRepository;
    }

    public async Task<CreateVideoOutput> Handle(CreateVideoInput request, CancellationToken cancellationToken)
    {
        var video = new DomainEntity.Video(request.Title, request.Description, request.Opened, request.Published, request.YearLaunched, request.Duration, request.Rating);

        var notificationValidationHandler = new NotificationValidationHandler();
        video.Validate(notificationValidationHandler);
        if (notificationValidationHandler.HasErrors())
            throw new EntityValidationException("There are validation errors", notificationValidationHandler.Errors);

        if ((request.CategoriesIds?.Count ?? 0) > 0)
            request.CategoriesIds!.ToList().ForEach(video.AddCategory);

        await _videoRepository.Insert(video, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CreateVideoOutput.FromVideo(video);
    }
}
