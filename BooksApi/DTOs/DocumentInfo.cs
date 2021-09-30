using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.DTOs
{
    public class DocumentInfo
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentFileName { get; set; }
        public string DocumentFileType { get; set; }
        public string FormFileName { get; set; }
    }
}
