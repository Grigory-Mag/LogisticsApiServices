using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogisticsApiServices.Security
{
    internal class JwtValidator : ISecurityTokenValidator
    {
        private readonly string Issuer;
        private readonly string Audience;
        private readonly SecurityKey SigningKey;

        public JwtValidator(string? issuer, string? audience, string? signingKey)
        {
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            Audience = audience ?? throw new ArgumentNullException(nameof(audience));
            var signingKeyString = signingKey ?? throw new ArgumentNullException(nameof(signingKey));

            SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKeyString));
        }

        public bool CanReadToken(string securityToken) =>
            (securityToken is not null) && (securityToken.Length > 0);

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                IssuerSigningKey = SigningKey
            };

            var claimsPrincipal = handler.ValidateToken(securityToken, tokenValidationParameters, out validatedToken);
            return claimsPrincipal;
        }

        public bool CanValidateToken { get; } = true;
        public int MaximumTokenSizeInBytes { get; set; } = int.MaxValue;
    }
}
