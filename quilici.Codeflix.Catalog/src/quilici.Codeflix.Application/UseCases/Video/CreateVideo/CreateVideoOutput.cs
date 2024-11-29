using quilici.Codeflix.Catalog.Domain.Enum;
using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
public record CreateVideoOutput(Guid Id, DateTime CreatedAt, string Title, bool Published, string Description, Rating Rating, int YearLaunched, bool Opened, int Duration) 
{
    public static CreateVideoOutput FromVideo(DomainEntity.Video video) 
    {
        return new CreateVideoOutput(video.Id, video.CreatedAt, video.Title, video.Published, video.Description, video.Rating, video.YearLaunched, video.Opened, video.Duration);
    }
}
