using AutoMapper;
using BooksApi.DTOs;
using BooksApi.Interfaces;
using BooksApi.Models;
using Microsoft.AspNetCore.StaticFiles;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BooksApi.Services
{
    public class FileService : IFileService
    {
        private readonly GridFSBucket _fSBucket;

        public FileService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _fSBucket = new GridFSBucket(database, new GridFSBucketOptions
            {
                BucketName = "Documents",
                ChunkSizeBytes = 261120, // 255KB
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });
        }

        public async Task<Document> DownloadFile(string id)
        {
            var docId = ObjectId.Parse(id);
            var docInfo = await GetFileInfo(id);

            return new Document
            {
                DocumentId = docInfo.DocumentId,
                DocumentFileName = docInfo.DocumentFileName,
                DocumentFileType = docInfo.DocumentFileType,
                DocumentName = docInfo.DocumentName,
                DocumentFileBytes = await _fSBucket.DownloadAsBytesAsync(docId)
            };
        }

        public async Task<Document> SaveFile(string id)
        {
            var docId = ObjectId.Parse(id);
            var docInfo = await GetFileInfo(id);

            var folderName = Path.Combine("Resources", "Files");
            var fileName = docInfo.DocumentFileName;
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            var fullPath = Path.Combine(pathToSave, fileName);

            using (Stream fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
            {
                await _fSBucket.DownloadToStreamAsync(docId, fs);
                fs.Close();
            }

            return new Document
            {
                DocumentId = docInfo.DocumentId,
                DocumentFileName = docInfo.DocumentFileName,
                DocumentFileType = docInfo.DocumentFileType,
                DocumentName = docInfo.DocumentName,
                DocumentFullFileName = fullPath,
            };
        }

        public async Task<DocumentInfo> GetFileInfo(string id)
        {
            var docId = ObjectId.Parse(id);
            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq("_id", docId);
            var fileInfos = await _fSBucket
                .FindAsync(filter);


            var fileInfo = await fileInfos.FirstOrDefaultAsync();

            if (fileInfo != null)
            {
                return GridFSFileInfoToDocumentInfo(fileInfo);
            }

            return null;
        }


        public async Task<ObjectId> UploadFile(CreDocumentDto document)
        {

            var options = new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    { "fileType", document.DocumentFile.ContentType},
                    { "fileName", document.DocumentFile.FileName},
                    { "fileLength", document.DocumentFile.Length },
                    { "name", document.DocumentFile.Name},
                    { "documentName", document.DocumentName }
                }
            };

            using (var ms = new MemoryStream())
            {
                document.DocumentFile.CopyTo(ms);
                var id = await _fSBucket.UploadFromBytesAsync(document.DocumentFile.FileName, ms.ToArray(), options);
                return id;
            }
        }

        public async Task<IEnumerable<DocumentInfo>> GetFilesInfo()
        {
            List<DocumentInfo> docList = new List<DocumentInfo>();

            var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Empty;
            var fileInfos = await _fSBucket.FindAsync(filter);

            foreach (var file in fileInfos.ToList())
            {
                docList.Add(GridFSFileInfoToDocumentInfo(file));
            }

            return docList;
        }

        private DocumentInfo GridFSFileInfoToDocumentInfo(GridFSFileInfo<ObjectId> fileInfo)
        {
            return new DocumentInfo
            {
                DocumentId = fileInfo.Id.ToString(),
                DocumentFileName = fileInfo.Filename,
                DocumentFileType = (string)fileInfo.Metadata.GetValue(0),
                DocumentName = (string)fileInfo.Metadata.GetValue(4),
                FormFileName = (string)fileInfo.Metadata.GetValue(3)
            };
        }

        public async Task DeleteFile(string id)
        {
            var docId = ObjectId.Parse(id);
            await _fSBucket.DeleteAsync(docId);
        }
    }
}
