using EducationCenter.Data.Models;
using EducationCenter.Data.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Data.Repositories.IRepository
{
    public interface ITeacherRepository
    {
        Task<PagedResult<Teacher>> GetAllTeachersAsync(int pageNumber, int pageSize, string search);
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task<Teacher> AddTeacherAsync(Teacher teacher);
        Task<Teacher> UpdateTeacherAsync(int id, Teacher teacher);
        Task<bool> DeleteTeacherAsync(int id);
    }
}