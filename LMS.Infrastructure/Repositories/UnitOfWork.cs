using Domain.Contracts;
using LMS.Infrastructure.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsContext _context;
    private readonly Lazy<IActivityRepository> _activityRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<ICourseRepository> _courseRepository;

    public IActivityRepository ActivityRepository => _activityRepository.Value;
    public IModuleRepository ModuleRepository => _moduleRepository.Value;
    public ICourseRepository CourseRepository => _courseRepository.Value;   


    public UnitOfWork(LmsContext context)
    {
        _context = context;
        _activityRepository = new Lazy<IActivityRepository>(() => new ActivityRepository(_context));
        _moduleRepository = new Lazy<IModuleRepository>(() => new ModuleRepository(_context));
        _courseRepository = new Lazy<ICourseRepository> (() => new CourseRepository(_context));
    }

    public async Task CompleteASync()
    {
        await _context.SaveChangesAsync();
    }
}
