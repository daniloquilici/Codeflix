using MediatR;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre
{
    public class DeleteGenreInput : IRequest
    {
        public Guid Id { get; set; }

        public DeleteGenreInput(Guid id)
        {
            Id = id;
        }
    }
}
