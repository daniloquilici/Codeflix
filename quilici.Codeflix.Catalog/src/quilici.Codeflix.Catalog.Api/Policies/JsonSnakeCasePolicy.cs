using quilici.Codeflix.Catalog.Api.Extension;
using System.Text.Json;

namespace quilici.Codeflix.Catalog.Api.Policies
{
    public class JsonSnakeCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}
