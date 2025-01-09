using Domain.Models.Entities;

namespace LMS.Infrastructure.Contracts
{
    public interface IActivityRepository
    {
        Task<Activity?> GetActivityByIdAsync(int activityId, bool trackChanges = false);
    }
}