using LMS.Shared.DTOs;

namespace Services.Contracts
{
    public interface IActivityService
    {
        Task<ActivityDto> GetActivityAsync(int activityId);
    }
}