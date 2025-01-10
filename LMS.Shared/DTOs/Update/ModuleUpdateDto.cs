using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Update
{
    public class ModuleUpdateDto
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public DateTime StartDate { get; init; }

        [Required]
        public DateTime EndDate { get; init; }
    }
}
