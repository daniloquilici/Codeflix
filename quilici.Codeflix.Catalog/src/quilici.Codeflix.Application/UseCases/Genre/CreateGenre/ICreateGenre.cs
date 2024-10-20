using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre
{
    public interface ICreateGenre : IRequestHandler<CreateGenreInput, GenreModelOutput>
    {
    }
}
