using System.Text.Json;

namespace GetzTest.Application.Jwts;

public static class JwkSerializerOptions
{
    public static JsonSerializerOptions DefaultSerializerOptions = new()
    {
        WriteIndented = true
    };
}
