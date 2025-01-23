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
            //ToDo : Check if the user is authorized to upload the file, check if user is in course or is a techer
            //ToDo : Check if the course, module or activity exists
            //ToDo : Add module and activity id to the query parameters
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded or the file is empty.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            try
            {
                var uploadedFile = await _serviceManager.FileService.UploadFileAsync(file, courseId, userId);
                return CreatedAtAction(nameof(DownloadFile), new { fileId = uploadedFile.UserFileId }, uploadedFile);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while uploading the file.");
            }
        }

        [HttpGet("course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetFilesForCourse(int courseId)
        {
            //ToDo : Check if the user is authorized to get the files, check roles and owner
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            try
            {
                var files = await _serviceManager.FileService.GetFilesByCourseIdAsync(courseId, userId);
                return Ok(files);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the files.");
            }
        }

        [HttpGet("{fileId}")]
        [Authorize]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            //ToDo : Check if the user is authorized to download the file, check owner or if shared with user
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            try
            {

                var (fileContent, contentType, fileName) = await _serviceManager.FileService.DownloadFileAsync(fileId);

                if (fileContent == null)
                    return NotFound("File not found.");
                
                var contentDisposition = new System.Net.Mime.ContentDisposition
                {
                    FileName = fileName,
                    Inline = false
                };
                Response.Headers.Append("Content-Disposition", contentDisposition.ToString());

                return File(fileContent, contentType);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while downloading the file.");
            }
        }
        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            // ToDo : Check if the user is authorized to delete the file, check owner or if admin
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            try
            {
                await _serviceManager.FileService.DeleteFileAsync(fileId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the file.");
            }
        }
    }
}
