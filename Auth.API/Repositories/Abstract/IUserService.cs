using Auth.API.Models.Domain;

namespace Auth.API.Repositories.Abstract
{
    public interface IUserService
    {

        public string GetMyName();
        
        Task<IEnumerable<UserModel>> GetAllUsersAsync();  

    }
}
