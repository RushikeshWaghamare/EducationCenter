using EducationCenter.Data.Models;
using EducationCenter.Dto.DTOs;
using EducationCenter.Service.Services.IService;
using EducationCenter.Web.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/students?pageNumber=1&pageSize=5&search=abc
        [HttpGet]
        public async Task<IActionResult> GetAllStudents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string search = "")
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new BadRequestException("Invalid pagination parameters.");

            var pagedResult = await _studentService.GetAllStudentsAsync(pageNumber, pageSize, search);

            if (pagedResult == null || !pagedResult.Items.Any())
                throw new NotFoundException("No students found.");

            var response = new
            {
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)pageSize),
                totalStudents = pagedResult.TotalCount,
                students = pagedResult.Items
            };

            return Ok(response);
        }

        // GET: api/students/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            return Ok(student);
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
                return BadRequest("Student data cannot be null.");

            var createdStudent = await _studentService.AddStudentAsync(studentDTO);

            return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.StudentID }, createdStudent);
        }

        // PUT: api/students/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null)
                return BadRequest("Invalid student data.");

            var updatedStudent = await _studentService.UpdateStudentAsync(id, studentDTO);
            if (updatedStudent == null)
                return NotFound($"Student with ID {id} not found.");

            return Ok(updatedStudent);
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var isDeleted = await _studentService.DeleteStudentAsync(id);
            if (!isDeleted)
                return NotFound($"Student with ID {id} not found.");

            return NoContent();
        }
    }
}