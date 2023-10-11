using quilici.Codeflix.Domain.Exceptions;
using quilici.Codeflix.Domain.SeedWork;

namespace quilici.Codeflix.Domain.Entity
{
    public class Category : AggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreateAt { get; private set; }
        public bool IsActive { get; private set; }

        public Category(string name, string description, bool isActive = true)
            : base()
        {            
            Name = name;
            Description = description;
            CreateAt = DateTime.Now;
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
            if (string.IsNullOrWhiteSpace(Name))
                throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

            if (Name.Length < 3)
                throw new EntityValidationException($"{nameof(Name)} shoud be at leats 3 charactrs long");

            if (Name.Length > 255)
                throw new EntityValidationException($"{nameof(Name)} shoud be less or equal 255 charactrs long");

            if (Description == null)
                throw new EntityValidationException($"{nameof(Description)} should not be empty or null");

            if (Description.Length > 10_000)
                throw new EntityValidationException($"{nameof(Description)} shoud be less or equal 10.000 charactrs long");
        }
    }
}
