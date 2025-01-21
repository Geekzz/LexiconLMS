using AutoMapper;
using Domain.Contracts;
using LMS.Shared.DTOs.Read;
using LMS.Shared.DTOs.Update;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<UserDto> PutUserAsync(string id, UserUpdateDto userUpdateDto)
        {
            var userToUpdate = await _uow.UserRepository.GetUserByIdAsync(id, true);
            if (userToUpdate == null) throw new KeyNotFoundException($"{id} not found.");

            userToUpdate.Email = userUpdateDto.Email;
            userToUpdate.FirstName = userUpdateDto.FirstName;
            userToUpdate.LastName = userUpdateDto.LastName;

            await _uow.CompleteAsync();

            return _mapper.Map<UserDto>(userToUpdate);
        }
    }
}
