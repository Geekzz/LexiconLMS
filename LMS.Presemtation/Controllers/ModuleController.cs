﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LMS.Shared.DTOs;
//using Microsoft.AspNetCore.Mvc;
//using Services.Contracts;

//namespace LMS.Presentation.Controllers
//{
//    [Route("api/modules")]
//    [ApiController]
//    public class ModuleController : ControllerBase
//    {
//        private readonly IServiceManager _serviceManager;

//        public ModuleController(IServiceManager serviceManager)
//        {
//            _serviceManager = serviceManager;
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<ModuleDto>> GetOneModule(int id)
//        {
//            var courseDto = await _serviceManager.CourseService.GetCourseByIdAsync(id);
//            return Ok(courseDto);
//        }
//    }
//}
