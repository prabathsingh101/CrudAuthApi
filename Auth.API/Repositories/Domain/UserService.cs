using Auth.API.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Auth.API.Repositories.Domain
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public string GetMyName()
        {
            var result = string.Empty;
            if (httpContextAccessor.HttpContext != null)
                result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            return result;
        }
    }
}
