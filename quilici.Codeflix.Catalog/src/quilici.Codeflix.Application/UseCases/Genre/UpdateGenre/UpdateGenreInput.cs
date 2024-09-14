using MediatR;
using quilici.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre
{
    public class UpdateGenreInput : IRequest<GenreModelOuput>
    {
        public UpdateGenreInput(Guid id, string name, bool? isActive)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool? IsActive { get; set; }
    }
}
