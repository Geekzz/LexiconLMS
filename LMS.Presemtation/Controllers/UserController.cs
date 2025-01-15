using Domain.Models.Entities;
using LMS.Shared.DTOs.Read;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllUsers([FromQuery] string targetId)
        {
            // Validate that logged in user is teacher to get all.
            if (targetId == null)
            {
                var users = await _userManager.Users
                    .Select(user => new
                    {
                        user.Email,
                        user.FirstName,
                        user.LastName
                    })
                    .ToListAsync();
                return Ok(users);
            }
            else
            {
                var user = await _userManager.Users.Where(u => u.Id == targetId).FirstOrDefaultAsync();
                return Ok(user);
            }


        }

        //[HttpGet(]
        //[Authorize]
        //public async Task<IActionResult> GetOneUser([FromQuery] string targetId)
        //{
        //    var user = await _userManager.Users.Where(u => u.UserName == userName).FirstOrDefaultAsync();
        //    return Ok(user);
        //}

    }
}
