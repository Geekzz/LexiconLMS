﻿using Domain.Contracts;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsContext _context;
    private readonly Lazy<IActivityRepository> _activityRepository;
    private readonly Lazy<IModuleRepository> _moduleRepository;
    private readonly Lazy<ICourseRepository> _courseRepository;
    private readonly Lazy<IFileRepository> _fileRepository;

    public IActivityRepository ActivityRepository => _activityRepository.Value;
    public IModuleRepository ModuleRepository => _moduleRepository.Value;
    public ICourseRepository CourseRepository => _courseRepository.Value;
    public IFileRepository FileRepository => _fileRepository.Value;


    public UnitOfWork(LmsContext context)
    {
        _context = context;
        _activityRepository = new Lazy<IActivityRepository>(() => new ActivityRepository(_context));
        _moduleRepository = new Lazy<IModuleRepository>(() => new ModuleRepository(_context));
        _courseRepository = new Lazy<ICourseRepository> (() => new CourseRepository(_context));
        _fileRepository = new Lazy<IFileRepository>(() => new FileRepository(_context));
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
