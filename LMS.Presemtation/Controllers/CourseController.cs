using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace LMS.Presentation.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CourseController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetOneCourse(int id)
        {
            var courseDto = await _serviceManager.CourseService.GetCourseByIdAsync(id);
            return Ok(courseDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
        {
            var courseDtos = await _serviceManager.CourseService.GetAllCoursesAsync();
            return Ok(courseDtos);
        }
    }
}
