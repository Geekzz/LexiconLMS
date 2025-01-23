﻿using LMS.Shared.DTOs.Create;
using LMS.Shared.DTOs.Read;
using LMS.Shared.DTOs.Update;

namespace Services.Contracts
{
    public interface IActivityService
    {
        Task<ActivityDto> GetActivityAsync(int activityId);
        Task<ActivityDto> PutActivityAsync(int id, ActivityUpdateDto activity);
        Task<ActivityDto> CreateActivityAsync(ActivityCreateDto dto);
    }
}