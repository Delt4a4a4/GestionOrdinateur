using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySecureApi;

var builder = WebApplication.CreateBuilder(args);

var baseUrl = builder.Configuration["Keycloak:BaseUrl"]?.TrimEnd('/')
    ?? throw new InvalidOperationException("Keycloak:BaseUrl est manquante.");
var realm = builder.Configuration["Keycloak:Realm"]
    ?? throw new InvalidOperationException("Keycloak:Realm est manquante.");
var issuer = $"{baseUrl}/realms/{realm}";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<KeycloakAuthService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            RoleClaimType = "roles"
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
        policy.WithOrigins("http://localhost:5289", "http://localhost:5000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BlazorClient");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();