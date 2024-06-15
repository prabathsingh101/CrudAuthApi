using Auth.API.Models.Domain;

namespace Auth.API.Repositories.Abstract
{
    public interface ITeacherRepository
    {

        Task<List<TeacherModel>> GetAllAsync(string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000
            );
        Task<TeacherModel> GetByIdAsync(int id);
        Task<TeacherModel>CreateAsync(TeacherModel model);
        Task<TeacherModel> UpdateAsync(int Id, TeacherModel model);
        Task<TeacherModel> DeleteAsync(int Id);
    }
}
