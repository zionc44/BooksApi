using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.DTOs
{
    public class CreDocumentDto
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public IFormFile DocumentFile { get; set; }
    }
}
