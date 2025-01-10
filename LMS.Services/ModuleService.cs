using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using AutoMapper;
using Domain.Models.Entities;
using Services.Contracts;
using LMS.Shared.DTOs.Read;
using LMS.Shared.DTOs.Create;

namespace LMS.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ModuleService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ModuleDto> GetModuleByIdAsync(int moduleId)
        {
            Module? module = await _uow.ModuleRepository.GetModuleByIdAsync(moduleId);
            return _mapper.Map<ModuleDto>(module);
        }

        public async Task<ModuleDto> CreateModuleAsync(ModuleCreateDto dto)
        {
            Module module = _mapper.Map<Module>(dto);

            _uow.ModuleRepository.Create(module);

            await _uow.CompleteAsync();

            return _mapper.Map<ModuleDto>(module);
        }
    }
}
