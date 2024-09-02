using Newtonsoft.Json.Serialization;

namespace quilici.Codeflix.Catalog.EndToEndTests.Extensions
{
    public static class StringSnakeCaseExtension
    {
        public readonly static NamingStrategy _snakeCaseNamingStrategy = new SnakeCaseNamingStrategy();

        public static string ToSnakeCase(this string stringToConvert) 
        {
            ArgumentNullException.ThrowIfNull(stringToConvert, nameof(stringToConvert));

            return _snakeCaseNamingStrategy.GetPropertyName(stringToConvert, false);
        }
    }
}
