using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using GestionOrdinateurs.Shared;
using System.Net;
using System.Net.Http.Json;


namespace AuthentificationApi;

[ApiController]
[Route("auth")]
public class AuthController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;


    [HttpPost("login")]
    public async Task<IActionResult> Login(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return BadRequest("Le login est obligatoire.");
        }

        var client = httpClientFactory.CreateClient("GestionApi");

        // Recherche l'utilisateur pour récupérer son rôle enregistré en base.
        using var response = await client.GetAsync(
            $"api/users/{Uri.EscapeDataString(userId.Trim())}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound("Cet utilisateur n'existe pas.");
        }

        response.EnsureSuccessStatusCode();

        var user = await response.Content.ReadFromJsonAsync<User>()
            ?? throw new InvalidOperationException(
                "L'API utilisateurs a renvoyé une réponse vide.");

        var jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException("Jwt:Key est manquante.");
        }

        var key = Encoding.ASCII.GetBytes(jwtKey);

        return Ok(GenTokenJWT(key, user.Login, user.Role));
    }

    private string GenTokenJWT(byte[] keyP, String userId, RoleEnum role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userId),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyP), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);
        return jwt;
    }
}