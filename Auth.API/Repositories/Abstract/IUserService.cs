using Auth.API.Models.Domain;

namespace Auth.API.Repositories.Abstract
{
    public interface IUserService
    {

        public string GetMyName();
        public string GetRoleName();


        Task<IEnumerable<UserModel>> GetAllUsersAsync();  

    }
}
