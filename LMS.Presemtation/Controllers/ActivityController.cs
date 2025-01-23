using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.DTOs.Read;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using LMS.Shared.DTOs.Update;
using LMS.Shared.DTOs.Create;


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

        [HttpPut("{id}")]
        public async Task<ActionResult> PutActivity(int id, ActivityUpdateDto activityUpdateDto)
        {
            if (activityUpdateDto is null) return BadRequest();

            var updatedActivity = await _serviceManager.ActivityService.PutActivityAsync(id, activityUpdateDto);
            return Ok(updatedActivity);
        }

        [HttpPost]
        public async Task<ActionResult> CreateActivity(ActivityCreateDto dto)
        {
            var createdActivityDto = await _serviceManager.ActivityService.CreateActivityAsync(dto);
            return Created();
        }
    }
}
