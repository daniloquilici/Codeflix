using quilici.Codeflix.Catalog.EndToEndTests.Base;

namespace quilici.Codeflix.Catalog.EndToEndTests.Api.Genre.Common;
public class GenreBaseFixture : BaseFixture
{
    public GenrePersistence Persistence { get; set; }

    public GenreBaseFixture() 
        : base()
    {
        Persistence = new GenrePersistence(CreateDbContext());
    }    
}
