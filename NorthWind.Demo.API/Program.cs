using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NorthWind.Demo.API.Authorization;
using NorthWind.Demo.API.POCOs;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettingsConfiguration = builder.Configuration.GetSection("AccessTokenSettings");
builder.Services.Configure<AccessTokenSettings>(jwtSettingsConfiguration);
var jwtSettings = jwtSettingsConfiguration.Get<AccessTokenSettings>();

RSA rsa = RSA.Create();

//url reference: https://vcsjones.dev/key-formats-dotnet-3/
//// Para BEGIN RSA PUBLIC KEY
//rsa.ImportRSAPublicKey(Convert.FromBase64String(jwtSettings.PublicKey.Replace("-----BEGIN RSA PUBLIC KEY-----", "-----END RSA PUBLIC KEY-----").Replace("", "")), out int _);
//// Para BEGIN PUBLIC KEY
//rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(jwtSettings.PublicKey.Replace("-----BEGIN PUBLIC KEY-----", "-----END PUBLIC KEY-----").Replace("", "")), out int _);

// Para las dos etiquetas PEM anteriores
rsa.ImportFromPem(jwtSettings.PublicKey);

var rsaKey = new RsaSecurityKey(rsa);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    if (builder.Environment.IsDevelopment()) options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        RequireSignedTokens = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = rsaKey,
        ClockSkew = TimeSpan.FromMinutes(0)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanReadWeather", policy =>
        policy.Requirements.Add(new GetWeatherRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, GetWeatherHandler>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
