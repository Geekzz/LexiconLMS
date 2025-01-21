using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public class FileRepository : RepositoryBase<UserFile>, IFileRepository
    {
        public FileRepository(LmsContext context) : base(context)
        {
        }

        public async Task<UserFile?> GetFileByIdAsync(Guid userFileId, bool trackChanges = false)
        {
            return await FindByCondition(f => f.UserFileId == userFileId, trackChanges).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserFile>> GetFilesByCourseIdAsync(int courseId, bool trackChanges = false)
        {
            return await FindByCondition(f => f.CourseId == courseId, trackChanges).ToListAsync();
        }
    }


}
