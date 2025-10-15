using EducationCenter.Data.Models;
using EducationCenter.Data.Pagination;
using EducationCenter.Data.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly EducationCenterContext _context;

        public DepartmentRepository(EducationCenterContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Department>> GetAllDepartmentsAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Department> query = _context.Departments;

            if (!string.IsNullOrWhiteSpace(search) && search.Trim().Length >= 2)
            {
                string lowerSearch = search.Trim().ToLower();
                query = query.Where(d =>
                    d.DepartmentName.ToLower().Contains(lowerSearch) ||
                    (d.Description != null && d.Description.ToLower().Contains(lowerSearch))
                );
            }

            int totalDepartments = await query.CountAsync();

            var departments = await query
                .OrderBy(d => d.DepartmentID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Department>
            {
                Items = departments,
                TotalCount = totalDepartments
            };
        }


        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<Department> AddDepartmentAsync(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> UpdateDepartmentAsync(int id, Department department)
        {
            var existingDepartment = await _context.Departments.FindAsync(id);
            if (existingDepartment == null)
                return null;

            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.Description = department.Description;

            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
