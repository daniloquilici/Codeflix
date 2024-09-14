using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.GetGenre
{
    public interface IGetGenre : IRequestHandler<GetGenreInput, GenreModelOuput>
    {
    }
}
