using BookStore.API.Contracts;
using BookStore.Core.Abstractions;
using BookStore.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        public BooksController(IBooksService booksService) 
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BooksResponce>>> GetBooks()
        {
            var books = await _booksService.GetAllBooks();

            var responce = books.Select(b => new BooksResponce(b.Id, b.Title, b.Description, b.Price));
            return Ok(responce);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateBook([FromBody] BooksRequest request)
        {
            var (book, error) = Book.Create(
                Guid.NewGuid(),
                request.Title,
                request.Description,
                request.Price);

            if(!string.IsNullOrEmpty(error)) 
            {
                return BadRequest(error);
            }
            var bookId = await _booksService.CreateBook(book);
            return Ok(bookId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateBooks(Guid id, [FromBody] BooksRequest request)
        {
            var bookId = await _booksService.UpdateBook(id , request.Title , request.Description , request.Price);
            return Ok(bookId);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteBook(Guid id)
        {
            return Ok(await _booksService.DeleteBook(id));
        }
    }
}
