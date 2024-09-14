using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public interface IDeleteGenre : IRequestHandler<DeleteGenreInput>
    {
    }
}
