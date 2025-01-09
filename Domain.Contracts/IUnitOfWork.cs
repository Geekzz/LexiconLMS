namespace Domain.Contracts;

public interface IUnitOfWork
{
    IActivityRepository ActivityRepository { get; }
    ICourseRepository CourseRepository { get; } 
    IModuleRepository ModuleRepository { get; } 
    Task CompleteASync();
}