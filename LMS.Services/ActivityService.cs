using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Read;
using Services.Contracts;

namespace LMS.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ActivityService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ActivityDto> GetActivityAsync(int activityId)
        {
            Activity? activity = await _uow.ActivityRepository.GetActivityByIdAsync(activityId);
            return _mapper.Map<ActivityDto>(activity);
        }

    }
}
