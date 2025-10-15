using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationCenter.Dto.DTOs
{
    public class TeacherDTO
    {
        public int TeacherID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateOnly? HireDate { get; set; }

        public int? DepartmentID { get; set; }
    }
}
