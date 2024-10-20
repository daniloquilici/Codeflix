using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.GetGenre
{
    public class GetGenreInput : IRequest<GenreModelOutput>
    {
        public Guid Id { get; set; }

        public GetGenreInput(Guid id)
        {
            Id = id;
        }
    }
}
