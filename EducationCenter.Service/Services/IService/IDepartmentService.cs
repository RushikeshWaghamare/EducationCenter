using EducationCenter.Data.Pagination;
using EducationCenter.Dto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Service.Services.IService
{
    public interface IDepartmentService
    {
        Task<PagedResult<DepartmentDTO>> GetAllSIDepartmentsAsync(int pageNumber, int pageSize, string search);
        Task<DepartmentDTO?> GetDepartmentByIdAsync(int id);
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO dept);
        Task<DepartmentDTO> UpdateDepartmentAsync(int id, DepartmentDTO dept);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
