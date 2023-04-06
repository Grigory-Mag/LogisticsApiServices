using GrpcGreeter.Services;
using ApiService;
//using CrudGrpcApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using LogisticsApiServices.DBPostModels;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);


/*builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.
    options.ListenLocalhost(6088);
});*/

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

// строка подключения


//string connStr = "Host=localhost;Port=5432;Database=logistics;Username=postgres;Password=12345;";


//optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=usersdb;Username=postgres;Password=здесь_указывается_пароль_от_postgres");
//"MySql.Data.MySqlClient" connectionString=
//"server=localhost;Port=3306;user id=develop;Password=develop;persistsecurityinfo=True;database=example;CharSet=utf8;SslMode=none"
// добавляем контекст ApplicationContext в качестве сервиса в приложение

//var connectionString = builder.Configuration.GetConnectionString(connStr);


//builder.Services.AddDbContext<DBContext>(options =>
//{
//	options.UseNpgsql(connStr);
//});


//builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connStr));


//var httpHandler = new HttpClientHandler();
//httpHandler.ServerCertificateCustomValidationCallback =
//HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;


//builder.Services.AddGrpcClient<UserApiService>(
//o =>
//{
//    o.Address = new Uri("http://localhost:5008"); // https://localhost:8005
//}).ConfigureChannel(
//o =>
//{
//    o.HttpHandler = httpHandler;
//});

builder.Services.AddDbContext<DBContext>();
builder.Services.AddGrpc();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);


app.Urls.Add("http://*:8008");



// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();

app.UseRouting();
app.MapGrpcService<UserApiService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
