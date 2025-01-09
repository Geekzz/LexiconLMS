using Domain.Models.Entities;

namespace Domain.Contracts
{
    public interface IActivityRepository
    {
        Task<Activity?> GetActivityByIdAsync(int activityId, bool trackChanges = false);
    }
}