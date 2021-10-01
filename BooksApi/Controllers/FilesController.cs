using BooksApi.DTOs;
using BooksApi.Interfaces;
using BooksApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;



namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[DisableRequestSizeLimit]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _filesService;

        public FilesController(IFileService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost("UploadFile")]
        public async Task<DocumentInfo> UploadFile([FromForm] CreDocumentDto document)
        {
            var fileId = await _filesService.UploadFile(document);

            return new DocumentInfo
            {
                DocumentId = fileId.ToString(),
                DocumentName = document.DocumentName,
                DocumentFileName = document.DocumentFile.FileName,
                DocumentFileType = document.DocumentFile.ContentType,
                FormFileName = document.DocumentFile.Name
            };
        }

        [HttpGet("GetFileInfo")]
        public async Task<ActionResult<DocumentInfo>> GetFileInfo(string documentId)
        {
            var docInfo = await _filesService.GetFileInfo(documentId);

            if (docInfo == null) return NotFound();

            return Ok(docInfo);

        }

        [HttpGet("SaveFile")]
        public async Task<IActionResult> SaveFile(string documentId)
        {
            var document =  await _filesService.SaveFile(documentId);
            return Ok(document);
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string documentId)
        {
            var document = await _filesService.DownloadFile(documentId);
            var memory = new MemoryStream(document.DocumentFileBytes);
            memory.Position = 0;

            return File(memory, document.DocumentFileType, document.DocumentFileName);
        }

        [HttpGet("GetFilesInfo")]
        public async Task<ActionResult<IEnumerable<DocumentInfo>>> GetFilesInfo()
        {
            var docList= await _filesService.GetFilesInfo();

            return Ok(docList);
        }

        [HttpDelete("DeleteFile")]
        public async Task<ActionResult> DeleteFile(string documentId)
        {
            await _filesService.DeleteFile(documentId);
            return Ok("File deleted successfully");
        }
    }
}
