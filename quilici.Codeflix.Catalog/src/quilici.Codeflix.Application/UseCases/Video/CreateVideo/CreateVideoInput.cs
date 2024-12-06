using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Video.Common;
using quilici.Codeflix.Catalog.Domain.Enum;

namespace quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
public record CreateVideoInput(string Title, string Description, int YearLaunched, bool Opened, bool Published, int Duration, Rating Rating, IReadOnlyCollection<Guid>? CategoriesIds = null, IReadOnlyCollection<Guid>? GenresIds = null, IReadOnlyCollection<Guid>? CastMembersIds = null, FileInput? Thumb = null) : IRequest<CreateVideoOutput>;
