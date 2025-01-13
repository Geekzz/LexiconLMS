using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Read
{
    public class CourseDto
    {
        // ToDo test with init on all reads
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<CourseDocumentDto> CourseDocuments { get; set; }
        public List<ModuleDto> Modules { get; set; }

        public List<UserDto> Users { get; set; }
    }
}
