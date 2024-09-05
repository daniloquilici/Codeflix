using Bogus;

namespace quilici.Codeflix.Catalog.UnitTest.Common
{
    public abstract class BaseFixture
    {
        public Faker Faker { get; set; }

        public BaseFixture() => Faker = new Faker("pt_BR");

        public bool GetRandoBoolean() => new Random().NextDouble() < 0.5;
    }
}
