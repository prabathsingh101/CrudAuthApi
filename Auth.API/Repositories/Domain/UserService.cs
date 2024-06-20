using Auth.API.Data;
using Auth.API.Models.Domain;
using Auth.API.Repositories.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Auth.API.Repositories.Domain
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly DatabaseContext context;

        public UserService(IHttpContextAccessor httpContextAccessor, DatabaseContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
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
