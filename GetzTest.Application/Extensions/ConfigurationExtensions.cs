namespace GetzTest.Application.Extensions;

public static class ConfigurationExtensions
{
    public static string GetRequireValue(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Value not found for key: {key} in configuration.");
        }

        return value;
    }
}
