using Bogus;

namespace quilici.Codeflix.UnitTest.Common
{
    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        public BaseFixture() => Faker = new Faker("pt_BR");
    }
}
