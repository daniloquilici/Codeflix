using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre
{
    public interface IUpdateGenre : IRequestHandler<UpdateGenreInput, GenreModelOutput>
    {
    }
}
