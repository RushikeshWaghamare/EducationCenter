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
            CreateMap<StudentDTO, Student>()
                .ForMember(dest => dest.StudentID, opt => opt.MapFrom(src => src.studentId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.lastName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.gender))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.dateOfBirth))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.phone))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.address))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.enrollmentDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ReverseMap();
        }
    }
}
