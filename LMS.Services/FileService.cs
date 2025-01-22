using AutoMapper;
using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Shared.DTOs.Read;
using Microsoft.AspNetCore.Http;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FileService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserFile> UploadFileAsync(IFormFile file, int? courseId, string userId)
        {

            var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName, "LMS.Infrastructure");
            var fileStoragePath = Path.Combine(basePath, "AppData", "Files");
            if (!Directory.Exists(fileStoragePath))
            {
                Directory.CreateDirectory(fileStoragePath);
            }

            var fileExtension = Path.GetExtension(file.FileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(fileStoragePath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var userFile = new UserFile
            {
                Name = fileNameWithoutExtension,
                Path = filePath,
                Extension = Path.GetExtension(file.FileName),
                Size = file.Length,
                UploadedAt = DateTime.UtcNow,
                IsShared = false,
                CourseId = courseId,
                ApplicationUserId = userId
            };

            _uow.FileRepository.Create(userFile);
            await _uow.CompleteAsync();

            return userFile;
        }

        public async Task<(byte[] FileContent, string ContentType, string FileName)> DownloadFileAsync(Guid fileId)
        {
            var file = await _uow.FileRepository.GetFileByIdAsync(fileId);
            if (file == null) throw new KeyNotFoundException("File not found.");

            var fileContent = await File.ReadAllBytesAsync(file.Path);
            var fileNameWithExtension = $"{file.Name}{file.Extension}";
            return (fileContent, "application/octet-stream", fileNameWithExtension);
        }

        public async Task DeleteFileAsync(Guid fileId, string userId)
        {
            var fileToDelete = await _uow.FileRepository.GetFileByIdAsync(fileId);
            if (fileToDelete == null) 
                throw new KeyNotFoundException("File not found.");
            if (fileToDelete.ApplicationUserId != userId) throw new UnauthorizedAccessException("You are not authorized to delete this file.");
            _uow.FileRepository.Delete(fileToDelete);
            await _uow.CompleteAsync();
        }

        public async Task<IEnumerable<UserFileReadDto>> GetFilesByCourseIdAsync(int courseId, string userId)
        {
            var files = await _uow.FileRepository.GetFilesByCourseIdAsync(courseId, userId);


            return _mapper.Map<IEnumerable<UserFileReadDto>>(files);
        }
    }

}
