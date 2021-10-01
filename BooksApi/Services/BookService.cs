using BooksApi.Interfaces;
using BooksApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Services
{
    public class BookService : IBookService
    {
        private readonly IMongoCollection<Book> _books;
        public BookService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>("Books");
        }

        public async Task<List<Book>> Get()
        {
            var books = await _books.FindAsync(book => true);
            return books.ToList();
        }

        public async Task<Book> Get(string id) => await _books.Find<Book>(book => book.Id == id).FirstOrDefaultAsync();

        public async Task<Book> Create(Book book)
        {

            await _books.InsertOneAsync(book);
            return book;
        }

        public async Task<ReplaceOneResult> Update(Book bookIn)
        {
            var result = await _books.ReplaceOneAsync(book => book.Id == bookIn.Id, bookIn);
            return result;
        }

        public async Task<DeleteResult> Remove(string id) => await _books.DeleteOneAsync(book => book.Id == id);
    }
}
