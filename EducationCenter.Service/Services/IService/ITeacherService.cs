using EducationCenter.Data.Pagination;
using EducationCenter.Dto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Service.Services.IService
{
    public interface ITeacherService
    {
        Task<PagedResult<TeacherDTO>> GetAllTeachersAsync(int pageNumber, int pageSize, string search);
        Task<TeacherDTO?> GetTeacherByIdAsync(int id);
        Task<TeacherDTO> AddTeacherAsync(TeacherDTO teacher);
        Task<TeacherDTO> UpdateTeacherAsync(int id, TeacherDTO teacher);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
