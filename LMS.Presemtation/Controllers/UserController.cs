﻿using Domain.Models.Entities;
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
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            // tillfällig lösning med, API hämtar alla users men osäker om detta är bra
            // authorize stoppar API att hämta users, måste vara authorized... men annars går det hämta på client iaf
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

    }
}