using EducationCenter.Data.Pagination;
using EducationCenter.Dto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Service.Services.IService
{
    public interface IStudentService
    {
        Task<PagedResult<StudentDTO>> GetAllStudentsAsync(int pageNumber, int pageSize, string search);
        Task<StudentDTO?> GetStudentByIdAsync(int id);
        Task<StudentDTO> AddStudentAsync(StudentDTO student);
        Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO student);
        Task<bool> DeleteStudentAsync(int id);
    }
}
