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
        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }
        public async Task<Student> AddStudentAsync(Student student)
        {
            if (string.IsNullOrEmpty(student.Status))
                student.Status = "Active";

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }
        public async Task<Student> UpdateStudentAsync(int id, Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student), "Invalid student data.");

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
                return null;

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.Phone = student.Phone;
            existingStudent.Address = student.Address;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Gender = student.Gender;

            await _context.SaveChangesAsync();
            return existingStudent;
        }
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
