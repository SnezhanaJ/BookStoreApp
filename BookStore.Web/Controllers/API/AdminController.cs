using BookStore.Domain.Domain;
using BookStore.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IBooksService _booksService;
        private readonly IAuthorService _authorService;

        public AdminController(IBooksService booksService, IAuthorService authorService)
        {
            _booksService = booksService;
            _authorService = authorService;
        }

        [HttpGet("[action]")]
        public List<Books> GetAllBooks()
        {
            return this._booksService.GetAll();
        }

        [HttpGet("[action]")]
        public List<Author> GetAllAuthors()
        {
            return this._authorService.GetAll();
        }

        [HttpPost("[action]")]
        public Author GetAuthorDetails(BaseEntity id)
        {
            return this._authorService.GetDetailsWithBaseEntity(id);
        }

        [HttpPost("[action]")]

        public IActionResult EditAuthor(Author author)
        {

            if (ModelState.IsValid)
            {
                var result =_authorService.UpdateAuthor(author);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update author");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
