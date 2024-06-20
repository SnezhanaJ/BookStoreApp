using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Repository;
using BookStore.Domain.Domain;
using BookStore.Service.Interface;
using BookStore.Service.Implementation;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BookStore.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;
        private readonly IShoppingCartService _shoppingCartService;

        public BooksController(IBooksService booksService, IAuthorService authorService, IPublisherService publisherService, IShoppingCartService shoppingCartService)
        {
            _booksService = booksService;
            _authorService = authorService;
            _publisherService = publisherService;
            _shoppingCartService = shoppingCartService;
        }

        // GET: Books
        public IActionResult Index()
        {
            return View(_booksService.GetAll());
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = _booksService.GetDetails(id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            // Fetch the authors from the database
            var authors = _authorService.GetAll().Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName  // Combine first and last names
            }).ToList();

            // Create a SelectList with FullName as the text and Id as the value
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,BookImage,Price,ReleaseDate,PublisherId,AuthorId")] Books books)
        {
            if (ModelState.IsValid)
            {
                _booksService.CreateNewBook(books);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_authorService.GetAll(), "Id", "Id", books.AuthorId);
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAll(), "Id", "Name", books.PublisherId);
            return View(books);
        }

        // GET: Books/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = _booksService.GetDetails(id);
            if (books == null)
            {
                return NotFound();
            }
            var authors = _authorService.GetAll().Select(a => new {
                Id = a.Id,
                FullName = a.FirstName + " " + a.LastName  // Combine first and last names
            }).ToList();

            // Create a SelectList with FullName as the text and Id as the value
            ViewData["AuthorId"] = new SelectList(authors, "Id", "FullName");
            ViewData["PublisherId"] = new SelectList(_publisherService.GetAll(), "Id", "Name", books.PublisherId);
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,BookImage,Price,ReleaseDate,PublisherId,AuthorId")] Books books)
        {
            if (id != books.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _booksService.UpdateExistingBook(books);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_authorService.GetAll(), "Id", "Id", books.AuthorId);

            ViewData["PublisherId"] = new SelectList(_publisherService.GetAll(), "Id", "Name", books.PublisherId);
            return View(books);
        }

        // GET: Books/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = _booksService.GetAll();
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _booksService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(Guid id)
        {
            var book = _booksService.GetDetails(id);
            return book != null;
        }

        public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _booksService.GetDetails(id);

            BooksInShoppingCart ps = new BooksInShoppingCart();

            if (product != null)
            {
                ps.BookId = product.Id;
                ps.Book = product;
            }

            return View(ps);
        }

        [HttpPost]
        public IActionResult AddToCartConfirmed(BooksInShoppingCart model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.AddToShoppingConfirmed(model, userId);

            return View("Index", _booksService.GetAll());
        }
    }
}
