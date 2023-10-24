using Bogus;

namespace quilici.Codeflix.IntegrationTest.Base
{
    public class BaseFixture
    {
        protected Faker Faker { get; set; }

        public BaseFixture()
        {
            Faker = new Faker("pt_BR");
        }
    }
}
