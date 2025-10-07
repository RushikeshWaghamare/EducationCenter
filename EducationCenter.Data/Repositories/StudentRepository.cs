using EducationCenter.Data.Models;
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

        public Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            throw new NotImplementedException();
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
