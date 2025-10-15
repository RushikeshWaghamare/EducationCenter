using EducationCenter.Data.Models;
using EducationCenter.Data.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Data.Repositories.IRepository
{
    public interface IStudentRepository
    {
        Task<PagedResult<Student>> GetAllStudentsAsync(int pageNumber, int pageSize, string search);
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(int id, Student student);
        Task<bool> DeleteStudentAsync(int id);
    }
}
