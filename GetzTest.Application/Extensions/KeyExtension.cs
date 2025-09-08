using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace GetzTest.Application.Extensions;

public static class KeyExtension
{
    public static RsaSecurityKey ToRsaSecurityKey(string input, ImportType importType)
    {
        var pem = importType switch
        {
            ImportType.Path => File.ReadAllText(input),
            ImportType.Pem => input,
            _ => throw new ArgumentOutOfRangeException(nameof(importType), importType, null)
        };

        var rsa = RSA.Create();
        rsa.ImportFromPem(pem);

        return new RsaSecurityKey(rsa);
    }
}

public enum ImportType
{
    Path,
    Pem
}
