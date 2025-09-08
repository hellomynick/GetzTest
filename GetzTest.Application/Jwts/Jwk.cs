using System.Text.Json.Serialization;

namespace GetzTest.Application.Jwts;

public class Jwk
{
    [JsonPropertyName("kty")] public string Kty { get; set; } = string.Empty;
    [JsonPropertyName("alg")] public string Alg { get; set; } = string.Empty;
    [JsonPropertyName("kid")] public string Kid { get; set; } = string.Empty;
    [JsonPropertyName("use")] public string Use { get; set; } = string.Empty;
    [JsonPropertyName("key_ops")] public string[] KeyOps { get; set; } = [];
    [JsonPropertyName("n")] public string Modulus { get; set; } = string.Empty;
    [JsonPropertyName("e")] public string PublicExponent { get; set; } = string.Empty;
}