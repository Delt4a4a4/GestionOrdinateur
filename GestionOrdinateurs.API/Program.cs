using GestionOrdinateurs.API;  
using GestionOrdinateurs.API.Service;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestionOrdinateurs",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez : Bearer {token}"
    });
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
}); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
// Scoped car le service dépend du DbContext
builder.Services.AddScoped<IOrdinateurService, OrdinateurService>();
builder.Services.AddDbContext<ParcInformatiqueContext>();
builder.Services.AddScoped<IReparationService, ReparationService>();
builder.Services.AddSingleton<StockService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(options =>
{  
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("http://localhost:5289", "http://localhost:5000") // URL de ton client Blazor
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
var key = Encoding.ASCII.GetBytes(jwtKey);

var issuer =
    $"{builder.Configuration["Keycloak:BaseUrl"]}/realms/{builder.Configuration["Keycloak:Realm"]}";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = issuer;
        options.RequireHttpsMetadata = false; // Développement local uniquement
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateLifetime = true,
            RoleClaimType = "roles"
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

//Activer CORS
app.UseCors("AllowBlazorClient");

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ParcInformatiqueContext>();
    
    db.Database.Migrate();

    if(!db.Users.Any())
    {
        db.Users.AddRange(
            new User { Login="user1", Role=RoleEnum.Admin },
            new User { Login="user2", Role=RoleEnum.Gestionnaire },
            new User { Login="user3", Role=RoleEnum.Gestionnaire }
        );

        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();


