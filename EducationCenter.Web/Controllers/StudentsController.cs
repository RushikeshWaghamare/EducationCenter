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
        
        [HttpGet]
        public async Task<ActionResult> GetAllStudents(int pageNumber = 1, int pageSize = 5, string search = "")
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination parameters.");

            var pagedResult = await _studentService.GetAllStudentsAsync(pageNumber, pageSize, search);

            var response = new
            {
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)pageSize),
                totalStudents = pagedResult.TotalCount,
                students = pagedResult.Items
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
