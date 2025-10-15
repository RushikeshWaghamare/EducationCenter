using EducationCenter.Data.Models;
using EducationCenter.Data.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Data.Repositories.IRepository
{
    public interface IDepartmentRepository
    {
        Task<PagedResult<Department>> GetAllDepartmentsAsync(int pageNumber, int pageSize, string search);
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<Department> AddDepartmentAsync(Department department);
        Task<Department> UpdateDepartmentAsync(int id, Department department);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
