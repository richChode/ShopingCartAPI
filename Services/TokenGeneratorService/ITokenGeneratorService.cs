using shoppingcart.Models;

namespace shoppingcart.Services.TokenGeneratorService;

public interface ITokenGeneratorService
{
    string GenerateToken(User user);
}