using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Create
{
    public class ActivityCreateDto
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        public ActivityTypeCreateDto ActivityType { get; init; }
    }
}
