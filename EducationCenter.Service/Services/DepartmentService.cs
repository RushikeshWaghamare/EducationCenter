using AutoMapper;
using EducationCenter.Data.Models;
using EducationCenter.Data.Pagination;
using EducationCenter.Data.Repositories;
using EducationCenter.Data.Repositories.IRepository;
using EducationCenter.Dto.DTOs;
using EducationCenter.Service.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<DepartmentDTO>> GetAllSIDepartmentsAsync(int pageNumber, int pageSize, string search)
        {
            var result = await _departmentRepository.GetAllDepartmentsAsync(pageNumber, pageSize, search);

            return new PagedResult<DepartmentDTO>
            {
                Items = _mapper.Map<List<DepartmentDTO>>(result.Items),
                TotalCount = result.TotalCount
            };
        }
        public async Task<DepartmentDTO?> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (department == null) return null;

            return _mapper.Map<DepartmentDTO>(department);
        }

        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO dept)
        {
            var department = _mapper.Map<Department>(dept);           

            var created = await _departmentRepository.AddDepartmentAsync(department);

            return _mapper.Map<DepartmentDTO>(department);
        }

        public async Task<DepartmentDTO?> UpdateDepartmentAsync(int id, DepartmentDTO dept)
        {
            var departmentEntity = _mapper.Map<Department>(dept);
            var updated = await _departmentRepository.UpdateDepartmentAsync(id, departmentEntity);
            return _mapper.Map<DepartmentDTO>(updated);
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            return await _departmentRepository.DeleteDepartmentAsync(id);
        }
    }
}
