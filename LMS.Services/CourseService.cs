using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs;
using Services.Contracts;

namespace LMS.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CourseService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<CourseDto> GetCourseByIdAsync(int courseId)
        {
            Course? course = await _uow.CourseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
            {
                // Do something
            }
            return _mapper.Map<CourseDto>(course);

            //return new CourseDto()
            //{
            //    CourseId = courseId,
            //    Name = course.Name,
            //    Description = course.Description,
            //    StartDate = course.StartDate,
            //    EndDate = course.EndDate,
            //    CourseDocumentDtos = new List<CourseDocumentDto>(),
            //    Modules = new List<Module>()
            //}
        }
    }
}
