using AutoMapper;
using EducationCenter.Data.Models;
using EducationCenter.Dto.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Dto.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Student, StudentDTO>()
                .ReverseMap();

            CreateMap<Department, DepartmentDTO>()
                .ReverseMap();

            CreateMap<Teacher, TeacherDTO>()
                .ReverseMap();
        }
    }
}
