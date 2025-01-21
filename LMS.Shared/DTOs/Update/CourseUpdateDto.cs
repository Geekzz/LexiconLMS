using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Update
{
    public class CourseUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The course name must be between 2 and 100 characters.")]
        public string Name { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public DateTime StartDate { get; init; }

        [Required]
        public DateTime EndDate { get; init; }
    }
}
