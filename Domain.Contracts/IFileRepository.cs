using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IFileRepository: IRepositoryBase<UserFile>
    {
        Task<UserFile?> GetFileByIdAsync(Guid fileId, bool trackChanges = false);
        Task<IEnumerable<UserFile>> GetFilesByCourseIdAsync(int courseId, string userId,  bool trackChanges = false);
    }
}
