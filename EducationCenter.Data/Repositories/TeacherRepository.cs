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
    public class TeacherRepository : ITeacherRepository
    {
        private readonly EducationCenterContext _context;

        public TeacherRepository(EducationCenterContext context)
        {
            _context = context;
        }

        // ✅ Get all teachers with pagination and search
        public async Task<PagedResult<Teacher>> GetAllTeachersAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Teacher> query = _context.Teachers;

            if (!string.IsNullOrWhiteSpace(search) && search.Trim().Length >= 2)
            {
                string lowerSearch = search.Trim().ToLower();
                query = query.Where(t =>
                    t.FirstName.ToLower().Contains(lowerSearch) ||
                    t.LastName.ToLower().Contains(lowerSearch) ||
                    t.Email.ToLower().Contains(lowerSearch));
            }

            int totalCount = await query.CountAsync();

            var teachers = await query
                .OrderBy(t => t.TeacherID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Teacher>
            {
                Items = teachers,
                TotalCount = totalCount
            };
        }

        // ✅ Get a teacher by ID
        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            return await _context.Teachers.FindAsync(id);
        }

        // ✅ Add a new teacher
        public async Task<Teacher> AddTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        // ✅ Update an existing teacher
        public async Task<Teacher> UpdateTeacherAsync(int id, Teacher teacher)
        {
            var existingTeacher = await _context.Teachers.FindAsync(id);
            if (existingTeacher == null)
                return null;

            // Update allowed properties
            existingTeacher.FirstName = teacher.FirstName;
            existingTeacher.LastName = teacher.LastName;
            existingTeacher.Email = teacher.Email;
            existingTeacher.Phone = teacher.Phone;
            existingTeacher.DepartmentID = teacher.DepartmentID;

            _context.Teachers.Update(existingTeacher);
            await _context.SaveChangesAsync();

            return existingTeacher;
        }

        // ✅ Delete teacher by ID
        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
                return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}