using AutoMapper;
using EducationCenter.Data.Models;
using EducationCenter.Data.Pagination;
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
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        // ✅ Get all teachers with pagination & mapping to DTO
        public async Task<PagedResult<TeacherDTO>> GetAllTeachersAsync(int pageNumber, int pageSize, string search)
        {
            var pagedTeachers = await _teacherRepository.GetAllTeachersAsync(pageNumber, pageSize, search);

            return new PagedResult<TeacherDTO>
            {
                Items = _mapper.Map<List<TeacherDTO>>(pagedTeachers.Items),
                TotalCount = pagedTeachers.TotalCount
            };           
        }

        // ✅ Get teacher by ID
        public async Task<TeacherDTO?> GetTeacherByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(id);
            if (teacher == null) return null;

            return _mapper.Map<TeacherDTO>(teacher);            
        }

        // ✅ Add new teacher
        public async Task<TeacherDTO> AddTeacherAsync(TeacherDTO teacherDto)
        {
            var teacherEntity = _mapper.Map<Teacher>(teacherDto);
            var createdTeacher = await _teacherRepository.AddTeacherAsync(teacherEntity);
            return _mapper.Map<TeacherDTO>(createdTeacher);           
        }

        // ✅ Update teacher
        public async Task<TeacherDTO> UpdateTeacherAsync(int id, TeacherDTO teacherDto)
        {
            var teacherEntity = _mapper.Map<Teacher>(teacherDto);
           
            var updatedTeacher = await _teacherRepository.UpdateTeacherAsync(id, teacherEntity);
            if (updatedTeacher == null) return null;

            return _mapper.Map<TeacherDTO>(updatedTeacher);
        }

        // ✅ Delete teacher
        public async Task<bool> DeleteTeacherAsync(int id)
        {
            return await _teacherRepository.DeleteTeacherAsync(id);
        }
    }
}