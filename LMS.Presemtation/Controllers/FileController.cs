using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public FileController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] int? courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            /*var userId = User.FindFirst("sub")?.Value; */// Assume JWT contains user ID in "sub" claim
            if (userId == null) return Unauthorized();

            var uploadedFile = await _serviceManager.FileService.UploadFileAsync(file, courseId, userId);
            return CreatedAtAction(nameof(DownloadFile), new { fileId = uploadedFile.UserFileId }, uploadedFile);
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var (fileContent, contentType, fileName) = await _serviceManager.FileService.DownloadFileAsync(fileId);

            return File(fileContent, contentType, fileName);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            await _serviceManager.FileService.DeleteFileAsync(fileId, userId);
            return NoContent();
        }
    }
}
