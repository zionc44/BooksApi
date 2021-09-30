using BooksApi.DTOs;
using BooksApi.Interfaces;
using BooksApi.Models;
using BooksApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<List<Book>>> Get() => await _bookService.Get();

        [HttpGet("GetBook/{id:length(24)}", Name = "GetBook")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }
            
            return book;
        }

        [HttpPost("CreateNewBook")]
        public async Task<ActionResult<Book>> Create(Book newBook)
        {
            //var newBook = new Book
            //{
            //    BookName = bookIn.BookName,
            //    Price = bookIn.Price,
            //    Category = bookIn.Category,
            //    Author = bookIn.Author,
            //    //AutherPhotoFile = bookIn.PhotoAutherFile.ToBsonDocument(),
            //    PhotoFileMineType = bookIn.PhotoAutherFileMineType,
            //    PhotoFileName = bookIn.PhotoAutherFileName,
            //    PhotoFileSize = bookIn.PhotoAutherFileSize

            //};

            //if (book.PhotoAutherFile.Length > 0)
            //{
            //    using (var ms = new MemoryStream())
            //    {
            //        book.PhotoAutherFile.CopyTo(ms);
            //        newBook.AutherPhotoFileBytes = ms.ToArray();
            //    }
            //}

            await _bookService.Create(newBook);

            return CreatedAtRoute("GetBook", new { id = newBook.Id.ToString() }, newBook);
        }

        [HttpPut("UpdateBook")]
        public async Task<IActionResult> Update(Book bookIn)
        {
            var book = _bookService.Get(bookIn.Id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(await _bookService.Update(bookIn));
        }

        [HttpDelete("DeleteBook/{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(await _bookService.Remove(id));
        }
    }
}
