using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Dto.DTOs
{
    public class StudentDTO
    {
        public int studentId { get; set; }
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string gender { get; set; }

        public DateOnly? dateOfBirth { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string address { get; set; }
        public DateOnly? enrollmentDate { get; set; }
        public string status { get; set; }
    }
}
