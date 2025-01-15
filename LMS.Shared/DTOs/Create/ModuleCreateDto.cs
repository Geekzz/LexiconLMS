﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Create
{
    public class ModuleCreateDto
    {   
        [Required]
        public string Name { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public DateTime StartDate { get; init; }

        [Required]
        public DateTime EndDate { get; init; }

        public List<ActivityCreateDto> Activities { get; init; }
    }
}
