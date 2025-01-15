using LMS.Shared.DTOs.Read;

namespace Services.Contracts
{
    public interface IActivityService
    {
        Task<ActivityDto> GetActivityAsync(int activityId);
    }
}