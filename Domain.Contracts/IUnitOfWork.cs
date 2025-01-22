namespace Domain.Contracts;

public interface IUnitOfWork
{
    IActivityRepository ActivityRepository { get; }
    ICourseRepository CourseRepository { get; } 
    IModuleRepository ModuleRepository { get; }
    IUserRepository UserRepository { get; }
    IFileRepository FileRepository { get; }
    Task CompleteAsync();
}