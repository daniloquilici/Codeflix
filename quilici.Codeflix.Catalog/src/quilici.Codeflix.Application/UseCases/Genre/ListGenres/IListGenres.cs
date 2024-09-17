using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.ListGenres
{
    internal interface IListGenres : IRequestHandler<ListGenresInput, ListGenresOutput>
    {
    }
}
