namespace Domain.Contracts;

public interface IUnitOfWork
{
    IActivityRepository ActivityRepository { get; }
    ICourseRepository CourseRepository { get; } 
    IModuleRepository ModuleRepository { get; }
    IFileRepository FileRepository { get; }
    Task CompleteAsync();
}