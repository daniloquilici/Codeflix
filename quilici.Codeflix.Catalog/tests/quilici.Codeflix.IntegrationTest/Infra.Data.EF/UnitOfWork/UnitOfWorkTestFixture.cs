using quilici.Codeflix.IntegrationTest.Base;
using Xunit;

namespace quilici.Codeflix.IntegrationTest.Infra.Data.EF.UnitOfWork
{
    [CollectionDefinition(nameof(UnitOfWorkTestFixture))]
    public class UnitOfWorkTestFixtureCollection : ICollectionFixture<UnitOfWorkTestFixture>
    {
    }

    public class UnitOfWorkTestFixture : BaseFixture
    {
    }
}
