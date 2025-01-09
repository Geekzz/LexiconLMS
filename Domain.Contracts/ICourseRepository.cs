using Domain.Models.Entities;

namespace Domain.Contracts
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync(bool trackChanges = false);
        Task<Course?> GetCourseByIdAsync(int courseId, bool trackChanges = false);
    }
}