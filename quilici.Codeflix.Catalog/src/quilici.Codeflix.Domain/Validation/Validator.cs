namespace quilici.Codeflix.Catalog.Domain.Validation;
public abstract class Validator
{
    private readonly ValidationHandler _handler;

    protected Validator(ValidationHandler handler)
    {
        _handler = handler;
    }

    public abstract void Validate();
}
