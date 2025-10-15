using EducationCenter.Dto.DTOs;
using EducationCenter.Service.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EducationCenter.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // GET: api/teachers?pageNumber=1&pageSize=5&search=abc
        [HttpGet]
        public async Task<IActionResult> GetAllTeachers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5, [FromQuery] string search = "")
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination parameters.");

            var pagedResult = await _teacherService.GetAllTeachersAsync(pageNumber, pageSize, search);

            if (pagedResult == null || pagedResult.Items.Count == 0)
                return NotFound("No teachers found.");

            var response = new
            {
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)pageSize),
                totalTeachers = pagedResult.TotalCount,
                teachers = pagedResult.Items
            };

            return Ok(response);
        }

        // GET: api/teachers/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(id);
            if (teacher == null)
                return NotFound($"Teacher with ID {id} not found.");

            return Ok(teacher);
        }

        // POST: api/teachers
        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] TeacherDTO teacherDto)
        {
            if (teacherDto == null)
                return BadRequest("Teacher data cannot be null.");

            var createdTeacher = await _teacherService.AddTeacherAsync(teacherDto);

            return CreatedAtAction(nameof(GetTeacher), new { id = createdTeacher.TeacherID }, createdTeacher);
        }

        // PUT: api/teachers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] TeacherDTO teacherDto)
        {
            if (teacherDto == null)
                return BadRequest("Invalid teacher data.");

            var updatedTeacher = await _teacherService.UpdateTeacherAsync(id, teacherDto);
            if (updatedTeacher == null)
                return NotFound($"Teacher with ID {id} not found.");

            return Ok(updatedTeacher);
        }

        // DELETE: api/teachers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var isDeleted = await _teacherService.DeleteTeacherAsync(id);
            if (!isDeleted)
                return NotFound($"Teacher with ID {id} not found.");

            return NoContent();
        }
    }
}
