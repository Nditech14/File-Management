﻿using Template.Application.Services;
using Template.Domain.Entities;
using Template.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace TemplateADLS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService<FileEntity> _fileService;

        public FilesController(IFileService<FileEntity> fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                // Save the file using the file service
                var fileEntity = await _fileService.UploadFileAsync(file.OpenReadStream(), file.FileName);

                return Ok(new { filePath = fileEntity.FilePath });
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                Console.WriteLine($"An error occurred while uploading the file: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{fileId}")]
        public async Task<IActionResult> DownloadFile(string fileId)
        {
            var fileStream = await _fileService.DownloadFileAsync(fileId);
            if (fileStream == null) return NotFound();

            return File(fileStream, "application/octet-stream", fileId);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var result = await _fileService.DeleteFileAsync(fileId);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _fileService.GetFilesAsync();
            return Ok(files);
        }
    }
}
