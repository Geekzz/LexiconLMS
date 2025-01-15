using LMS.Shared.DTOs.Create;
using LMS.Shared.DTOs.Read;

namespace Services.Contracts
{
    public interface IModuleService
    {
        Task<ModuleDto> GetModuleByIdAsync(int moduleId);
        Task<ModuleDto> CreateModuleAsync(ModuleCreateDto dto);
    }
}