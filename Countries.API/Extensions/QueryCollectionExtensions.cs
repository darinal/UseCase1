namespace Countries.API.Extensions;

public static class QueryCollectionExtensions
{
    public static int? GetInt(this IQueryCollection queryCollection, string key)
    {
        if (int.TryParse(queryCollection[key], out int parsedValue))
        {
            return parsedValue;
        }

        return null;
    }

    public static string? GetString(this IQueryCollection queryCollection, string key)
    {
        return queryCollection[key].ToString();
    }
}