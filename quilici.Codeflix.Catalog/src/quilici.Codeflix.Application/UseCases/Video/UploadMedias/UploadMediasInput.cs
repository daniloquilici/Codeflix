using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Video.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Video.UploadMedias;
public record UploadMediasInput(Guid VideoId, FileInput? VideoFile, FileInput? TrailerFile) : IRequest;
