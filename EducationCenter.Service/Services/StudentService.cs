using AutoMapper;
using EducationCenter.Data.Models;
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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper) 
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        public async Task<StudentDTO> AddStudentAsync(StudentDTO studentDto)
        {
            var studentEntity = _mapper.Map<Student>(studentDto);
            var addedStudent = await _studentRepository.AddStudentAsync(studentEntity);
            return _mapper.Map<StudentDTO>(addedStudent);
        }

        public Task<bool> DeleteStudentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<StudentDTO?> GetStudentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentAsync(int id, StudentDTO student)
        {
            throw new NotImplementedException();
        }
    }
}
