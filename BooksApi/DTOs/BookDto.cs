using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.DTOs
{
    public class BookDto
    {
        public string Id { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
        public IFormFile PhotoAutherFile { get; set; }
        public string PhotoAutherFileName { get; set; }
        public string PhotoAutherFileMineType { get; set; }
        public int PhotoAutherFileSize { get; set; }
    }
}
