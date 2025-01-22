using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Create
{
    public class UserFileCreateDto
    {
        public string Name { get; set; }
        //public string Path { get; set; }
        //public string Extension { get; set; }
        //public long Size { get; set; }
        //public DateTime UploadedAt { get; set; }
        //public bool IsShared { get; set; }
        //public string ApplicationUserId { get; set; }
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
    }
}
