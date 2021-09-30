using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Models
{
    public class Document
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentFileType { get; set; }
        public string FormFileName { get; set; }
        public string DocumentFullFileName { get; set; }
        public Byte[] DocumentFileBytes { get; set; }
    }
}
