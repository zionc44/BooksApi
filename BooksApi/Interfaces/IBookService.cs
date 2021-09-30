using BooksApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Interfaces
{
    public interface IBookService
    {
        public Task<List<Book>> Get();

        public Task<Book> Get(string id);

        public Task<Book> Create(Book book);

        public Task<ReplaceOneResult> Update(Book bookIn);

        public Task<DeleteResult> Remove(string id);
    }
}
