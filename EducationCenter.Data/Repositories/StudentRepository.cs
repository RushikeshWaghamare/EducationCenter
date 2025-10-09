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
    public class StudentRepository : IStudentRepository
    {
        private readonly EducationCenterContext _context;
        public StudentRepository(EducationCenterContext context) 
        {
            _context = context;
        }
        public async Task<Student> AddStudentAsync(Student student)
        {          
            if (string.IsNullOrEmpty(student.Status))
                student.Status = "Active";

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<Student>> GetAllStudentsAsync(int pageNumber, int pageSize, string search)
        {
            IQueryable<Student> query = _context.Students;

            if (!string.IsNullOrWhiteSpace(search) && search.Trim().Length >= 2)
            {
                string lowerSearch = search.Trim().ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(lowerSearch) ||
                    s.LastName.ToLower().Contains(lowerSearch) ||
                    s.Address.ToLower().Contains(lowerSearch));
            }

            int totalStudents = await query.CountAsync();

            var students = await query
                .OrderBy(s => s.StudentID)
                .GetPagedAsync(pageNumber, pageSize);

            return new PagedResult<Student>
            {
                Items = students,
                TotalCount = totalStudents
            };
        }


        public Task<Student?> GetStudentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentAsync(int id, Student student)
        {
            throw new NotImplementedException();
        }
    }
}
