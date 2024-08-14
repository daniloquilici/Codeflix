using quilici.Codeflix.Catalog.Domain.SeedWork;
using quilici.Codeflix.Catalog.Domain.Validation;

namespace quilici.Codeflix.Catalog.Domain.Entity
{
    public class Category : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        public Category(string name, string description, bool isActive = true)
            : base()
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.Now;
            IsActive = isActive;

            Validade();
        }

        public void Activate()
        {
            IsActive = true;
            Validade();
        }

        public void Deactivate()
        {
            IsActive = false;
            Validade();
        }

        public void Update(string name, string description = null!)
        {
            Name = name;
            Description = description ?? Description;
            Validade();
        }

        private void Validade()
        {
            DomainValidation.NotNullOrEmpty(Name, nameof(Name));
            DomainValidation.MinLength(Name, 3, nameof(Name));
            DomainValidation.MaxLength(Name, 255, nameof(Name));

            DomainValidation.NotNull(Description, nameof(Description));
            DomainValidation.MaxLength(Description, 10_000, nameof(Description));


            //if (string.IsNullOrWhiteSpace(Name))
            //    throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

            //if (Name.Length < 3)
            //    throw new EntityValidationException($"{nameof(Name)} shoud be at least 3 charactrs long");

            //if (Name.Length > 255)
            //    throw new EntityValidationException($"{nameof(Name)} shoud be less or equal 255 characters long");

            //if (Description == null)
            //    throw new EntityValidationException($"{nameof(Description)} should not be null");

            //if (Description.Length > 10_000)
            //    throw new EntityValidationException($"{nameof(Description)} shoud be less or equal 10000 charactrs long");
        }
    }
}
