using ApiService;
using Grpc.Core;
using LogisticsApiServices.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiService
{
    public partial class UserApiService
    {
        [AllowAnonymous]
        public override async Task<LoginReply> LoginUser(LoginRequest request, ServerCallContext context)
        {
            var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
            IConfiguration _configuration = configurationBuilder.Build();

            var jwt_config = _configuration.GetSection("JwtToken");

            string uniqueKey = jwt_config["SigningKey"];
            string issuer = jwt_config["Issuer"];
            string audience = jwt_config["Audience"];
            string config_val = jwt_config["Expiration"];
            var expiration = TimeSpan.FromSeconds(int.Parse(config_val));

            var token = JwtHelper.GetJwtTokenString(
                    request.Data,
                    uniqueKey,
                    issuer,
                    audience,
                    expiration);
            //claims)

            var data = dbContext.Users
                .Include(rn => rn.RoleNavigation)
                .First(item => item.Login == request.Data.Login);
            data.Password = "";

            //Claim[] claims = { new Claim(ClaimTypes.Role, "Student") };
            return await Task.FromResult(new LoginReply
            {
                Token = token,
                User = (LoginObject) data
                
            });

        }
    }
}
