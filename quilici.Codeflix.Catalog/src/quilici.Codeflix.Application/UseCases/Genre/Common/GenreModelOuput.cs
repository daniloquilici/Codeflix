using DomainEntity = quilici.Codeflix.Catalog.Domain.Entity;

namespace quilici.Codeflix.Catalog.Application.UseCases.Genre.Common
{
    public class GenreModelOuput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public IReadOnlyList<Guid> Categories { get; set; }

        public GenreModelOuput(Guid id, string name, bool isActive, DateTime createdAt, IReadOnlyList<Guid> categories)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CreatedAt = createdAt;
            Categories = categories;
        }

        public static GenreModelOuput FromGenre(DomainEntity.Genre genre)
            => new GenreModelOuput(genre.Id, genre.Name, genre.IsActive, genre.CreatedAt, genre.Categories);
    }
}
