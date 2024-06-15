using Auth.API.Data;
using Auth.API.Models.Domain;
using Auth.API.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repositories.Domain
{
    public class SQLTeacherRepository : ITeacherRepository
    {
        private readonly DatabaseContext context;

        public SQLTeacherRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public async Task<TeacherModel> CreateAsync(TeacherModel model)
        {
            await context.Teachers.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<TeacherModel> DeleteAsync(int Id)
        {
            var existTeacher = await context.Teachers.FirstOrDefaultAsync(x => x.Id == Id);

            if (existTeacher is null)            
                return null;
            context.Teachers.Remove(existTeacher);
            await context.SaveChangesAsync();
            return existTeacher;
        }

        public async Task<List<TeacherModel>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var teacher = context.Teachers.AsQueryable();

            //filtering

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    teacher = teacher.Where(x => x.Name.Contains(filterQuery));
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    teacher = isAscending ? teacher.OrderBy(x => x.Name) : teacher.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    teacher = isAscending ? teacher.OrderBy(x => x.ContactNo) : teacher.OrderByDescending(x => x.ContactNo);
                }
            }

            //pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await teacher.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<TeacherModel> GetByIdAsync(int id)
        {
            return context.Teachers.FirstOrDefault(x => x.Id == id);
        }

        public async Task<TeacherModel> UpdateAsync(int Id, TeacherModel model)
        {
            var existsTeacher = await context.Teachers.FirstOrDefaultAsync(y => y.Id == Id);

            if (existsTeacher is null)
                return null;
            existsTeacher.Name = model.Name;
            existsTeacher.Degree = model.Degree;
            existsTeacher.Proficiency = model.Proficiency;

            existsTeacher.Proficiency = model.Proficiency;
            existsTeacher.Address = model.Address;
            existsTeacher.ContactNo = model.ContactNo;
            await context.SaveChangesAsync();
            return existsTeacher;
        }
    }
}
