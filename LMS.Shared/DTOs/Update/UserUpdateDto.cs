using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.Update
{
    public class UserUpdateDto
    {
        public string Email { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Role { get; set; }
    }
}
