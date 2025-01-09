using Domain.Models.Entities;

namespace LMS.Infrastructure.Contracts
{
    public interface IModuleRepository
    {
        Task<Module?> GetModuleByIdAsync(int moduleId, bool trackChanges = false);
    }
}