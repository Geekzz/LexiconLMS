using LMS.Shared.DTOs;

namespace Services.Contracts
{
    public interface IModuleService
    {
        Task<ModuleDto> GetModuleByIdAsync(int moduleId);
    }
}