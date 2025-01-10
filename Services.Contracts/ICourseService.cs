using LMS.Shared.DTOs;

namespace Services.Contracts
{
    public interface ICourseService
    {
        Task<CourseDto> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
    }
}