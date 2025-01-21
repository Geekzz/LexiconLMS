using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    public class UserFile
    {
        public Guid UserFileId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsShared { get; set; }

        //Uploader
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //FK
        public int? CourseId { get; set; }
        //NP
        public Course? Course { get; set; }
        //FK
        public int? ModuleId { get; set; }
        //NP
        public Module? Module { get; set; }
        //FK
        public int? ActivityId { get; set; }
        //NP
        public Activity? Activity { get; set; }
        
    }
}
