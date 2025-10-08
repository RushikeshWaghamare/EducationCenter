using EducationCenter.Data.Models;
using EducationCenter.Dto.DTOs;
using EducationCenter.Service.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EducationCenter.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly EducationCenterContext _context;
        private readonly IStudentService _studentService;

        public StudentsController(EducationCenterContext context, IStudentService studentService)
        {
            _context = context;
            _studentService = studentService;
        }

        // ✅ GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents(
            int pageNumber = 1,
            int pageSize = 5,
            string search = "")
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination parameters.");

            IQueryable<Student> query = _context.Students;

            if (!string.IsNullOrWhiteSpace(search) && search.Trim().Length >= 2)
            {
                string lowerSearch = search.Trim().ToLower();
                query = query.Where(s =>
                    s.FirstName.ToLower().Contains(lowerSearch) ||
                    s.LastName.ToLower().Contains(lowerSearch) ||
                    s.Address.ToLower().Contains(lowerSearch));
            }

            var totalStudents = await query.CountAsync();

            var students = await query
                .OrderBy(s => s.StudentID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize),
                totalStudents,
                students
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
                return BadRequest("Student data is null.");

            var student = await _studentService.AddStudentAsync(studentDTO);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentID) return BadRequest();

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
