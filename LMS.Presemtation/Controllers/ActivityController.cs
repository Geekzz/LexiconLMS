﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.DTOs.Read;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Domain.Models.Entities;


namespace LMS.Presentation.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ActivityController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetOneActivity(int id)
        {
            var activityDto = await _serviceManager.ActivityService.GetActivityAsync(id);
            return Ok(activityDto);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<ActivityTypeDto>>> GetAllActivityTypes()
        {
            var activityTypeDtos = await _serviceManager.ActivityService.GetAllActivityTypes();
            return Ok(activityTypeDtos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(int id)
        {
            await _serviceManager.ActivityService.DeleteActivityAsync(id);
            return NoContent();
        }
    }
}
