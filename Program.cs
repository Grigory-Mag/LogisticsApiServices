using GrpcGreeter.Services;
using ApiService;
//using CrudGrpcApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using LogisticsApiServices.DBPostModels;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Grpc.Core;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContext>();
builder.Services.AddGrpc();


// Adding an Authentication Service
// Getting setting for JwtToken from app settings
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var config = builder.Configuration.GetSection("JwtToken");
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = config["Issuer"],
        ValidateAudience = true,
        ValidAudience = config["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SigningKey"]
                                                                    ?? throw new ArgumentNullException("SigningKey")))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

app.Urls.Add("http://*:8008");

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<UserApiService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
