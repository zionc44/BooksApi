using BooksApi.DTOs;
using BooksApi.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Interfaces
{
    public interface IFileService
    {
        Task<ObjectId> UploadFile(CreDocumentDto document);
        Task<DocumentInfo> GetFileInfo(string id);
        Task<Document> SaveFile(string id);
        Task<Document> DownloadFile(string id);
    }
}
