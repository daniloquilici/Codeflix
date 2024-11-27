using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;
public interface ICreateVideo : IRequestHandler<CreateVideoInput, CreateVideoOutput>
{
}
