using shoppingcart.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

namespace shoppingcart.Services.TokenGeneratorService;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly IConfiguration _config;

    public TokenGeneratorService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim("userId", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
          _config["Jwt:Audience"],
          claims,
          expires: DateTime.Now.AddMinutes(15),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}