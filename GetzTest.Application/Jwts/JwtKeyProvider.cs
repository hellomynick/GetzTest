using GetzTest.Application.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace GetzTest.Application.Jwts;

public class JwtKeyProvider
{
    public RsaSecurityKey PrivateKey { get; private set; }
    public RsaSecurityKey PublicKey { get; private set; }

    public JwtKeyProvider(string privateKeyPath, string publicKeyPath)
    {
        PrivateKey = KeyExtension.ToRsaSecurityKey(privateKeyPath, ImportType.Path);
        PublicKey = KeyExtension.ToRsaSecurityKey(publicKeyPath, ImportType.Path);
    }
}
