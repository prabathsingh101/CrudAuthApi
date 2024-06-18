
using Auth.API.Models.DTO;
using System.Security.Claims;

namespace Auth.API.Repositories.Abstract
{
    public interface ITokenService
    {
        TokenResponseDto GetToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

       
    }
}
