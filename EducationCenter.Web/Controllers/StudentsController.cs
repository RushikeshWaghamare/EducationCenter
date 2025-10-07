using EducationCenter.Data.Models;
using EducationCenter.Dto.DTOs;
using EducationCenter.Service.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDto)
        {
            //_context.Students.Add(student);
            //await _context.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetStudent), new { id = student.StudentID }, student);

            var student = await _studentService.AddStudentAsync(studentDto);
            return Ok(studentDto);
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
