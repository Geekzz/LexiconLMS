using Domain.Models.Entities;
using LMS.Shared.DTOs.Read;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IFileService
    {
        Task<UserFile> UploadFileAsync(IFormFile file, int? courseId, string userId);
        Task<(byte[] FileContent, string ContentType, string FileName)> DownloadFileAsync(Guid userFileId);
        Task DeleteFileAsync(Guid fileId, string userId);

        Task<IEnumerable<UserFileReadDto>> GetFilesByCourseIdAsync(int courseId, string userId);
    }
}
