using quilici.Codeflix.Catalog.Application.Common;
using quilici.Codeflix.Catalog.Application.Interfaces;
using quilici.Codeflix.Catalog.Domain.Repository;

namespace quilici.Codeflix.Catalog.Application.UseCases.Video.UploadMedias;
public class UploadMedias : IUploadMedias
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVideoRepository _videoRepository;
    private readonly IStorageService _storageService;

    public UploadMedias(IUnitOfWork unitOfWork, IVideoRepository videoRepository, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _videoRepository = videoRepository;
        _storageService = storageService;
    }

    public async Task Handle(UploadMediasInput request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.Get(request.VideoId, cancellationToken);
        await UploadVideo(request, video, cancellationToken);
        await UploadTrailer(request, video, cancellationToken);

        await _videoRepository.Update(video, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task UploadTrailer(UploadMediasInput request, Domain.Entity.Video video, CancellationToken cancellationToken)
    {
        if (request.TrailerFile is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.Trailer), request.TrailerFile.Extension);
            var uplodadeFilePath = await _storageService.Upload(fileName, request.TrailerFile.FileStream, cancellationToken);
            video.UpdateTrailer(uplodadeFilePath);
        }
    }

    private async Task UploadVideo(UploadMediasInput request, Domain.Entity.Video video, CancellationToken cancellationToken)
    {
        if (request.VideoFile is not null)
        {
            var fileName = StorageFileName.Create(video.Id, nameof(video.Media), request.VideoFile.Extension);
            var uplodadeFilePath = await _storageService.Upload(fileName, request.VideoFile.FileStream, cancellationToken);
            video.UpdateMedia(uplodadeFilePath);
        }
    }
}
