namespace quilici.Codeflix.Catalog.EndToEndTests.Extensions
{
    internal static class DateTimeExtensions
    {
        public static System.DateTime TrimMillisseconds(this System.DateTime dateTime) =>
            new System.DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0, dateTime.Kind);
    }
}
