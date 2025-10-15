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
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/departments?pageNumber=1&pageSize=5&search=abc
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "")
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination parameters.");

            var pagedResult = await _departmentService.GetAllSIDepartmentsAsync(pageNumber, pageSize, search);

            if (pagedResult == null || !pagedResult.Items.Any())
                return NotFound("No departments found.");

            var response = new
            {
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)pageSize),
                totalDepartments = pagedResult.TotalCount,
                departments = pagedResult.Items
            };

            return Ok(response);
        }

        // GET: api/departments/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);

            if (department == null)
                return NotFound($"Department with ID {id} not found.");

            return Ok(department);
        }

        // POST: api/departments
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO departmentDTO)
        {
            if (departmentDTO == null)
                return BadRequest("Department data cannot be null.");

            var createdDepartment = await _departmentService.AddDepartmentAsync(departmentDTO);

            return CreatedAtAction(nameof(GetDepartment), new { id = createdDepartment.DepartmentID }, createdDepartment);
        }

        // PUT: api/departments/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDTO departmentDTO)
        {
            if (departmentDTO == null)
                return BadRequest("Invalid department data.");

            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(id, departmentDTO);

            if (updatedDepartment == null)
                return NotFound($"Department with ID {id} not found.");

            return Ok(updatedDepartment);
        }

        // DELETE: api/departments/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var isDeleted = await _departmentService.DeleteDepartmentAsync(id);

            if (!isDeleted)
                return NotFound($"Department with ID {id} not found.");

            return NoContent();
        }
    }
}