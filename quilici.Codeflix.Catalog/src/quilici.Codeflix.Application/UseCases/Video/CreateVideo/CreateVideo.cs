using quilici.Codeflix.Catalog.Application.Exceptions;
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
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;

    public CreateVideo(IUnitOfWork unitOfWork, IVideoRepository videoRepository, ICategoryRepository categoryRepository, IGenreRepository genreRepository)
    {
        _unitOfWork = unitOfWork;
        _videoRepository = videoRepository;
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
    }

    public async Task<CreateVideoOutput> Handle(CreateVideoInput request, CancellationToken cancellationToken)
    {
        var video = new DomainEntity.Video(request.Title, request.Description, request.Opened, request.Published, request.YearLaunched, request.Duration, request.Rating);

        var notificationValidationHandler = new NotificationValidationHandler();
        video.Validate(notificationValidationHandler);
        if (notificationValidationHandler.HasErrors())
            throw new EntityValidationException("There are validation errors", notificationValidationHandler.Errors);

        if ((request.CategoriesIds?.Count ?? 0) > 0)
        {
            var persistencesIds = await _categoryRepository.GetIdsListByIds(request.CategoriesIds!.ToList(), cancellationToken);
            if (persistencesIds.Count < request.CategoriesIds!.Count)
            {
                var notfound = request.CategoriesIds.ToList().FindAll(category => !persistencesIds.Contains(category));
                throw new RelatedAggregateException($"Related category Id not found: {string.Join(',', notfound)}");
            }

            request.CategoriesIds!.ToList().ForEach(video.AddCategory);
        }

        if ((request.GenresIds?.Count() ?? 0) > 0)
        {
            request.GenresIds!.ToList().ForEach(video.AddGenre);
            var persistencesIds = await _genreRepository.GetIdsListByIds(request.GenresIds!.ToList(), cancellationToken);
            if (persistencesIds.Count < request.GenresIds!.Count)
            {
                var notfound = request.GenresIds.ToList().FindAll(genre => !persistencesIds.Contains(genre));
                throw new RelatedAggregateException($"Related genre Id not found: {string.Join(',', notfound)}");
            }
        }

        await _videoRepository.Insert(video, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CreateVideoOutput.FromVideo(video);
    }
}
