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
    private readonly ICastMemberRepository _castMemberRepository;
    private readonly IStorageService _storageService;

    public CreateVideo(IUnitOfWork unitOfWork, IVideoRepository videoRepository, ICategoryRepository categoryRepository, IGenreRepository genreRepository, ICastMemberRepository castMemberRepository, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _videoRepository = videoRepository;
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
        _castMemberRepository = castMemberRepository;
        _storageService = storageService;
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
            await ValidateCategoriesIds(request, cancellationToken);
            request.CategoriesIds!.ToList().ForEach(video.AddCategory);
        }

        if ((request.GenresIds?.Count() ?? 0) > 0)
        {
            await ValidateGenresIds(request, cancellationToken);
            request.GenresIds!.ToList().ForEach(video.AddGenre);
        }

        if ((request.CastMembersIds?.Count() ?? 0) > 0)
        {
            await ValidateCastMembersIds(request, cancellationToken);
            request.CastMembersIds!.ToList().ForEach(video.AddCastMember);
        }

        if (request.Thumb is not null) 
        {
            var thumbUrl = await _storageService.Upload($"{video.Id}-thumb.{request.Thumb.Extension}", request.Thumb.FileStream, cancellationToken);
            video.UpdateThumb(thumbUrl);
        }

        if (request.Banner is not null)
        {
            var bannerUrl = await _storageService.Upload($"{video.Id}-banner.{request.Banner.Extension}", request.Banner.FileStream, cancellationToken);
            video.UpdateBanner(bannerUrl);
        }

        if (request.ThumbHalf is not null)
        {
            var thumbHalfbUrl = await _storageService.Upload($"{video.Id}-thumbHalf.{request.ThumbHalf.Extension}", request.ThumbHalf.FileStream, cancellationToken);
            video.UpdateThumbHalf(thumbHalfbUrl);
        }

        await _videoRepository.Insert(video, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return CreateVideoOutput.FromVideo(video);
    }

    private async Task ValidateCastMembersIds(CreateVideoInput request, CancellationToken cancellationToken)
    {
        var persistencesIds = await _castMemberRepository.GetIdsListByIds(request.CastMembersIds!.ToList(), cancellationToken);
        if (persistencesIds.Count < request.CastMembersIds!.Count)
        {
            var notfound = request.CastMembersIds.ToList().FindAll(castmember => !persistencesIds.Contains(castmember));
            throw new RelatedAggregateException($"Related castmember Id not found: {string.Join(',', notfound)}");
        }
    }

    private async Task ValidateGenresIds(CreateVideoInput request, CancellationToken cancellationToken)
    {
        var persistencesIds = await _genreRepository.GetIdsListByIds(request.GenresIds!.ToList(), cancellationToken);
        if (persistencesIds.Count < request.GenresIds!.Count)
        {
            var notfound = request.GenresIds.ToList().FindAll(genre => !persistencesIds.Contains(genre));
            throw new RelatedAggregateException($"Related genre Id not found: {string.Join(',', notfound)}");
        }
    }

    private async Task ValidateCategoriesIds(CreateVideoInput request, CancellationToken cancellationToken)
    {
        var persistencesIds = await _categoryRepository.GetIdsListByIds(request.CategoriesIds!.ToList(), cancellationToken);
        if (persistencesIds.Count < request.CategoriesIds!.Count)
        {
            var notfound = request.CategoriesIds.ToList().FindAll(category => !persistencesIds.Contains(category));
            throw new RelatedAggregateException($"Related category Id not found: {string.Join(',', notfound)}");
        }
    }
}
