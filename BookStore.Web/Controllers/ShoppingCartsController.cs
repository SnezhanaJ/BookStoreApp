using BookStore.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BookStore.Web.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController(IShoppingCartService _shoppingCartService)
        {
            this._shoppingCartService = _shoppingCartService;
        }

        // GET: ShoppingCarts
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dto = _shoppingCartService.getShoppingCartInfo(userId);
            return View(dto);
            //return (IActionResult)dto;
        }


        public IActionResult DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _shoppingCartService.deleteBookFromShoppingCart(userId, id);

            return RedirectToAction("Index");

        }
    }
}
